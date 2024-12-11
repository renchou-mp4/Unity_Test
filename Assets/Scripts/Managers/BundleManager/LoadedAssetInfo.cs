using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 已加载的资源信息
    /// </summary>
    public class LoadedAssetInfo
    {
        public int _RefCount;
        private Object _asset;
        public Object _Asset
        {
            get
            {
                _RefCount++;
                return _asset;
            }
            set => _asset = value;
        }
    }
}