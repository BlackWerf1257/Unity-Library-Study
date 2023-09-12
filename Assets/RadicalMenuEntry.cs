using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public class RadicalMenuEntry : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image logo;
    [SerializeField] private RectTransform rect;
    [SerializeField] private TextMeshProUGUI currentWeaponTextObj;
    [SerializeField] private Image thisObj;
    [SerializeField] private float fadeTime = 0.1f;
    
    [Header("선택한 버튼의 Index")]
    private int buttonIndex;

    
    private void OnEnable()
    {
        //currentWeaponTextObj = GameObject.FindGameObjectWithTag("PieUIText").GetComponent<TextMeshProUGUI>();
    }

    public void SetLabel(string pText)
    {
        label.text = pText;
    }

    void SetIcon(Sprite image)
    {
        logo.sprite = image;
    }

    public void SetTextObj(TextMeshProUGUI text)
    {
        currentWeaponTextObj = text;
    }

    
    public void DoFadeIn(Sprite img)
    {
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

    public int IndexFunc
    {
        get { return buttonIndex; }
        set { buttonIndex = value; }
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        currentWeaponTextObj.text = "Test"; 
            //label.text;
        rect.DOComplete();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOComplete();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentWeaponTextObj.text = buttonIndex.ToString();
    }
}
