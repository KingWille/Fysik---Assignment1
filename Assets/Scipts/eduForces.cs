using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduForces : MonoBehaviour
{
    Vector2 gravity = new Vector2(0, -9.82f);
    float breaking_torque = -150f;

    GameObject[] players;
    eduRigidBody curr_script;
    public bool m_use_gravity, m_use_breaking_torque;

    // Update is called once per frame

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (m_use_gravity)
        {
            foreach (GameObject g in players)
            {
                curr_script = g.GetComponent<eduRigidBody>();
                curr_script.ApplyForce(gravity * curr_script.m_mass);
            }
        }

        if (m_use_breaking_torque)
        {
            foreach(GameObject g in players)
            {
                curr_script = g.GetComponent<eduRigidBody>();
                curr_script.ApplyTorque(breaking_torque);
            }
        }
    }
}
