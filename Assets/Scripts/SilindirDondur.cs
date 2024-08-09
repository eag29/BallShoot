using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SilindirDondur : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject silindir;

    bool buttonPressed;
    [SerializeField] float aci;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
    void Update()
    {
        if (buttonPressed)
        {
            silindir.transform.Rotate(0,aci,0);
        }
    }
}