using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadesController : MonoBehaviour {

    private DayNightController dnc;

    private void OnEnable()
    {
        dnc = FindObjectOfType<DayNightController>();
    }
    private void Start()
    {
        
    }

    void Update () {
        if (dnc.currentTimeOfDay > 0.25f && dnc.currentTimeOfDay < 0.75f)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
	}
}
