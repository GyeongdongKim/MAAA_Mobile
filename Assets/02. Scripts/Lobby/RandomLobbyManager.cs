using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomLobbyManager : Photon.PunBehaviour{
    private PhotonView pv;
    private bool isStart=false;
    private bool isCount = false;
    public Text textCount;
    public int minimumPlayer;
    public int waitTime;
    public GameObject black;
    [System.Serializable]
    public class PlayerItem
    {
        public Text name;
    }
    public List<PlayerItem> playerItems;

    void Start() {
        pv = GetComponent<PhotonView>();
        textCount.text = "";
    }

    void Update() {

    }
    
    public void ClickButtonAndRefreshList()
    {
        StartCoroutine(RefreshList());
    }

    public void OnClickExitRoom()
    {
        StopCoroutine(RefreshList());
        this.gameObject.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        Debug.Log("OnPPConnected");
        //RefreshList();
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
        Debug.Log("OnPPDisconnected");
        //RefreshList();
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("OnCreatedRoom");
        //RefreshList();
    }

    IEnumerator RefreshList()
    {
        if (PhotonNetwork.room != null)
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
            //if (playerList.Length >= minimumPlayer && PhotonNetwork.player.IsMasterClient&&!isStart) // && !isCount
            if (playerList.Length >= minimumPlayer)
            {
                if (!isStart)
                {
                    //pv.RPC("StartCount", PhotonTargets.All);
                    Debug.Log("isStart" + isStart);
                    isStart = true;
                    StartCount();
                    //isCount = false;
                }
            }
            else
            {
                isStart = false;
                //isCount = true;
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RefreshList());
    }

    //[PunRPC]
    public void StartCount()
    {
        if (isStart)
        {
            StartCoroutine(Count());
        }
        else
        {
            StopCoroutine(Count());
        }
    }

    IEnumerator Count()
    {
        textCount.text = "Now Loading...";
        yield return new WaitForSeconds(waitTime);
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.room.IsOpen = false;
            pv.RPC("LoadBattleField", PhotonTargets.All);
        }            
    }

    [PunRPC]
    public void LoadBattleField()
    {
        black.SetActive(true);
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("Stage");
    }
}
