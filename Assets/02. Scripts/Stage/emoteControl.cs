using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emoteControl : MonoBehaviour {
    public GameObject emoteSquare;
    public GameManager gameManager;
    public void ClickSmile()
    {
        emoteSquare.SetActive(!emoteSquare.activeSelf);
    }
    public void OnClickHi()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(0);
        emoteSquare.SetActive(false);
    }

    public void OnClickMafia()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(1);
        emoteSquare.SetActive(false);
    }

    public void OnClickPolice()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(2);
        emoteSquare.SetActive(false);
    }

    public void OnClickDoctor()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(3);
        emoteSquare.SetActive(false);
    }

    public void OnClickCitizen()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(4);
        emoteSquare.SetActive(false);
    }

    public void OnClickYou()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(5);
        emoteSquare.SetActive(false);
    }

    public void OnClickAttention()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(6);
        emoteSquare.SetActive(false);
    }

    public void OnClickNo()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(7);
        emoteSquare.SetActive(false);
    }

    public void OnClickOk()
    {
        gameManager.localPlayer.GetComponent<EmoteManager>().DisplayEmote(8);
        emoteSquare.SetActive(false);
    }
}
