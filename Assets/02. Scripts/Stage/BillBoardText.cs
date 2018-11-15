using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardText : MonoBehaviour
{

    public GameObject mTarget;

    // Use this for initialization
    void Start()
    {
        if (mTarget == null)
            mTarget = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(mTarget.transform);

        Vector3 rot = gameObject.transform.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(0, rot.y + 180, 0);
    }
}