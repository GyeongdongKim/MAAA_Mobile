using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteRotate : MonoBehaviour
{

    public GameObject votePaper;
    private Animator animator;
    private bool voteOff = true;
    private int temp;
    private GameManager gameManager;
    public Text voteTimer;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();
    }

    #region Vote
    public void VoteOn()
    {
        gameManager.CursorOn();
        votePaper.SetActive(true);
        votePaper.GetComponentInChildren<VotePaperPanel>().InitVoteList();
        animator.SetBool("VoteOn", true);
        voteOff = false;
    }
    public void VoteOff()
    {
        gameManager.CursorOff();
        animator.SetBool("VoteOn", false);
        voteOff = true;
    }

    public void VoteDeactive()
    {
        votePaper.SetActive(false);
    }
    public void VoteActive()
    {
        votePaper.SetActive(true);
    }

    public void VoteTrigger()
    {
        if (PhotonNetwork.player.IsLocal)
        {
            VoteOn();
            //temp = 10;
            //StartCoroutine(VoteStart());
        }        
    }

    /*IEnumerator VoteStart()
    {
        if (temp != 0 && !voteOff)
        {
            temp--;
            voteTimer.text = temp.ToString();
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(VoteStart());
        }
        else
        {
            VoteOff();
        }
    }*/
    #endregion
}
