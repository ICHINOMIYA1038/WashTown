using UnityEngine;
using UnityEditor;

// �J�X�^���G�f�B�^�̃N���X
[CustomEditor(typeof(Texture2D))]
public class Texture2DEditor : Editor
{
    private Texture2D texture;  // �\������e�N�X�`��
    private Color color = Color.white; // �e�N�X�`���ɓK�p����F

    // �G�f�B�^�[��GUI��\������
    public override void OnInspectorGUI()
    {
        // �e�N�X�`���̏����擾����
        texture = target as Texture2D;

        // �e�N�X�`����null�̏ꍇ�͉������Ȃ�
        if (texture == null)
        {
            return;
        }

        // �e�N�X�`���̃v���r���[��\������
        GUILayout.Label(texture, GUILayout.MaxHeight(200));

        // �e�N�X�`���ɓK�p����F��ݒ肷��
        color = EditorGUILayout.ColorField("Color", color);

        // �e�N�X�`���̃s�N�Z���f�[�^���擾����
        Color[] pixels = texture.GetPixels();

        // �s�N�Z���f�[�^��ύX����
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] *= color;
        }

        // �ύX�����s�N�Z���f�[�^���e�N�X�`���ɓK�p����
        texture.SetPixels(pixels);
        texture.Apply();
    }
}