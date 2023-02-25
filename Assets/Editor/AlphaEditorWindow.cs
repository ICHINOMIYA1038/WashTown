using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


public class AlphaEditorWindow : EditorWindow
{
    private Texture2D _texture;

    [MenuItem("Original/Alpha Editor Window")]
    private static void OpenWindow()
    {
        var window = GetWindow<AlphaEditorWindow>();
        window.titleContent = new GUIContent("Alpha Editor");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a texture:");

        // �e�N�X�`����I������
        _texture = (Texture2D)EditorGUILayout.ObjectField(_texture, typeof(Texture2D), false);

        // �e�N�X�`�����I������Ă���ꍇ
        if (_texture != null)
        {
            // �e�N�X�`���̃T�C�Y���擾����
            var textureSize = new Vector2(_texture.width, _texture.height);

            // �e�N�X�`����`�悷��
            GUILayout.Box(_texture, GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));

            // ���l��ύX����{�^����\������
            if (GUILayout.Button("Invert Alpha"))
            {
                InvertAlpha(_texture);
            }
        }
    }

    private void InvertAlpha(Texture2D texture)
    {
        // �e�N�X�`���̃s�N�Z���f�[�^���擾����
        Color32[] pixels = texture.GetPixels32();

        // ���ׂẴs�N�Z���𑖍����A���l�𔽓]������
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 pixel = pixels[i];
            // ���l��0�̏ꍇ��1�ɁA0�ȊO�̏ꍇ��0�ɕύX����
            pixel.a = (pixel.a == 0) ? (byte)1 : (byte)0;
            pixels[i] = pixel;
        }

        // �s�N�Z���f�[�^���e�N�X�`���ɏ������݁A�K�p����
        texture.SetPixels32(pixels);
        texture.Apply();
    }
}
