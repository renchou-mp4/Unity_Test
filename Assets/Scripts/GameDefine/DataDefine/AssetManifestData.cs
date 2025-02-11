using System;
using UnityEngine.Serialization;

namespace GameDefine.DataDefine
{
    [Serializable]
    public class AssetManifestData
    {
        public string _assetName;
        public string _bundleName;

        public AssetManifestData()
        {
        }

        public AssetManifestData(string assetName, string bundleName)
        {
            _assetName  = assetName;
            _bundleName = bundleName;
        }
    }
}