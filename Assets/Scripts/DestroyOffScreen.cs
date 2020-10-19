using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{

    public float offset = 16f;
    public delegate void OnDestroy();
    public event OnDestroy DestroyCallback;

    private bool offScreen;
    private float offScreenX = 0f;
    private Rigidbody2D body2d;

    void Awake()
    {
        body2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        offScreenX = (Screen.width/PixelPerfectCamera.pixelsToUnits) / 2 + offset;
    }

    // Update is called once per frame
    void Update()
    {
        var posX = transform.position.x;
        var dirX = body2d.velocity.x;

        if (Mathf.Abs(posX) > offScreenX)
        {
            if (dirX < 0 && posX < -offScreenX)
            {
                offScreen = true;
            }
            else if (dirX > 0 && posX > offScreenX)
            {
                offScreen = true;
            }
        }
        else
        {
            offScreen = false;
        }

        if (offScreen)
        {
            OnOutOfBounds();
        }
    }

    void OnOutOfBounds()
    {
        offScreen = false;
        GameObjectUtility.Destroy(gameObject);

        if(DestroyCallback != null)
        {
            DestroyCallback();
        }

    }
}
