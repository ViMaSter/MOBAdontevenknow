using UnityEngine;
using System.Collections;

public class HasHealth : MonoBehaviour
{
    public int DeadUnder = 0;
    public int CurrentHealth = 0;
    public int MaxHealth = 100;

    bool IsAlive;

	void Start ()
    {
        CurrentHealth = MaxHealth;
	}
	
	void Update () 
    {
        if (IsAlive)
        {
            // Check when this object dies
            if (CurrentHealth < DeadUnder)
            {
                Die();
            }
        }
	}

    void Die()
    {
        IsAlive = true;
    }
}
