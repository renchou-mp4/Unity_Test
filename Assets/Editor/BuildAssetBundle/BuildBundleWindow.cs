using Sirenix.OdinInspector.Editor;
using System;
using UnityEditor;
using UnityEngine;

public class BuildBundleWindow : OdinEditorWindow
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


    private const int Btn_Build_Width = 200;
    private const int Btn_Build_Height = 50;
    //打包方式
    private int selectType = 0;
    protected override void OnImGUI()
    {
        //标题
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("打包设置窗口", new GUIStyle
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white },
        });
        EditorGUILayout.Space(20);

        //信息展示

        EditorGUILayout.TextField("Bundle文件夹路径", BuildBundleManager._BundlePath);
        EditorGUILayout.TextField("输出路径", BuildBundleManager._OutputPath);

        //打包相关设置
        selectType = EditorGUILayout.Popup("打包方式", selectType, Enum.GetNames(typeof(BuildBundleManager.BuildType)));
        if (selectType != (int)BuildBundleManager._CurType)
        {
            BuildBundleManager._CurType = (BuildBundleManager.BuildType)selectType;
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("开始打包", new GUIStyle(GUI.skin.button)
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
