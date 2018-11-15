using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour {

    public RectTransform rectPing;
    public GameObject ping;
    public RectTransform panel;
    private float w, h,garo,sero,ratio_w,ratio_h;
    private Vector3 zero, width, height;
    
	void Start () {
        w = panel.rect.width;
        h = panel.rect.height;
        zero = new Vector3(-149.575f, 0, -134.459f);
        width = new Vector3(44.485f,0,-134.459f);
        height = new Vector3(-149.575f,0,39.44f);
        garo = width.x - zero.x;
        sero = height.z - zero.z;
        ratio_w = w / garo;
        ratio_h = h / sero;
    }

    public void DisplayPing(Vector3 playerPosition)
    {
        float temp_w, temp_h;
        temp_w = ratio_w * (width.x - playerPosition.x);
        temp_h = ratio_h * (height.z - playerPosition.z);
        rectPing.localPosition = new Vector2(w-temp_w, h-temp_h);
        StartCoroutine(Ping());
        Debug.Log("Ping" + playerPosition);
    }

    IEnumerator Ping()
    {
        if (this.gameObject.activeSelf)
        {
            ping.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            ping.SetActive(false);
        }            
    }
}
