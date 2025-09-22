using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("Flag");

            other.gameObject.GetComponent<PlayerMovement>().spawnPosition = transform.position;
        }
    }
}
