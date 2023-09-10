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
    //[SerializeField] private

    private void OnEnable()
    {
        currentWeaponText = GameObject.FindGameObjectWithTag("PieUIText").GetComponent<TextMeshProUGUI>();
    }

    public void SetLabel(string pText)
    {
        label.text = pText;
    }

    public void SetIcon(Sprite image)
    {
        logo.sprite = image;
    }

    public Image GetIcon()
    {
        return logo;
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
