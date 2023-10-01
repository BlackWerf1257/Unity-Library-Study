
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAim : MonoBehaviour
{
    //참고할 적
    [SerializeField] private Transform _enemy;
    // Start is called before the first frame update
    private InputMaps _inputMaps;
    void Start()
    {
        _inputMaps = new InputMaps();
        _inputMaps.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //계산 각  목적지 - 소스
        Vector2 mousePos = _inputMaps.Test.Move.ReadValue<Vector2>();
        //Inverse 탄젠트 이용해 각 계산
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f;

        if (angle < 0)
            angle += 360f;
        
        
        Debug.Log("Angle : " + angle);
        //Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        //특정 축 따라 각도 이용해 회전 
        //transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);

        //현재 각도에서 특정 각으로 선형보강
        transform.eulerAngles = Vector3.forward * angle;
        //Or 현재 euler 각 구해서, 새 각 더하기
    }
}
