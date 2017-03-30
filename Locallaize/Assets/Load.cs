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
            
           // yield return scene;
            AssetBundle bundle = scene.assetBundle;
            Application.LoadLevel("main");
            
            yield return new WaitForSeconds(2);

        }

       
        using (WWW asset = new WWW(BundleURL))
        {
            yield return asset;
            AssetBundle bundle = asset.assetBundle;
            Instantiate(bundle.LoadAsset("Prefab"));
            bundle.Unload(false);
        
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
