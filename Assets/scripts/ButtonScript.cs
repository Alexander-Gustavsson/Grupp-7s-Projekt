using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {

            Transform target = transform.parent.Find("Target");

            switch (target.GetComponent<MonoBehaviour>().GetType().ToString())
            {
                case ("DrawbridgeScript"):
                    target.GetComponent<DrawbridgeScript>().Trigger();
                    break;
                case ("MovingPlatform"):
                    target.GetComponent<MovingPlatform>().Trigger();
                    break;
            }

            transform.Find("Sprite").Translate(Vector3.down * 0.2f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.Find("Sprite").Translate(Vector3.up * 0.2f);
        }
    }
}