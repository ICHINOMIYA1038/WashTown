using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class NPCConversation : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterText;
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;
    public GameObject conversationPanel;

    private string currentCharacter;
    private string currentMessage;
    private string currentOption1;
    private string currentOption2;
    private string currentOption3;
    private string currentOption4;
    private int currentLineIndex = 0;
    private List<ConversationLine> conversationLines = new List<ConversationLine>();

    [SerializeField]FPSCon fpscon;

    // Start is called before the first frame update
    void Start()
    {
        fpscon.canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        LoadConversationData();
        StartConversation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    class ConversationLine
    {
        public string character;
        public string message;
        public int option1;
        //もし、選択肢がない場合には即時でoption1を表示
        //その場合、option2は-1を入れる。
        public int option2;
        public int option3;
        public int option4;
    }

    // Load conversation data from CSV file
    void LoadConversationData()
    {
        string filePath = Application.streamingAssetsPath + "/CSV/conversation.csv";
        StreamReader reader = new StreamReader(filePath);

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            ConversationLine conversationLine = new ConversationLine();
            conversationLine.character = values[1];
            conversationLine.message = values[2];
            conversationLine.option1 = int.Parse(values[3]);
            conversationLine.option2 = int.Parse(values[4]);
            conversationLine.option3 = int.Parse(values[5]);
            conversationLine.option4 = int.Parse(values[6]);

            conversationLines.Add(conversationLine);
        }

        reader.Close();
    }

    // Start conversation with first character
    void StartConversation()
    {
        currentLineIndex = 0;
        var currentDialog = conversationLines[currentLineIndex];
        currentCharacter = currentDialog.character;
        currentMessage = currentDialog.message;
        
        currentOption1 = conversationLines[currentDialog.option1].message;
        currentOption2 = conversationLines[currentDialog.option2].message;
        currentOption3 = conversationLines[currentDialog.option3].message;
        currentOption4 = conversationLines[currentDialog.option4].message;

        dialogueText.text = currentMessage;
        characterText.text = currentDialog.character;

        option1Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption1;
        option1Button.onClick.AddListener(Option1Clicked);
        option2Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption2;
        option2Button.onClick.AddListener(Option2Clicked);
        option3Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption3;
        option3Button.onClick.AddListener(Option3Clicked);
        option4Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption4;
        option4Button.onClick.AddListener(Option4Clicked);

        conversationPanel.SetActive(true);
    }

    // Proceed to next conversation line based on selected option
    void NextConversationLine(int selectedOption)
    {
        //エラー処理
        if (selectedOption == -1)
        {
            return;
        }

        currentLineIndex = selectedOption;

        if (currentLineIndex < conversationLines.Count)
        {
            var currentDialog = conversationLines[currentLineIndex];
            currentCharacter = currentDialog.character;
            currentMessage = currentDialog.message;
            
            //終了判定
            if (currentDialog.option1 == -1)
            {
                EndConversation();
            }

            



            //文章の更新
            if (currentDialog.option1 != -1)
            {
                currentOption1 = conversationLines[currentDialog.option1].message;
                option1Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption1;
                option1Button.onClick.AddListener(Option1Clicked);
            }
            if (currentDialog.option2 != -1)
            {
                currentOption2 = conversationLines[currentDialog.option2].message;
                option2Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption2;
                option2Button.onClick.AddListener(Option2Clicked);
            }
            if (currentDialog.option3 != -1)
            {
                currentOption3 = conversationLines[currentDialog.option3].message;
                option3Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption3;
                option3Button.onClick.AddListener(Option3Clicked);
            }
            if (currentDialog.option4 != -1)
            {
                currentOption4 = conversationLines[currentDialog.option4].message;
                option4Button.GetComponentInChildren<TextMeshProUGUI>().text = currentOption4;
                option4Button.onClick.AddListener(Option4Clicked);
            }

            dialogueText.text = currentMessage;
            characterText.text = currentDialog.character;

            conversationPanel.SetActive(true);
        }
    }

    // Handle click on option 1 button
    void Option1Clicked()
    {
        NextConversationLine(conversationLines[currentLineIndex].option1);
    }

    // Handle click on option 2 button
    void Option2Clicked()
    {
        NextConversationLine(conversationLines[currentLineIndex].option2);
    }

    void Option3Clicked()
    {
        NextConversationLine(conversationLines[currentLineIndex].option3);
    }
    void Option4Clicked()
    {
        NextConversationLine(conversationLines[currentLineIndex].option4);
    }

    void EndConversation()
    {

    }
}