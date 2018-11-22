using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class FriendsManager : MonoBehaviour {
    public List<PlayFab.ClientModels.FriendInfo> _friends = null;
    public GameObject playerContent;
    public RectTransform scrollContents;


    public LobbyManager lobbyManager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FriendListRefresh()
    {
        for (int i = 0; i < _friends.Count; i++)
        {
            GameObject a = Instantiate(playerContent, scrollContents);
            a.GetComponent<PlayerContent>().friendInfo = _friends[i];
            a.GetComponent<PlayerContent>().playerName.text = _friends[i].Profile.DisplayName;
        }
        scrollContents.sizeDelta = new Vector2(875, _friends.Count * 110);
    }

    public void OnClickButton()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            if(this.gameObject.activeSelf)
            {
                var request = new GetFriendsListRequest();
                request.ProfileConstraints = new PlayerProfileViewConstraints();
                request.ProfileConstraints.ShowDisplayName = true;

                PlayFabClientAPI.GetFriendsList(request, (result) => {
                    _friends = result.Friends;
                    FriendListRefresh();
                }, (error) => { lobbyManager.ErrorPopup(error.GenerateErrorReport(), true); });
            }
        }
        else
            lobbyManager.ErrorPopup("PlayfabIsNotLoggined", true);
    }

    enum FriendIdType { PlayFabId, Username, Email, DisplayName };

     void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }

    public void RemoveFriend(PlayFab.ClientModels.FriendInfo friendInfo)
    {
        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
        {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, result => {
            _friends.Remove(friendInfo);
        }, DisplayPlayFabError);
    }
    void DisplayFriends(List<PlayFab.ClientModels.FriendInfo> friendsCache) { friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId)); }
    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
    void DisplayError(string error) { Debug.LogError(error); }

}
