using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : OriginalBtn
{
    [SerializeField]
    LoadDataCon loadDatacon;
    [SerializeField]
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = ClickEvent;
    }

    // Update is called once per frame
    void ClickEvent()
    {
        var playerNum = loadDatacon.getIndex();
        gameManager.Load(playerNum);
        gameManager.SceneChanage(GameManager.SCENE_TITLE,GameManager.SCENE_MAIN);
        SceneManager.LoadScene(1);


    }
}
