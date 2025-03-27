using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduCircleCollider : MonoBehaviour
{
    public float m_radius;
    public float m_density;
    public float m_area;

    public eduRigidBody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.m_drag_coef = 0.47f;
        //Beräkning av radie
        m_radius = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z) / 2;

        m_area = Mathf.PI * m_radius * m_radius;

        //Beräkning av massa
        rb.m_mass = m_density * m_area;

        //Beräkning av tröghetsmoment
        rb.m_inertia = 0.5f * rb.m_mass * Mathf.Pow(m_radius, 2);
    }
}
