using UnityEngine;
using System.Collections;
using AS.Client.Unity3D;
using AS.Client.Logging;
using AS.Client.Messages;

public class UnityClientActorSystemWrapper : MonoBehaviour
{

    private UnityClientActorSystem _system;
    private float _timer;

    [SerializeField]
    private UnityClientMonoActor[] _clientMonoActors;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Starting UnityClientActorSystemWrapper");
        //Logger.SetLogger(LogLevel.Debug, (text) => { Debug.Log(text); });
        Logger.SetLogger(LogLevel.Warning, (text) => { Debug.LogWarning(text); });
        Logger.SetLogger(LogLevel.Error, (text) => { Debug.LogError(text); });

        var factory = new UnityClientActorFactory();
        _system = new UnityClientActorSystem(factory);

        factory.SetClientMonoActors(_clientMonoActors);
        Debug.Log("Finished UnityClientActorSystemWrapper");
    }

    // Update is called once per frame
    void Update()
    {
        _system.Tick();

        return;
        _timer += Time.deltaTime;
        if (_timer >= 1f)
        {
            string statsText = _system.GetStats();
            Debug.Log(statsText);
            _timer = 0;
        }
    }

    public void Spawn100()
    {
        _system.SendMessage(new ClientSpawnEntityRequest("test", 100));
    }

    public void RegisterDebugLogger()
    {
        Logger.SetLogger(LogLevel.Debug, (text) => { Debug.Log(text); });
    }
}
