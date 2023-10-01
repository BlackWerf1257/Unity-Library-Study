using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private Image barImg;
    [SerializeField] private Button minus, plus;
    private HPSystem hpSystem;

    private void Start()
    {
        hpSystem = new HPSystem(100);
        SetHealth(hpSystem.GetHealthNormalized());
        
        hpSystem.OnDamaged += HpSystemOnOnDamaged;
        hpSystem.OnHealed += HpSystemOnOnHealed;

        minus.onClick.AddListener(() => hpSystem.Damage(10));
        plus.onClick.AddListener(() => hpSystem.Heal(10));
    }

    
    //https://youtu.be/cR8jP8OGbhM?si=4UM0ZfoG242kzjj-&t=352
    //5:52초 부터 시청
    
    private void HpSystemOnOnDamaged(object sender, EventArgs e)
    {
        SetHealth(hpSystem.GetHealthNormalized());
    }
    private void HpSystemOnOnHealed(object sender, EventArgs e)
    {
        SetHealth(hpSystem.GetHealthNormalized());
    }
    

    private void SetHealth(float healthNormalized)
    {
        barImg.fillAmount = healthNormalized;
    }
}
