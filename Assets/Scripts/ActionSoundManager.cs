using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActionSoundManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip SEGameClear;
    [SerializeField] AudioClip SEGameFailure;
    [SerializeField] AudioClip SEWater;
    [SerializeField] AudioClip ButtonSound;
    // Start is called before the first frame update

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }   

    public void playSoundGameClear()
    {
        audioSource.PlayOneShot(SEGameClear);
    }

    public void playSoundGameFailure()
    {
        audioSource.PlayOneShot(SEGameFailure);
    }

    public void playSoundWater()
    {
        audioSource.clip = SEWater;
        audioSource.Play();
        Debug.Log("waterSE");
    }

    public void stopSoundWater()
    {
        audioSource.Stop();
    }

    public void playButtonSound()
    {
        audioSource.PlayOneShot(ButtonSound);
    }


}
