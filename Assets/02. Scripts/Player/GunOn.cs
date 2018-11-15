using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOn : MonoBehaviour {
    public GameObject gun;
    public void Active()
    {
        gun.SetActive(true);
    }
    public void Deactive()
    {
        gun.SetActive(false);
    }
}
