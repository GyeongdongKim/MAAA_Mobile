using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootStepSound : MonoBehaviour {

    public AudioClip[] audioClips;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(audioClips[0], this.transform.position);
    }
    
}
