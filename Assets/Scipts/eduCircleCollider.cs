using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduCircleCollider : MonoBehaviour
{
    public float m_radius;
    public float m_density;

    public eduRigidBody rb;
    // Start is called before the first frame update
    void Start()
    {
        //Beräkning av radie
        m_radius = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z) / 2;

        //Beräkning av massa
        rb.m_mass = m_density * (Mathf.PI * Mathf.Pow(m_radius, 2));

        //Beräkning av tröghetsmoment
        rb.m_inertia = 0.5f * rb.m_mass * Mathf.Pow(m_radius, 2);
    }
}
