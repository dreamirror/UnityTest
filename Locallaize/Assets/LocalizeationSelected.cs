using UnityEngine;
using System.Collections;
using System.IO;
public class LocalizeationSelected : MonoBehaviour {

	// Use this for initialization
    void Awake() {
        Debug.Log("1111111111");
       
        //再次加载语言
        string languageFile = "Assets/Resources/English.txt";
        string languageFile2 = "Assets/Resources/Chinese.txt";
 

        byte[] buff = File.ReadAllBytes(languageFile);
        Localization.Set("English", buff);
       
        byte[] buff2 = File.ReadAllBytes(languageFile2);
        Localization.Set("Chinese", buff2);

         Localization.language = "English";
        Debug.Log(Localization.language);
       
    }
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
