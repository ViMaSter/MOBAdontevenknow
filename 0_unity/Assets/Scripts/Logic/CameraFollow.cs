using UnityEditor;
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    #region Gamefeel
    public Vector3 PositionOffset = new Vector3(0, 12, -10);
    public Vector3 TargetOffset = Vector3.zero;

    public float CameraPositionLerp = 0.3f;
    public float CameraPositionPeek = 10.0f;
    public float PeekLerp = 0.2f;
    #endregion

    private Vector3 PostLerpPosition;
    private Vector3 LastPeek;

    [MenuItem("CONTEXT/CameraFollow/Use current offset to Vector3.zero")]
    private static void SetOffset(MenuCommand menuCommand)
    {
        (menuCommand.context as CameraFollow).PositionOffset = ((GameObject)Selection.activeObject).GetComponent<Transform>().position;
    }

    public void Start()
    {
        PostLerpPosition = transform.position;
    }

	public void Update() {
        // Calculate the rotation angle
        transform.rotation = Quaternion.LookRotation(TargetOffset - PositionOffset, Vector3.up);

        // Final position the camera will have: Target.position + PositionOffset
        Vector3 target = Target.position + PositionOffset;

        // Lerp to final position
        PostLerpPosition = Vector3.Lerp(PostLerpPosition, target, CameraPositionLerp);

        // Peek to the direction of movement
        Vector3 peek = Vector3.Normalize(target - PostLerpPosition) * (target - PostLerpPosition).magnitude;
                peek.y = 0.0f;
                peek *= CameraPositionPeek;

        LastPeek = Vector3.Lerp(LastPeek, peek, PeekLerp);

        transform.position = Vector3.Lerp(transform.position, target + LastPeek, CameraPositionLerp);
    }
}
