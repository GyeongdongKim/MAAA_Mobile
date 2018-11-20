using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using PlayFab;
using PlayFab.ClientModels;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using EasyMobile;

public class LobbyManager : Photon.PunBehaviour {

    [SerializeField] private string GameVersion = "0.1";

    [HideInInspector] public string userId;
    public Text playerNameText;
   
    private GameObject target;
    public GameObject moniter;
    public GameObject crossHair;
    public GameObject customLobby;
    public GameObject randomLobby;
    public GameObject customRoomList;
    public GameObject exitUI;
    public GameObject panelBlack, panelSplash;
    public RectTransform playerContents;
    public RigidbodyFirstPersonController controller;
    private string _playFabPlayerIdCache;
    private UserProfileManager userProfileManager;
    TypedLobby randomLobbyType = new TypedLobby("Random", LobbyType.Default);
    
    private void Awake()
    {
        //게임 버전 확인 후 네트워크 연결
        if (!PhotonNetwork.connected)
        {
            GameServices.Init();
            //PhotonNetwork.ConnectUsingSettings(GameVersion);
            StartCoroutine(NameSet());
        }
        else { panelBlack.SetActive(false); }
        userProfileManager = GetComponent<UserProfileManager>();
        PhotonNetwork.JoinLobby(randomLobbyType);
    }
    IEnumerator NameSet()
    {
        while (!GameServices.IsInitialized()||!PhotonNetwork.connected)
            yield return null;
        panelBlack.SetActive(false);
        panelSplash.SetActive(true);
        userId = GameServices.LocalUser.userName;
        Debug.Log(userId);
        playerNameText.text = userId;
        PhotonNetwork.player.NickName = "M_"+userId;
        StartCoroutine(ProfileLoad());
        StopCoroutine(NameSet());
    }
    IEnumerator ProfileLoad()
    {
        while (!PlayFabClientAPI.IsClientLoggedIn())
            yield return null;
        userProfileManager.InitCoinAndName();
        StopCoroutine(ProfileLoad());
    }
    void Start() {
    }

    void Update() {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                exitUI.SetActive(true);
            }
        }
        
    }
    #region PhotonNetworkFunction
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby"+PhotonNetwork.lobby.Name);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 15;
        PhotonNetwork.CreateRoom(PhotonNetwork.player.NickName.ToString(), roomOptions, randomLobbyType);
        Debug.Log("Create random room");
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonCreateRoomFailed(codeAndMsg);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public void OnClickExitRoom() //Exit 버튼 눌렀을 때 작동하는 함수
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() //방 나갔을 때 실행되는 콜백함수
    {
        base.OnLeftRoom();
    }

    public override void OnConnectedToPhoton()
    {
        //SteamUser.CancelAuthTicket(hAuthTicket);
    }

    #endregion

    #region MONITER_BUTTON_FUNCTION
    public void OnClickExitButton()
    {
        Application.Quit();
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion
}