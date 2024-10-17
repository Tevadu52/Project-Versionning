using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonController : MonoBehaviour
{
    public int Id { get; set; }
    private Vector2 velocity = Vector2.zero;
    private Vector2 destination;
    private GameObject target;
    [SerializeField]
    private Transform mouth;
    private Rigidbody2D rb;
    private float moveSpeed = 0;
    private float DistanceToTouch => Vector2.Distance(transform.position, mouth.position);
    public bool isAtDestination => Vector2.Distance(destination, transform.position) < DistanceToTouch;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.position = getRandomDestination();
        destination = getRandomDestination();
        moveSpeed = PoissonsManager.Instance.GetPoisson(Id).Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) return;
        if (isAtDestination) destination = getRandomDestination();
        if (!isAtDestination)
        {
            Vector2 targetVelocity = (destination - rb.position).normalized * moveSpeed;
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.2f);
        }
        else rb.velocity = Vector2.SmoothDamp(rb.velocity, Vector2.zero, ref velocity, 0.05f);
    }

    private Vector2 getRandomDestination()
    {
        Vector2 destin = Vector2.zero;
        destin.x = Random.Range(-PoissonsManager.Instance.GetXmax(), PoissonsManager.Instance.GetXmax());
        Zone zone = PoissonsManager.Instance.GetZone(PoissonsManager.Instance.GetPoisson(Id).Level);
        destin.y = Random.Range(zone.minY, zone.maxY);
        return destin;
    }


    private void LateUpdate()
    {
        flip(rb.velocity.x);
        if (rb.velocity.magnitude > 0.3f)
        {
            Vector2 v = rb.velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void flip(float _velocity)
    {
        if (_velocity > 0.2f)
        {
            GetComponentInChildren<SpriteRenderer>().flipY = false;
        }
        else if (_velocity < -0.2f)
        {
            GetComponentInChildren<SpriteRenderer>().flipY = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hook") && collision.GetComponent<Hook>().Bait == PoissonsManager.Instance.GetPoisson(Id).Level)
        {
            Hook hook = collision.GetComponent<Hook>();
            float random = Random.Range(0f, 1f);
            float chance = PoissonsManager.Instance.GetPoisson(Id).prob[hook.Level] / 100f;
            if (hook.asPoisson != null) Destroy(hook.asPoisson);
            if (random <= chance)
            {
                hook.Bait++;
                hook.asPoisson = gameObject;
                target = collision.gameObject;
                transform.SetParent(target.transform, false);
                transform.localPosition = new Vector2(0, -DistanceToTouch);
                transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                rb.velocity = Vector3.zero;
            }
            else
            {
                hook.Bait = 0;
                hook.asPoisson = null;
                if (random > .5f) destination = new Vector2(100, transform.position.y);
                else destination = new Vector2(-100, transform.position.y);
                moveSpeed *= 5f;
                moveSpeed = Mathf.Min(moveSpeed , 10);
            }
        }
        if (collision.CompareTag("Shark"))
        {
            if (target != null) target.GetComponent<Hook>().Bait = 0;
            Destroy(gameObject);
        }
    }
}
