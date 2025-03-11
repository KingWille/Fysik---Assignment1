using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduForces : MonoBehaviour
{
    Vector2 m_gravity = new Vector2(0, -9.82f);
    GameObject[] m_players;
    eduRigidBody curr_script;
    public bool use_gravity;

    // Update is called once per frame

    private void Start()
    {
        m_players = GameObject.FindGameObjectsWithTag("Player");
    }
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (use_gravity)
        {
            foreach (GameObject g in m_players)
            {
                curr_script = g.GetComponent<eduRigidBody>();
                curr_script.ApplyForce(m_gravity * curr_script.m_mass);
            }
        }
    }
}
