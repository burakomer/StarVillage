using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testweapon : MonoBehaviour
{
    private Transform grip;
    public float gripLength = 1f;

    // Start is called before the first frame update
    void Start()
    {
        grip = transform.parent;
    }

    public float offset;

    void Flip()
    {
        Vector2 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    // Update is called once per frame
    private void Update()
    {
        //makes the weapon face the mouse while orbiting on the circle
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - grip.position;
        difference.z = 0;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        // add gripLength to position
        transform.position = grip.position + (gripLength * difference.normalized);

        if ((difference.x <= 0) != (transform.localScale.y <= 0))
        {
            Flip();
        }
    }
}