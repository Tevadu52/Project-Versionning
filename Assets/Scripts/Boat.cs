using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boat : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private float _movementVector;
    public float MoveVector { get { return _movementVector; } }
    public float MoveSpeed { get { return moveSpeed; } }

    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_movementVector) > 0.1f)
        {
            Move();
        }
    }


    public void Move()
    {
        if (Mathf.Abs(rb.position.x + _movementVector * moveSpeed * Time.fixedDeltaTime) < PoissonsManager.Instance.GetXmax())
        {
            rb.MovePosition(new Vector2(rb.position.x + _movementVector * moveSpeed * Time.fixedDeltaTime, rb.position.y));
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementVector = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            _movementVector = 0f;
        }
    }
}
