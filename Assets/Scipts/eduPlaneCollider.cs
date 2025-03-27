using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduPlaneCollider : MonoBehaviour
{
    public Vector2 m_normal;
    public float m_density;
    public float m_area;

    public eduRigidBody rb;
    // Update is called once per frame
    void Update()
    {
        rb.m_drag_coef = 1.05f;

        m_area = transform.lossyScale.x * transform.lossyScale.y;

        rb.m_mass = m_density * m_area;

        m_normal = transform.up;

        Debug.DrawLine(transform.position, transform.position + (Vector3)m_normal, Color.green);
    }
}
