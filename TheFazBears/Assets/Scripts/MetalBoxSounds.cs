using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBoxSounds : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource player;

    private void Start()
    {
        player = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("Play Clip");
        int randNum = Random.Range(0, audioClips.Length - 1);
        player.clip = audioClips[randNum];
        player.Play();
    }
}
