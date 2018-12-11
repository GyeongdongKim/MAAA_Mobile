using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class ModelChoose : MonoBehaviour {

    public Transform[] models;
    public GameObject bottomIcons,startButton;
    public CinemachineVirtualCamera vCam1,vCam2,lobbyCam;
    public GameObject buttonleft, buttonright,buttonreturn;
    [HideInInspector]public int modelIndex=0;
    private bool toggle=true;
    private bool chooseOn = false;

    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (chooseOn)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                OnClickOut();
            }
        }
	}

    public void OnClickRandomGame()
    {
        chooseOn = true;
        lobbyCam.Priority = -1;
        buttonleft.SetActive(true);
        buttonright.SetActive(true);
        buttonreturn.SetActive(true);
        bottomIcons.transform.DOLocalMoveY(-670f, 1);
        startButton.transform.DOLocalMoveY(-460f, 1);
    }

    public void OnClickOut()
    {
        chooseOn = false;
        lobbyCam.Priority = 10;
        buttonleft.SetActive(false);
        buttonright.SetActive(false);
        buttonreturn.SetActive(false);
        bottomIcons.transform.DOLocalMoveY(-415f, 1);
        startButton.transform.DOLocalMoveY(-620f, 1);
    }

    IEnumerator ChangeTarget(bool Right)
    {
        toggle = !toggle;
        if (Right)
        {
            if (modelIndex == models.Length-1)
                modelIndex = 0;
            else
                modelIndex++;
            if (toggle)
            {
                vCam1.Follow = models[modelIndex];
                vCam1.LookAt = models[modelIndex];
                vCam1.Priority = 1;
                vCam2.Priority = 0;
                buttonleft.SetActive(false);buttonright.SetActive(false);
                yield return new WaitForSeconds(1.0f);
                buttonleft.SetActive(true); buttonright.SetActive(true);
                
                StopCoroutine("ChangeTarget");
            }
            else
            {
                vCam2.Follow = models[modelIndex];
                vCam2.LookAt = models[modelIndex];
                vCam1.Priority = 0;
                vCam2.Priority = 1;
                buttonleft.SetActive(false); buttonright.SetActive(false);
                yield return new WaitForSeconds(1.0f);
                buttonleft.SetActive(true); buttonright.SetActive(true);
                StopCoroutine("ChangeTarget");
            }
        }
        else
        {
            if (modelIndex == 0)
            {
                modelIndex = models.Length - 1;
            }
            else
                modelIndex--;

            if (toggle)
            {
                vCam1.Follow = models[modelIndex];
                vCam1.LookAt = models[modelIndex];
                vCam1.Priority = 1;
                vCam2.Priority = 0;
                buttonleft.SetActive(false); buttonright.SetActive(false);
                yield return new WaitForSeconds(1.0f);
                buttonleft.SetActive(true); buttonright.SetActive(true);
                StopCoroutine("ChangeTarget");
            }
            else
            {
                vCam2.Follow = models[modelIndex];
                vCam2.LookAt = models[modelIndex];
                vCam1.Priority =0;
                vCam2.Priority = 1;
                buttonleft.SetActive(false); buttonright.SetActive(false);
                yield return new WaitForSeconds(1.0f);
                buttonleft.SetActive(true); buttonright.SetActive(true);
                StopCoroutine("ChangeTarget");
            }
        }
    }

    public void OnClickLeftButton()
    {
        StartCoroutine(ChangeTarget(false));
        //ChangeTarget(false);
    }

    public void OnClickRightButton()
    {
        StartCoroutine(ChangeTarget(true));
        //ChangeTarget(true);
    }
}
