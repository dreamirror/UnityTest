using UnityEngine;
using System.Collections;

public class WScript : MonoBehaviour {

    public string url = "https://ss0.baidu.com/73F1bjeh1BF3odCf/it/u=458330244,1654965337&fm=85&s=95C4954E9BE08B720A66CBAF0300900A";

    IEnumerator Start() { 
        WWW www = new WWW(url);
        Debug.Log("==========="+www);
        yield return www;
        UITexture te = GetComponent<UITexture>();
        te.mainTexture = www.texture;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
