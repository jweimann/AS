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
    private Vector3 _localPositionAtUpdate;
    [SerializeField]
    private Vector3 _serverPositionAtUpdate;
    [SerializeField]
    private float _serverToLocalPositionDelta;
    [SerializeField]
    private float _serverToLocalPositionDeltaPerSecond;
    [SerializeField]
    private float _deltaSinceUpdate;

    [SerializeField]
    private AS.Common.EntityType _entityType;

    private Vector3 _velocity;
    private DateTime _lastUpdateTimeDateTime;

    public override AS.Common.EntityType EntityType { get { return _entityType; } }

    public override void Receive(object message)
    {
        if (message is UpdatePosition)
        {
            //Debug.LogFormat("UpdatePosition {0} {1}",
            //    transform.position.ToString(),
            //    ((UpdatePosition)message).Position.ToUnity());

            _localPositionAtUpdate = transform.position;
            transform.position = ((UpdatePosition)message).Position.ToUnity();
            _serverPositionAtUpdate = transform.position;
            _serverToLocalPositionDelta = Vector3.Distance(_localPositionAtUpdate, _serverPositionAtUpdate);

            _velocity = ((UpdatePosition)message).Velocity.ToUnity();

            _deltaSinceUpdate = (float)(DateTime.Now - _lastUpdateTimeDateTime).TotalSeconds;
            _serverToLocalPositionDeltaPerSecond = _serverToLocalPositionDelta / _deltaSinceUpdate;

            _lastUpdateTimeDateTime = DateTime.Now;
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
        if (_velocity != Vector3.zero)
            transform.Translate(_velocity * Time.deltaTime);
    }
}
