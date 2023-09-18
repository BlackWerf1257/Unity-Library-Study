using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Cysharp.Threading.Tasks;

public class RadialUIOnOff : MonoBehaviour
{
    private InputAction inputMove;
    InputMaps inputAction;
    private RadicalMenu radMenu;

    private GameObject radialObj;
    [SerializeField] private GameObject radialObjPrefab;
    public bool isButtonOn;
    
    [Range(1,4)]
    [SerializeField] private int buttonCount = 4;
    
    // Start is called before the first frame update
    void Start()
    {
        inputAction = new InputMaps();
        inputMove =  inputAction.Test.Move;
        inputMove.Enable();
    }
    
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if(isButtonOn)
            radMenu.MoveVectorVoid(context);
         
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && context.interaction is HoldInteraction)
        {
            isButtonOn = true;
            Open();
        }
        if (context.canceled)
        {
            if (isButtonOn)
                Close();
        }
    }

    void Open()
    {
        radialObj = Instantiate(radialObjPrefab, this.transform.position, Quaternion.identity);
        radialObj.transform.SetParent(this.gameObject.transform);
        radialObj.GetComponent<RadicalMenu>().GetParentScript(this.GetComponent<RadialUIOnOff>());

        radMenu = radialObj.GetComponent<RadicalMenu>();
        radMenu.SetButtonCount(buttonCount);
    }

    public void Close()
    {
        Destroy(radialObj);
    }
}
