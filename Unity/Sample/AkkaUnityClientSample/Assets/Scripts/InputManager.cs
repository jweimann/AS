using UnityEngine;
using System.Collections;
using SG.Client.Messages;

public class InputManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Clicked");
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
            {
                Asteroid asteroid = hitInfo.collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.GetComponent<Renderer>().material.color = Color.red;
                    var entityId = (int)asteroid.GetComponent<ClientEntityActor>().EntityId;
                    UnityClientActorSystemWrapper.SendMessage(new SetTargetObject(entityId, entityId));
                }
            }
        }
    }
}
