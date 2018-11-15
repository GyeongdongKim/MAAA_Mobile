using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseActionManager : MonoBehaviour {
    public GameObject text;

    private void OnMouseEnter()
    {
        text.SetActive(true);
    }
    private void OnMouseExit()
    {
        text.SetActive(false);
    }
}
