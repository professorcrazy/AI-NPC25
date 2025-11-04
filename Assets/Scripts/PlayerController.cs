using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 move;
    public float speed = 10f;
    public float rotationSpeed = 5f;

    public InputSystem_Actions inputActions;


    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, move.y * rotationSpeed, 0);
        rb.linearVelocity = transform.forward * (move.x * speed * Time.deltaTime);
    }
}
