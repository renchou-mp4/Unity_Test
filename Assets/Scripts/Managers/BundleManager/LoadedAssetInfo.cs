using System;
using Object = UnityEngine.Object;

namespace Managers
{
    /// <summary>
    ///     已加载的资源信息
    /// </summary>
    [Serializable]
    public class LoadedAssetInfo
    {
        public  int    _refCount;
        private Object _asset;
        public  Object _Asset { get; set; }
    }
}