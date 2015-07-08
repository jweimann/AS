using UnityEngine;
using System.Collections;


public class InjectionTest : MonoBehaviour {

    public void InjectTest()
    {
        Debug.Log("InjectTest");
        GameObject.FindObjectOfType<Camera>().backgroundColor = Color.green;
        GameObject.FindObjectOfType<Camera>().clearFlags = CameraClearFlags.SolidColor;
    }
    // Use this for initialization

    [NRTime]
    void Start () {
        Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
