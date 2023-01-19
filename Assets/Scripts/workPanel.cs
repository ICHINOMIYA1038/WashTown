using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class workPanel : MonoBehaviour
{

    [SerializeField] private UIDocument _uiDocument;
    // ----------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //Button button = _uiDocument.rootVisualElement.Q<Button>();
       // button.clickable.clicked += clickEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void clickEvent()
    {
        Debug.Log("clicked");
    }
}
