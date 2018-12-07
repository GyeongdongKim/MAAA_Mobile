﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using EasyMobile;

public class LobbyManager : Photon.PunBehaviour {
    

    [HideInInspector] public string userId;
    public Text playerNameText;
   
    private GameObject target;
    public GameObject exitUI;
    public GameObject panelBlack, panelSplash;
    public GameObject errorPopup;
    public Text errorText;
    public GameObject successPopup;
    public Text successText;
    [Header("MoniterIconUI")]
    public RandomLobbyManager randomLobbyManager;
    public CustomLobbyManager customLobbyManager;
    public GameObject customLobby;
    public GameObject randomLobby;
    public GameObject customRoomList;

    private string _playFabPlayerIdCache;
    private UserProfileManager userProfileManager;
    TypedLobby randomLobbyType = new TypedLobby("Random", LobbyType.Default);
    
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        userProfileManager = GetComponent<UserProfileManager>();
        if (!PhotonNetwork.connected)
        {
            GameServices.Init();
            StartCoroutine(NameSet());
        }
        else {
            panelBlack.SetActive(false);
            userProfileManager.InitCoinAndName();
        }
        PhotonNetwork.JoinLobby(randomLobbyType);
    }
    IEnumerator NameSet()
    {
        while (!GameServices.IsInitialized()||!PhotonNetwork.connected)
            yield return null;
        panelBlack.SetActive(false);
        panelSplash.SetActive(true);
        userId = GameServices.LocalUser.userName;
        playerNameText.text = "M_" + userId;
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
        roomOptions.MaxPlayers = 8;
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
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
            randomLobby.SetActive(true);
            randomLobbyManager.ClickButtonAndRefreshList();
        }
        else
            ErrorPopup("PhotonNetworkIsNotConnected",true);
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
        ErrorPopup("PlayfabLoginError", true);
        ErrorPopup(error.GenerateErrorReport(), false);
        
    }

    public void ErrorPopup(string errorMessage,bool clean)
    {
        errorPopup.SetActive(true);
        if (clean)
            errorText.text = errorMessage;
        else
            errorText.text += errorMessage + "\n";
    }

    public void SuccessPopup(string message, bool clean)
    {
        successPopup.SetActive(true);
        if (clean)
            successText.text = message;
        else
            successText.text += message + "\n";
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        base.OnFailedToConnectToPhoton(cause);
        ErrorPopup("FailedToConnectToPhoton",true);
        ErrorPopup(cause.ToString(),false);
        PhotonNetwork.Reconnect();
    }
    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        base.OnCustomAuthenticationFailed(debugMessage);
        ErrorPopup("PhotonAuthenticaionFailed",true);
        ErrorPopup(debugMessage,false);
    }
    #endregion
}