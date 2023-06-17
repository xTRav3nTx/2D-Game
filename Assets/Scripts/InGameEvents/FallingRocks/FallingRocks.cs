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
    [SerializeField]
    private LayerMask groundLayer;

    public float shadowSize = 1f;

    [SerializeField]
    private float fallingRockSpawnHeight = 10f;

    [SerializeField]
    private GameObject shadowPrefab;

    [SerializeField]
    private GameObject fallingRockPrefab;
    [SerializeField]
    private GameObject rockHolder;

    private Vector2 fallingRockSpawnPos;

    [SerializeField]
    private Collider2D eventStart;
    [SerializeField]
    private Collider2D eventEnd;

 
    public bool startEvent = false;
    public bool stopEvent = false;  

    [SerializeField]
    private float growTime = 2f;

    [SerializeField]
    private float spawnTimer = 1.5f;

    float timer = 0f;

    // Update is called once per frame
    void Update()
    {      
        if (startEvent)
        {
            startEvent = false;
            StartCoroutine(Spawn());
        }
        if(stopEvent)
        {
            //StopAllCoroutines();
        }
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTimer);
            if(!stopEvent)
            {
                RockSpawn();
            }
        }
    }



    private void RockSpawn()
    {
        float leftBound = cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        float rightBound = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0f, 0f)).x;

        if (rightBound > eventStart.transform.position.x)
        {
            rightBound = eventStart.transform.position.x;
        }
        if (leftBound < eventEnd.transform.position.x)
        {
            leftBound = eventEnd.transform.position.x;
        }

        float random = Random.Range(leftBound, rightBound);

        GameObject rock = Instantiate(fallingRockPrefab, new Vector2(random, player.position.y + fallingRockSpawnHeight), Quaternion.identity);
        rock.transform.parent = rockHolder.transform;

        CreateShadowfromRaycast(rock);
    }

    private void CreateShadowfromRaycast(GameObject rock)
    {
        bool growing = false;

        RaycastHit2D hit = Physics2D.Raycast(rock.transform.GetChild(0).position, -Vector2.up, 40f, groundLayer);
        Vector2 shadowPos = Vector2.zero;
        GameObject shadowClone = null;

        if (hit.collider.CompareTag("Ground"))
        {
            shadowPos = hit.point;
            shadowClone = Instantiate(shadowPrefab, shadowPos, Quaternion.identity);
            Vector2 startScale = shadowClone.transform.localScale;
            Vector2 maxScale = new Vector2(shadowSize, shadowSize);
            Vector2 startPos = shadowClone.transform.position;
            Vector2 endPos = new Vector2(shadowClone.transform.position.x, shadowClone.transform.position.y - 1f);

            if (!growing)
            {
                growing = true;
                StartCoroutine(GrowAndDestroyShadow(shadowClone, startScale, maxScale, timer, startPos, endPos, rock));
            }
        }        
    }

    private IEnumerator GrowAndDestroyShadow(GameObject shadowClone, Vector2 startScale, Vector2 maxScale, float timer, Vector2 startPos, Vector2 endPos, GameObject rock)
    {
        do
        {
            if(rock != null)
            {
                shadowClone.transform.localScale = Vector3.Lerp(startScale, maxScale, timer / growTime);
                shadowClone.transform.position = Vector3.Lerp(startPos, endPos, timer / growTime);
                timer += Time.deltaTime;
            }
            else
            {
                break;
            }
            
            yield return null;
        }
        while (true);
        Destroy(shadowClone);
    }

    /*private IEnumerator CreateShadow()
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
    }*/
}
