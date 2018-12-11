using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorNotePanel : MonoBehaviour {

    public PhotonView pv;
    [System.Serializable]
    public class Players
    {
        public Text playerName;
    }
    public List<Players> players;
    public NoteRotate noteRotate;

    private void Start()
    {
    }

    public void OnClickPlayerName(Text playerName)
    {
        FindObjectOfType<GameManager>().playerToSurvive=playerName.text;
        noteRotate.NoteOff();
    }

    public void NoSelectPlayer()
    {
        FindObjectOfType<GameManager>().playerToSurvive = PhotonNetwork.player.NickName;
    }

    public void InitNoteList()
    {
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {
            if (!PhotonNetwork.playerList[i].IsLocal)
            {
                players[i].playerName.text = PhotonNetwork.playerList[i].NickName;
                if ((bool)PhotonNetwork.playerList[i].CustomProperties["Death"])
                {
                    players[i].playerName.GetComponent<ButtonActiveController>().dujul.SetActive(true);
                    players[i].playerName.GetComponent<ButtonActiveController>().ButtonDisabled();
                }
            }
        }
        for (int i = 0; i < 14; i++)
        {
            if(players[i].playerName.text=="New Text")
                players[i].playerName.text = "";
        }
        if (PhotonNetwork.playerList.Length == 15)
        {
            for (int i = 0; i < 14; i++)
            {
                if (players[i].playerName.text.Length < 1 ||players[i].playerName.text=="New Text")
                {
                    players[i].playerName.text = PhotonNetwork.playerList[14].NickName;
                    break;
                }
            }
        }
        players[14].playerName.text = PhotonNetwork.player.NickName;
        for (int i = 0; i < 14; i++)
        {
            GetComponentsInChildren<ButtonActiveController>()[i].ButtonControl();
        }
    }
}
