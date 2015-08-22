using UnityEditor;
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    #region Gamefeel
    public float CameraPositionLerp = 0.9f;
    public float CameraPositionPeek = 3.0f;
    public float PeekLerp = 0.9f;
    #endregion

    public Transform Target;

    public Vector3 PositionOffset;
    public Vector3 TargetOffset;
    public Vector3 cameraPositionPreLerp;

    public Vector3 PostLerpPosition;
    public Vector3 LastPeek;
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
        transform.rotation = Quaternion.LookRotation((TargetOffset) - PositionOffset, Vector3.up);

        // Final position the camera will have: Target.position + PositionOffset
        Vector3 target = Target.position + PositionOffset;

        // Lerp to final position
        PostLerpPosition = Vector3.Lerp(PostLerpPosition, target, CameraPositionLerp);

        // How far to peek: (target - transform.position).magnitude * CameraPositionPeek
        Vector3 peek = Vector3.Normalize(target - PostLerpPosition) * (target - PostLerpPosition).magnitude;
                peek.y = 0.0f;
                peek *= CameraPositionPeek;

                LastPeek = Vector3.Lerp(LastPeek, peek, PeekLerp);

        transform.position = Vector3.Lerp(transform.position, target + LastPeek, CameraPositionLerp);
    }
}
