using UnityEngine;

public class Missile : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Apply an initial force to move the missile forward as soon as it’s instantiated
        //rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
