using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

public class WebsocketMinionControl : WebSocketSharp.Server.WebSocketBehavior
{
    public GameObject AssociatedMinion;

    protected override void OnOpen()
    {
        Debug.Log("[WEB] New connection: " + Context.Host);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        if (e.Data.StartsWith("register"))
        {
            lock (WebsocketClient.QueuedAssociations)
            {
                WebsocketClient.QueuedAssociations.Add(this);
            }
        }

        if (e.Data.StartsWith("movement"))
        {
            lock (WebsocketClient.QueuedMovements)
            {
                string[] commands = e.Data.Split(' ');
                WebsocketClient.QueuedMovements.Add(this, new float[] { float.Parse(commands[1]), float.Parse(commands[2]) });
            }
        }
    }
}

public class WebsocketClient : MonoBehaviour
{
    public string IP = "127.0.0.1";
    public int Port = 9999;

    private bool VerboseLogging = false;

    public WebSocketServer Server;

    public static List<WebsocketMinionControl> QueuedAssociations;
    public static Dictionary<WebsocketMinionControl, float[]> QueuedMovements;

    public void Start()
    {
        QueuedAssociations = new List<WebsocketMinionControl>();
        QueuedMovements = new Dictionary<WebsocketMinionControl, float[]>();

        Server = new WebSocketServer(string.Format("ws://{0}:{1}", IP, Port));

        if (VerboseLogging)
        {
            Server.Log.Level = LogLevel.Trace;
            Server.Log.Output = (LogData d, string s) =>
            {
                Debug.Log(d);
            };
        }

        Server.AddWebSocketService<WebsocketMinionControl>("/", () => new WebsocketMinionControl()
        {
            // Unity does not support the gzip-compression features
            // of .NET 3.5 used by WebSocketSharp
            IgnoreExtensions = true
        });
        Server.Start();
    }

    ~WebsocketClient()
    {
        Server.Stop();
    }

    public void ProcessQueues()
    {
        lock (QueuedMovements)
        {
            foreach (KeyValuePair<WebsocketMinionControl, float[]> item in QueuedMovements)
            {
                if (item.Key.AssociatedMinion == null)
                {
                    Debug.LogWarning("[WEB] User sent movement without registering.");
                }

                NavMeshHit hit;
                NavMesh.SamplePosition(
                    new Vector3(
                        item.Value[0] * 50,
                        0,
                        item.Value[1] * 25
                    ),
                    out hit,
                    2.0f,
                    NavMesh.AllAreas
                );

                Debug.Log(string.Format("[WEB] Moving '{0}' to [{1}, {2}] ([])", item.Key.AssociatedMinion.name, item.Value[0] * 50, item.Value[1] * 25, item.Value[0], item.Value[1]));

                item.Key.AssociatedMinion.GetComponent<NavMeshAgent>().SetDestination(
                    hit.position
                );
            }
            QueuedMovements.Clear();
        }

        lock (QueuedAssociations)
        {
            foreach (WebsocketMinionControl item in QueuedAssociations)
            {
                Debug.Log("[WEB] Registration: " + item.Context.Host);

                GameObject minion = Minions.GetControllableMinion();
                if (minion != null)
                {
                    Debug.Log(string.Format("[WEB] {0} now controls minion {1}", item.Context.Host, minion.name), minion);
                    item.AssociatedMinion = minion;
                    item.AssociatedMinion.name = "[C]" + item.AssociatedMinion.name;
                    item.AssociatedMinion.GetComponent<MeshRenderer>().material.color = new Color(0.27f, 0.61f, 0.1333f, 1.0f);
                }
                else
                {
                    Debug.Log(string.Format("[WEB] {0} couldn't get a minion: No minions left", item.Context.Host));
                }
            }
            QueuedAssociations.Clear();
        }
    }

    public void Update()
    {
        ProcessQueues();
    }
}
