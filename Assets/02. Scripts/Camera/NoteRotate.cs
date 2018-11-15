using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteRotate : MonoBehaviour {

    public GameObject note;
    public DoctorNotePanel doctorNotePanel;
    private Animator animator;
    private bool noteOff = true;
    private int temp;
    private GameManager gameManager;
    public Text noteTimer;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();
	}
    public void NoteOn()
    {
        gameManager.CursorOn();
        note.SetActive(true);
        note.GetComponentInChildren<DoctorNotePanel>().InitNoteList();
        animator.SetBool("NoteOn", true);
        noteOff = false;
    }
    public void NoteOff()
    {
        animator.SetBool("NoteOn", false);
        noteOff = true;
    }

    public void NoteDeactive()
    {
        note.SetActive(false);
    }
    public void NoteActive()
    {
        note.SetActive(true);
    }

    public void NoteTrigger()
    {
        NoteOn();
        temp = 10;
        StartCoroutine(NoteStart());
    }

    IEnumerator NoteStart()
    {
        if (temp != 0 && !noteOff)
        {
            temp--;
            noteTimer.text = temp.ToString();
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(NoteStart());
        }
        else
        {
            doctorNotePanel.NoSelectPlayer();
            NoteOff();
        }
    }
}
