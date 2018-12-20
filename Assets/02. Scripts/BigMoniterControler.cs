using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigMoniterControler : MonoBehaviour
{


    [System.Serializable]
    public class PlayerText
    {
        public Text playerName;
        public GameObject dujul, skeleton;
    }

    public List<PlayerText> playerTexts;
    
    public DayNightController daynightController;
    public Slider slider;
    public Text dayText;
    private void Awake()
    {
    }
    private void Update()
    {
        if (daynightController.enabled)
            slider.value = daynightController.currentTimeOfDay;
    }
    public void NoteUpdate()
    {
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {
            if (PhotonNetwork.playerList[i].CustomProperties["Death"] != null)
            {
                if ((bool)PhotonNetwork.playerList[i].CustomProperties["Death"])
                {
                    playerTexts[i].playerName.text = PhotonNetwork.playerList[i].NickName;
                    playerTexts[i].skeleton.SetActive(true);
                    playerTexts[i].dujul.SetActive(true);
                }
                else
                    playerTexts[i].playerName.text = PhotonNetwork.playerList[i].NickName;
            } else
                playerTexts[i].playerName.text = PhotonNetwork.playerList[i].NickName;
        }
        for (int i = PhotonNetwork.room.PlayerCount; i < 8; i++)
        {
            playerTexts[i].playerName.text = "-";
        }
    }
}