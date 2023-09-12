using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public class RadicalMenuEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image logo;
    [SerializeField] private RectTransform rect;
    [SerializeField] private TextMeshProUGUI currentWeaponText;
    [SerializeField] private Image thisObj;
    [SerializeField] private float fadeTime = 0.1f;

    
    private void OnEnable()
    {
        currentWeaponText = GameObject.FindGameObjectWithTag("PieUIText").GetComponent<TextMeshProUGUI>();
    }

    public void SetLabel(string pText)
    {
        label.text = pText;
    }

    void SetIcon(Sprite image)
    {
        logo.sprite = image;
    }

    
    public void DoFadeIn(Sprite img)
    {
//        logo.DOFade(1.0f, fadeTime);
        thisObj.DOFade(1.0f, fadeTime).onComplete =
            delegate
            {
                SetIcon(img);
                logo.DOFade(1.0f, fadeTime);
            };
        
    }
    public void DoFadeOut()
    {
        logo.DOFade(0f, fadeTime).onComplete = 
            delegate
        {
            Destroy(this.gameObject);
        }
        ;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOComplete();
        //rect.DOScale(Vector3.one * 1.5f, .3f).SetEase(Ease.OutQuad);
        currentWeaponText.text = label.text;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOComplete();
        //rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
    }
}
