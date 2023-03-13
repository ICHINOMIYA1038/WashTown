
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/*********************************/
//EditorWindowクラスを継承した自作クラス。拡張エディタを作成。
/*********************************/
#region EditorWindow
public class DialogCreator : EditorWindow
{
    [MenuItem("Original/DialogCreater")]
    public static void Open()
    {
        DialogCreator dialogCreaterwindow = GetWindow<DialogCreator>();
        dialogCreaterwindow.titleContent = new GUIContent(ObjectNames.NicifyVariableName(nameof(DialogCreator)));
    }

    void OnEnable()
    {
        var graphView = new NodeEditor(this);
        //rootVisualElementは、EditorWindowクラスが持つプロパティ

        rootVisualElement.Add(graphView);
    }
}
#endregion

/*********************************/
//GraphViewクラスを継承した自作クラス。GraphViewはVisualElementsを継承している。
/*********************************/
#region GraphView

public class NodeEditor : GraphView
{

    private DialogCreator dialogCreator;
    public NodeEditor(EditorWindow editorWindow)
    {
        dialogCreator = (DialogCreator)editorWindow;
        // ノードを追加
        AddElement(new ExampleNode());
        
        //最初はエレメントの大きさが0なので、
        // 親のサイズに合わせてGraphViewのサイズを設定
        this.StretchToParentSize();
        //見た目じゃわかりにくいが、グリッドを入れる。

        AddGridBackground();
        AddStyles();

        AddManipulator();
        CreateNode();


        // 右クリックメニューを追加
        var menuWindowProvider = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
        menuWindowProvider.Initialize(this, editorWindow);
        nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
        };
    }

    private void CreateNode()
    {
        //DSNode node = new DSNode();
        //node.Initialize();
        //node.Draw();
        //AddElement(node);
        
    }
    void AddManipulator()
    {
        // MMBスクロールでズームインアウトができるように
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        // MMBドラッグで描画範囲を動かせるように
        this.AddManipulator(new ContentDragger());
        // LMBドラッグで選択した要素を動かせるように
        this.AddManipulator(new SelectionDragger());
        // LMBドラッグで範囲選択ができるように
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateGroupContextualMenu());
    }

    private IManipulator CreateGroupContextualMenu() 
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
            )));
        return contextualMenuManipulator;
    }

    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePosition = mousePosition;

        if (isSearchWindow)
        {
            worldMousePosition = dialogCreator.rootVisualElement.ChangeCoordinatesTo(dialogCreator.rootVisualElement.parent, mousePosition - dialogCreator.position.position);
        }

        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

        return localMousePosition;
    }

    private GraphElement CreateGroup(string title,Vector2 localMousePosition)
    {
        Group group = new Group()
        {
            title = title
        };
        group.SetPosition(new Rect(localMousePosition, Vector2.zero));
        return group;
    }

    void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0, gridBackground);
    }

    void AddStyles()
    {
        StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("GridBackground.uss");
        styleSheets.Add(styleSheet);
        StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("DSNodeStyles.uss");
        styleSheets.Add(nodeStyleSheet);
    }

    // GetCompatiblePortsをオーバーライドする
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ///AddRangeで複数項目をまとめてポートに
        compatiblePorts.AddRange(ports.ToList().Where(port =>
        {
            // 同じノードには繋げない
            if (startPort.node == port.node)
                return false;

            // Input同士、Output同士は繋げない
            if (port.direction == startPort.direction)
                return false;

            // ポートの型が一致していない場合は繋げない
            if (port.portType != startPort.portType)
                return false;

            return true;
        }));

        return compatiblePorts;
    }
}
#endregion


#region NodeType
public enum DSDialogType
{
    singleChoice,
    MultipleChoice
}
#endregion

//******************************************//
//ノードの設定//
//*****************************************//
#region Node Definition

public class DSSingleChoiceNode : DSNode
{
    public DSSingleChoiceNode()
    {

    }
    public override void Initialize()
    {
        base.Initialize();
        DialogType = DSDialogType.singleChoice;

        Choices.Add("Next Dialogue");
       
    }

    public override void Draw()
    {
        base.Draw();

        foreach(string choice in Choices)
        {
            Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            choicePort.portName = choice;
            outputContainer.Add(choicePort);
        }
        RefreshExpandedState();
    }
}

public class DSMultipleChoiceNode : DSNode
{
    public DSMultipleChoiceNode()
    {

    }
    public override void Initialize()
    {
        base.Initialize();
        DialogType = DSDialogType.MultipleChoice;

        Choices.Add("New Choice");
    }

    public override void Draw()
    {
        base.Draw();

        Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", ()=>
        {
            Port choicePort = CreateChoicePort("New Choice");
            Choices.Add("New Choice");
            outputContainer.Add(choicePort);



        });
        addChoiceButton.AddToClassList("ds-node__button");
        mainContainer.Insert(1,addChoiceButton);

        foreach (string choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);
            outputContainer.Add(choicePort);
        }
        RefreshExpandedState();
    }

    private Port CreateChoicePort(string choice)
    {
        Port choicePort = this.CreatePort();

        Button deleteChoiceButton = DSElementUtility.CreateButton("x", () =>
        {
            if (Choices.Count == 1)
            {
                return;
            }

            if (choicePort.connected)
            {
                graphView.DeleteElements(choicePort.connections);
            }

            Choices.RemoveAt(0);

            graphView.RemoveElement(choicePort);
        });
    
        deleteChoiceButton.AddToClassList("ds-node__button");
        TextField choiceTextField = DSElementUtility.CreateTextField(choice);
        choiceTextField.AddToClassList("ds-node__textfield");
        choiceTextField.AddToClassList("ds-node__choice-textfield");
        choiceTextField.AddToClassList("ds-node__textfield__hideen");

        choicePort.Add(choiceTextField);
        choicePort.Add(deleteChoiceButton);
        return choicePort;
    }
}


