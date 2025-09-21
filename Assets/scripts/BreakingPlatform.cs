using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BreakingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject crumblingStone;
    private List<GameObject> SpriteParts = new List<GameObject>();
    private readonly float TremorStrength = 0.1f;
    private readonly float TremorCount = 15;
    private readonly float Duration = 1.2f;
    private readonly float RespawnTime = 5f;

    private Coroutine breaker;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            SpriteParts.Add(child.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && breaker == null)
        {
            breaker = StartCoroutine(breakPlatform());
        }
    }

    IEnumerator breakPlatform()
    {
        int tremors = 0;

        Vector3 lastPoint = Vector3.zero;

        while (tremors < TremorCount)
        {
            Vector3 newPoint = new Vector3(Random.Range(-TremorStrength, TremorStrength), Random.Range(-TremorStrength, TremorStrength), 0);

            foreach (GameObject part in SpriteParts)
            {
                part.transform.position += newPoint - lastPoint;
            }

            lastPoint = newPoint;
            tremors++;

            yield return new WaitForSeconds(Duration / TremorCount);
        }

        Instantiate(crumblingStone, transform.position, Quaternion.identity);
        Collider2D collider = GetComponent<Collider2D>();
        
        collider.enabled = false;
        

        foreach (GameObject child in SpriteParts)
        {
            if (!child.CompareTag("Player"))
            {
                child.SetActive(false);
                child.transform.position -= lastPoint;
            }
        }


        
        yield return new WaitForSeconds(RespawnTime);

        foreach (GameObject child in SpriteParts)
        {
            if (!child.CompareTag("Player")) child.SetActive(true);
        }

        collider.enabled = true;

        breaker = null;
        yield break;
    }
}
