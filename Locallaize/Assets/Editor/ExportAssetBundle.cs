using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections;

static public class ExportAssetBundle{

    [MenuItem("Assets/Build AssetBundle From Selection")]
    static void ExportResourceRGB2()
    {
        // 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        if (path.Length != 0)
        {
            // 选择的要保存的对象  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包  
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);
        }
    }

    [MenuItem("Assets/Save Scene")]
    
    static void ExportScene()
    {
        // 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("s", "E:/", "B", "unity3d");

        if (path.Length != 0)
        {
            // 选择的要保存的对象  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            string[] scenes = {};
            //打包  
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.StandaloneWindows, BuildOptions.BuildAdditionalStreamedScenes);
        }
    } 
   /* static void MyBuild()
    {
        // 需要打包的场景名字
        string[] levels = { "Assets/2.unity" };
        // 注意这里【区别】通常我们打包，第2个参数都是指定文件夹目录，在此方法中，此参数表示具体【打包后文件的名字】
        // 记得指定目标平台，不同平台的打包文件是不可以通用的。最后的BuildOptions要选择流格式
        BuildPipeline.BuildPlayer(levels, Application.dataPath + "/Scene.unity3d", BuildTarget.Android, BuildOptions.BuildAdditionalStreamedScenes);
        // 刷新，可以直接在Unity工程中看见打包后的文件
        AssetDatabase.Refresh();

    }*/
}

