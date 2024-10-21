using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hook : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int level = 0;
    [SerializeField] private Sprite[] hamecon;
    public GameObject asPoisson { get; set; }
    [SerializeField] private GameObject Line;
    public int Level {  get { return level; } }
    [SerializeField] private int bait = 0;
    public int Bait { get { return bait; } set { bait = value; } }
    private float _movementVector;
    private Rigidbody2D rb;
    private Boat boat;

    [SerializeField]
    private UpgradeController upgradeController;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = hamecon[level];
        boat = transform.parent.GetComponent<Boat>();
    }

    private void Update()
    {
        if (upgradeController.GetShop()) return;
        Line.GetComponent<SpriteRenderer>().size = new Vector2(0.07f, Vector2.Distance(transform.position, Line.transform.position));
        if (rb.position.y > .5f && !upgradeController.GetGameplay())
        {
            if (asPoisson != null)
            {
                PoissonsStock.Instance.PoissonsAdd(asPoisson.GetComponent<PoissonController>().Id);
                upgradeController.DisplayFishName(asPoisson.GetComponent<PoissonController>().Id);
                Destroy(asPoisson);
                asPoisson = null;
                Bait = 0;
                upgradeController.IncrementCounter();
                SaveData.Instance.NumberOfFishAdd = 1;
            }
            upgradeController.SetGameplay(true);
        }
        else if (rb.position.y < .5f && upgradeController.GetGameplay()) upgradeController.SetGameplay(false);
    }

    private void FixedUpdate()
    {
        if (upgradeController.GetShop()) return;
        if (Mathf.Abs(_movementVector) > 0.1f)
        {
            Move();
            transform.parent.GetComponent<Animator>().SetBool("mouline", true);
        }
        else transform.parent.GetComponent<Animator>().SetBool("mouline", false);
    }

    public void UpgradeHook()
    {
        level++;
        GetComponent<SpriteRenderer>().sprite = hamecon[level];
    }

    public void Move()
    {
        float nextMove = rb.position.y + _movementVector * moveSpeed * Time.fixedDeltaTime;
        float boatMove = rb.position.x + boat.MoveVector * boat.MoveSpeed * Time.fixedDeltaTime;
        if (Mathf.Abs(boatMove) < PoissonsManager.Instance.GetXmax())
        {
            rb.position = new Vector2(boatMove, rb.position.y);
        }
        if (nextMove < 1.5f && nextMove > PoissonsManager.Instance.Ymin)
        {
            rb.MovePosition(new Vector2(rb.position.x, nextMove));
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
