using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    AudioSource audioSource;
    [SerializeField] AudioClip SEbuttonNormal;
    [SerializeField] AudioClip SEbuttonSuccess;
    [SerializeField] AudioClip SEbuttonFailure;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void playNormalSound()
    {
        audioSource.PlayOneShot(SEbuttonNormal);
    }

    public void playSuccessSound()
    {
        audioSource.PlayOneShot(SEbuttonSuccess);
    }

    public void playFailureSound()
    {
        audioSource.PlayOneShot(SEbuttonFailure);
    }
    



}
