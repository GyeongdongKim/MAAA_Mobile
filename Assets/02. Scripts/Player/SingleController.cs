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
        noteRotate = GameObject.FindGameObjectWithTag("CAM").GetComponentInChildren<NoteRotate>();
    }

    void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Kill") && FindObjectOfType<GameManager>().isKillUIOn)
        {
            jobManager.AttackCoroutine(FindObjectOfType<GameManager>().GetObjectWithPoint(15.0f));
        }
    }
}
