using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {

    public bool isUIHover = false;

    private void Start()
    {
        if (this.gameObject.tag == "CAM")        
            GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>().NoteHover(this);
        if(this.gameObject.tag=="VOTEPAPER")
            GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>().VoteHover(this);
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