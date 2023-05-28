using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float shadowSize = 1f;
    [SerializeField]
    private float fallingRockSpawnHeight = 10f;

    [SerializeField]
    private GameObject shadowPrefab;

    [SerializeField]
    private GameObject fallingRockPrefab;

    private GameObject shadowClone;
    private GameObject fallingRockClone;
    private Rigidbody2D rockRB;

    private Vector2 fallingRockSpawnPos;

    private float timer = 0f;
    private float spawnTime = 5f;

    [SerializeField]
    private Collider2D eventStart, stopEvent;

    [HideInInspector]
    public bool startEvent = false;

    private int shadowCount = 0;
    private int rockCount = 0;

    //private bool shadowGrown = false;

    private GameObject[] shadows = new GameObject[10];
    private GameObject[] rocks = new GameObject[10];



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

    /*private void createInstances()
    {
        for (int i = 0; i < 10; i++)
        {
            shadows[i] = Instantiate(shadowPrefab);
        }
    }*/


    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(2f);
            CreateShadow();
        }
    }

    private void CreateShadow()
    {
        Vector2 tempScale = Vector2.zero;
        float random = Random.Range(-130, -40);
        Vector2 shadowPos = new Vector2(random, -1f + 0.8360431f);
        GameObject shadowClone = null;
        bool shadowGrown = false;

        
        shadowClone = GameObject.Instantiate(shadowPrefab, shadowPos, Quaternion.identity);
        shadowClone.transform.position = shadowPos;
        shadowCount++;
        timer = 0f;
        

        while(!shadowGrown)
        {
            shadowPos.y -= Time.deltaTime * .65f;
            shadowClone.transform.position = new Vector2(shadowClone.transform.position.x, shadowPos.y);
            tempScale = shadowClone.transform.localScale;
            tempScale.x += Time.deltaTime;
            tempScale.y += Time.deltaTime;
            shadowClone.transform.localScale = tempScale;
            if (tempScale.x > shadowSize && tempScale.y > shadowSize)
            {
                shadowGrown = true;
                //SpawnRock();
                tempScale = Vector2.zero;
            }
        }

    }

    /*private void SpawnRock()
    {
        if(shadowGrown && rockCount < 10f)
        {
            fallingRockSpawnPos = new Vector2(shadowClone.transform.position.x, shadows[shadowCount].transform.position.y + fallingRockSpawnHeight);
            Instantiate(fallingRockPrefab, fallingRockSpawnPos, Quaternion.identity);
            rockCount++;
        }
    }*/


    private void Destroy()
    {
        if(fallingRockClone != null)
        {
            rockRB = fallingRockClone.GetComponent<Rigidbody2D>();
            if (rockRB.velocity.y >= 0 && rockRB.position != fallingRockSpawnPos)
            {
                Destroy(fallingRockClone);
                Destroy(shadows[shadowCount]);
                shadowCount--;
                rockCount--;
            }
        }
    }




}
