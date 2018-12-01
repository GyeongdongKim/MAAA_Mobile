using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNVoiceController : MonoBehaviour {

    private PhotonVoiceSettings settings;
    private PhotonVoiceRecorder rec;
    private PhotonVoiceSpeaker speaker;

	// Use this for initialization
	void Start () {
        settings = FindObjectOfType<PhotonVoiceSettings>();
        rec = FindObjectOfType<PhotonVoiceRecorder>();
	}

    public void OnClickAll()
    {
        rec.Transmit = true;
        rec.AudioGroup = (byte)0;
    }
    public void OnClickMafia()
    {
        rec.Transmit = true;
        rec.AudioGroup = (byte)1;
    }
    public void OnClickOff()
    {
        rec.Transmit = false;
    }
}
