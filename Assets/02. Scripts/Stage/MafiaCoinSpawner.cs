using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MafiaCoinSpawner : MonoBehaviour {

    public class CoinPos
    {
        public Transform transform;
    }
    public List<CoinPos> coinPoses;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < GetComponentsInChildren<Transform>().Length; i++)
        {
            //coinPoses[i].transform = GetComponentsInChildren<Transform>()[i];
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.InstantiateSceneObject("MafiaCoin", GetComponentsInChildren<Transform>()[i].transform.position, GetComponentsInChildren<Transform>()[i].transform.rotation, 0, null);
                //a.transform.parent = coinPoses[i].transform;
            }
        }
	}
}