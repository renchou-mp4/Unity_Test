using System.Collections.Generic;

namespace Managers
{
    public class BundleInfo<T>
    {
        public string _BundleName;
        public int _IndexCount;
        public T _Asset;
    }

    public class BundleManager : MonoSingleton<BundleManager>
    {
        private Dictionary<string, BundleInfo<T>> _loadedBundle = new();

    }
}
