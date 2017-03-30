using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ResUpdate : MonoBehaviour {


    public string VERSION_FILE;
    public string LOCAL_RES_URL;
    public string SERVER_RES_URL;
    public string LOCAL_RES_PATH;

    private Dictionary<string,string> LocalResVersion;
    private Dictionary<string, string> ServerResVersion;
    private List<string> NeedDownFile;
    private bool NeedUpdateLocalVersionFile = false;
    void Awake() {
        VERSION_FILE = "version.txt";
        LOCAL_RES_URL = "file:///" + Application.dataPath + "/Res/";
        SERVER_RES_URL = "file:///E:/Res/";
        LOCAL_RES_PATH = Application.dataPath + "/Res/";
        Debug.Log("Application.dataPath ==" + Application.dataPath);
    }

    void Start() {
        LocalResVersion = new Dictionary<string,string>();
        ServerResVersion = new Dictionary<string,string>();
        NeedDownFile = new List<string>();
        StartCoroutine(GetLocalVersion());

    }

    private IEnumerator GetLocalVersion() {
        WWW www = new WWW(LOCAL_RES_URL + VERSION_FILE);
        yield return www;
        ParseVersionFile(www.text,LocalResVersion);
        yield return null;
        StartCoroutine(DownLoad(SERVER_RES_URL + VERSION_FILE, delegate(WWW serverVersion)
        {
            ParseVersionFile(serverVersion.text, ServerResVersion);
            CompareVersion();
            DownLoadRes();
        }));

    
    }

    private void DownLoadRes() {
        Debug.Log("Begin DowloadRes");
        if (NeedDownFile.Count == 0) {

            UpdateLocalVersionFile();
            return;
        }
        string file = NeedDownFile[0];
        NeedDownFile.RemoveAt(0);
        StartCoroutine(this.DownLoad(SERVER_RES_URL + file, delegate(WWW w)
        {

            ReplaceLocalRes(file, w.bytes);
            DownLoadRes(); 
        }));
        Debug.Log("End DowloadRes");
    }

    private void ReplaceLocalRes(string fileName, byte[] data)
    {
        Debug.Log("Begin ReaplaceLocalRes");
        string filePath = LOCAL_RES_PATH + fileName;
        FileStream stream = new FileStream(LOCAL_RES_PATH + fileName, FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
        Debug.Log("End ReplaceLocalRes");
    }   
    private void UpdateLocalVersionFile() {
        Debug.Log("Begin UpdateLocalVersionFile");
        if (NeedUpdateLocalVersionFile) {
            StringBuilder version = new StringBuilder();
            foreach(var item in ServerResVersion){
                version.Append(item.Key).Append(",").Append(item.Value).Append("\n");

            }

            FileStream stream = new FileStream(LOCAL_RES_PATH + VERSION_FILE,FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(version.ToString());
            stream.Write(data,0,data.Length);
            stream.Flush();
            stream.Close();
            Debug.Log("End UpdateLocalVersionFile");
        }
        //StartCoroutine(Show());
    
    }
    private void ParseVersionFile(string content, Dictionary<string,string> dict) {
        Debug.Log("Begin ParseVersionFile");
        if (content == null || content.Length == 0) {
            Debug.LogError("content is null");
            return;
        }
        Debug.Log("content is =="+content);
        string[] items = content.Split(new char[] { '\n' });
        foreach (string item in items) {
            string[] info = item.Split(new char[] { ',' });
            if (info != null && info.Length == 2) {
                Debug.Log("info0=="+info[0]);
                Debug.Log("info1==" + info[1]);
                dict.Add(info[0],info[1]);
            }
        }
        Debug.Log("End ParseVersionFile");
    }

    private IEnumerator Show() {
        Debug.Log("Begin Show");
        WWW asset = new WWW(LOCAL_RES_URL + "cube.assetbundle");
        yield return asset;
        AssetBundle bundle = asset.assetBundle;
        Instantiate(bundle.LoadAsset("Prefab"));
        bundle.Unload(false);
        Debug.Log("End Show");
    }

    private void CompareVersion() {
        Debug.Log("Begin CompareVersion");
        foreach(var version in ServerResVersion){
            string fileName = version.Key;
            string serverMd5 = version.Value;
            if (!LocalResVersion.ContainsKey(fileName))
            {
                NeedDownFile.Add(fileName);
            }
            else {
                string localMd5;
                    LocalResVersion.TryGetValue(fileName,out localMd5);
                if(!serverMd5.Equals(localMd5)){
                    NeedDownFile.Add(fileName);
                }
            }
        }
        NeedUpdateLocalVersionFile = NeedDownFile.Count > 0;
        Debug.Log("End CompareVersion");
    }
   

    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        Debug.Log("Begin Downlaod");

        WWW www = new WWW(url);
        Debug.Log("------------------------"+www.progress);
        yield return www;
        Debug.Log("------------------------" + www.progress);
        if (finishFun != null)
        {
            finishFun(www);
        }
        www.Dispose();
        Debug.Log("End Download");
    }

    public delegate void HandleFinishDownload(WWW www);  

}