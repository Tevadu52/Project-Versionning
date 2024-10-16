using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamMove : MonoBehaviour
{
    private Camera cam;
    private Vector3 currentposition;
    private Vector3 offset = new Vector3(0, 0, -10);

    [SerializeField]private Transform target;
    private Rect _boundsRect = new Rect(0f, 0f, 10f, 10f);
    public Rect BoundsRect { set { _boundsRect = value; } }
    public static CamMove Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        cam = Camera.main;
    }

    private void Update()
    {
        currentposition = target.position;
        setPosition(_ClampPositionIntoBounds(new Vector3(currentposition.x, currentposition.y, offset.z)));
    }

    private Vector3 _ClampPositionIntoBounds(Vector3 position)
    {
        Rect boundsRect = _boundsRect;
        Vector3 worldBottomLeft = cam.ScreenToWorldPoint(new Vector3(0f, 0f));
        Vector3 worldTopRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));
        Vector3 worldScreenSize = new Vector2(worldTopRight.x - worldBottomLeft.x, worldTopRight.y - worldBottomLeft.y);
        Vector3 worldHalfScreenSize = worldScreenSize / 2f;

        if (position.x > boundsRect.xMax - worldHalfScreenSize.x)
        {
            position.x = boundsRect.xMax - worldHalfScreenSize.x;
        }
        if (position.x < boundsRect.xMin + worldHalfScreenSize.x)
        {
            position.x = boundsRect.xMin + worldHalfScreenSize.x;
        }

        if (position.y > boundsRect.yMax - worldHalfScreenSize.y)
        {
            position.y = boundsRect.yMax - worldHalfScreenSize.y;
        }
        if (position.y < boundsRect.yMin + worldHalfScreenSize.y)
        {
            position.y = boundsRect.yMin + worldHalfScreenSize.y;
        }

        return position;
    }
    public void setPosition(Vector3 pos)
    {
        cam.transform.position = pos;
    }

}
