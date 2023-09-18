using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public class RadicalMenuEntry : MonoBehaviour, IPointerEnterHandler,IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private Image logo;
    [SerializeField] private RectTransform rect;
    [SerializeField] private TextMeshProUGUI currentWeaponTextObj;
    [SerializeField] private Image thisObj;
    [SerializeField] private float fadeTime = 10f;

    [SerializeField] private GameObject parentObj;
    private RadicalMenu parentScriptObj;
    private bool isButtonOn;
    
    [Header("선택한 버튼의 Index")]
    private int buttonIndex;

    [Tooltip("아이템의 이름")] 
    private string itemName;

    
    private void OnEnable()
    {
        parentScriptObj = parentObj.GetComponent<RadicalMenu>();
    }

    public string SetItemName(string name)
    {
        return itemName = name;
    }
    void SetIcon(Sprite image)
    {
        logo.sprite = image;
    }

    public void ButtonBool(bool boolVar)
    {
        isButtonOn = boolVar;
    }

    public void SetTextObj(TextMeshProUGUI text)
    {
        currentWeaponTextObj = text;
    }

    
    public void DoFadeIn(Sprite img)
    {
        SetIcon(img);
        thisObj.DOFade(1.0f, fadeTime).onComplete =
            delegate
            {
                logo.enabled = true;
                logo.DOFillAmount(1.0f, fadeTime);
                thisObj.DOFillAmount(1.0f, fadeTime);
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



    public void OnPointerDown(PointerEventData eventData)
    {
        currentWeaponTextObj.text = "Test"; 
            //label.text;
            DoFadeOut();
            //Destroy(parentObj);
            
        rect.DOComplete();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOComplete();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentWeaponTextObj.text = buttonIndex.ToString();
        parentScriptObj.CurrentIndexUpdate(buttonIndex);
        Debug.Log("Selected Index from Child Button : " + IndexFunc);
    }
}
