using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SingleController : MonoBehaviour {

    private GameManager gameManager;
    private JobManager jobManager;
    private NoteRotate noteRotate;

    private void Start()
    {
        jobManager = GetComponent<JobManager>();
        gameManager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();
        noteRotate = GameObject.FindGameObjectWithTag("CAM").GetComponentInChildren<NoteRotate>();
    }

    void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Kill") && gameManager.isKillUIOn)
        {
            jobManager.AttackCoroutine(gameManager.GetObjectWithPoint(15.0f));
        }
    }
}
