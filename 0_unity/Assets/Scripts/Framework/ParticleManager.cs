using UnityEngine;
using System.Collections;

public static class Particles {

    #region MousePointer
    private static Transform SuccessTransform;
    private static ParticleSystem SuccessParticle;
    private static Transform ErrorTransform;
    private static ParticleSystem ErrorParticle;
    public static void InitMousePointer(Transform successTransform, ParticleSystem successParticle, Transform errorTransform, ParticleSystem errorParticle)
    {
        SuccessTransform = successTransform;
        SuccessParticle  = successParticle;
        ErrorTransform   = errorTransform;
        ErrorParticle    = errorParticle;
    }
    public static void MousePointer(Vector3 position, bool isValid)
    {
        (isValid ? SuccessTransform : ErrorTransform).position = position + new Vector3(0f, 0.1f, 0f);
        (isValid ? SuccessParticle : ErrorParticle).Stop();
        (isValid ? SuccessParticle : ErrorParticle).Play();
    }
    #endregion
}

public class ParticleManager : MonoBehaviour  {

	// Use this for initialization
	void Start ()
    {
        Particles.InitMousePointer(
            GameObject.Find("Success").GetComponent<Transform>(),
            GameObject.Find("Success").GetComponent<ParticleSystem>(),
            GameObject.Find("Error").GetComponent<Transform>(),
            GameObject.Find("Error").GetComponent<ParticleSystem>()
        );
    }
}
