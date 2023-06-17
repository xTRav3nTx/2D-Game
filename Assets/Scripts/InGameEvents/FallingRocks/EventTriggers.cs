using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    [SerializeField]
    private FallingRocks parent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.name == "StartEventCollider" && other.CompareTag("Player"))
        {
            parent.startEvent = true;
        }
        if (this.name == "StopEventCollider" && other.CompareTag("Player"))
        {
            parent.stopEvent = true;
        }
    }
}
