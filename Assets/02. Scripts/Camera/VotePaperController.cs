using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotePaperController : MonoBehaviour {
    public Text voteTimer;
    public VoteRotate voteRotate;
    private int temp;
    private void OnEnable()
    {
        temp = 10;
        StartCoroutine(Disable());
    }
    IEnumerator Disable()
    {
        if (temp != 0)
        {
            temp--;
            voteTimer.text = temp.ToString();
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(Disable());
        }
        else
        {
            voteRotate.VoteOff();
        }
    }
    private void OnDisable()
    {
        StopCoroutine(Disable());
    }
}
