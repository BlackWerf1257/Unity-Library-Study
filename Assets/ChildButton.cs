using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChildButton : MonoBehaviour
{
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        EventTrigger et;
        if (gameObject.GetComponent<EventTrigger>() == null)
        {
            et = gameObject.AddComponent<EventTrigger>();
            et.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
        }
        else
            et = gameObject.GetComponent<EventTrigger>();
        
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener((eventData) => { Output(); });
        
        
        et.triggers.Add(enter);
    }

    void Output()
    {
        Debug.Log("dw");
    }

}
