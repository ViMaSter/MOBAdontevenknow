using UnityEngine;
using UnityEditor;
using System.Collections;

public class Inverter : MonoBehaviour {
    [MenuItem("CONTEXT/Transform/Flip object")]
    private static void SetOffset(MenuCommand menuCommand)
    {
        (menuCommand.context as Transform).localPosition = new Vector3(
            (menuCommand.context as Transform).localPosition.x * -1,
            (menuCommand.context as Transform).localPosition.y,
            (menuCommand.context as Transform).localPosition.z
        );
        (menuCommand.context as Transform).localEulerAngles = new Vector3(
            (menuCommand.context as Transform).localEulerAngles.x,
            180 - (menuCommand.context as Transform).localEulerAngles.y,
            (menuCommand.context as Transform).localEulerAngles.z
        );
    }
}
