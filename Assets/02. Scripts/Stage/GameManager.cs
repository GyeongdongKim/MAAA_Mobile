using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using LetterboxCamera;

public class GameManager : Photon.PunBehaviour {
    #region Private Variables
    private PhotonView pv;
    public NoteList noteList;

    [HideInInspector] public MouseHover doctorNoteHover, voteHover;
    private EasyTween easyTween;
    private ReadyManager readyManager;
    [System.Serializable]
    public class JobImage
    {
        public string tag;
        public Sprite jobImage;
    }

    [HideInInspector] public GameObject localPlayer, localCam;
    public List<JobImage> jobImages;

    [HideInInspector]public string localPlayerJob;
    private DayNightController dayNightController;

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
    #endregion

    #region UI Variables
    [Header("UI Variables")]
    public Canvas canvas;
    public Text narrText;
    public Text txtLogMsg;
    [SerializeField] private Image imageJob;
    [SerializeField] private GameObject playerNote;
    [SerializeField] private GameObject panelLogo;
    public GameObject miniMap;
    public GameObject killUI;
    [HideInInspector] public bool isKillUIOn;
    public GameObject mafiaMic;
    public GameObject panelMenu;
    public Text bmcDay;
    #endregion

    #region Job Variables
    private bool shooted;
    private bool isNotePop, isVotePop;
    [HideInInspector]public bool execution;
    private int tempDay = 0;
    [HideInInspector] public PhotonView playerToKill;
    #endregion

    private void OnEnable()
    {
        StartCoroutine(Logo());
        PhotonNetwork.isMessageQueueRunning = true;
        pv = GetComponent<PhotonView>();
        easyTween = GetComponent<EasyTween>();
        readyManager = FindObjectOfType<ReadyManager>();
        dayNightController = GetComponent<DayNightController>();

        localCam = readyManager.localCam;
        localPlayer = readyManager.localPlayer;
        localPlayer.GetComponent<JobManager>().enabled = true;
        thirdPersonUserControl = readyManager.thirdPersonUserControl;
        freeLookCam = readyManager.freeLookCam;
        

        if (PhotonNetwork.player.IsMasterClient)
        {
            SetPlayerJob();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            panelMenu.SetActive(true);
        }
        if (!isDead)
        {
            if (localPlayerJob=="MAFIA")
            {
                if (dayNightController.currentTimeOfDay < 0.24 || dayNightController.currentTimeOfDay > 0.75)
                {
                    if (!shooted)
                    {
                        Debug.Log("MAFIA KILL UI");
                        PrintKillUI(15.0f);
                    }
                }
            }
            else if (localPlayerJob=="DOCTOR")
            {
                if (0.75 < dayNightController.currentTimeOfDay && !isNotePop)
                {
                    Debug.Log("JOBMANAGER DOCTOR NOTE POP");
                    localCam.GetComponentInChildren<NoteRotate>().NoteTrigger();
                    isNotePop = true;
                }
            }
            //if (dayCount != tempDay && dayNightController.currentTimeOfDay > 0.24)
            //{
            //    shooted = false;
            //    tempDay = dayCount;
            //    isNotePop = false; isVotePop = false;
            //    //DayChangeInit();
            //    PhotonNetwork.player.SetScore(0);
            //    killUI.SetActive(false);
            //}
            if (2f / 3f < dayNightController.currentTimeOfDay && !isVotePop)
            {
                localCam.GetComponentInChildren<VoteRotate>().VoteTrigger();
                isVotePop = true;
            }
            if (0.75 < dayNightController.currentTimeOfDay && execution)
            {
                if (localPlayer.GetComponent<JobManager>().chanban >= PhotonNetwork.playerList.Length / 2)
                {
                    PhotonNetwork.Destroy(GetComponent<PhotonView>());
                    DeathCam();
                }
                else
                {
                    localPlayer.GetComponent<JobManager>().chanban = 0;
                    GetComponent<ThirdPersonUserControl>().isStop = false;
                }
                execution = false;
            }
        }
    }
    #region JobManagerMethod

