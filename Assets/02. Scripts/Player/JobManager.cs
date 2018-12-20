using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class JobManager : Photon.PunBehaviour {

    private PhotonView pv;
    private Animator animator;
    private GameManager gameManager;
    private int tempDay = 0;
    public TextMesh info;
    public AudioClip fireSound;
    public GameObject firePoint;
    private string job;
    private NoteRotate noteRotate;
    private VoteRotate voteRotate;
    public GameObject surviveAnimation;
    [HideInInspector] public int chanban;
    [HideInInspector] public PhotonView playerToKill;
    [HideInInspector] public bool isDeath=false, mafiaOn, shooted, isSurvived,isNotePop=false,isVotePop=false,execution=false;
    [HideInInspector] public bool isMafia = false, isPolice = false, isDoctor = false, isCitizen = false;

    Hashtable playerCustomProps = new Hashtable();
    [HideInInspector] public int coinScore;

    private DayNightController dayNightController;
    void Start() {/*
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();
        dayNightController = gameManager.GetComponent<DayNightController>();
        noteRotate = GameObject.FindGameObjectWithTag("CAM").GetComponentInChildren<NoteRotate>();
        voteRotate = GameObject.FindGameObjectWithTag("CAM").GetComponentInChildren<VoteRotate>();
        shooted = false;*/
    }
    private void OnEnable()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        dayNightController = gameManager.GetComponent<DayNightController>();
        noteRotate = GameObject.FindGameObjectWithTag("CAM").GetComponentInChildren<NoteRotate>();
        voteRotate = GameObject.FindGameObjectWithTag("CAM").GetComponentInChildren<VoteRotate>();
        shooted = false;
    }
    private void Update()
    {
        /*if (!gameManager.isDead)
        {
            if (isMafia)
            {
                if (dayNightController.currentTimeOfDay < 0.24 || dayNightController.currentTimeOfDay > 0.75)
                {
                    if (!shooted && !isDeath)
                    {
                        Debug.Log("MAFIA KILL UI");
                        gameManager.PrintKillUI(15.0f);
                    }
                    else
                    {
                        gameManager.killUI.SetActive(false);
                    }
                }
            }
            if (isDoctor)
            {
                if (0.75 < dayNightController.currentTimeOfDay && !isNotePop)
                {
                    Debug.Log("JOBMANAGER DOCTOR NOTE POP");
                    FindObjectOfType<ReadyManager>().localCam.GetComponentInChildren<NoteRotate>().NoteTrigger();
                    isNotePop = true;
                }
            }
            if (gameManager.dayCount != tempDay && dayNightController.currentTimeOfDay > 0.24)
            {
                shooted = false;
                tempDay = gameManager.dayCount;
                isNotePop = false; isVotePop = false;
                DayChangeInit();
                PhotonNetwork.player.SetScore(0);
            }
            if (2f/3f < dayNightController.currentTimeOfDay && !isVotePop)
            {
                FindObjectOfType<ReadyManager>().localCam.GetComponentInChildren<VoteRotate>().VoteTrigger();
                isVotePop = true;
            }
            if (0.75 < dayNightController.currentTimeOfDay && execution)
            {
                if (chanban >= PhotonNetwork.playerList.Length/2)
                {
                    PhotonNetwork.Destroy(GetComponent<PhotonView>());
                    gameManager.DeathCam();
                    playerCustomProps["Death"] = true;
                    PhotonNetwork.player.SetCustomProperties(playerCustomProps);
                }
                else
                {
                    chanban = 0;
                    GetComponent<ThirdPersonUserControl>().isStop=false;
                }
                execution = false;
            }
        }*/
    }

    public void AttackCoroutine(GameObject target)
    {
        if(!animator.GetBool("isAttack") &&!shooted)
            StartCoroutine(Attack(target));
    }

    IEnumerator Attack(GameObject target)
    {
        shooted = true;
        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(1f);
        Fire(target);
        yield return new WaitForSeconds(1f);
        animator.SetBool("isAttack", false);
    }
    public void Active()
    {
        GetComponentInChildren<GunOn>().Active();
    }
    public void Deactive()
    {
        GetComponentInChildren<GunOn>().Deactive();
    }

    public void Fire(GameObject killTarget)
    {
        gameManager.PlaySfx(firePoint.transform.position, 10.0f, 100f, fireSound);

        killTarget.GetComponent<PhotonView>().RPC("Death", PhotonTargets.Others, killTarget.GetComponent<PhotonView>().viewID,killTarget.transform.position);
    }

    #region RPC
    /*[PunRPC]
    public void Death(int pvID, Vector3 targetPos)
    {
        shooted = true;
        PingMiniMap(targetPos);
        playerToKill = PhotonView.Find(pvID);
        if (pv.owner == PhotonView.Find(pvID).owner)
        {
            isDeath = true;
            animator.SetBool("isDie", true);
            GetComponent<ThirdPersonUserControl>().isStop=true;
        }
    }*/

    /*[PunRPC]
    public void Voted(string name)
    {
        Debug.Log(pv.owner.NickName + " / " + name);
        //if(pv.owner.NickName==name)
        //{
            pv.owner.AddScore(1);
            Debug.Log(name + " Voted : " + pv.owner.GetScore());
        //}
    }*/

    /*[PunRPC]
    public void Survive(PhotonPlayer player)
    {
        if (pv.owner == player)
        {
            Instantiate(surviveAnimation, transform.position, transform.rotation);
            isDeath = false;
            animator.SetBool("isDie", false);
            GetComponent<ThirdPersonUserControl>().isStop=false;
        }
    }*/
    /*[PunRPC]
    public void Execution(int id)
    {
        if (pv.owner.ID == id)
        {
            Debug.Log("Execution");
            GetComponent<ThirdPersonUserControl>().isStop = true;
            GetComponent<Transform>().position = new Vector3(-59f, 10f, -50f); //execution location
            GetComponent<Transform>().rotation = Quaternion.Euler(0, -90f, 0);
            execution = true;
        }        
    }
    [PunRPC]
    public void Chanban(int id)
    {
        if(pv.owner.ID==id)
        {
            chanban++;
            Debug.Log(chanban);
        }        
    }*/
    #endregion
    public void DayChangeInit()
    {
        if (!pv.isMine)
            return;
        if (!isDeath)
        {
            if (playerToKill == null)
                return;
            else if (playerToKill.owner.NickName == gameManager.playerToSurvive)
            {
                playerToKill.RPC("Survive", PhotonTargets.All, playerToKill.owner);
                gameManager.playerToSurvive = null;
            }
        }
        else
        {
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
            gameManager.DeathCam();
            playerCustomProps["Death"] = true;
            PhotonNetwork.player.SetCustomProperties(playerCustomProps);
            Debug.Log("Day_Init Function DeathCam");
        }

    }

    public void PingMiniMap(Vector3 playerPos)
    {
        //gameManager.miniMap.GetComponent<MiniMapManager>().DisplayPing(playerPos);
        gameManager.PingMiniMap(playerPos);
    }

    public void SetPlayerJob()
    {
        job = (string)PhotonNetwork.player.CustomProperties["Job"];
        switch (job)
        {
            case "MAFIA":
                isMafia = true;
                break;
            case "POLICE":
                isPolice = true;
                //gameManager.miniMap.SetActive(true);
                break;
            case "DOCTOR":
                isDoctor = true;
                break;
            case "CITIZEN":
                isCitizen = true;
                break;
            default:
                break;
        }
    }

    [PunRPC]
    public void CoinGet()
    {
        coinScore++;
        gameManager.CoinScoreUpdate(coinScore);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(chanban);
        }
        else
        {
            chanban = (int)stream.ReceiveNext();
        }
    }
}