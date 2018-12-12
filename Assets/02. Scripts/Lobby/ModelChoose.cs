using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class ModelChoose : MonoBehaviour {

    public Transform[] models;
    public GameObject bottomIcons,startButton;
    public Light[] lightObjects;
    public CinemachineVirtualCamera lobbyCam;
    public CinemachineVirtualCamera[] vCams;
    public GameObject buttonleft, buttonright,buttonreturn;
    [HideInInspector]public int modelIndex=0;
    private bool toggle=true;
    private bool chooseOn = false;

    // Use this for initialization
	void Start () {
        SetAnimate();
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

    void SetAnimate()
    {
        models[0].GetComponent<Animator>().SetBool("isBboy",true);
        models[1].GetComponent<Animator>().SetBool("isLean", true);
        models[2].GetComponent<Animator>().SetBool("isGolf", true);
        models[3].GetComponent<Animator>().SetBool("isMaleStanding2", true);
        models[4].GetComponent<Animator>().SetBool("isMaleStanding4", true);
        models[5].GetComponent<Animator>().SetBool("isFemaleSitting", true);
        models[6].GetComponent<Animator>().SetBool("isSitTalk", true);
        models[7].GetComponent<Animator>().SetBool("isSitting", true);
        models[8].GetComponent<Animator>().SetBool("isSinging", true);
        models[9].GetComponent<Animator>().SetBool("isMaleStanding", true);

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
        LightControl(true);
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
        LightControl(false);
    }

    void LightControl(bool isTurn)
    {
        for (int i = 0; i < lightObjects.Length; i++)
        {
            if (isTurn)
            {
                lightObjects[i].intensity = 1;
            }
            else
            {
                lightObjects[i].intensity = 0;
            }
        }
    }

    void ChangeTarget(bool isRight)
    {
        vCams[modelIndex].Priority = 0;
        if (isRight)
        {
            if (modelIndex == 9)
                modelIndex = 0;
            else
                modelIndex++;
        }
        else
        {
            if (modelIndex == 0)
                modelIndex = 9;
            else
                modelIndex--;
        }
        vCams[modelIndex].Priority = 1;

    }

    public void OnClickLeftButton()
    {
        //StartCoroutine(ChangeTarget(false));
        ChangeTarget(false);
    }

    public void OnClickRightButton()
    {
        //StartCoroutine(ChangeTarget(true));
        ChangeTarget(true);
    }

    /*IEnumerator ChangeTarget(bool Right)
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
    }*/
}
