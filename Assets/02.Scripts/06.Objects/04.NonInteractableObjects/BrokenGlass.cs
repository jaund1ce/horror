using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]AudioClip clip;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(clip);
    }
}