    public void DayChangeInit()
    {
        if (!isDead)
        {
            if (playerToKill == null)
                return;
            else if (playerToKill.owner.NickName == playerToSurvive)
            {
                playerToKill.RPC("Survive", PhotonTargets.All, playerToKill.owner);
                playerToSurvive = null;
            }
        }
        else
        {
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
            DeathCam();
            playerCustomProps["Death"] = true;
            PhotonNetwork.player.SetCustomProperties(playerCustomProps);
            Debug.Log("Day_Init Function DeathCam");
        }

    }

    public void AttackCoroutine(GameObject target)
    {
        if (!localPlayer.GetComponent<Animator>().GetBool("isAttack") && !shooted)
            StartCoroutine(Attack(target));
    }

    IEnumerator Attack(GameObject target)
    {
        shooted = true;
        localPlayer.GetComponent<Animator>().SetBool("isAttack", true);
        yield return new WaitForSeconds(1f);
        //Fire(target);

        PhotonNetwork.Instantiate("GunSound", localPlayer.GetComponent<JobManager>().firePoint.transform.position, GetComponent<JobManager>().firePoint.transform.rotation, 0);
        target.GetComponent<PhotonView>().RPC("Death", PhotonTargets.Others, target.GetComponent<PhotonView>().viewID, target.transform.position);

        yield return new WaitForSeconds(1f);
        localPlayer.GetComponent<Animator>().SetBool("isAttack", false);
    }
    public void Fire(GameObject killTarget)
    {
        //PlaySfx(firePoint.transform.position, 10.0f, 100f, fireSound);
        //PhotonNetwork.Instantiate("GunSound", localPlayer.GetComponent<JobManager>().firePoint.transform.position, GetComponent<JobManager>().firePoint.transform.rotation, 0);
        //killTarget.GetComponent<PhotonView>().RPC("Death", PhotonTargets.Others, killTarget.GetComponent<PhotonView>().viewID, killTarget.transform.position);
    }

    [PunRPC]
    public void Death(int pvID, Vector3 targetPos)
    {
        shooted = true;
        PingMiniMap(targetPos);
        playerToKill = PhotonView.Find(pvID);
        if (pv.owner == PhotonView.Find(pvID).owner)
        {
            isDead = true;
            localPlayer.GetComponent<Animator>().SetBool("isDie", true);
            GetComponent<ThirdPersonUserControl>().isStop = true;
        }
    }
    [PunRPC]
    public void Survive(PhotonPlayer player)
    {
        if (pv.owner == player)
        {
            PhotonNetwork.Instantiate("surviveAnimation", localPlayer.transform.position, localPlayer.transform.rotation,0);
            isDead = false;
            localPlayer.GetComponent<Animator>().SetBool("isDie", false);
            GetComponent<ThirdPersonUserControl>().isStop = false;
        }
    }
    #endregion
    #region DayPrint
    public void DayPrint()
    {
        DayChangeInit();
        shooted = false;
        tempDay = dayCount;
        isNotePop = false; isVotePop = false;
        PhotonNetwork.player.SetScore(0);
        killUI.SetActive(false);

        StartCoroutine(DayPrinter());
    }
    IEnumerator DayPrinter()
    {
        dayCount++;
        if (dayCount == 1)
        {
            narrText.text = "Hello, MAAA World!";
            narrText.GetComponent<Animator>().SetBool("FadeIn", true);
        }
        else
        {
            narrText.text = dayCount.ToString() + " DAY Good Morning :)";
            narrText.GetComponent<Animator>().SetBool("FadeIn", true);
        }
        bmcDay.text = dayCount + " Day";
        yield return new WaitForSeconds(4.0f);
        narrText.GetComponent<Animator>().SetBool("FadeIn", false);
        //narrText.text = "";
        StopCoroutine(DayPrinter());
    }
    #endregion

    public void DeathCam()
    {
        GetComponent<DayNightController>().NarrationWhat("You are Dead :(");
        localCam.GetComponent<cameraPV>().DeathCam();
        canvas.worldCamera = localCam.GetComponent<cameraPV>().deathCam;
        canvas.planeDistance = 0.1f;

        PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[0], new byte[0]);

