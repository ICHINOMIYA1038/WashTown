using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class OriginalBtn : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler  
{

    public System.Action onClickCallback;
    // タップ  
    public void OnPointerClick(PointerEventData eventData) {

        onClickCallback?.Invoke();
    }
    // タップダウン  
    public void OnPointerDown(PointerEventData eventData) { }
    // タップアップ  
    public void OnPointerUp(PointerEventData eventData) { }

}
