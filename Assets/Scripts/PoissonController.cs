using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoissonController : MonoBehaviour
{
    public int Id { get; set; }
    private bool isRoaming = true;
    private Vector2 velocity = Vector2.zero;
    private Vector2 destination;
    private Rigidbody2D rb;
    public bool isAtDestination => Vector2.Distance(destination, transform.position) < .1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        getRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAtDestination) getRandomDestination();
        if (!isAtDestination)
        {
            Vector2 targetVelocity = (destination - rb.position).normalized * PoissonsManager.Instance.GetPoisson(Id).Speed;
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.2f);
        }
        else rb.velocity = Vector2.SmoothDamp(rb.velocity, Vector2.zero, ref velocity, 0.2f);
    }

    private void getRandomDestination()
    {
        destination.x = Random.Range(-PoissonsManager.Instance.GetXmax(), PoissonsManager.Instance.GetXmax());
        Zone zone = PoissonsManager.Instance.GetZone(PoissonsManager.Instance.GetPoisson(Id).Level);
        destination.y = Random.Range(zone.minY, zone.maxY);
    }
}
