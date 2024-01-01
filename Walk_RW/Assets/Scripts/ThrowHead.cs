using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHead : MonoBehaviour
{
    public float maxDragDistance = 2f;
    public float power = 10f;

    private Rigidbody2D rb;
    private SpringJoint2D springJoint;
    private Vector2 startPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        springJoint = GetComponent<SpringJoint2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = startPos - currentPos;
            float distance = direction.magnitude;

            if (distance > maxDragDistance)
            {
                direction = direction.normalized * maxDragDistance;
            }

            rb.position = startPos - direction;
        }

        if (Input.GetMouseButtonUp(0))
        {
            springJoint.enabled = false;
            rb.AddForce(-springJoint.frequency * (rb.position - startPos) * power, ForceMode2D.Impulse);
        }
    }
}