using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class UserProfileManager : MonoBehaviour
{

    [Header("UI Components")]
    public Text displayName;
    //public Text CoinCount;
    public Image avatarImage;
    public GameObject playerContentsParent;
    public GameObject playerContents;
    public Text[] displayFriendsNames;
    private string friendName;
    [HideInInspector] public string mafiaCoin;
    public Text textMafiaCoin;

    ////////////////avatar variable/////////////////////////
    int avatarInt;
    uint width, height;
    Texture2D downloadedAvatar;
    Rect rect = new Rect(0, 0, 184, 184);
    Vector3 pivot = new Vector2(0.5f, 0.5f);

    // Use this for initialization
    public void InitCoinAndName()
    {
        if (GetUserData("MafiaCoin") == null)
        {
            SetUserData("MafiaCoin", "0");
            mafiaCoin = "0";
        }
        else
            mafiaCoin = GetUserData("MafiaCoin");
        textMafiaCoin.text = mafiaCoin;

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest { DisplayName = displayName.text },
            result => {
                Debug.Log("Success to set Name");
            }, (error) => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
    }

    public void SetUserData(string key, string data)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {key, data}}
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public string GetUserData(string key)
    {
        string value = null;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data.ContainsKey(key))
                value = result.Data[key].Value;
            else
                Debug.Log("No key : " + key);
            //if (result.Data == null || !result.Data.ContainsKey("Ancestor")) Debug.Log("No Ancestor");
            //else Debug.Log("Ancestor: " + result.Data["Ancestor"].Value);
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
            value = null;
        });
        return value;
    }
}