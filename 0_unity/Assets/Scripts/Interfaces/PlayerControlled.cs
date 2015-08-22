using UnityEngine;
using System.Collections;

public class PlayerControlled : MonoBehaviour
{
    MoveableObject MoveableObjectScript;

	void Start () {
        MoveableObjectScript = GetComponent<MoveableObject>();
	}

    void MoveableObjectScriptRoutine()
    {
        if (MoveableObjectScript == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200.0f, ~(1 << 8))) {
                MoveableObjectScript.CurrentTarget = new Vector2(hit.point.x, hit.point.z);
                Debug.Log(string.Format("Click at: {0}\nMove to: {1}", Input.mousePosition, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane))));
                Particles.MousePointer(hit.point, true);
                // init success particle
            } else {
                Debug.Log(string.Format("Click at: {0}\nMove to: Nowhere!", Input.mousePosition));
                Particles.MousePointer(hit.point, false);
                // init error particle
            }
        }
    }
	
	void Update () {
        StartCoroutine("MoveableObjectScriptRoutine");
        MoveableObjectScriptRoutine();
	}
}