        localPlayer.GetComponent<PhotonVoiceRecorder>().Transmit = false;
        isDead = true;
        playerCustomProps["Death"] = true;
        PhotonNetwork.player.SetCustomProperties(playerCustomProps);
        playerNote.SetActive(false); miniMap.SetActive(false); killUI.SetActive(false);
        GetComponent<PhotonView>().RPC("NoteAndMoniterUpdate", PhotonTargets.All);
    }

    [PunRPC]
    public void NoteAndMoniterUpdate()
    {
        NoteUpdate();
        FindObjectOfType<BigMoniterControler>().NoteUpdate();
    }

    public void NoteUpdate()
    {
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {
            if ((bool)PhotonNetwork.playerList[i].CustomProperties["Death"])
                return;
            noteList.playerLists[i].GetComponent<Text>().text = PhotonNetwork.playerList[i].NickName;
        }
        for (int i = PhotonNetwork.room.PlayerCount; i < 8; i++)
        {
            noteList.playerLists[i].GetComponent<Text>().text = "";
        }
    }

    [PunRPC]
    public void GameStart()
    {
        localPlayerJob = PhotonNetwork.player.CustomProperties["Job"].ToString();
        for (int i = 0; i < jobImages.Count; i++)
        {
            if (jobImages[i].tag == localPlayerJob)
                imageJob.sprite = jobImages[i].jobImage;
        }

        if (localPlayerJob == "MAFIA")
            mafiaMic.SetActive(true);

        NoteUpdate();

        dayNightController.gameOver = false;
    }
    [PunRPC]
    public void Execution()
    {
        Debug.Log("Execution");
        localPlayer.GetComponent<Transform>().position = new Vector3(-59f, 6f, -50f);
        localPlayer.GetComponent<Transform>().rotation = Quaternion.Euler(0, -90f, 0);
        execution = true;
    }
    [PunRPC]
    public void Chanban()
    {
        localPlayer.GetComponent<JobManager>().chanban++;
        Debug.Log(localPlayer.GetComponent<JobManager>().chanban);
    }
    
    IEnumerator SetPlayerNote()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < jobImages.Count; i++)
        {
            if (jobImages[i].tag == PhotonNetwork.player.CustomProperties["Job"].ToString())
                imageJob.sprite = jobImages[i].jobImage;
        }
        NoteUpdate();
        StopCoroutine(SetPlayerNote());
    }

    private void SetPlayerJob()
    {
        int temp = PhotonNetwork.room.PlayerCount;
        int mafia1=0, mafia2=0, doctor=0, police=0;

        if (temp < 4)
        {
            for (int i = 0; i < temp; i++)
            {
                playerCustomProps["Job"] = "CITIZEN";
                playerCustomProps["Death"] = false;
                PhotonNetwork.playerList[i].SetCustomProperties(playerCustomProps);
            }
        }
        else
        {
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
                if (i == mafia1 || i == mafia2)
                {
                    playerCustomProps["Job"] = "MAFIA";
                }
                else if (i == doctor)
                {
                    playerCustomProps["Job"] = "DOCTOR";
                }
                else if (i == police)
                {
                    playerCustomProps["Job"] = "POLICE";
                }
                else
                {
                    playerCustomProps["Job"] = "CITIZEN";
                }
                playerCustomProps["Death"] = false;
                playerList[i].SetCustomProperties(playerCustomProps);
            }
        }
        pv.RPC("GameStart", PhotonTargets.All);
        localPlayer.GetComponent<JobManager>().SetPlayerJob();
    }

    IEnumerator Logo()
    {
        this.GetComponent<AudioSource>().PlayOneShot(logoSound, 0.9f);
        panelLogo.SetActive(true);
        PhotonNetwork.room.IsOpen = false;
        yield return new WaitForSeconds(4.0f);
        panelLogo.SetActive(false);
    }

    public void PrintKillUI(float length)
    {
        Debug.Log("GAMEMANAGER MAFIA PRINTKILLUI");
        if (!shooted)
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
    
    public void PingMiniMap(Vector3 pos)
    {
        miniMap.SetActive(true);
        miniMap.GetComponent<MiniMapManager>().DisplayPing(pos);
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
        AttackCoroutine(GetObjectWithPoint(15.0f));
    }
    #region PhotonNetworkCallbackFunction
    
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        NoteUpdate();
    }
    #endregion
}