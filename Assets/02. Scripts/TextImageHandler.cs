using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextImageHandler : MonoBehaviour {
    private Text text;
    private Image image;
    
	void Start () {
        text = GetComponentInParent<Text>();
        image = GetComponent<Image>();
	}
	
	void Update () {
        if (text.text.Length > 1)
        {
            image.color = new Color(1, 1, 1, 60/255f);
        }else if(text.text.Length<1)
        {
            image.color = new Color(1, 1, 1, 0);
        }
	}
}
