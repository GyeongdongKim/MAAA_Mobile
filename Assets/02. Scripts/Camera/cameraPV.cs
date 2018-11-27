using UnityEngine;

public class cameraPV : MonoBehaviour {
    public GameObject notDeadCam;
    public GameObject deadCam;
    public Camera cam;
    void Start () {
        if (!GetComponent<PhotonView>().isMine) this.gameObject.SetActive(false);
	}

    public void DeathCam()
    {
        notDeadCam.SetActive(false);
        deadCam.SetActive(true);
    }

}