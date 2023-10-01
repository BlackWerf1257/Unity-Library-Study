using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

public class RadicalMenu : MonoBehaviour
{
    [Tooltip("생성된 오브젝트 저장용")]    
    private List<GameObject> weaponObject = new List<GameObject>();
    private List<RectTransform> weaponObjRect = new List<RectTransform>();

    [SerializeField] private TextMeshProUGUI currentSelectedWeaponText;
    
    private List<RadicalMenuEntry> entries;

    private float angle;

    public bool isButtonOn
    {
        get;
        set;
    }= false;

    [Header("무기 선택 버튼 UI")] 
    [SerializeField]  private GameObject[] ButtonItemObj;

    
    [Header("무기 아이콘")]
    [SerializeField] private List<Sprite> imageList;

    [SerializeField] private float radius = 300f;
    
    int buttonCount;

    [SerializeField] private RectTransform arrowHolder, arrowObj;

    private RadialUIOnOff radialUIOnOff;
    
    
    InputMaps inputAction;
    private InputAction inputMove;
    //private float angle = 0;
    private int selectedIndex;
    [Tooltip("각 버튼별 간격")]
    private int buttonAngle;


    public int SetButtonCount(int count)
    {
        return buttonCount = count;
    }

    public static Action closeAction;
    

    // Start is called before the first frame update
    void Start()
    {
        isButtonOn = true;
        arrowObj.localPosition = new Vector3(0, radius * .65f, 0);
        entries = new List<RadicalMenuEntry>();

        Open();
        
        buttonAngle = 360 / buttonCount;
        closeAction += delegate { Close(); };
    }


    public void GetParentScript(RadialUIOnOff script)
    {
        radialUIOnOff = script;
    }
    

    void AddEntry(string pLabel, Sprite img, int objectIndex)
    {
        weaponObject.Add(Instantiate(ButtonItemObj[objectIndex], this.transform));
        
        RadicalMenuEntry rme = weaponObject[objectIndex].GetComponent<RadicalMenuEntry>();
        
        rme.DoFadeIn(img);
        rme.SetTextObj(currentSelectedWeaponText);
        rme.IndexFunc = objectIndex; 

        rme.SetItemName(pLabel);
        entries.Add(rme);
        
        weaponObjRect.Add(weaponObject[objectIndex].GetComponent<RectTransform>());
        
        

    }


    void Open()
    {
            if (gameObject.activeSelf)
            {
                isButtonOn = true;
                for (int i = 0; i < buttonCount; i++)
                    AddEntry("Button" + i.ToString(), imageList[i], i);

                ReArrange();
            }
    }
    public void Close()
    {
        
            for (int i = 0; i < buttonCount; i++)
            {
                if (entries[i] != null)
                    Destroy(entries[i].gameObject);
            }
            isButtonOn = false;
    }


    public int CurrentIndexUpdate(int index)
    {
        return selectedIndex = index;
    }

 

    void ReArrange()
    {
        switch (entries.Count)
        {
            case 1:
            {
                weaponObjRect[0].anchoredPosition = this.gameObject.transform.localPosition;
            } break;
            case 2:
            {
                weaponObjRect[0].anchoredPosition = new Vector2(radius, 0);
                weaponObjRect[1].anchoredPosition = new Vector2(-radius, 0);
            } break;
            case 3:
            {
                weaponObjRect[0].anchoredPosition = new Vector2(radius, radius);
                weaponObjRect[1].anchoredPosition = new Vector2(-radius, radius);
                weaponObjRect[2].anchoredPosition = new Vector2(0, -radius);
            } break;
            case 4:
            {
                weaponObjRect[0].anchoredPosition = new Vector2(radius, radius);
                weaponObjRect[1].anchoredPosition = new Vector2(-radius, radius);
                weaponObjRect[2].anchoredPosition = new Vector2(-radius, -radius);
                weaponObjRect[3].anchoredPosition = new Vector2(radius, -radius);
            } break;
        }
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        Debug.Log(selectedIndex);
    }
   

    public void MoveVectorVoid(InputAction.CallbackContext context)
    {
        if (radialUIOnOff)
        {
            //계산 각  목적지 - 소스
            Vector2 mousePos = context.ReadValue<Vector2>(); // - (Vector2)this.transform.position;
            //Inverse 탄젠트 이용해 각 계산
            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f;

            if (angle < 0)
                angle += 360f;

            //현재 각도에서 특정 각으로 선형보강
            arrowHolder.transform.eulerAngles = Vector3.forward * angle;

            AngleToIndex(angle);
        }
        ArrowRotate();
    }
    public void MouseToAngle(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(context.ReadValue<Vector2>());
        // 스크린의 중앙을 (0, 0)으로 하는 마우스 좌표(-0.5 ~ 0.5 범위)
        Vector2 centerMousePos = new Vector2(mousePos.x - 0.5f, mousePos.y - 0.5f);
        
        float mouseDist = new Vector2(centerMousePos.x * Screen.width / Screen.height, centerMousePos.y).magnitude;

        if (mouseDist > .01f)
        {
            angle = Mathf.Atan2(centerMousePos.y, centerMousePos.x) * Mathf.Rad2Deg - 90f;

            
            if (angle < 0)
                angle += 360f;
            AngleToIndex(angle);
        }
        ArrowRotate();
    }


    void AngleToIndex(float angle)
    {
            selectedIndex = (int)(angle / buttonAngle);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, buttonCount - 1);

            currentSelectedWeaponText.text = entries[selectedIndex].GetItemName();
    }

    void ArrowRotate()
    {
        arrowHolder.eulerAngles = Vector3.forward * angle;
    }
}
