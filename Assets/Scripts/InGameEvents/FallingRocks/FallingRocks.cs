using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    public float shadowSize = 1f;

    [SerializeField]
    private float fallingRockSpawnHeight = 10f;

    [SerializeField]
    private GameObject shadowPrefab;

    [SerializeField]
    private GameObject fallingRockPrefab;

    private GameObject fallingRockClone;
    private Rigidbody2D rockRB;

    private Vector2 fallingRockSpawnPos;

    [SerializeField]
    private Collider2D eventStart, stopEvent;

    [HideInInspector]
    public bool startEvent = false;

    private int shadowCount = 0;

    //private bool shadowGrown = false;

    private GameObject[] shadows = new GameObject[10];
    private GameObject[] rocks = new GameObject[10];

    private float growTime = 2f;

    [SerializeField]
    private float spawnTimer = 1f;

    // Update is called once per frame
    void Update()
    {
        if (startEvent)
        {
            startEvent = false;
            // InvokeRepeating("CreateShadow", 10f, 10f);
            StartCoroutine(Spawn());
            /*timer += Time.deltaTime;
            CreateShadow();*/
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
        float random = Random.Range(-130, -40);
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
