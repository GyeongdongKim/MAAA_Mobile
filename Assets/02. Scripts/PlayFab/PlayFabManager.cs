using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour {
    void SetUserData(string key, string data)
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
    void GetUserData(string key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            //Keys = null
            Keys=new List<string>() { key }
        }, result => {
            Debug.Log("Got user data:");
            //if (result.Data == null || !result.Data.ContainsKey("Ancestor")) Debug.Log("No Ancestor");
            //else Debug.Log("Ancestor: " + result.Data["Ancestor"].Value);
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
