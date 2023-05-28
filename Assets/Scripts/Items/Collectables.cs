using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private float flashSpeed = 1f;
    [SerializeField]
    private float length = 1f;
    [SerializeField]
    private float waitForFlash = 5f;
    [SerializeField]
    private float waitForDestroy = 3f;

    private Color startColor;
    private Color endColor;

    private SpriteRenderer collectable;

    private void Awake()
    {
        collectable = GetComponent<SpriteRenderer>();
        startColor = collectable.color;
        endColor = collectable.color;
        endColor.a = 0f;
    }

    private void Update()
    {
        if(collectable.transform.CompareTag("SpawnedCollectable"))
        {
            StartCoroutine(Flash());
        }
        
    }

    IEnumerator Flash()
    {
        yield return new WaitForSeconds(waitForFlash);
        collectable.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * flashSpeed, length));
        yield return new WaitForSeconds(waitForDestroy);
        Destroy(collectable.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
