﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using SevenZip.Compression.LZMA;
public class CompressAndDecompress : Editor {

    [MenuItem("Compress/Cpmpress Unity3D File")]
    static void CompressUnity3DFile() {
        string path = EditorUtility.OpenFilePanel("Compress File","","unity3d");
        CompressFile(path);
    }
    [MenuItem("Compress/Compress lua File")]
    static void CompressLuaFile() {
        string path = EditorUtility.OpenFilePanel("Compress file","","lua");
        CompressFile(path);
    }
    static void CompressFile(string path)
    {
        if (path.Length != 0) {
            int lastDotIndex = path.LastIndexOf(".");
            string outputPath = path.Substring(0,lastDotIndex+1) + "zip";
            CompressFileLZMA(path, outputPath);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("压缩文件","文件已压缩","确定");
        }
    
    }
    //解压unity3d文件
    [MenuItem("Compress/Decompress Unity3D File")]
    static void DecompressUnity3DFile() {
        DecompressFile("unity3d");
    }
    //解压lua文件
    [MenuItem("Compress/Decompress Lua File")]
    static void DecompressLuaFile() {
        DecompressFile("lua");
    }

    static void DecompressFile(string extension) {
        Debug.Log("begin DecompressFile");
        string path = EditorUtility.OpenFilePanel("Decompress file","","zip");
        if (path.Length != 0) {
            int lastDotIndex = path.LastIndexOf(".");
            string outputPath = path.Substring(0, lastDotIndex + 1) + extension;
            DecompressFileLZMA(path, outputPath);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("解压文件","文件已解压完成","确认");
        }
        Debug.Log("end DecompressFile");
    }

    //压缩文件夹中的文件
    [MenuItem("Compress/Compress File In Directory")]
    static void CompressFiilesInDirectory() {
        Debug.Log("begin CompressFilesInDirectory");
        string path = EditorUtility.OpenFolderPanel("Compress File In Directory", "", "");
        if (path.Length != 0) {
            DirectoryInfo source = new DirectoryInfo(path);
            DirectoryInfo target = new DirectoryInfo(path+"_compress");
            if (target.Exists) {
                target.Delete(true);
            }

            target.Create(); //创建一个文件夹
            target.Refresh();
            Debug.Log("path==" + path);
            CopyAll(source,target);
            ListFiles((FileSystemInfo)target,true);
            EditorUtility.DisplayDialog("压缩文件夹", "文件夹中所有的文件都已压缩完成", "确定");

        }
        Debug.Log("end CompressFilesInDirectory");
    }


    //解压文件夹中的文件
    [MenuItem("Compress/Decompress File In Directory")]
    static void DecompressFileInDirectory() {
        Debug.Log("begin DecimpressFileInDirectory");
        string path = EditorUtility.OpenFolderPanel("Decompress File In Directory", "", "");
        if (path.Length != 0) {
            DirectoryInfo source = new DirectoryInfo(path);
            DirectoryInfo target = new DirectoryInfo(path + "_decompress");
            if (target.Exists) {
                target.Delete(true);
            }
            target.Create();
            target.Refresh();
           
            CopyAll(source,target);
            ListFiles(target,false);
            EditorUtility.DisplayDialog("解压文件夹","文件夹中所有的文件已经解压完成","确定");

        }
        Debug.Log("end DecimpressFileInDirectory");
    }
    static void CopyAll(DirectoryInfo source,DirectoryInfo target,ArrayList extensions = null) {
        Debug.Log("begin CpoyAll");
        if (source.FullName.ToLower() == target.FullName.ToLower()) {
            return;
        }

        if (!Directory.Exists(target.FullName)) {
            Directory.CreateDirectory(target.FullName);
            target.Refresh();
        }
        Debug.Log("source.GetFiles()==="+source.GetFiles().Length);
        foreach(FileInfo fi in source.GetFiles()){
            Debug.Log("!!!!!!!!!!!!!!!!!!");
            if (extensions == null || extensions.Count == 0)
            {
                if (fi.Extension != ".meta")
                {
                    Debug.Log("1111111111111");
                   fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }
            }
            else {
                if (extensions.Contains(fi.Extension)) {
                    Debug.Log("22222222222222");
                    fi.CopyTo(Path.Combine(target.ToString(),fi.Name),true);
                }
            }
        }
        Debug.Log("source.GetDirectories()==" + source.GetDirectories().Length);
        foreach(DirectoryInfo sourceSubDir in source.GetDirectories()){ //遍历子目录
            Debug.Log("####################");
            DirectoryInfo targetSubDir = target.CreateSubdirectory(sourceSubDir.Name);
            CopyAll(sourceSubDir,targetSubDir,extensions);
        }
        Debug.Log("end CopyAll");
    }

    //遍历文件夹中的所有文件，压缩或解压缩
    static void ListFiles(FileSystemInfo info,bool isCompress) {
        Debug.Log("begin ListFiles");
        if (!info.Exists) {
            return;
        }

        DirectoryInfo dir = info as DirectoryInfo;

        if (dir == null) {
            return;
        }
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        Debug.Log("aaaaaaaaaaaaa=="+files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;

            // 是文件  
            if (file != null)
            {
                string fullName = file.FullName;

                // 如果是压缩文件  
                if (isCompress)
                {
                    if (file.Extension == ".unity3d" || file.Extension == ".lua")
                    {
                        string outputPath = file.FullName.Replace(file.Extension, ".zip");
                        CompressFileLZMA(fullName, outputPath);
                        File.Delete(fullName);
                    }
                }
                // 如果是解压文件  
                else
                {
                    if (file.Extension == ".zip")
                    {
                        string outputPath = file.FullName.Replace(file.Extension, ".unity3d");
                        DecompressFileLZMA(fullName, outputPath);
                        File.Delete(fullName);
                    }
                }

            }
            else
            {
                ListFiles(files[i], isCompress);
            }
        }
        Debug.Log("end ListFiles");
    }

    //使用LZMA算法压缩文件
    private static void CompressFileLZMA(string inFile, string outFile) {
        Debug.Log("begin CompressFileLZMA");
        Encoder coder = new Encoder();
        FileStream input = new FileStream(inFile,FileMode.Open);
        FileStream output = new FileStream(outFile,FileMode.Create);

        coder.WriteCoderProperties(output);
        byte[] data = BitConverter.GetBytes(input.Length);
        output.Write(data,0,data.Length);

        coder.Code(input,output,input.Length,-1,null);
        output.Flush();
        output.Close();
        input.Close();
        Debug.Log("end CompressFileLZMA");
    }
    //使用LZMA算法解压文件
    private static void DecompressFileLZMA(string inFile, string outFile) {
        Debug.Log("begin DecompresFileLZMA");
        Decoder coder = new Decoder();
        FileStream input = new FileStream(inFile,FileMode.Open);
        FileStream output = new FileStream(outFile,FileMode.Create);

        byte[] properties = new byte[5];
        input.Read(properties,0,5);

        byte[] fileLengthByte = new byte[8];
        input.Read(fileLengthByte,0,8);
        long fileLength = BitConverter.ToInt64(fileLengthByte,0);

        coder.SetDecoderProperties(properties);
        coder.Code(input,output, output.Length, fileLength, null);
        output.Flush();
        output.Close();
        input.Close();
        Debug.Log("end DecompressFileLZMA");
    }

}
