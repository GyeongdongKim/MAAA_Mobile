using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContent : MonoBehaviour {

    public Text playerName;
    private FriendsManager friendsManager;

    [HideInInspector] public PlayFab.ClientModels.FriendInfo friendInfo;
    
	void Start () {
        friendsManager = GetComponentInParent<FriendsManager>();
	}
	
	void Update () {
		
	}

    public void OnClickAddButton()
    {
        
    }

    public void OnClickDeleteButton()
    {
        friendsManager.RemoveFriend(friendInfo);
        Destroy(this.gameObject);
    }
}
