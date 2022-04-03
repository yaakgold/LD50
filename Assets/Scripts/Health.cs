using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent Death, TookDamage, Healed;

    public int currentHealth;

    public int MaxHealth { get; private set; }

    private void Awake()
    {
        MaxHealth = currentHealth;
    }

    public void DoDamage(int amount)
    {
        //if (MaxHealth == 0) MaxHealth = currentHealth;

        if(TryGetComponent(out PlayerController pc))
        {
            if(amount > 0)
                amount = (int)(pc.healthDamageMult * amount);
        }

        currentHealth -= amount;
        currentHealth = Mathf.Min(currentHealth, MaxHealth);

        if(currentHealth <= 0)
        {
            Death.Invoke();
        }
        else if(amount > 0)
        {
            TookDamage.Invoke();
        }
        else
        {
            Healed.Invoke();
        }
    }

}
