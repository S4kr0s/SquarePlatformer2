#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SpriteCreator : EditorWindow
{
    [MenuItem("Sakros' Tools/Sprite Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SpriteCreator));
    }

    Color color = Color.white;

    private void OnGUI()
    {
        GUILayout.Label("Sprite Creator", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Select Color");
        color = EditorGUILayout.ColorField(color);
        if(GUILayout.Button("Create Color"))
        {
            Texture2D newTexture = new Texture2D(1, 1);
            newTexture.SetPixel(0, 0, color);
            byte[] bytes = newTexture.EncodeToPNG();
            string path = Application.dataPath + $"/Texture{Random.Range(0, 10000)}.png";
            File.WriteAllBytes(path, bytes);
            Debug.Log($"Texture saved: {path}");
        }
    }
}
#endif