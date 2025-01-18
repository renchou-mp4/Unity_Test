namespace Editor.AssetChangeTools
{
    public interface IAssetVerify
    {
        public string _WindowName { get; }
        public bool   _Enable     { get; set; }
        public void   AssetVerifyImport(string[] importedAssets);
        public void   AssetVerifyDelete(string[] deletedAssets);
        public void   AssetVerifyMove(string[]   movedAssets, string[] movedFromAssetPaths);
        public void   ResetAssetManifest();
        public bool   AssetTypeVerify(string importedAssets);
    }
}