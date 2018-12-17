using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNVoiceController : MonoBehaviour {

    private PhotonVoiceSettings settings;
    private PhotonVoiceRecorder rec;

	// Use this for initialization
	IEnumerator Start () {
        while (FindObjectOfType<PhotonVoiceRecorder>() == null)
            yield return null;
        settings = FindObjectOfType<PhotonVoiceSettings>();
        rec = FindObjectOfType<PhotonVoiceRecorder>();

        PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[0], null);
        rec.AudioGroup = (byte)0;
	}

    public void OnClickAll()
    {
        rec.Transmit = true;
        PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[0], null);
        rec.AudioGroup = (byte)0;
    }
    public void OnClickMafia()
    {
        rec.Transmit = true;
        PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[0], null);
        rec.AudioGroup = (byte)1;
    }
    public void OnClickOff()
    {
        rec.Transmit = false;
    }
}
