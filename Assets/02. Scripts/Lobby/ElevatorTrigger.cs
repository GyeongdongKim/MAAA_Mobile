using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour {

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            animator.SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("Open", false);
        }
    }
}
