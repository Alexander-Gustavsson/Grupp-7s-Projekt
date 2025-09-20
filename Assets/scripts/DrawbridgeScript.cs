using UnityEngine;

public class DrawbridgeScript : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private bool StartsUp;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.SetBool("StartsUp", StartsUp);
        Animator.SetTrigger("Start");
    }

    public void Trigger()
    {
        Animator.SetTrigger("Trigger");
    }
}
