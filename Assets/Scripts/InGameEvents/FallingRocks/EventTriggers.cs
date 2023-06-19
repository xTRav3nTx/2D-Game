using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    [SerializeField]
    private FallingRocks parent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.name == "StartEventCollider" && other.CompareTag(StringConstants.PLAYER))
        {
            parent.startEvent = true;
        }
        if (this.name == "StopEventCollider" && other.CompareTag(StringConstants.PLAYER))
        {
            parent.stopEvent = true;
        }
    }
}
