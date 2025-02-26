using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Managers;
using Newtonsoft.Json;
using Tools;
using UnityEditor;

namespace Editor.AssetChangeTools
{
    public class AssetChangeTools : AssetPostprocessor
    {
        /// <summary>
        ///     全部资源检测是否开启
        /// </summary>
        private static bool _allAssetVerifyEnable = true;

        /// <summary>
        ///     资源字典，包含项目中已导入的资源《资源名称，资源路径》
        /// editor下使用，每次修改json文件都需要读取整个文件，修改，再写回i，后期考虑使用数据库
        /// </summary>
        public static Dictionary<string, string> _AssetDic { get; private set; } = new(); //这个静态字典在Editor下，因此在unity处于编辑器状态下会常驻内存直到unity关闭或手动清理

        /// <summary>
        ///     资源验证列表
        /// </summary>
        public static List<IAssetVerify> _AssetVerifyList { get; } = new();

        public static bool _AllAssetVerifyEnable
        {
            get => _allAssetVerifyEnable;
            set
            {
                _allAssetVerifyEnable = value;
                foreach (var assetVerify in _AssetVerifyList) assetVerify._Enable = value;
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets.Length > 0)
                AssetVerifyImport(importedAssets);

            if (deletedAssets.Length > 0)
                AssetVerifyDelete(deletedAssets);

            if (movedAssets.Length > 0 && movedFromAssetPaths.Length > 0)
                AssetVerifyMove(movedAssets, movedFromAssetPaths);
        }

        [InitializeOnLoadMethod] //在编译器启动或重新编译脚本后调用
        private static void Init()
        {
#if UNITY_EDITOR
            if (_AssetDic.Count <= 0)
            {
                using StreamReader reader = new StreamReader(PathTools._AssetManifestPath);

                string context = reader.ReadToEnd();
                _AssetDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(context);
            }
#endif
            _AssetVerifyList.Add(new AssetVerifyPrefab());
        }

        /// <summary>
        ///     导入资源验证
        /// </summary>
        /// <param name="importedAssets">导入资源的路径</param>
        private static void AssetVerifyImport(string[] importedAssets)
        {
            foreach (var assetVerify in _AssetVerifyList) assetVerify.AssetVerifyImport(importedAssets);
        }

        /// <summary>
        ///     删除资源验证
        /// </summary>
        /// <param name="deletedAssets">删除资源的路径</param>
        private static void AssetVerifyDelete(string[] deletedAssets)
        {
            foreach (var assetVerify in _AssetVerifyList) assetVerify.AssetVerifyDelete(deletedAssets);
        }

        /// <summary>
        ///     移动资源验证
        /// </summary>
        /// <param name="movedAssets">移动资源的路径</param>
        /// <param name="movedFromAssetPaths">移动资源的原路径</param>
        private static void AssetVerifyMove(string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetVerify in _AssetVerifyList) assetVerify.AssetVerifyMove(movedAssets, movedFromAssetPaths);
        }

        /// <summary>
        ///     更新AssetManifest文件
        /// </summary>
        public static void UpdateAssetManifest()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            if (!File.Exists(PathTools._AssetManifestPath))
            {
                LogTools.Log($"不存在指定路径：{PathTools._AssetManifestPath}，创建新的文件");
                File.Create(PathTools._AssetManifestPath).Close();
            }

            string context = JsonConvert.SerializeObject(_AssetDic);

            using StreamWriter writer = new StreamWriter(PathTools._AssetManifestPath);
            writer.Write(context);
            stopwatch.Stop();
            LogTools.Log($"AssetManifest已更新，用时：【{stopwatch.ElapsedMilliseconds}毫秒】");
        }

        /// <summary>
        ///     重置AssetManifest文件
        /// </summary>
        public static void ResetAssetManifest()
        {
            File.Delete(PathTools._AssetManifestPath);
            _AssetDic.Clear();
            foreach (var assetVerify in _AssetVerifyList) assetVerify.ResetAssetManifest();
            UpdateAssetManifest();
        }
    }
}