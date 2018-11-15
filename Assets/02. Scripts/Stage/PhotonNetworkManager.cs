using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//로비에서 방 입장 등 네트워크의 전반적인 부분을 담당하는 스크립트

public class PhotonNetworkManager : Photon.PunBehaviour {
    
    #region Private Variables
    [SerializeField] private Text ServerLogText;

    [SerializeField] private string GameVersion = "0.1";

    [SerializeField] private InputField userId;

    [SerializeField] private InputField roomName;

    [SerializeField] private InputField roomMaxPlayer;

    //[SerializeField] private GameObject scrollContents;

    //[SerializeField] private GameObject roomItem;

    [SerializeField] private GameObject toggleVisible;

    [SerializeField] private bool roomVisible=true;

    #endregion
    // Use this for initialization
    private void Awake () // 처음 실행시 작동하는 함수
    {
        PhotonNetwork.ConnectUsingSettings(GameVersion);
        
        roomName.text = "ROOM_" + Random.Range(0, 999);
	}

    public override void OnJoinedLobby() //로비 접속시 실행되는 콜백함수
    {
        userId.text = GetUserId();
    }

    string GetUserId() //사용자 이름 가져오는 함수
    {
        string userId = PlayerPrefs.GetString("USER_ID");

        if (string.IsNullOrEmpty(userId))
        {
            userId = "USER_" + Random.Range(0, 999);
        }
        return userId;
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) //랜덤룸 입장 실패시 동작하는 photonnetwork 콜백함수
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = roomVisible;
        roomOptions.CustomRoomProperties = new Hashtable() { { "MAP", "WildWest" },{"ISPLAY","N" } };
        if(roomMaxPlayer.text.Length > 1)
            roomOptions.MaxPlayers = byte.Parse(roomMaxPlayer.text);
        else
            roomOptions.MaxPlayers = 15;
        PhotonNetwork.CreateRoom("MyRoom",roomOptions,TypedLobby.Default);
    }

    #region UI FUNCTION
    public void OnClickJoinRandomRoom() //랜덤룸 입장 버튼 클릭시 작동하는 함수
    {
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClickCreateRoom() //방 생성시 작동하는 함수
    {
        string _roomName = roomName.text;
        if (string.IsNullOrEmpty(roomName.text))
        {
            _roomName = "ROOM_" + Random.Range(0, 999); //방 이름 랜덤 생성
        }
        
        PhotonNetwork.player.NickName = userId.text; //플레이어 이름 지정
        PlayerPrefs.SetString("USER_ID", userId.text);
        RoomOptions roomOptions = new RoomOptions(); //방 옵션 
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = roomVisible;
        roomOptions.CustomRoomProperties = new Hashtable() { { "Map", "WildWest" } };  //룸 프로퍼티에 맵 해시테이블 생성 및 기본값 wildwest 지정
        if (roomMaxPlayer.text.Length>1)
            roomOptions.MaxPlayers = byte.Parse(roomMaxPlayer.text);
        else
            roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public void OnClickQuitButton() //Quit 버튼 눌렀을 때 작동하는 함수
    {
        Application.Quit();
    }

    public void OnClickToggleVisible() //Visible 토글 체크 시 작동하는 함수
    {
        //toggleVisible.GetComponent<Toggle>().isOn = roomVisible;
        roomVisible=toggleVisible.GetComponent<Toggle>().isOn;
    }
    #endregion
    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) //방 생성 실패시 실행되는 PhotonNetwork 콜백함수
    {
        base.OnPhotonCreateRoomFailed(codeAndMsg);
    }

    public override void OnJoinedRoom() //방 입장했을 시 실행되는 PhotonNetwork 콜백함수
    {
        StartCoroutine(this.LoadBattleField());
    }
	
    IEnumerator LoadBattleField() // 다음 Scene 로딩 코루틴 함수
    {
        PhotonNetwork.isMessageQueueRunning = false;
        //Application.LoadLevel("Stage");
        SceneManager.LoadScene("Stage");
        yield return null;        
    }

    private void Update () {
        ServerLogText.text= PhotonNetwork.connectionStateDetailed.ToString(); //Photon Network 연결 상황 로그 표시
	}
}