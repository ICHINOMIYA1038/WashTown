
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
        // ノードを追加
        AddElement(new ExampleNode());

        // 親のサイズに合わせてGraphViewのサイズを設定
        this.StretchToParentSize();

        // MMBスクロールでズームインアウトができるように
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        // MMBドラッグで描画範囲を動かせるように
        this.AddManipulator(new ContentDragger());
        // LMBドラッグで選択した要素を動かせるように
        this.AddManipulator(new SelectionDragger());
        // LMBドラッグで範囲選択ができるように
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new GraphViewCopyPaste());

        // 右クリックメニューを追加
        var menuWindowProvider = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
        menuWindowProvider.Initialize(this, editorWindow);
        nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
        };
    }

    // GetCompatiblePortsをオーバーライドする
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

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




    //******************************************//
    //ノードの設定//
    //*****************************************//

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

            _graphView.AddElement(node);
            return true;
        }
    }

