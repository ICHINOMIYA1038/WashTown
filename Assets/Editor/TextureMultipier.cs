using UnityEngine;
using UnityEditor;

public class TextureMultiplier : EditorWindow
{
    private Texture2D _texture1;
    private Texture2D _texture2;
    private string _outputPath = "Assets/OutputTexture.png";

    [MenuItem("Original/Texture Multiplier")]
    private static void ShowWindow()
    {
        var window = GetWindow<TextureMultiplier>();
        window.titleContent = new GUIContent("Texture Multiplier");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(20f);

        // �e�N�X�`��1�̓��͗�
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Texture 1:");
        _texture1 = (Texture2D)EditorGUILayout.ObjectField(_texture1, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        // �e�N�X�`��2�̓��͗�
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Texture 2:");
        _texture2 = (Texture2D)EditorGUILayout.ObjectField(_texture2, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20f);

        // �o�͐�̃p�X�̓��͗�
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Output Path:");
        _outputPath = EditorGUILayout.TextField(_outputPath);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20f);

        // ���s�{�^��
        if (GUILayout.Button("Multiply Textures"))
        {
            var textureImporter1 = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_texture1)) as TextureImporter;
            textureImporter1.isReadable = true;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_texture1));

            var textureImporter2 = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_texture2)) as TextureImporter;
            textureImporter2.isReadable = true;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_texture2));
            DebugLogTextureFormat(_texture1);
            DebugLogTextureFormat(_texture2);
            InvertAlpha(_texture1);
            MultiplyTextures();
        }
    }


    public void MultiplyTextures()
    {
        // �e�N�X�`����2���͂���Ă��邩�m�F����
        if (_texture1 == null || _texture2 == null)
        {
            Debug.LogError("Please input two textures.");
            return;
        }

        // �e�N�X�`���̃T�C�Y���������m�F����
        if (_texture1.width != _texture2.width || _texture1.height != _texture2.height)
        {
            Debug.LogError("Texture sizes are different.");
            return;
        }

        // �o�͂���e�N�X�`�����쐬����
        var outputTexture = new Texture2D(_texture1.width, _texture1.height);

        // ���ׂẴs�N�Z���𑖍����A�e�N�X�`������Z����
        for (int y = 0; y < outputTexture.height; y++)
        {
            for (int x = 0; x < outputTexture.width; x++)
            {
                // 2�̃e�N�X�`���̃s�N�Z�����擾����
                var color1 = _texture1.GetPixel(x, y);
                var color2 = _texture2.GetPixel(x, y);

                // 2�̃s�N�Z���̐F����Z���āA�o�͂���F���쐬����
                var outputColor = new Color(color1.r * color2.r, color1.g * color2.g, color1.b * color2.b, color1.a * color2.a);
                outputTexture.SetPixel(x, y, outputColor);
            }
        }

        // �s�N�Z���f�[�^���e�N�X�`���ɏ������݁A�K�p����
        outputTexture.Apply();

        // �e�N�X�`����PNG�`���ɃG���R�[�h���ăt�@�C���ɕۑ�����
        var bytes = outputTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_outputPath, bytes);

        // Unity��AssetDatabase���X�V����
        AssetDatabase.Refresh();
        Debug.Log("Output texture has been created.");
    }

    //����̃e�N�X�`���̃��l�̔��]
    public void InvertAlpha(Texture2D texture)
    {
        // �e�N�X�`���̃s�N�Z���f�[�^���擾����
        Color[] pixels = texture.GetPixels();

        // ���ׂẴs�N�Z���𑖍����A���l�𔽓]������
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            // ���l��0���傫���ꍇ��0�ɕύX���A0�̏ꍇ��1.0f�ɕύX����
            if (pixel.a > 0)
            {
                pixel.a = 0;
            }
            else
            {
                pixel.a = 1.0f;
            }
            pixels[i] = pixel;
        }

        // �s�N�Z���f�[�^���e�N�X�`���ɏ������݁A�K�p����
        texture.SetPixels(pixels);
        texture.Apply();
    }

    //�t�H�[�}�b�g�G���[���N�����̂ŁA���̂Ƃ��͂��������g���B
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

    public void DebugLogTextureFormat(Texture2D texture)
    {
        // �e�N�X�`���̃t�H�[�}�b�g���擾����
        TextureFormat format = texture.format;

        // �e�N�X�`���̃t�H�[�}�b�g�𕶎���ɕϊ�����
        string formatString = format.ToString();

        // �f�o�b�O���O�Ƀe�N�X�`���̃t�H�[�}�b�g���o�͂���
        Debug.Log("Texture format: " + formatString);
    }


    public Texture2D ConvertDXT5(Texture2D srcTexture)
    {
        Texture2D texture = new Texture2D(srcTexture.width, srcTexture.height, TextureFormat.DXT5, false);
        texture.SetPixels(srcTexture.GetPixels());
        texture.Apply();
        return texture;
    }
}