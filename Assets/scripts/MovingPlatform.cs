using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform target1, target2;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private bool moving = true;

    private Transform currentTarget;

    void Start()
    {
        currentTarget = target1;

        if (target1 == null) target1 = transform; // Om man inte vill att plattformen ska röra sig lämnar man targets tomma.
        if (target2 == null) target2 = transform; // Om man vill att plattformen ska röra sig endast en gång anger man bara en av dessa.
    }
    void FixedUpdate()
    {
        if (transform.position == target1.position)
        {
            currentTarget = target2;
        }
        else if (transform.position == target2.position)
        {
            currentTarget = target1;
        }

        if(moving) transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    public void Trigger()
    {
        moving = !moving;
    }
}
