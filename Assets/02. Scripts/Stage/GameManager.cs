using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : Photon.PunBehaviour {
    #region Private Variables
    //private PhotonView pv;
    public NoteList noteList;

    [HideInInspector] public MouseHover doctorNoteHover, voteHover;
    private EasyTween easyTween;

    [System.Serializable]
    public class JobImage
    {
        public string tag;
        public Sprite jobImage;
    }

    [HideInInspector] public GameObject localPlayer, localCam;
    public List<JobImage> jobImages;

    public Text coinScoreText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;
    [HideInInspector] public FreeLookCam freeLookCam;
    [HideInInspector] public ThirdPersonUserControl thirdPersonUserControl;
    [HideInInspector] public string playerToSurvive;
    [SerializeField] private AudioClip logoSound;
    //Hashtable roomType = new Hashtable();
    Hashtable playerCustomProps = new Hashtable();
    [HideInInspector] public int dayCount = -1;
    [HideInInspector] public int getCoin = 0;

    public Animator emoteAnimation;

    public float sfxVolumn = 1.0f;
    public bool isSfxMute = false;

    private bool trigger = true;
    [HideInInspector] public bool isDead = false, gameOver = false;
    [Header("Mouse Hover")]
    public MouseHover noteMouseHover;
    public MouseHover menuMouseHover0;
    public MouseHover menuMouseHover1;
    public MouseHover debugHover;
    public MouseHover killHover;
    public MouseHover endButtonHover;
    #endregion

    #region UI Variables
    [Header("UI Variables")]
    public Canvas canvas;
    public Canvas joystickCanvas;
    public Text narrText;
    public Text txtLogMsg;
    [SerializeField] private Image imageJob;
    [SerializeField] private GameObject playerNote;
    [SerializeField] private GameObject panelLogo;
    public GameObject miniMap;
    public GameObject killUI;
    [HideInInspector] public bool isKillUIOn;
    public GameObject mafiaMic;
    #endregion

    void Awake()
    {
        //pv = GetComponent<PhotonView>();
        easyTween = GetComponent<EasyTween>();
        
        if (PhotonNetwork.player.IsMasterClient)
        {
            SetPlayerJob();
        }
        //StartCoroutine(SetPlayerNote());
        //SceneManager.LoadScene("DemoScene5", LoadSceneMode.Additive);
        //StartCoroutine(Init());
    }

    void Start()
    {
        StartCoroutine(Logo());
        
        //yield return new WaitForSeconds(1.0f);
        //string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.NickName + "] Connected</color>";
        //pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
    }

    void Update()
    {
        
        /*if (!isDead || !gameOver)
        {
            while (Input.GetKey(KeyCode.LeftShift) && trigger)
            {
                if (trigger)
                { emoteAnimation.SetBool("isPressAlt", true); trigger = !trigger; }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(7);
            else if (Input.GetKeyDown(KeyCode.Alpha9) && !trigger)
                localPlayer.GetComponent<EmoteManager>().DisplayEmote(8);
            while (!Input.GetKey(KeyCode.LeftShift) && !trigger)
            {
                if (!trigger)
                { emoteAnimation.SetBool("isPressAlt", false); trigger = !trigger; }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
                easyTween.OpenCloseObjectAnimation();

            else if (Input.GetMouseButtonDown(0) && Cursor.visible)
            {
                if (noteMouseHover.isUIHover || menuMouseHover0.isUIHover || menuMouseHover1.isUIHover ||
                    doctorNoteHover.isUIHover || debugHover.isUIHover || voteHover.isUIHover || killHover.isUIHover || endButtonHover.isUIHover)
                    return;
                CursorOff();
            }

            if (GetComponent<DayNightController>().currentTimeOfDay > 0.25 && GetComponent<DayNightController>().currentTimeOfDay < 0.75)
            {
                isKillUIOn = false;
                killUI.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!freeLookCam.enabled)
                    return;
                CursorOn();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (Input.GetMouseButtonDown(0) && Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }*/
    }

    #region DayPrint
    public void DayPrint()
    {
        StartCoroutine(DayPrinter());
    }
    IEnumerator DayPrinter()
    {
        dayCount++;
        narrText.text = dayCount.ToString() + " DAY";
        yield return new WaitForSeconds(4.0f);
        narrText.text = "";
        StopCoroutine(DayPrinter());
    }
    #endregion

    public void NoteHover(MouseHover mouseHover)
    {
        doctorNoteHover = mouseHover;
    }
    public void VoteHover(MouseHover mouseHover)
    {
        voteHover = mouseHover;
    }

    public void DeathCam()
    {
        localCam.GetComponent<cameraPV>().DeathCam();
        isDead = true;
        playerNote.SetActive(false); miniMap.SetActive(false); killUI.SetActive(false);
    }
    //[PunRPC]
    public void NoteUpdate()
    {
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {
            if ((bool)PhotonNetwork.playerList[i].CustomProperties["Death"])
                return;
            noteList.playerLists[i].GetComponent<Text>().text = PhotonNetwork.playerList[i].NickName;
        }
        for (int i = PhotonNetwork.room.PlayerCount; i < 15; i++)
        {
            noteList.playerLists[i].GetComponent<Text>().text = "";
        }
    }
    [PunRPC]
    public void GameStart()
    {
        StartCoroutine(SetPlayerNote());
        //StartCoroutine(LoadScene());
        LoadScene();
    }
    [PunRPC]
    public void Execution()
    {
        //if (photonView.owner.ID == id)
        //{
            Debug.Log("Execution");
            localPlayer.GetComponent<ThirdPersonUserControl>().isStop = true;
            localPlayer.GetComponent<Transform>().position = new Vector3(-59f, 10f, -50f); //execution location
            localPlayer.GetComponent<Transform>().rotation = Quaternion.Euler(0, -90f, 0);
            localPlayer.GetComponent<JobManager>().execution = true;
        //}
    }
    [PunRPC]
    public void Chanban(int id)
    {
        //if (photonView.owner.ID == id)
        //{
            localPlayer.GetComponent<JobManager>().chanban++;
            Debug.Log(localPlayer.GetComponent<JobManager>().chanban);
        //}
    }

    void LoadScene()
    {
        //yield return new WaitForSeconds(1.0f);

        GetComponent<DayNightController>().gameOver = false;
        SceneManager.LoadScene("DemoScene5", LoadSceneMode.Additive);
        StartCoroutine(Init());
        //StopCoroutine(LoadScene());
    }
    IEnumerator Init()
    {
        Vector3 spawnPoint = new Vector3(Random.Range(-60, -40), 10, Random.Range(-60, -40));
        localPlayer = PhotonNetwork.Instantiate(player.name, spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        localCam = PhotonNetwork.Instantiate(playerCamera.name, spawnPoint, Quaternion.Euler(0, 0, 0), 0);
        freeLookCam = localCam.GetComponentInChildren<FreeLookCam>();
        thirdPersonUserControl = localPlayer.GetComponent<ThirdPersonUserControl>();
        canvas.worldCamera = localCam.GetComponent<cameraPV>().cam;
        canvas.planeDistance = 0.1f;
        yield return null;
    }

    IEnumerator SetPlayerNote()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < jobImages.Count; i++)
        {
            if (jobImages[i].tag == PhotonNetwork.player.CustomProperties["Job"].ToString())
                imageJob.GetComponent<Image>().sprite = jobImages[i].jobImage;
        }
        NoteUpdate();
        localPlayer.GetComponent<JobManager>().SetPlayerJob();
        StopCoroutine(SetPlayerNote());
    }

    public void SetPlayerJob()
    {
        int temp = PhotonNetwork.room.PlayerCount;
        int mafia1=0, mafia2=0, doctor=0, police=0;

        if (!PhotonNetwork.isMasterClient || temp < 4)
        {
            for (int i = 0; i < temp; i++)
            {
                playerCustomProps["Job"] = "CITIZEN";
                playerCustomProps["Death"] = false;
                playerCustomProps["Avatar"] = Random.Range(0, 10);
                PhotonNetwork.playerList[i].SetCustomProperties(playerCustomProps);
            }
            GetComponent<PhotonView>().RPC("GameStart", PhotonTargets.All);
            return;
        }
            
        PhotonPlayer[] playerList = PhotonNetwork.playerList;

        mafia1 = Random.Range(0, temp);
        while (mafia1 == mafia2)
            mafia2 = Random.Range(0, temp);
        while (mafia1 == doctor || mafia2 == doctor)
            doctor = Random.Range(0, temp);
        while (mafia1 == police || mafia2 == police || doctor == police)
            police = Random.Range(0, temp);

        for (int i = 0; i < temp; i++)
        {
            if (i == mafia1||i==mafia2)
            {
                playerCustomProps["Job"] = "MAFIA";
                playerCustomProps["Avatar"] = Random.Range(0, 10);
            }
            else if (i == doctor)
            {
                playerCustomProps["Job"] = "DOCTOR";
                playerCustomProps["Avatar"] = Random.Range(0, 10);
            }
            else if (i==police)
            {
                playerCustomProps["Job"] = "POLICE";
                playerCustomProps["Avatar"] = Random.Range(0, 10);
            }else
            {
                playerCustomProps["Job"] = "CITIZEN";
                playerCustomProps["Avatar"] = Random.Range(0, 10);
            }
            playerCustomProps["Death"] = false;
            playerList[i].SetCustomProperties(playerCustomProps);
        }
        GetComponent<PhotonView>().RPC("GameStart", PhotonTargets.All);
        if ((string)PhotonNetwork.player.CustomProperties["Job"] != "MAFIA")
            mafiaMic.SetActive(false);

    }

    IEnumerator Logo()
    {
        this.GetComponent<AudioSource>().PlayOneShot(logoSound, 0.9f);
        panelLogo.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        panelLogo.SetActive(false);
    }

    public void PrintKillUI(float length)
    {
        if (!localPlayer.GetComponent<JobManager>().shooted)
        {
            if (GetObjectWithPoint(length) == null)
                return;
            if(GetObjectWithPoint(length).tag=="Player"&&!GetObjectWithPoint(length).GetPhotonView().isMine)
            {
                isKillUIOn = true;
                killUI.SetActive(true);
            }
            else
            {
                isKillUIOn = false;
                killUI.SetActive(false);
            }
        }
    }

    public void CursorOn()
    {
        freeLookCam.enabled = false;
        thirdPersonUserControl.isStop=true;
        //thirdPersonUserControl.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorOff()
    {
        freeLookCam.enabled = true;
        //thirdPersonUserControl.enabled = true;
        thirdPersonUserControl.isStop=false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void PlaySfx(Vector3 pos, float minDis,float maxDis,AudioClip sfx)
    {
        if (isSfxMute) return;

        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;

        AudioSource _audioSource = soundObj.AddComponent<AudioSource>();

        _audioSource.clip = sfx;
        _audioSource.minDistance = minDis;
        _audioSource.maxDistance = maxDis;
        _audioSource.volume = sfxVolumn;
        _audioSource.Play();

        Destroy(soundObj, sfx.length);
    }

    public GameObject GetObjectWithPoint(float length)
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2f,Screen.height/2f));

        if (Physics.Raycast(ray.origin, ray.direction, out hit, length))
            target = hit.collider.gameObject;
        return target;
    }

    public void CoinScoreUpdate(int score)
    {
        getCoin = score;
        coinScoreText.text = score.ToString();
    }

    public void ClickKillButton()
    {
        CrossPlatformInputManager.SetButtonDown("Kill");
    }
    #region PhotonNetworkCallbackFunction
    
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        NoteUpdate();
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        txtLogMsg.text = txtLogMsg.text + msg;
    }
    
    public void OnClickExitRoom()
    {
        //string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.NickName + "] Disconnected</color>";
        //pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        //photonView.RPC("NoteUpdate", PhotonTargets.All);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Lobby");
    }
    #endregion
}