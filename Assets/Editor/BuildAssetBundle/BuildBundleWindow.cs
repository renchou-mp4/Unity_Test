using UnityEditor;
using UnityEngine;

public class BuildBundleWindow : EditorWindow
{

    public BuildBundleWindow()
    {
        this.titleContent = new UnityEngine.GUIContent("打包选项窗口");
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
        GUILayout.Label("打包设置窗口");

        EditorGUILayout.TextField("Bundle文件夹路径", BuildBundleTool._BundlePath);
        EditorGUILayout.TextField("输出路径", BuildBundleTool._OutputPath);
    }
}
