using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]

public class OriginalBtn : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler  
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public System.Action onClickCallback;
    // タップ  
    public void OnPointerClick(PointerEventData eventData) {

        onClickCallback?.Invoke();
        if(audioClip != null&&audioSource!=null)
        audioSource.PlayOneShot(audioClip);

    }
    // タップダウン  
    public void OnPointerDown(PointerEventData eventData) { }
    // タップアップ  
    public void OnPointerUp(PointerEventData eventData) { }

}
