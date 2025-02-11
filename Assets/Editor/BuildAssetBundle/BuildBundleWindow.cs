using System;
using Tools;
using UnityEditor;
using UnityEngine;

namespace Editor.BuildAssetBundle
{
    public class BuildBundleWindow : EditorWindow
    {
        // ReSharper disable once InconsistentNaming
        private const int BTN_BUILD_INFO_WIDTH = 150; //构建信息文本宽度

        /// <summary>
        ///     打包方式
        /// </summary>
        private int _selectType;

        public BuildBundleWindow()
        {
            titleContent = new GUIContent("打包选项窗口");
        }

        [MenuItem("Window/BuildBundle")]
        private static void ShowWindow()
        {
            GetWindow<BuildBundleWindow>();
        }

        protected void OnGUI()//如果inspector面板出现重影，把这里改成OnGUI，编译代码后再改回来
        {
            EditorGUILayout.BeginVertical(); //整体开始
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(20, 20, 20, 20),
                normal  = { background = Texture2D.grayTexture }
            }); //打包信息开始
            //标题
            EditorGUILayout.LabelField("打包设置窗口", new GUIStyle
            {
                fontSize  = 24,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal    = { textColor = Color.white }
            });
            EditorGUILayout.Space(20);

            //信息展示
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bundle文件夹路径", GUILayout.Width(BTN_BUILD_INFO_WIDTH));
            if (GUILayout.Button(BuildBundleTools._BundlePath)) System.Diagnostics.Process.Start(BuildBundleTools._BundlePath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AB包输出路径", GUILayout.Width(BTN_BUILD_INFO_WIDTH));
            if (GUILayout.Button(BuildBundleTools._OutputPath)) System.Diagnostics.Process.Start(BuildBundleTools._OutputPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Asset_Version输出路径", GUILayout.Width(BTN_BUILD_INFO_WIDTH));
            if (GUILayout.Button(BuildBundleTools._ManifestOutputPath)) System.Diagnostics.Process.Start(BuildBundleTools._ManifestOutputPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            //打包相关设置
            _selectType = EditorGUILayout.Popup("打包方式", _selectType, Enum.GetNames(typeof(BuildBundleTools.BuildType)));
            if (_selectType != (int)BuildBundleTools._CurType) BuildBundleTools._CurType = (BuildBundleTools.BuildType)_selectType;

            EditorGUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("开始打包", new GUIStyle(GUI.skin.button)
                {
                    fontSize  = 24,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    normal    = { textColor = Color.yellow },
                    padding   = new RectOffset(15, 15, 10, 10)
                }))
                BuildBundleTools.BuildBundle();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical(); //打包信息结束

            //资源更改相关设置
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(20, 20, 20, 20),
                normal  = { background = Texture2D.grayTexture }
            }); //资源更改设置开始

            //标题
            EditorGUILayout.LabelField("资源检测开关", new GUIStyle
            {
                fontSize  = 24,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal    = { textColor = Color.white }
            });
            EditorGUILayout.Space(20);

            //设置按钮
            AssetChangeTools.AssetChangeTools._AllAssetVerifyEnable = GUILayout.Toggle(AssetChangeTools.AssetChangeTools._AllAssetVerifyEnable, "全部资源检测");
            foreach (var value in AssetChangeTools.AssetChangeTools._AssetVerifyList)
            {
                EditorGUILayout.Space(4);
                value._Enable = GUILayout.Toggle(value._Enable, value._WindowName);
            }

            //AssetManifest文件路径
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AssetManifest文件夹路径", GUILayout.Width(BTN_BUILD_INFO_WIDTH));
            if (GUILayout.Button(PathTools._AssetManifestPath)) System.Diagnostics.Process.Start(PathTools._AssetManifestPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("重置AssetManifest文件", new GUIStyle(GUI.skin.button)
                {
                    fontSize  = 20,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    normal    = { textColor = Color.yellow },
                    padding   = new RectOffset(15, 15, 10, 10)
                }))
                AssetChangeTools.AssetChangeTools.ResetAssetManifest();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical(); //资源更改设置结束

            EditorGUILayout.EndVertical(); //整体结束
        }
    }
}