using UnityEngine;
using System.Collections;
using System;

public class Load : MonoBehaviour {

	// Use this for initialization
    private string BundleURL = "file:///E:/cube.assetbundle";
    private string SceneURL = "file:///E:/main.unity3d";


	void Start () {
        Debug.Log(BundleURL);
        StartCoroutine(DownloadAssetAndScene());
	
	}

    IEnumerator DownloadAssetAndScene() {

        using (WWW scene = new WWW(SceneURL)) {
            Debug.Log("3333333333333333333");
            
           // yield return scene;
            AssetBundle bundle = scene.assetBundle;
            Application.LoadLevel("main");
            
            yield return new WaitForSeconds(2);
            Debug.Log("4444444444444444444");

        }

       
        using (WWW asset = new WWW(BundleURL))
        {
            Debug.Log("111111111111111111111");
            yield return asset;
            AssetBundle bundle = asset.assetBundle;
            Instantiate(bundle.LoadAsset("Prefab"));
            bundle.Unload(false);
            Debug.Log("222222222222222222");
        
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
