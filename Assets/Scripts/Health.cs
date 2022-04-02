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

    private void Start()
    {
        MaxHealth = currentHealth;
    }

    public void DoDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Min(currentHealth, MaxHealth);

        if(currentHealth < 0)
        {
            Death.Invoke();
        }
        else if(amount < 0)
        {
            TookDamage.Invoke();
        }
        else
        {
            Healed.Invoke();
        }

        print(name + " " + currentHealth);
    }

}
