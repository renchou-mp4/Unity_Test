using UnityEditor;
using UnityEngine;

public class BuildBundleWindow : EditorWindow
{

    public BuildBundleWindow()
    {
        this.titleContent = new UnityEngine.GUIContent("���ѡ���");
    }

    [MenuItem("Window/BuildBundle")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow<BuildBundleWindow>();
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUILayout.Label("������ô���");

        EditorGUILayout.TextField("Bundle�ļ���·��", BuildBundleTool._BundlePath);
        EditorGUILayout.TextField("���·��", BuildBundleTool._OutputPath);
    }
}
