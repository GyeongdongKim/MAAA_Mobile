using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySurviveAnimation : MonoBehaviour {

    public void DestroyThis()
    {
        //PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }
}
