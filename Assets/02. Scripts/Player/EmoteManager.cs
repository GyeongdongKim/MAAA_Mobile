using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteManager : Photon.PunBehaviour {

    [System.Serializable]
    public class Emotes
    {
        public Sprite emote;
    }
    public List<Emotes> emotes;

    public Image emoteImage;
    public Image background;

    private int temp;
    public void DisplayEmote(int index)
    {
        StartCoroutine(Display(index));
    }

    IEnumerator Display(int index)
    {
        if (!emoteImage.IsActive())
        {
            temp = index;
            emoteImage.enabled = true;
            background.enabled = true;
            //emoteImage.sprite = emotes[temp].emote;
            yield return new WaitForSeconds(5.0f);
            emoteImage.enabled = false;
            background.enabled = false;
        }
    }
    private void Update()
    {
        if (emoteImage.enabled)
        {
            emoteImage.sprite = emotes[temp].emote;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(emoteImage.enabled);
            stream.SendNext(background.enabled);
            stream.SendNext(temp);
        }
        else
        {
            emoteImage.enabled = (bool)stream.ReceiveNext();
            background.enabled = (bool)stream.ReceiveNext();
            temp = (int)stream.ReceiveNext();
        }
   }
}
