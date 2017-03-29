using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;


public class GetVersion : MonoBehaviour {

	// Use this for initialization
    private string resPath;
	void Start () {
        resPath = Application.dataPath + "/Res/";
        SetMD5();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetMD5() {
        // 获取Res文件夹下所有文件的相对路径和MD5值  
        string[] files = Directory.GetFiles(resPath, "*", SearchOption.AllDirectories);
        Debug.Log("resPath==" + resPath);
        Debug.Log(files.Length);
        StringBuilder versions = new StringBuilder();
        for (int i = 0, len = files.Length; i < len; i++)
        {
            string filePath = files[i];
            string extension = filePath.Substring(files[i].LastIndexOf("."));
            if (extension == ".assetbundle")
            {
                string relativePath = filePath.Replace(resPath, "").Replace("\\", "/");
                string md5 = MD5File(filePath);
                versions.Append(relativePath).Append(",").Append(md5).Append("\n");
            }
        }
        // 生成配置文件  
        FileStream stream = new FileStream(resPath + "version.txt", FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close(); 
    }
    public static string MD5File(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }


}
