using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Build AssetBundles/Build AssetBundles Compressed")]
    static void BuildAllAssetBundlesCompressed()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.None, 
                                        BuildTarget.StandaloneWindows);
    }
    
    [MenuItem("Build AssetBundles/Build AssetBundles Uncompressed")]
    static void BuildAllAssetBundlesUncompressed()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.UncompressedAssetBundle, 
                                        BuildTarget.StandaloneWindows);
    }
}