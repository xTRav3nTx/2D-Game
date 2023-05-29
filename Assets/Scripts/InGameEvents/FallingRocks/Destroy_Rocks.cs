using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Rocks : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }

}
