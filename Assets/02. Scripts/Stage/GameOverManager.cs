using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    private PhotonView pv;
    private GameManager gameManager;
    public GameObject[] quitObjects;
    private int finalScore;
    private int tempScore;

    [Header("EndScreenContents")]
    public GameObject endScreen;
    public GameObject winIcon;
    public GameObject defeatIcon;
    public Text drop;
    public Text day;
    public Text win;
    public Text total;
    

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        gameManager = GetComponent<GameManager>();
    }

    public void CheckGame()
    {
        if (gameManager.dayCount == 1)
            return;
        int mafia=0, noMafia=0;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if ((string)players[i].GetPhotonView().owner.CustomProperties["Job"] == "MAFIA")
                mafia++;
            else
                noMafia++;
        }
        Debug.Log("mafia: " + mafia + " nomafia: " + noMafia);
        if (PhotonNetwork.isMasterClient)
        {
            if (mafia == 0)
                pv.RPC("GameOver", PhotonTargets.All, false);
            else if (mafia >= noMafia)
                pv.RPC("GameOver", PhotonTargets.All, true);
        }
    }

    [PunRPC]
    public void GameOver(bool mafiaWin)
    {
        GetComponent<DayNightController>().gameOver = true;
        GetComponent<GameManager>().gameOver = true;
        int totalScore = 0;
        for (int i = 0; i < quitObjects.Length; i++)
        {
            quitObjects[i].SetActive(false);
        }
        if (mafiaWin)
        {
            if(gameManager.localPlayerJob=="MAFIA")
            {//Mafia win
                winIcon.SetActive(true);
                defeatIcon.SetActive(false);
                win.text = "5";
                totalScore = gameManager.getCoin + gameManager.dayCount + 5;
            }
            else
            {//Citizen Lose
                winIcon.SetActive(false);
                defeatIcon.SetActive(true);
                win.text = "-";
                totalScore = gameManager.getCoin + gameManager.dayCount;
            }
        }else
        {
            if (gameManager.localPlayerJob != "MAFIA")
            {//Citizen Win
                winIcon.SetActive(true);
                defeatIcon.SetActive(false);
                win.text = "5";
                totalScore = gameManager.getCoin + gameManager.dayCount + 5;
            }
            else
            {//Mafia Lose
                winIcon.SetActive(false);
                defeatIcon.SetActive(true);
                win.text = "-";
                totalScore = gameManager.getCoin + gameManager.dayCount;
            }
        }
        drop.text = gameManager.getCoin.ToString();
        day.text = gameManager.dayCount.ToString();
        total.text = totalScore.ToString();
        finalScore = totalScore;
        CoinSet();
        endScreen.SetActive(true);
    }

    private void CoinSet()
    {
        tempScore = System.Convert.ToInt32(AudioManager._Instance.GetUserData("MafiaCoin"));
        finalScore += tempScore;
        AudioManager._Instance.SetUserData("MafiaCoin", finalScore.ToString());
    }

}
