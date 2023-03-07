using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;


public class TextureEditor : EditorWindow
{
    private int textureWidth;
    private int textureHeight;
    private Texture2D _texture;
    public Texture2D _completeTexture;
    public int[] textureSize;
    // �C�ӂ̊֐������s���邽�߂̃f���Q�[�g
    private delegate void MyFunctionDelegate();
    private MyFunctionDelegate myFunction;
    public int brushSize = 10; // �u���V�̃T�C�Y
    public bool paintOnMouseDrag = true; // �}�E�X�h���b�O���Ƀy�C���g���邩�ǂ���
    Color color;
    int layoutOffset = 450;


    // �摜���N���b�N�����Ƃ��Ɏ��s����֐�
    private string fileName = "test.png"; // �쐬����t�@�C����
    private string directoryPath;// �쐬����f�B���N�g���̃p�X
    private bool isPainting = false; // �y�C���g�����ǂ���
    private List<Vector2Int> paintedIndices = new List<Vector2Int>(); // �y�C���g�ς݂̃C���f�b�N�X���ꎞ�I�Ɋi�[���郊�X�g
    private Queue<Vector2Int> paintQueue = new Queue<Vector2Int>();
    
    /// <summary>
    /// ����������
    /// </summary>
    private void OnEnable()
    {
        directoryPath = Application.dataPath;
        myFunction = clickBtn;
        color = Color.red;
        if (_texture == null)
        {
            _texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBAFloat, false);
            for (int y = 0; y < _texture.height; y++)
                for (int x = 0; x < _texture.width; x++)
                    _texture.SetPixel(x, y, new Color(1f, 1f, 1f, 0f));
        }
    }

    [MenuItem("Original/Texture Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<TextureEditor>("Texture Editor");
        
    }

    private void OnGUI()
    {
        //�e�N�X�`���T�C�Y�̓��͗�
        GUILayout.Label("Texture Size");
        GUILayout.BeginHorizontal();
        GUILayout.Label("x:");
        textureWidth = EditorGUILayout.IntField(textureWidth);
        GUILayout.Label("y:");
        textureHeight = EditorGUILayout.IntField(textureHeight);
        
        if (GUILayout.Button("Resize Texture"))
        {
            ResizeTexture();
        }
        if (GUILayout.Button("Reset Texture"))
        {
            _texture = null;
            _texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBAFloat, false);
            for (int y = 0; y < _texture.height; y++)
                for (int x = 0; x < _texture.width; x++)
                    _texture.SetPixel(x, y, new Color(1f, 1f, 1f, 0f));
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("�h���b�O���Ƀy�C���g���邩�ǂ���");
        paintOnMouseDrag = EditorGUILayout.Toggle(paintOnMouseDrag);
        GUILayout.Label("�u���V�T�C�Y");
        brushSize = EditorGUILayout.IntField(brushSize);
        color = EditorGUILayout.ColorField(color,null);
        if (GUILayout.Button("Paint All"))
        {
            _texture = null;
            _texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBAFloat, false);
            for (int y = 0; y < _texture.height; y++)
                for (int x = 0; x < _texture.width; x++)
                    _texture.SetPixel(x, y, color);
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Invert Alpha"))
        {
            InvertAlpha32(_texture);
        }
        
        if (_texture!=null)
        {
            EditorGUI.DrawTextureTransparent(new Rect(0, layoutOffset, position.width, position.height - layoutOffset), _texture);
        }
        
        

        GUILayout.Label("�쐬����t�@�C����");
        fileName = GUILayout.TextField(fileName);

        GUILayout.Label("�쐬����f�B���N�g���̃p�X");
        directoryPath = GUILayout.TextField(directoryPath);

        // �{�^�����쐬���āA�N���b�N���ꂽ��t�@�C�����쐬���鏈�������s����
        if (GUILayout.Button("�t�@�C�����쐬"))
        {
            string filePath = Path.Combine(directoryPath, fileName);

            if (!File.Exists(filePath))
            {
                byte[] bytes = _texture.EncodeToPNG();
                File.WriteAllBytes(filePath, bytes);
            }
            else
            {
                byte[] bytes = _texture.EncodeToPNG();
                File.WriteAllBytes(filePath, bytes);
                Debug.LogWarning("�t�@�C�������łɑ��݂��Ă��܂�: " + filePath);
            }
        }

        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.sharedMaterial;
                if (material != null)
                {
                    EditorGUILayout.LabelField("Material Info");
                    EditorGUILayout.LabelField("Material Name: " + material.name);
                    EditorGUILayout.LabelField("Material Type: " + material.shader.name);
                    EditorGUILayout.LabelField("Render Queue: " + material.renderQueue);
                    if (GUILayout.Button("setTexture"))
                    {
                        material.SetTexture("_DirtyTex", _texture);
                    }
                    if (GUILayout.Button("getTexture"))
                    {
                        Texture texture = material.GetTexture("_DirtyTex");
                        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 0, RenderTextureFormat.Default);
                        Graphics.Blit(texture, renderTexture);
                        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
                        RenderTexture.active = renderTexture;
                        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                        texture2D.Apply();
                        RenderTexture.active = null;
                        _texture = texture2D;
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No Material assigned to Renderer.");
                }
            }
            else
            {
                EditorGUILayout.LabelField("Selected GameObject does not have a Renderer component.");
            }
        }
        else
        {
            EditorGUILayout.LabelField("No GameObject selected.");
        }


        /*
        // �I���W�i���̉摜�����[�h����
        if (originalTexture == null)
        {
            originalTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Example/MyTexture.png");
        }

        // �I���W�i���̉摜��`�悷��
        GUI.DrawTexture(new Rect(30, 200, 50, 50), originalTexture);

        // �摜���N���b�N������C�ӂ̊֐������s����
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && 
            new Rect(30, 200, 50, 50).Contains(Event.current.mousePosition))
        {
            if (myFunction != null)
            {
                myFunction();
            }
        }
        */

        if (!new Rect(0, layoutOffset, position.width, position.height - layoutOffset).Contains(Event.current.mousePosition))
        {
            return;
        }
            Vector2 mousePosition = Event.current.mousePosition;
        mousePosition.x = mousePosition.x / position.width * textureWidth;
            mousePosition.y = (1 - ((mousePosition.y - layoutOffset) / (position.height - layoutOffset))) * textureHeight;
            Vector2Int uvPosi = new Vector2Int(Mathf.CeilToInt(mousePosition.x), (Mathf.CeilToInt(mousePosition.y)));
        

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                
                
                Paint(uvPosi);
                Repaint();
            }
        if (paintOnMouseDrag && (Event.current.type == EventType.MouseDrag))
        {
            
            if (!paintedIndices.Contains(uvPosi))
            {
                paintedIndices.Add(uvPosi);
            }
        }
        if(Event.current.type == EventType.MouseUp)
        {
            foreach(var elem in paintedIndices)
            {
                Paint(elem);
            }
            paintedIndices.Clear();
            
        }
    }

   void clickBtn()
    {
        Debug.Log("clickBtn");
        color = Color.blue;
    }

    //�e�N�X�`�������T�C�Y����Ƃ��̊֐�
    private void ResizeTexture()
    {
        /*
        TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_texture)) as TextureImporter;
        textureImporter.isReadable = true;
        */
        // ���T�C�Y��̃e�N�X�`�����쐬
        Texture2D resizedTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        // ���T�C�Y����
        Color[] pixels = new Color[textureWidth * textureHeight];
        float ratioX = (float)_texture.width / (float)textureWidth;
        float ratioY = (float)_texture.height / (float)textureHeight;
        int index = 0;
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                int sourceX = Mathf.FloorToInt(x * ratioX);
                int sourceY = Mathf.FloorToInt(y * ratioY);
                pixels[index] = _texture.GetPixel(sourceX, sourceY);
                index++;
            }
        }
        resizedTexture.SetPixels(pixels);
        resizedTexture.Apply();
        _texture = resizedTexture;
        /*
        // �e�N�X�`���̕ۑ�
        byte[] bytes = resizedTexture.EncodeToPNG();
        string savePath = AssetDatabase.GetAssetPath(_texture);
        savePath = savePath.Substring(0, savePath.LastIndexOf("/") + 1);
        System.IO.File.WriteAllBytes(savePath + _texture.name + "_" + textureWidth + "x" + textureHeight + ".png", bytes);
        AssetDatabase.Refresh();

        textureImporter.isReadable = false;
        */
        }

    void Paint(Vector2Int uv)
    {
        if (_texture == null) return;

        // �}�E�X���W����e�N�X�`����̍��W�����߂�

        // �u���V�͈̔͂����߂�
        int xMin = Mathf.Max(0, (int)(uv.x  - (brushSize - 1)));
        int yMin = Mathf.Max(0, (int)(uv.y  - (brushSize - 1)));
        int xMax = Mathf.Min(textureWidth, (int)(uv.x + (brushSize - 1)));
        int yMax = Mathf.Min(textureHeight,(int)(uv.y + (brushSize - 1)));

        // �u���V�̃T�C�Y�ɉ����ăe�N�X�`����ύX����
        for (int y = yMin; y <= yMax; y++)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                    _texture.SetPixel(x, y, color);
                
            }
        }

        // �e�N�X�`�����X�V����
        _texture.Apply();
    }

    private void StartPainting()
    {
        Debug.Log("StartPainting()");
        Thread paintThread = new Thread(PaintThread);
        paintThread.Start();
    }

    private void PaintThread()
    {
        //isPainting = true;
        List<Vector2Int> paintedPositions = new List<Vector2Int>();

        while (paintQueue.Count > 0)
        {
            Vector2Int position = paintQueue.Dequeue();
            Paint(position);
            paintedPositions.Add(position);
        }
            isPainting = false;
            paintedIndices.Clear();
    }

    public void InvertAlpha32(Texture2D texture)
    {
        // �e�N�X�`���̃s�N�Z���f�[�^���擾����
        Color32[] pixels = texture.GetPixels32();

        // ���ׂẴs�N�Z���𑖍����A���l�𔽓]������
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 pixel = pixels[i];
            // ���l��0���傫���ꍇ��0�ɕύX���A0�̏ꍇ��255�ɕύX����
            if (pixel.a > 0)
            {
                pixel.a = 0;
            }
            else
            {
                pixel.a = 255;
            }
            pixels[i] = pixel;
        }

        // �s�N�Z���f�[�^���e�N�X�`���ɏ������݁A�K�p����
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    private Vector2 ConvertToTexturePosition(Vector2 mousePosition)
    {
        float textureX = mousePosition.x / Screen.width * _texture.width;
        float textureY = mousePosition.y / Screen.height * _texture.height;
        return new Vector2(textureX, textureY);
    }

    private bool IsInsideTexture(Vector2Int position)
    {
        return position.x >= 0 && position.x < _texture.width &&
               position.y >= 0 && position.y < _texture.height;
    }
}