using UnityEngine;

[RequireComponent(typeof(HasHealth))]
public class DamageInterface : MonoBehaviour {
    HasHealth HasHealthScript;

    void Start()
    {
        HasHealthScript = GetComponent<HasHealth>();
    }

    public void ApplyDamage(int Damage)
    {
        HasHealthScript.CurrentHealth -= Damage;
    }
}
