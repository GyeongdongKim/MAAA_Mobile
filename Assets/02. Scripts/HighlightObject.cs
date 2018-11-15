using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class HighlightObject : MonoBehaviour {

    #region variable
    public CinemachineVirtualCamera vCam;
    public GameObject uiObject;
    [HideInInspector] public bool mouseOver = false;
    #endregion
    
    private void OnMouseDown()
    {
        
        mouseOver = true;
        if (vCam.Priority == 0)
            vCam.Priority++;
        else if (vCam.Priority == 1)
        {
            vCam.Priority++;
            if (this.tag == "QUIT")
            {
                Application.Quit();
            }
            else
            {
                if (uiObject == null) return;
                uiObject.SetActive(true);
            }
        }
    }

    private void OnMouseExit()
    {
        if (vCam.Priority==1) mouseOver = false;
    }

    public void eventPointerExit()
    {
        mouseOver = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mouseOver && vCam.Priority == 2)
        {
            if (uiObject == null) return;
            uiObject.SetActive(false);
            vCam.Priority--;
        }
        else if (Input.GetMouseButtonDown(0) && !mouseOver && vCam.Priority == 1)
            vCam.Priority--;
        if (Input.GetKeyDown(KeyCode.Escape) && vCam.Priority == 2)
        {
            if (uiObject == null) return;
            uiObject.SetActive(false);
            vCam.Priority--;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && vCam.Priority == 1)
            vCam.Priority--;
    }
}
