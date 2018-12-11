using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emoteControl : MonoBehaviour {
    public GameObject emoteSquare;
    public ReadyManager readyManager;
    public void ClickSmile()
    {
        emoteSquare.SetActive(!emoteSquare.activeSelf);
    }
    public void OnClickHi()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(0);
        emoteSquare.SetActive(false);
    }

    public void OnClickMafia()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(1);
        emoteSquare.SetActive(false);
    }

    public void OnClickPolice()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(2);
        emoteSquare.SetActive(false);
    }

    public void OnClickDoctor()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(3);
        emoteSquare.SetActive(false);
    }

    public void OnClickCitizen()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(4);
        emoteSquare.SetActive(false);
    }

    public void OnClickYou()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(5);
        emoteSquare.SetActive(false);
    }

    public void OnClickAttention()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(6);
        emoteSquare.SetActive(false);
    }

    public void OnClickNo()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(7);
        emoteSquare.SetActive(false);
    }

    public void OnClickOk()
    {
        readyManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(8);
        emoteSquare.SetActive(false);
    }
}
