
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class ExampleGraphEditorWindow : EditorWindow
{
    [MenuItem("Original/NodeEditor")]
    public static void Open()
    {
        GetWindow<ExampleGraphEditorWindow>(ObjectNames.NicifyVariableName(nameof(ExampleGraphEditorWindow)));
    }

    void OnEnable()
    {
        var graphView = new NodeEditor(this);
        rootVisualElement.Add(graphView);
    }
}

public class NodeEditor : GraphView
{
    public NodeEditor(EditorWindow editorWindow)
    {
        // �m�[�h��ǉ�
        AddElement(new ExampleNode());

        // �e�̃T�C�Y�ɍ��킹��GraphView�̃T�C�Y��ݒ�
        this.StretchToParentSize();

        // MMB�X�N���[���ŃY�[���C���A�E�g���ł���悤��
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        // MMB�h���b�O�ŕ`��͈͂𓮂�����悤��
        this.AddManipulator(new ContentDragger());
        // LMB�h���b�O�őI�������v�f�𓮂�����悤��
        this.AddManipulator(new SelectionDragger());
        // LMB�h���b�O�Ŕ͈͑I�����ł���悤��
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new GraphViewCopyPaste());

        // �E�N���b�N���j���[��ǉ�
        var menuWindowProvider = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
        menuWindowProvider.Initialize(this, editorWindow);
        nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
        };
    }

    // GetCompatiblePorts���I�[�o�[���C�h����
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

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




    //******************************************//
    //�m�[�h�̐ݒ�//
    //*****************************************//

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

            _graphView.AddElement(node);
            return true;
        }
    }

