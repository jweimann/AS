using UnityEngine;
using System.Collections;

public class CameraShipFollower : MonoBehaviour
{

    private Ship _ship;
    private SU_CameraFollow _cameraFollow;

    void Start()
    {
        _cameraFollow = gameObject.GetComponent<SU_CameraFollow>();
    }

    void Update()
    {
        if (_ship == null)
            _ship = GameObject.FindObjectOfType<Ship>();

        if (_ship != null)
            _cameraFollow.target = _ship.transform;
    }
}
