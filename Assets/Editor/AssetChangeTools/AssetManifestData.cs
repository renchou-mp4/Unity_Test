using System;
using UnityEngine.Serialization;

namespace Editor.AssetChangeTools
{
    [Serializable]
    public class AssetManifestData
    {
        public string _bundleName;
        public string _bundlePath;

        public AssetManifestData()
        {
        }

        public AssetManifestData(string bundleName, string bundlePath)
        {
            _bundleName = bundleName;
            _bundlePath = bundlePath;
        }
    }
}