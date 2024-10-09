using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class BuildBundleWindow : OdinEditorWindow
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


    private int Btn_Build_Width = 200;
    private int Btn_Build_Height = 50;
    protected override void OnImGUI()
    {
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("������ô���", new GUIStyle
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white },
        });
        EditorGUILayout.Space(20);


        EditorGUILayout.TextField("Bundle�ļ���·��", BuildBundleTool._BundlePath);
        EditorGUILayout.TextField("���·��", BuildBundleTool._OutputPath);

        GUILayout.BeginArea(new Rect((Screen.width - Btn_Build_Width) / 2, Screen.height - 100, Screen.width, Btn_Build_Height));

        GUILayout.BeginHorizontal();


        if (GUILayout.Button("��ʼ���", new GUIStyle(GUI.skin.button)
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.yellow },

        }, GUILayout.Width(Btn_Build_Width), GUILayout.Height(Btn_Build_Height)))
        {
            StartBuildBundle();
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void StartBuildBundle()
    {

    }
}
