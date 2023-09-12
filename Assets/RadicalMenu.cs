using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//https://youtu.be/tdkdRguH_dE?si=c-59g8Ipp6J-mFGS
public class RadicalMenu : MonoBehaviour
{
    [Tooltip("생성된 오브젝트 저장용")]    
    private List<GameObject> weaponObject = new List<GameObject>();
    
    private List<RadicalMenuEntry> entries;

    [Header("무기 선택 버튼 UI")] 
    [SerializeField]  private GameObject[] ButtonItemObj;


    [Header("무기 아이콘")]
    [SerializeField] private List<Sprite> imageList;

    [SerializeField] private float radius = 300f;
    [SerializeField] private int buttonCount;


    // Start is called before the first frame update
    void Start()
    {
        entries = new List<RadicalMenuEntry>();
    }

    void AddEntry(string pLabel, Sprite img, int objectIndex)
    {
        //weaponObject.Add(Instantiate(entryPrefab, this.transform));
        weaponObject.Add(Instantiate(ButtonItemObj[objectIndex], this.transform));
        
        RadicalMenuEntry rme = weaponObject[objectIndex].GetComponent<RadicalMenuEntry>();
        
        rme.DoFadeIn(img);

        rme.SetLabel(pLabel);
        entries.Add(rme);
    }


    void Open()
    {
            for (int i = 0; i < buttonCount; i++)
                AddEntry("Button" + i.ToString(), imageList[i], i);

            ReArrange();
    }
    void Close()
    {
        for (int i = 0; i < buttonCount; i++)
        {
            RadicalMenuEntry rme = weaponObject[i].GetComponent<RadicalMenuEntry>();
            
            entries[i].DoFadeOut();
        }
        
        entries.Clear();
        weaponObject.Clear();
    }

    public void Toggle()
    {
        if(entries.Count == 0)
            Open();
        else
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
}
