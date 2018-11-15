using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public class ElevatorExit : MonoBehaviour {

    public GameObject exitUI;
    public GameObject crossHair;
    public GameObject moniter;
    public RigidbodyFirstPersonController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            StartCoroutine(Exit());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(Exit());
            crossHair.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!moniter.GetActive())
        {
            if (controller.enabled)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controller.enabled = false;
                crossHair.SetActive(false);
            }
            else
            {
                exitUI.SetActive(true); 
            }
        }
        if (Input.GetMouseButtonDown(0)&&!controller.enabled&&!exitUI.GetActive()&&!moniter.GetActive())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            controller.enabled = true;
            crossHair.SetActive(true);
        }
    }

    public void ClickYes()
    {
        Application.Quit();
    }

    public void ClickNo()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        exitUI.SetActive(false);
        crossHair.SetActive(true);
        controller.enabled = true;
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(1.0f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        exitUI.SetActive(true);
        crossHair.SetActive(false);
        controller.enabled = false;
    }
}
