using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool isButtonOn = false;

    [Header("무기 선택 버튼 UI")] 
    [SerializeField]  private GameObject[] ButtonItemObj;

    
    [Header("무기 아이콘")]
    [SerializeField] private List<Sprite> imageList;

    [SerializeField] private float radius = 300f;
    
    int buttonCount;

    [SerializeField] private Transform arrowObj;

    private RadialUIOnOff radialUIOnOff;
    
    
    InputMaps inputAction;
    private InputAction inputMove;
    private float angle = 0;
    private int selectedIndex;
    [Tooltip("각 버튼별 간격")]
    private int buttonAngle;

    private Vector2 radlength, moveLocalPos;
    private Vector2 thisVec2Pos;


    public int SetButtonCount(int count)
    {
        return buttonCount = count;
    }

    // Start is called before the first frame update
    void Start()
    {
        entries = new List<RadicalMenuEntry>();
        radlength = new Vector2(0, radius / 2);
        moveLocalPos = (Vector2)this.transform.localPosition + radlength;

        ArrowTransVoid().Forget();
        MouseAngleVoid().Forget();
        Open();
        
        buttonAngle = 360 / buttonCount;

        thisVec2Pos = this.transform.localPosition;
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
            for (int i = 0; i < buttonCount; i++)
                AddEntry("Button" + i.ToString(), imageList[i], i);

            isButtonOn = true;

            ReArrange();
    }
    void Close()
    {
        if (radialUIOnOff.isButtonOn)
        {
            for (int i = 0; i < buttonCount; i++)
            {
                RadicalMenuEntry rme = weaponObject[i].GetComponent<RadicalMenuEntry>();

                entries[i].DoFadeOut();
            }

            isButtonOn = false;

            entries.Clear();
            weaponObject.Clear();
        }
    }

    public void Toggle()
    {
        if(entries.Count != 0)
            Close();
    }

    public int CurrentIndexUpdate(int index)
    {
        Debug.Log("Selected Index: " + index);
        return selectedIndex = index;
    }

 

    void ReArrange()
    {
        /*float radiansOfSeperation = (Mathf.PI * 2) / entries.Count;
        for (int i = 0; i < entries.Count; i++)
        {
            float x = Mathf.Sin(radiansOfSeperation * i) * radius;
            float y = Mathf.Cos(radiansOfSeperation * i) * radius;

            entries[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }*/

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
         angle = Mathf.Atan2(context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360;
        
        
        Debug.Log("Move Angle : " + angle);
        AngleToIndex();
    }

    async UniTaskVoid ArrowTransVoid()
    {
        var token = this.GetCancellationTokenOnDestroy();
        while (true)
        {
            arrowObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            arrowObj.transform.localPosition = moveLocalPos;
            
            //arrowObj.eulerAngles = Vector3.forward * angle;
            //arrowObj.localPosition = Vector3.zero + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (radius / 2);
            await UniTask.Delay(100);
            await UniTask.Delay(100, cancellationToken: token);

        }
    }

    async UniTaskVoid MouseAngleVoid()
    {
        var token = this.GetCancellationTokenOnDestroy();
        while (true)
        {
            angle = Mathf.Atan2(Input.mousePosition.y - this.transform.position.y,
                Input.mousePosition.x - this.transform.position.x) * Mathf.Rad2Deg;
            
            if (angle < 0)
                angle += 360;
            
            await UniTask.Delay(10);
            
            await UniTask.Delay(100, cancellationToken: token);
        }
    }
    void AngleToIndex()
    {
            selectedIndex = Mathf.RoundToInt(angle / buttonAngle);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, buttonCount - 1);

            currentSelectedWeaponText.text = selectedIndex.ToString();
            //Debug.Log("Selected Index from Menu Script : " + selectedIndex);
    }

    private void Update()
    {
        AngleToIndex();
    }
}
