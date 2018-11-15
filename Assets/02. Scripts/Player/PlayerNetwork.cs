using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour {
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private MonoBehaviour[] playerControlScripts;

    private PhotonView photonView;

    public int playerHealth = 100;


    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        Initialize();
    }

    private void Initialize()
    {
        if (photonView.isMine)
        {

        }else
        {

            foreach(MonoBehaviour m in playerControlScripts)
            {
                m.enabled = false;
            }
        }
    }
    private void Update()
    {
        if (!photonView.isMine) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerHealth -= 5;
        }
    }
}