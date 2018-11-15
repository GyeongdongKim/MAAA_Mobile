using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillButtonController : MonoBehaviour {

    private void OnEnable()
    {
        StartCoroutine(disable());
    }
    IEnumerator disable()
    {
        yield return new WaitForSeconds(20.0f);
        this.gameObject.SetActive(false);
        StopCoroutine(disable());
    }
}
