using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRoll : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField]
    private float maxspeed = 5f;
    [SerializeField]
    private float addForce = 5f;
    private bool pause = false;

    public bool knockBack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();         
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.x < maxspeed && !pause)
        {
            rb.AddForce(new Vector2(addForce, rb.velocity.y));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            knockBack = true;
            StartCoroutine(Pause());
        }
    }

    IEnumerator Pause()
    {
        rb.mass = 10000f;
        pause = true;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        yield return new WaitForSeconds(5f);
        rb.mass = 40f;
        pause = false;
        knockBack = false;
        StopCoroutine(Pause()); 
    }


}