public class DSNode : Node
{
    public DSNode()
    {
        Initialize();
        Draw();
    }
        
    public string ID { get; set; }
    public string DialogueName { get; set; }
    public List<string> Choices { get; set; }
    public string Text { get; set; }
    public DSDialogType DialogType { get; set; }

    protected NodeEditor graphView;

    public virtual void Initialize()
    {
        DialogueName = "DialogueName";
        Choices = new List<string>();
        Text = "Dialogue Text.";
        //スタイルシートの適応のためのクラスの追加
        extensionContainer.AddToClassList("ds-node__extension-container");
        mainContainer.AddToClassList("ds-node__main-container");

    }

    public void setGraphView(NodeEditor graphview)
    {
        graphView = graphview;
    }

    public virtual void Draw()
    {
        /*タイトルコンテナの中身*/

        TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName);
        titleContainer.Insert(0, dialogueNameTextField);
        dialogueNameTextField.AddToClassList("ds-node__textfield");
        dialogueNameTextField.AddToClassList("ds-node__filename-textfield");
        dialogueNameTextField.AddToClassList("ds-node__textfield__hidden");

        /*ポートコンテナの中身*/
        Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
        inputContainer.Add(inputPort);

        /*拡張コンテナの中身*/
        VisualElement customDataContainer = new VisualElement();
        Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
        TextField textTextField = DSElementUtility.CreateTextArea(Text);
        textTextField.AddToClassList("ds-node__textfield");
        textTextField.AddToClassList("ds-node__quote-textfield");
        
        textFoldout.Add(textTextField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);
        //追加した拡張コンテナを可視化するために追加
        RefreshExpandedState();
    }
}
    public class ExampleNode : Node
    {
        public ExampleNode()
        {
            title = "Example";

            // 入力用のポートを作成
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
            inputPort.portName = "Input";
            inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する

            // 出力用のポートを作る
            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = "Value";
            outputContainer.Add(outputPort); // 出力用ポートはoutputContainerに追加する
        }
    }

    public class AddNode : Node
    {
        public AddNode()
        {
            title = "Add";

            var inputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort1.portName = "A";
            inputContainer.Add(inputPort1);

            var inputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort2.portName = "B";
            inputContainer.Add(inputPort2);

            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Out";
            outputContainer.Add(outputPort);
        }
    }

    public class OutputNode : Node
    {
        public OutputNode()
        {
            title = "Output";
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            port.portName = "Value";
            inputContainer.Add(port);
        }
    }

    public class ValueNode : Node
    {
        public ValueNode()
        {
            title = "Value";

            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            port.portName = "Value";
            outputContainer.Add(port);

            extensionContainer.Add(new FloatField());
            RefreshExpandedState();
        }
    }

public class StringValueNode : Node
{
    public StringValueNode()
    {
        title = "String";

        // 入力用のポートを作成
        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(int)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
        inputPort.portName = "Input";
        inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する

        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(string));
        port.portName = "Value";
        outputContainer.Add(port);

        extensionContainer.Add(new TextField("Textt"));
        RefreshExpandedState();
    }
}
#endregion


public class SearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private NodeEditor _graphView;
        private EditorWindow _editorWindow;

        public void Initialize(NodeEditor graphView, EditorWindow editorWindow)
        {
            _graphView = graphView;
            _editorWindow = editorWindow;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

            // Exampleというグループを追加
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Example")) { level = 1 });

        // Exampleグループの下に各ノードを作るためのメニューを追加
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(ValueNode))) { level = 2, userData = typeof(ValueNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(nameof(AddNode))) { level = 2, userData = typeof(AddNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(nameof(OutputNode))) { level = 2, userData = typeof(OutputNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(nameof(StringValueNode))) { level = 2, userData = typeof(StringValueNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(nameof(DSNode))) { level = 2, userData = typeof(DSNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(nameof(DSSingleChoiceNode))) { level = 2, userData = typeof(DSSingleChoiceNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(nameof(DSMultipleChoiceNode))) { level = 2, userData = typeof(DSMultipleChoiceNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(nameof(Group))) { level = 2, userData = typeof(Group) });
        return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as Type;
            var node = Activator.CreateInstance(type) as Node;

            // マウスの位置にノードを追加
            var worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
            var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
            node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));
        if(node is DSNode)
        {
            DSNode dsnode = (DSNode)node;

            dsnode.setGraphView(_graphView);
        }
            _graphView.AddElement(node);
            return true;
        }
    }


    public static class DSElementUtility
    {

    public static Port CreatePort(this DSNode node,string portName = "",Orientation orientation = Orientation.Horizontal,Direction direction = Direction.Output,Port.Capacity capacity = Port.Capacity.Single )
    {
        Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
        port.portName = portName;
        return port;
    
    }
    public static Button CreateButton(string text, Action onClick = null)
    {
        Button button = new Button(onClick)
        {
            text = text
        };
        return button;
    }
    public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };
            return foldout;
        }
        public static TextField CreateTextField(string value = null, EventCallback<ChangeEvent<string>>onValueChanged = null)
        {
            TextField textField = new TextField() { value = value };
            if(onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }
            return textField;
        }

        public static TextField CreateTextArea(string value = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }
    }
