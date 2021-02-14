using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    //This class is used for objects that get destroyed when their sound plays. Destroyed game objects can't play sounds

    private AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound, Transform location)
    {
        transform.position = location.position;
        audioPlayer.clip = sound;

        audioPlayer.Play();
    }
}
