using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActiveController : MonoBehaviour {

    private Button button;
    private Text text;
    public GameObject dujul;
	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
	}

    public void ButtonControl()
    {
        if (text.text.Length < 1)
            button.enabled = false;
        else
            button.enabled = true;
    }

    public void ButtonDisabled()
    {
        button.enabled = false;
    }
}
