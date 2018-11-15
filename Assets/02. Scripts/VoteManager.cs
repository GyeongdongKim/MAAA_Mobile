using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteManager : MonoBehaviour {

    private PhotonView pv;
    public PhotonPlayer[] votePlayers = new PhotonPlayer[15];
    public GameObject killButton;
    private PhotonPlayer votedPlayer;
    private DayNightController dayNightController;
    [HideInInspector]public bool trigger1=false,trigger2=false;
    

    void Start() {
        pv = GetComponent<PhotonView>();
        if(PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                votePlayers[i] = PhotonNetwork.playerList[i];
            }
        }
        dayNightController = GetComponent<DayNightController>();
    }
    public void WhoIsVoted()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if (i == 0)
                votedPlayer = votePlayers[i];
            else
                if (votedPlayer.GetScore() < votePlayers[i].GetScore())
                    votedPlayer = votePlayers[i];
        }
        if (votedPlayer.GetScore() == 0)
            votedPlayer = null;
    }

    private void Update()
    {
        if (dayNightController.currentTimeOfDay > 25f/36f && !trigger1)
        {
            if (PhotonNetwork.isMasterClient)
            {
                WhoIsVoted();
                if(votedPlayer==null)
                {
                    return;
                }
                else
                {
                    //GetComponent<GameManager>().localPlayer.GetPhotonView().RPC("Execution", votedPlayer,votedPlayer.ID);
                    pv.RPC("Execution", votedPlayer);
                    Debug.Log(votedPlayer.NickName + " execution / getscore : " + votedPlayer.GetScore());
                }                
            }
            trigger1 = true;
        }
        if (dayNightController.currentTimeOfDay > 26f / 36f && !trigger2)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (votedPlayer == null)
                    return;
                else
                    pv.RPC("ButtonActive", PhotonTargets.All, votedPlayer.ID);
            }
            trigger2 = true;
        }
    }

    public void OnClickKillButton(GameObject button)
    {
        //GetComponent<GameManager>().localPlayer.GetPhotonView().RPC("Chanban", votedPlayer, votedPlayer.ID);
        pv.RPC("Chanban", votedPlayer, votedPlayer.ID);
        button.SetActive(false);
        button.GetComponent<MouseHover>().isUIHover = false;
    }

    [PunRPC]
    public void ButtonActive(int player)
    {
        killButton.SetActive(true);
        votedPlayer = PhotonPlayer.Find(player);
        Debug.Log(votedPlayer.NickName +" is Execution Voted");
    }
    
}
