using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomLobbyManager : MonoBehaviour {
    private PhotonView pv;
    private bool canStart = false;
    public GameObject warningText;
    public GameObject startButton;

    [System.Serializable]
    public class PlayerItem
    {
        public Text name;
    }
    public List<PlayerItem> playerItems;
    
    void Start () {
        pv = GetComponent<PhotonView>();
	}

    void Update()
    {
        if (!PhotonNetwork.player.IsMasterClient)
            startButton.SetActive(false);
    }

    public void ClickButtonAndRefreshList()
    {
        StartCoroutine(RefreshList());
    }

    public void OnClickExitRoom()
    {
        StopCoroutine(RefreshList());
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickStartButton()
    {
        if (canStart)
            pv.RPC("LoadBattleField", PhotonTargets.All);
        else
            StartCoroutine(Warning());
    }
    
    IEnumerator Warning()
    {
        warningText.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        warningText.SetActive(false);
    }

    [PunRPC]
    public void LoadBattleField()
    {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("Stage");
    }
    
    IEnumerator RefreshList()
    {
        PhotonPlayer[] playerList = PhotonNetwork.playerList;
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (i < playerList.Length)
            {
                playerItems[i].name.GetComponent<Text>().text = playerList[i].NickName;
            }
            else
                playerItems[i].name.GetComponent<Text>().text = "";
        }
        if (playerList.Length >= 8)
            canStart = true;
        else
            canStart = false;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(RefreshList());
    }
}
