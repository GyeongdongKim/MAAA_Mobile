using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CustomLoomList : Photon.PunBehaviour { 

    public GameObject scrollContents;
    public GameObject roomItem;
    public GameObject customLobby;
    public GameObject customRoomList;
    public LobbyManager lobbyManager;

    TypedLobby customLobbyType = new TypedLobby("Custom", LobbyType.Default);
    TypedLobby randomLobbyType = new TypedLobby("Random", LobbyType.Default);

    public override void OnReceivedRoomListUpdate()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("ROOM_ITEM").Length; i++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("ROOM_ITEM")[i]);
        }

        int rowCount = 0;
        scrollContents.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        for (int i = 0; i < PhotonNetwork.GetRoomList().Length; i++)
        {
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContents.transform, false);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = PhotonNetwork.GetRoomList()[i].Name;
            roomData.connectPlayer = PhotonNetwork.GetRoomList()[i].PlayerCount;
            roomData.maxPlayers = PhotonNetwork.GetRoomList()[i].MaxPlayers;

            roomData.DispRoomData();
            roomData.GetComponent<Button>().onClick.AddListener(
                delegate
                {
                    OnClickRoomItem(roomData.roomName);
                    customLobby.SetActive(true);
                    customRoomList.SetActive(false);
                    customLobby.GetComponent<CustomLobbyManager>().ClickButtonAndRefreshList();
                });

            rowCount++;
        }
        scrollContents.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rowCount * 50);
    }

    public void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinCustomLobby()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinLobby(customLobbyType);
            customRoomList.SetActive(true);
            OnReceivedRoomListUpdate();
        }else
        {
            lobbyManager.ErrorPopup("PhotonIsNotConnected", true);
        }
        
    }

    public void LeaveCustomLobby()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby(randomLobbyType);
    }
    public void OnClickCustomRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        //roomOptions.CustomRoomProperties = new Hashtable() { { "CUSTOM", "T" } };
        roomOptions.MaxPlayers = 15;

        PhotonNetwork.CreateRoom(PhotonNetwork.player.NickName, roomOptions, customLobbyType);
    }
}
