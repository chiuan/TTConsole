using UnityEngine;
using System.Collections;
using TinyTeam.Debuger;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
    public int max = 10;
    bool start = false;
    public GameObject go = null;

	// Use this for initialization
	void Start () {
        TTDebuger.Log("Hello Chiuan");
        TTDebuger.Log("socket...", "NET");
        TTDebuger.Log("load xxxx.ab", "Loader");
        TTDebuger.Log("success mission", "mission");

        //EventTrigger tr = go.AddComponent<EventTrigger>();
        //EventTrigger.Entry en = new EventTrigger.Entry();

        ////EventTrigger.TriggerEvent te
        ////en.callback = 
        //tr.delegates = new System.Collections.Generic.List<EventTrigger.Entry>();
        //tr.delegates.Add(en);

        //TTDebuger.RegisterCommand("load", LoadLevel, "");
        StartCoroutine(IEDebug());

        //TTDebuger.Log("hello man");
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnGUI()
    {
        GUILayout.Label("Press ~ button to open the console in destop. 3 touches upside screen in mobiles.");
    }

    //object LoadLevel(string[] input)
    //{
    //    Console.IsOpen = false;
    //    Application.LoadLevel(0);
    //    return "load level";
    //}

    IEnumerator IEDebug()
    {
        int i = 0;
        while(true)
        {
            yield return null;
            if (i == max) break;
            i++;
            TTDebuger.Log(i + " [Add] cached assetbundle :comm.texture.xulie-0118.png.assetbundle[Add] cached assetbundle :comm.texture.xulie-0118.png.assetbundle[Add] cached assetbundle :comm.texture.xulie-0118.png.assetbundlecomm.texture.xulie-0118.png.assetbundlecomm.texture.xulie-0118.png.assetbundlecomm.texture.xulie-0118.png.assetbundlecomm.texture.xulie-0118.png.assetbundlecomm.texture.xulie-0118.png.assetbundle");
            //Debug.Log("gogogogo");
        }

        start = !start;
    }

    public void OnSubmit(InputField input)
    {
        Debug.Log(input.text);
    }
}
