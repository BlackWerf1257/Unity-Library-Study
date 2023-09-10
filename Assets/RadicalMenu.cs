using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//https://youtu.be/tdkdRguH_dE?si=c-59g8Ipp6J-mFGS
public class RadicalMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponUiSprite; 
    
    private List<RadicalMenuEntry> entries;
    [Tooltip("dawdwa")]
    [SerializeField] GameObject entryPrefab;


    [SerializeField] private List<Sprite> imageList;

    [SerializeField] private float radius = 300f;
    [SerializeField] private int buttonCount;

    public void SetButtonCount(int buttonCount)
    {
        this.buttonCount = buttonCount;
    }


    
    // Start is called before the first frame update
    void Start()
    {
        entries = new List<RadicalMenuEntry>();
    }

    void AddEntry(string pLabel, Sprite img)
    {
        GameObject entry = Instantiate(entryPrefab, this.transform);
        
        RadicalMenuEntry rme = entry.GetComponent<RadicalMenuEntry>();

        rme.GetComponent<Image>().DOFade(1.0f, 1f);

        
        rme.SetLabel(pLabel);
        rme.SetIcon(img);
        
        entries.Add(rme);
    }

    void Open()
    {
            for (int i = 0; i < buttonCount; i++)
                AddEntry("Button" + i.ToString(), imageList[i]);

            ReArrange();
    }
    void Close()
    {
        for (int i = 0; i < buttonCount; i++)
        {
            Image rect = entries[i].GetComponent<Image>();
            GameObject entry = entries[i].gameObject;
            rect.DOFade(0.0f, 1f).onComplete =
                delegate
                {
                    Destroy(entry);
                };
        }
        
        entries.Clear();
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
