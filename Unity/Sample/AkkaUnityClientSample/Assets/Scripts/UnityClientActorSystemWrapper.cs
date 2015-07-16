using UnityEngine;
using System.Collections;
using AS.Client.Unity3D;

public class UnityClientActorSystemWrapper : MonoBehaviour
{

    UnityClientActorSystem _system;
    // Use this for initialization
    void Start()
    {
        _system = new UnityClientActorSystem();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
