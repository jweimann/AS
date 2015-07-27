using AS.Client.Unity3D;
using System;
using AS.Client.Messages.Entities;
using AS.Client.Unity3D.Converters;
using UnityEngine;

public class ClientEntityActor : UnityClientMonoActor
{
    [SerializeField]
    private string _lastUpdateTime;

    [SerializeField]
    private AS.Common.EntityType _entityType;

    private Vector3 _velocity;

    public override AS.Common.EntityType EntityType { get { return _entityType; } }

    public override void Receive(object message)
    {
        if (message is UpdatePosition)
        {
            //Debug.LogFormat("UpdatePosition {0} {1}",
            //    transform.position.ToString(),
            //    ((UpdatePosition)message).Position.ToUnity());

            transform.position = ((UpdatePosition)message).Position.ToUnity();
            _velocity = ((UpdatePosition)message).Velocity.ToUnity();
            _lastUpdateTime = DateTime.Now.ToString();
        }
        //throw new NotImplementedException();
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        transform.Translate(_velocity * Time.deltaTime);
    }
}
