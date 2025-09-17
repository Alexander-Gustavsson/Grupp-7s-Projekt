using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hej

public class DestroyAfterLifetime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1.0f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}
