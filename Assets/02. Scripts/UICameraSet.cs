using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraSet : MonoBehaviour {
    private Canvas canvas;
	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
        if(Camera.main!=null)
            canvas.worldCamera = Camera.main;
	}
}
