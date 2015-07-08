using UnityEngine;
using System.Collections;
using AS.Client.Helios;

public class AkkaHeliosConnector : MonoBehaviour
{

    private AkkaClient client;

    void Start()
    {
        client = new AkkaClient();
        client.Initialize();
    }

    void OnDestroy()
    {
        client.Dispose();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
