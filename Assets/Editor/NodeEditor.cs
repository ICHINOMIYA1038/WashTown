
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/*********************************/
//EditorWindow�N���X���p����������N���X�B�g���G�f�B�^���쐬�B
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
        //rootVisualElement�́AEditorWindow�N���X�����v���p�e�B

        rootVisualElement.Add(graphView);
    }
}
#endregion

/*********************************/
//GraphView�N���X���p����������N���X�BGraphView��VisualElements���p�����Ă���B
/*********************************/
#region GraphView

public class NodeEditor : GraphView
{

    private DialogCreator dialogCreator;
    public NodeEditor(EditorWindow editorWindow)
    {
        dialogCreator = (DialogCreator)editorWindow;
        // �m�[�h��ǉ�
        AddElement(new ExampleNode());
        
        //�ŏ��̓G�������g�̑傫����0�Ȃ̂ŁA
        // �e�̃T�C�Y�ɍ��킹��GraphView�̃T�C�Y��ݒ�
        this.StretchToParentSize();
        //�����ڂ���킩��ɂ������A�O���b�h������B

        AddGridBackground();
        AddStyles();

        AddManipulator();
        CreateNode();


        // �E�N���b�N���j���[��ǉ�
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
        // MMB�X�N���[���ŃY�[���C���A�E�g���ł���悤��
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        // MMB�h���b�O�ŕ`��͈͂𓮂�����悤��
        this.AddManipulator(new ContentDragger());
        // LMB�h���b�O�őI�������v�f�𓮂�����悤��
        this.AddManipulator(new SelectionDragger());
        // LMB�h���b�O�Ŕ͈͑I�����ł���悤��
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

    // GetCompatiblePorts���I�[�o�[���C�h����
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ///AddRange�ŕ������ڂ��܂Ƃ߂ă|�[�g��
        compatiblePorts.AddRange(ports.ToList().Where(port =>
        {
            // �����m�[�h�ɂ͌q���Ȃ�
            if (startPort.node == port.node)
                return false;

            // Input���m�AOutput���m�͌q���Ȃ�
            if (port.direction == startPort.direction)
                return false;

            // �|�[�g�̌^����v���Ă��Ȃ��ꍇ�͌q���Ȃ�
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
//�m�[�h�̐ݒ�//
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
        //�X�^�C���V�[�g�̓K���̂��߂̃N���X�̒ǉ�
        extensionContainer.AddToClassList("ds-node__extension-container");
        mainContainer.AddToClassList("ds-node__main-container");

    }

    public void setGraphView(NodeEditor graphview)
    {
        graphView = graphview;
    }

    public virtual void Draw()
    {
        /*�^�C�g���R���e�i�̒��g*/

        TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName);
        titleContainer.Insert(0, dialogueNameTextField);
        dialogueNameTextField.AddToClassList("ds-node__textfield");
        dialogueNameTextField.AddToClassList("ds-node__filename-textfield");
        dialogueNameTextField.AddToClassList("ds-node__textfield__hidden");

        /*�|�[�g�R���e�i�̒��g*/
        Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
        inputContainer.Add(inputPort);

        /*�g���R���e�i�̒��g*/
        VisualElement customDataContainer = new VisualElement();
        Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
        TextField textTextField = DSElementUtility.CreateTextArea(Text);
        textTextField.AddToClassList("ds-node__textfield");
        textTextField.AddToClassList("ds-node__quote-textfield");
        
        textFoldout.Add(textTextField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);
        //�ǉ������g���R���e�i���������邽�߂ɒǉ�
        RefreshExpandedState();
    }
}
    public class ExampleNode : Node
    {
        public ExampleNode()
        {
            title = "Example";

            // ���͗p�̃|�[�g���쐬
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float)); // ��O������Port.Capacity.Multiple�ɂ���ƕ����̃|�[�g�ւ̐ڑ����\�ɂȂ�
            inputPort.portName = "Input";
            inputContainer.Add(inputPort); // ���͗p�|�[�g��inputContainer�ɒǉ�����

            // �o�͗p�̃|�[�g�����
            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = "Value";
            outputContainer.Add(outputPort); // �o�͗p�|�[�g��outputContainer�ɒǉ�����
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

        // ���͗p�̃|�[�g���쐬
        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(int)); // ��O������Port.Capacity.Multiple�ɂ���ƕ����̃|�[�g�ւ̐ڑ����\�ɂȂ�
        inputPort.portName = "Input";
        inputContainer.Add(inputPort); // ���͗p�|�[�g��inputContainer�ɒǉ�����

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

            // Example�Ƃ����O���[�v��ǉ�
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Example")) { level = 1 });

        // Example�O���[�v�̉��Ɋe�m�[�h����邽�߂̃��j���[��ǉ�
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

            // �}�E�X�̈ʒu�Ƀm�[�h��ǉ�
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
