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
            PhotonNetwork.ConnectUsingSettings(GameVersion);
            StartCoroutine(NameSet());
        }
        userProfileManager = GetComponent<UserProfileManager>();
        PhotonNetwork.JoinLobby(randomLobbyType);
    }
    #region Playfab Auth with Photon
    private void AuthenticateWithPlayFab()
    {
        LogMessage("PlayFab authenticating using Custom ID...");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = PlayFabSettings.DeviceUniqueIdentifier,
            
            //CustomId = userId
        }, RequestPhotonToken, OnPlayFabError);
    }

    /*
    * Step 2
    * We request Photon authentication token from PlayFab.
    * This is a crucial step, because Photon uses different authentication tokens
    * than PlayFab. Thus, you cannot directly use PlayFab SessionTicket and
    * you need to explicitely request a token. This API call requires you to 
    * pass Photon App ID. App ID may be hardcoded, but, in this example,
    * We are accessing it using convenient static field on PhotonNetwork class
    * We pass in AuthenticateWithPhoton as a callback to be our next step, if 
    * we have acquired token succesfully
    */
    private void RequestPhotonToken(LoginResult obj)
    {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppID
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    /*
     * Step 3
     * This is the final and the simplest step. We create new AuthenticationValues instance.
     * This class describes how to authenticate a players inside Photon environment.
     */
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        LogMessage("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };

        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service

        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        //We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = customAuth;
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        LogMessage(obj.GenerateErrorReport());
    }

    public void LogMessage(string message)
    {
        Debug.Log("PlayFab + Photon Example: " + message);
    }
    #endregion
    IEnumerator NameSet()
    {
        while (!GameServices.IsInitialized())
            yield return null;

        userId = GameServices.LocalUser.userName;
        Debug.Log(userId);
        playerNameText.text = userId;
        PhotonNetwork.player.NickName = userId;
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
    public void ClickQuitButton()
    {
        moniter.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crossHair.SetActive(true);
        controller.enabled = true;
        PhotonNetwork.LeaveLobby();
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