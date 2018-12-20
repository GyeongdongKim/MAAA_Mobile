using System.Collections;

using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;
using LetterboxCamera;

public class ReadyManager : Photon.PunBehaviour {

    public Text timeText;
    public GameObject[] gameObjects;
    public float t;
    public int needPlayer;
    private int playersCount;
    public BigMoniterControler bmc;
    private bool isGameStart = false;
    [HideInInspector] public GameObject localPlayer, localCam;
    [HideInInspector] public MouseHover doctorNoteHover, voteHover;
    public Canvas canvas;
    [HideInInspector] public FreeLookCam freeLookCam;
    [HideInInspector] public ThirdPersonUserControl thirdPersonUserControl;

    // Use this for initialization
    void Start () {
        timeText.GetComponent<Animator>().SetBool("FadeIn",true);
        Vector3 spawnPoint = new Vector3(Random.Range(-60, -40), 10, Random.Range(-60, -40));
        localPlayer = PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        localCam = PhotonNetwork.Instantiate("Cameras", spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        freeLookCam = localCam.GetComponentInChildren<FreeLookCam>();
        thirdPersonUserControl = localPlayer.GetComponent<ThirdPersonUserControl>();
        canvas.worldCamera = localCam.GetComponent<cameraPV>().cam;
        canvas.planeDistance = 0.1f;
        bmc.NoteUpdate();
        //localPlayer.GetComponent<JobManager>().SetPlayerJob();
        FindObjectOfType<ForceCameraRatio>().Start();
    }
    private void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (playersCount == needPlayer && t > 0 && t < 90)
            {
                t -= Time.deltaTime;
                timeText.text = string.Format("{0:00}", t);
            }
            else if (!isGameStart)
            {
                timeText.text = "Wait For 8 Players";
            }
            if (string.Format("{0:00}", t) == "00")
            {
                t = 1000;
                timeText.text = "";
                timeText.GetComponent<Animator>().SetBool("FadeIn", false);

                GetComponent<PhotonView>().RPC("GameReady", PhotonTargets.All);
            }
        }
    }
    
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        playersCount = PhotonNetwork.room.PlayerCount;
        bmc.NoteUpdate();
        if (playersCount == needPlayer)
        {
            PhotonNetwork.room.IsOpen = false;
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
        playersCount = PhotonNetwork.room.PlayerCount;
        bmc.NoteUpdate();
        if (timeText.text.Length>0&&!isGameStart)
        {
            timeText.text = "";
            t = 60;

            PhotonNetwork.room.IsOpen = true;
        }
    }

    [PunRPC]
    public void GameReady()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(true);
        }

        isGameStart = true;
        localPlayer.GetComponent<JobManager>().enabled = true;
    }

    public void NoteHover(MouseHover mouseHover)
    {
        doctorNoteHover = mouseHover;
    }
    public void VoteHover(MouseHover mouseHover)
    {
        voteHover = mouseHover;
    }

    public void OnClickExitRoom()
    {
        AudioManager._Instance.Fade(AudioManager._Instance.lobbyMusic, 0.3f, true);
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Lobby");
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(timeText.text);
        }
        else
        {
            timeText.text = (string)stream.ReceiveNext();
        }
    }
}
