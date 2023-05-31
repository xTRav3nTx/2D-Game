using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [SerializeField]
    private Camera cam;    

    [SerializeField]
    private Transform player;

    public float shadowSize = 1f;

    [SerializeField]
    private float fallingRockSpawnHeight = 10f;

    [SerializeField]
    private GameObject shadowPrefab;

    [SerializeField]
    private GameObject fallingRockPrefab;

    private Vector2 fallingRockSpawnPos;

    [SerializeField]
    private Collider2D eventStart, stopEvent;

    [HideInInspector]
    public bool startEvent = false;

    [SerializeField]
    private float growTime = 2f;

    [SerializeField]
    private float spawnTimer = 1f;

    // Update is called once per frame
    void Update()
    {        
        if (startEvent)
        {
            startEvent = false;
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(spawnTimer);
            StartCoroutine(CreateShadow());
        }
    }

    private IEnumerator CreateShadow()
    {
        float leftBound = cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        float rightBound = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0f, 0f)).x;

        if(rightBound > eventStart.transform.position.x)
        {
            rightBound = eventStart.transform.position.x;
        }

        float random = Random.Range(leftBound, rightBound);
        Vector2 shadowPos = new Vector2(random, -1f + 0.8360431f);
        GameObject shadowClone = GameObject.Instantiate(shadowPrefab, shadowPos, Quaternion.identity);
        Vector2 startScale = shadowClone.transform.localScale;
        Vector2 maxScale = new Vector2(shadowSize, shadowSize);
        Vector2 startPos = shadowClone.transform.position;
        Vector2 endPos = new Vector2(shadowClone.transform.position.x, -1.02f);
        float timer = 0f;

        do
        {
            shadowClone.transform.localScale = Vector3.Lerp(startScale, maxScale, timer / growTime);
            shadowClone.transform.position = Vector3.Lerp(startPos, endPos, timer / growTime);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < growTime);

        GameObject rock = SpawnRock(shadowClone);

        StartCoroutine(DestroyShadow(rock, shadowClone));
    }

    private GameObject SpawnRock(GameObject shadow)
    {
        fallingRockSpawnPos = new Vector2(shadow.transform.position.x, shadow.transform.position.y + fallingRockSpawnHeight);
        GameObject rock = Instantiate(fallingRockPrefab, fallingRockSpawnPos, Quaternion.identity);
        return rock;
    }

    private IEnumerator DestroyShadow(GameObject rock, GameObject shadow)
    {
        while(rock != null)
        {
            yield return null;
        }
        Destroy(shadow);
    }
}
