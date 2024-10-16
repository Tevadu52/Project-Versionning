using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private int level = 0;
    public int Level {  get { return level; } }
    private float _movementVector;
    private Rigidbody2D rb;

    [SerializeField]
    private UpgradeController upgradeController;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.position.y  < .5f) upgradeController.HideGameplay(true);
        else upgradeController.HideGameplay(false);
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_movementVector) > 0.1f)
        {
            Move();
        }
    }

    public void UpgradeHook()
    {
        level++;
    }

    public void Move()
    {
        if (rb.position.y + _movementVector * moveSpeed * Time.fixedDeltaTime < 0f)
        {
            rb.MovePosition(new Vector2(transform.parent.position.x, rb.position.y + _movementVector * moveSpeed * Time.fixedDeltaTime));
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
