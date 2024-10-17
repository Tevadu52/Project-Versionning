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
    private float DistanceToTouch => Vector2.Distance(transform.position, mouth.position);
    public bool isAtDestination => Vector2.Distance(destination, transform.position) < DistanceToTouch;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.position = getRandomDestination();
        destination = getRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) return;
        if (isAtDestination) destination = getRandomDestination();
        if (!isAtDestination)
        {
            Vector2 targetVelocity = (destination - rb.position).normalized * PoissonsManager.Instance.GetPoisson(Id).Speed;
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
        if (collision.CompareTag("Hook") && !collision.GetComponent<Hook>().asPoisson && target == null)
        {
            collision.GetComponent<Hook>().asPoisson = gameObject;
            target = collision.gameObject;
            transform.SetParent(target.transform, false);
            transform.localPosition = new Vector2(0, -DistanceToTouch);
            transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            rb.velocity = Vector3.zero;
        }
        if (collision.CompareTag("Shark")) Destroy(gameObject);
    }
}
