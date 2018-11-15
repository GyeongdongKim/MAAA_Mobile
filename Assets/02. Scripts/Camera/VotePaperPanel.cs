using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class VotePaperPanel : MonoBehaviour {

    private PhotonView pv;
    public FreeLookCam freeLookCam;
    public VoteRotate voteRotate;
    public Button[] buttons;

	void Start () {
        pv = freeLookCam.m_Target.GetComponent<PhotonView>();
	}
	
    public void OnClickVoted(GameObject button)
    {
        PhotonPlayer.Find(button.GetComponent<PlayerIDinButton>().id).AddScore(1);
        voteRotate.VoteOff();
    }

    public void InitVoteList()
    {
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = PhotonNetwork.playerList[i].NickName;
            buttons[i].GetComponent<PlayerIDinButton>().id = PhotonNetwork.playerList[i].ID;
        }
        for (int i = PhotonNetwork.room.PlayerCount; i < 15; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = "";
            buttons[i].GetComponent<Button>().interactable = false;
        }
    }
}
