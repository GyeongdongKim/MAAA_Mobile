using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotate : MonoBehaviour {

	void Update () {
        GetComponent<Transform>().Rotate(Vector3.up, 1f);
	}
}
