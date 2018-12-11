using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {

    public bool isUIHover = false;

    private void Start()
    {
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isUIHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isUIHover = false;
    }
}