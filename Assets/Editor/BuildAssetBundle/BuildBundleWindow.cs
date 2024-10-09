using Sirenix.OdinInspector.Editor;
using System;
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


    private const int Btn_Build_Width = 200;
    private const int Btn_Build_Height = 50;
    //�����ʽ
    private int selectType = 0;
    protected override void OnImGUI()
    {
        //����
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("������ô���", new GUIStyle
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white },
        });
        EditorGUILayout.Space(20);

        //��Ϣչʾ

        EditorGUILayout.TextField("Bundle�ļ���·��", BuildBundleManager._BundlePath);
        EditorGUILayout.TextField("���·��", BuildBundleManager._OutputPath);

        //����������
        selectType = EditorGUILayout.Popup("�����ʽ", selectType, Enum.GetNames(typeof(BuildBundleManager.BuildType)));
        if (selectType != (int)BuildBundleManager._CurType)
        {
            BuildBundleManager._CurType = (BuildBundleManager.BuildType)selectType;
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
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
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void StartBuildBundle()
    {
        BuildBundleManager.BuildBundle();
    }
}
