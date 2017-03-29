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
        // �򿪱�����壬����û�ѡ���·��  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        if (path.Length != 0)
        {
            // ѡ���Ҫ����Ķ���  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //���  
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);
        }
    }

    [MenuItem("Assets/Save Scene")]
    
    static void ExportScene()
    {
        // �򿪱�����壬����û�ѡ���·��  
        string path = EditorUtility.SaveFilePanel("s", "E:/", "B", "unity3d");

        if (path.Length != 0)
        {
            // ѡ���Ҫ����Ķ���  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            string[] scenes = {};
            //���  
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.StandaloneWindows, BuildOptions.BuildAdditionalStreamedScenes);
        }
    } 
   /* static void MyBuild()
    {
        // ��Ҫ����ĳ�������
        string[] levels = { "Assets/2.unity" };
        // ע���������ͨ�����Ǵ������2����������ָ���ļ���Ŀ¼���ڴ˷����У��˲�����ʾ���塾������ļ������֡�
        // �ǵ�ָ��Ŀ��ƽ̨����ͬƽ̨�Ĵ���ļ��ǲ�����ͨ�õġ�����BuildOptionsҪѡ������ʽ
        BuildPipeline.BuildPlayer(levels, Application.dataPath + "/Scene.unity3d", BuildTarget.Android, BuildOptions.BuildAdditionalStreamedScenes);
        // ˢ�£�����ֱ����Unity�����п����������ļ�
        AssetDatabase.Refresh();

    }*/
}

