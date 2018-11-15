using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerCardData : MonoBehaviour
{
    private string playerName;
    private int index=0;
    [System.Serializable]
    public class JobIcon
    {
        public string tag;
        public Sprite iconImage;
    }
    public List<JobIcon> jobIcons;

    public Text textPlayerName;
    public GameObject imageJobIcon;

    public void SetPlayerCardData()
    {
        textPlayerName.text = playerName;
    }

    public void OnClickJob()
    {
        if (index < jobIcons.Count)
        {
            imageJobIcon.GetComponent<Image>().sprite = jobIcons[index].iconImage;
            index++;
        }
        else
        {
            imageJobIcon.GetComponent<Image>().sprite = jobIcons[0].iconImage;
            index = 1;
        }
    }

    private void Update()
    {
        if (textPlayerName.text.Length < 1)
            imageJobIcon.SetActive(false);
        else
            imageJobIcon.SetActive(true);
    }
}