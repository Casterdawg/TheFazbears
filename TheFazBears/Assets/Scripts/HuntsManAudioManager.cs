using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntsManAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] mechanicalSounds;
    public AudioClip[] painSounds;
    public AudioClip scream;
    public AudioClip death;
    public AudioClip stun;
    public AudioClip[] swings;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MechanicalSoundHuntsman()
    {
        int rand = Random.Range(0, mechanicalSounds.Length - 1);
        audioSource.clip = mechanicalSounds[rand];
        audioSource.Play();
    }

    public void PainSoundHuntsman()
    {
        int rand = Random.Range(0, painSounds.Length - 1);
        audioSource.clip = painSounds[rand];
        audioSource.Play();
    }

    public void ScreamHuntsman()
    {
        audioSource.clip = scream;
        audioSource.Play();
    }

    public void DeathHuntsman()
    {
        audioSource.clip = death;
        audioSource.Play();
    }

    public void StunHuntsman()
    {
        audioSource.clip = stun;
        audioSource.Play();
    }

    public void SwingHuntsman()
    {
        int rand = Random.Range(0, swings.Length - 1);
        audioSource.clip = swings[rand];
        audioSource.Play();
    }

}
