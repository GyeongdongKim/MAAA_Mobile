using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour {

    //public Avatar[] avatars;

    [System.Serializable]
    public class Avatars
    {
        public Avatar avatar;
        public GameObject prefab;
    }
    public List<Avatars> avatars;
    private Animator animator;
    private PhotonView pv;
    private GameObject ob;
    // Use this for initialization
	void Start () {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        StartCoroutine(avatar());
    }
    IEnumerator avatar()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if (PhotonNetwork.playerList[i] == pv.owner)
            {
                ob = Instantiate(avatars[(int)PhotonNetwork.playerList[i].CustomProperties["Avatar"]].prefab, this.gameObject.transform);
                ob.transform.position = this.gameObject.transform.position;
                ob.transform.rotation = this.gameObject.transform.rotation;
                ob.GetComponent<Animator>().enabled = false;
                animator.avatar = avatars[(int)PhotonNetwork.playerList[i].CustomProperties["Avatar"]].avatar;
            }
        }
    }
}
