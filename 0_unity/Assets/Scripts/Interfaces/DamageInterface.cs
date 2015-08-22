using UnityEngine;

[RequireComponent(typeof(HasHealth))]
public class DamageInterface : MonoBehaviour
{
    HasHealth HasHealthScript;
    HasShield HasShieldScript;

    void Start()
    {
        HasHealthScript = GetComponent<HasHealth>();
        HasShieldScript = GetComponent<HasShield>();
    }

    public void ApplyDamage(int Damage)
    {
        if (HasShieldScript)
        {
            Damage -= HasShieldScript.GeneralDefense;
            if (Damage <= 0)
            {
                return;
            }
        }
        HasHealthScript.CurrentHealth -= Damage;
    }
}
