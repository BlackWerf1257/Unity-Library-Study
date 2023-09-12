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
    public InputActionAsset ply;
    private bool isButtonOn = false;

    [Header("무기 선택 버튼 UI")] 
    [SerializeField]  private GameObject[] ButtonItemObj;


    [Header("무기 아이콘")]
    [SerializeField] private List<Sprite> imageList;

    [SerializeField] private float radius = 300f;
    [Range(1,4)]
    [SerializeField] private int buttonCount;

    private int selectedIndex;


    // Start is called before the first frame update
    void Start()
    {
        entries = new List<RadicalMenuEntry>();
    }

    void AddEntry(string pLabel, Sprite img, int objectIndex)
    {
        weaponObject.Add(Instantiate(ButtonItemObj[objectIndex], this.transform));
        
        RadicalMenuEntry rme = weaponObject[objectIndex].GetComponent<RadicalMenuEntry>();
        
        rme.DoFadeIn(img);
        rme.SetTextObj(currentSelectedWeaponText);
        rme.IndexFunc = objectIndex;

        rme.SetLabel(pLabel);
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
        for (int i = 0; i < buttonCount; i++)
        {
            RadicalMenuEntry rme = weaponObject[i].GetComponent<RadicalMenuEntry>();
            
            entries[i].DoFadeOut();
        }

        isButtonOn = false;
        
        entries.Clear();
        weaponObject.Clear();
    }

    public void Toggle()
    {
        if(entries.Count != 0)
            Close();
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


    private void Update()
    {
        ply.FindAction("Fire").performed +=
            context =>
            {
                if(context.interaction is HoldInteraction)
                    Open();
            };
        ply.FindAction("Fire").canceled +=
            context =>
            {
                if (isButtonOn)
                    Close();
            };

        ply.FindAction("Confirm").performed +=
            context =>
            {
                Debug.Log(selectedIndex);
            };
    }
}
