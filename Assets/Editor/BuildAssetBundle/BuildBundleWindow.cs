using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.BuildAssetBundle
{
    public class BuildBundleWindow : OdinEditorWindow
    {
        public BuildBundleWindow()
        {
            this.titleContent = new GUIContent("打包选项窗口");
        }

        [MenuItem("Window/BuildBundle")]
        private static void ShowWindow()
        {
            GetWindow<BuildBundleWindow>();
        }


        private const int BTN_BUILD_WIDTH  = 200;   //构建按钮宽
        private const int BTN_BUILD_HEIGHT = 50;    //构建按钮高
        /// <summary>
        /// 打包方式
        /// </summary>
        private int _selectType;
        protected override void OnImGUI()
        {
            //标题
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("打包设置窗口", new GUIStyle
            {
                fontSize  = 24,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal    = { textColor = Color.white },
            });
            EditorGUILayout.Space(20);

            //信息展示
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Bundle文件夹路径", BuildBundleTools._BundlePath);
            if (GUILayout.Button("打开路径", GUILayout.Width(70)))
            {
                System.Diagnostics.Process.Start(BuildBundleTools._BundlePath);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextField("AB包输出路径", BuildBundleTools._OutputPath);
            if (GUILayout.Button("打开路径", GUILayout.Width(70)))
            {
                System.Diagnostics.Process.Start(BuildBundleTools._OutputPath);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Asset_Version输出路径", BuildBundleTools._ManifestOutputPath);
            if (GUILayout.Button("打开路径",GUILayout.Width(70)))
            {
                System.Diagnostics.Process.Start(BuildBundleTools._ManifestOutputPath);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            //打包相关设置
            _selectType = EditorGUILayout.Popup("打包方式", _selectType, Enum.GetNames(typeof(BuildBundleTools.BuildType)));
            if (_selectType != (int)BuildBundleTools._CurType)
            {
                BuildBundleTools._CurType = (BuildBundleTools.BuildType)_selectType;
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("开始打包", new GUIStyle(GUI.skin.button)
                {
                    fontSize  = 24,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    normal    = { textColor = Color.yellow },
                }, GUILayout.Width(BTN_BUILD_WIDTH), GUILayout.Height(BTN_BUILD_HEIGHT)))
            {
                StartBuildBundle();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void StartBuildBundle()
        {
            BuildBundleTools.BuildBundle();
        }
    }
}
