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
    [HideInInspector] public GameObject localPlayer, localCam;
    [HideInInspector] public MouseHover doctorNoteHover, voteHover;
    public Canvas canvas;
    [HideInInspector] public FreeLookCam freeLookCam;
    [HideInInspector] public ThirdPersonUserControl thirdPersonUserControl;

    // Use this for initialization
    void Start () {
        Vector3 spawnPoint = new Vector3(Random.Range(-60, -40), 10, Random.Range(-60, -40));
        localPlayer = PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        localCam = PhotonNetwork.Instantiate("Cameras", spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        freeLookCam = localCam.GetComponentInChildren<FreeLookCam>();
        thirdPersonUserControl = localPlayer.GetComponent<ThirdPersonUserControl>();
        canvas.worldCamera = localCam.GetComponent<cameraPV>().cam;
        canvas.planeDistance = 0.1f;

        localPlayer.GetComponent<JobManager>().SetPlayerJob();
        FindObjectOfType<ForceCameraRatio>().Start();
    }
    private void Update()
    {
        if (playersCount == needPlayer&&t>0&&t<50)
        {
            t -= Time.deltaTime;
            timeText.text = t.ToString();
        }
        if (t <= 0)
        {
            GameStart();
            timeText.text = "";
            t = 100;
        }
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(1.0f);
        Vector3 spawnPoint = new Vector3(Random.Range(-60, -40), 10, Random.Range(-60, -40));
        localPlayer = PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        localCam = PhotonNetwork.Instantiate("Cameras", spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        freeLookCam = localCam.GetComponentInChildren<FreeLookCam>();
        freeLookCam.m_Target = localPlayer.transform;
        thirdPersonUserControl = localPlayer.GetComponent<ThirdPersonUserControl>();
        canvas.worldCamera = localCam.GetComponent<cameraPV>().cam;
        canvas.planeDistance = 0.1f;

        localPlayer.GetComponent<JobManager>().SetPlayerJob();
        FindObjectOfType<ForceCameraRatio>().Start();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        playersCount = PhotonNetwork.room.PlayerCount;
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
        playersCount = PhotonNetwork.room.PlayerCount;
    }

    public void GameStart()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(true);
            localPlayer.GetComponent<JobManager>().enabled = true;
        }
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
        AudioManager._Instance.Stop();
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Lobby");
    }
}
