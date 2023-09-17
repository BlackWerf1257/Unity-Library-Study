using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class RadicalMenu : MonoBehaviour
{
    [Tooltip("생성된 오브젝트 저장용")]    
    private List<GameObject> weaponObject = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI currentSelectedWeaponText;
    
    private List<RadicalMenuEntry> entries;
    private bool isButtonOn = false;

    [Header("무기 선택 버튼 UI")] 
    [SerializeField]  private GameObject[] ButtonItemObj;

    
    [Header("무기 아이콘")]
    [SerializeField] private List<Sprite> imageList;

    [SerializeField] private float radius = 300f;
    [Range(1,4)]
    [SerializeField] private int buttonCount;

    [SerializeField] private Transform arrowObj;

    private RadialUIOnOff radialUIOnOff;
    
    
    InputMaps inputAction;
    private InputAction inputMove;
    private float angle = 0;
    private int selectedIndex;
    [Tooltip("각 버튼별 간격")]
    private int buttonAngle;


    // Start is called before the first frame update
    void Start()
    {
        entries = new List<RadicalMenuEntry>();

        /*inputAction = new InputMaps();
        inputMove =  inputAction.Test.Move;
        inputMove.Enable();*/

        Open();
        
        buttonAngle = 360 / buttonCount;
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
        float radiansOfSeperation = (Mathf.PI * 2) / entries.Count;
        for (int i = 0; i < entries.Count; i++)
        {
            float x = Mathf.Sin(radiansOfSeperation * i) * radius;
            float y = Mathf.Cos(radiansOfSeperation * i) * radius;

            entries[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
    }

    /*public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && context.interaction is HoldInteraction)
        {
                Open();
        }
        
        if (context.canceled)
        {
            if (isButtonOn)
                Close();
        }
    }*/

    public void OnConfirm(InputAction.CallbackContext context)
    {
        Debug.Log(selectedIndex);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        angle = Mathf.Atan2 (context.ReadValue<Vector2>().y, inputMove.ReadValue<Vector2>().x) * Mathf.Rad2Deg;
        if (angle < 0)  angle += 360;
        angle -= 90;
        
        selectedIndex = Mathf.RoundToInt(angle / buttonAngle);
        selectedIndex = Mathf.Clamp(selectedIndex, 0, buttonCount - 1);
        
        Debug.Log("Selected Index from Menu Script : " + selectedIndex);
    }

    private void Update()
    {
        arrowObj.eulerAngles = Vector3.forward * angle;
    }
}
