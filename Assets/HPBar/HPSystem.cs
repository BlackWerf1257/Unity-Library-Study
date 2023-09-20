using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystem : MonoBehaviour
{
    public event EventHandler OnDamaged, OnHealed;

    private int healAmount;
    private int minHP, maxHP;

    public HPSystem(int healAmount)
    {
        maxHP = healAmount;
        this.healAmount = maxHP;
    }

    public void Damage(int amount)
    {
        healAmount -= amount;
        if (healAmount < 0)  healAmount = 0;
        
        if(OnDamaged != null) OnDamaged(this, EventArgs.Empty);
    }
    public void Heal(int amount)
    {
        healAmount += amount;
        if (healAmount > maxHP)  healAmount = maxHP;
        
        if(OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)healAmount / maxHP;
    }
}
