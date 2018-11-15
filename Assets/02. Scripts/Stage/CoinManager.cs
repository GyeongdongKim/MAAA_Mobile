using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour {
    
    private void OnTriggerEnter(Collider coll)
    {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        if(coll.GetComponent<PhotonView>().owner.IsLocal)
            coll.GetComponent<JobManager>().CoinGet();
        //PhotonNetwork.Destroy(this.gameObject);
        
    }
}
