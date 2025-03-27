using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduForces : MonoBehaviour
{
    //Konstanter
    Vector2 gravity = new Vector2(0, -9.82f);
    Vector2 wind_force;
    Vector2 buoyancy_force;

    float breaking_torque = -150f;
    float air_density = 1.23f;

    public float m_wind_speed;
    public float m_fluid_density;
    public float m_fluid_level;

    
    //Allmänna variabler
    GameObject[] players;
    eduRigidBody curr_script;
    public bool m_use_gravity, m_use_breaking_torque, m_use_wind, m_use_buoyancy;

    // Update is called once per frame

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void FixedUpdate()
    {
        Gravity();
        BreakTorque();
        Wind();
        Buoyancy();
    }

    private void Gravity()
    {
        if (m_use_gravity)
        {
            foreach (GameObject g in players)
            {
                curr_script = g.GetComponent<eduRigidBody>();
                curr_script.ApplyForce(gravity * curr_script.m_mass);
            }
        }
    }

    private void BreakTorque()
    {
        if (m_use_breaking_torque)
        {
            foreach (GameObject g in players)
            {
                curr_script = g.GetComponent<eduRigidBody>();
                curr_script.ApplyTorque(breaking_torque);
            }
        }
    }


    private void Wind()
    {
        if (m_use_wind)
        {
            foreach (GameObject g in players)
            {
                curr_script = g.GetComponent<eduRigidBody>();

                wind_force = (Vector2)transform.right * (0.5f * air_density * m_wind_speed * m_wind_speed * g.GetComponent<eduCircleCollider>().m_area * curr_script.m_drag_coef + Mathf.Abs(g.transform.position.y)); //Siste termen är för variation i styrka i y-led

                curr_script.ApplyForce(wind_force);
            }
        }
    }

    private void Buoyancy()
    {
        Debug.DrawLine(new Vector2(0, m_fluid_level) + Vector2.left * 10, new Vector2(0, m_fluid_level) + Vector2.right * 10, Color.blue);

        if(m_use_buoyancy)
        {
            foreach(GameObject g in players)
            {
                eduCircleCollider collider = g.GetComponent<eduCircleCollider>();
                curr_script = g.GetComponent<eduRigidBody>();

                if (g.transform.position.y - collider.m_radius > m_fluid_level)
                    buoyancy_force = Vector2.zero;
                else
                {

                    float pt = gravity.y * m_fluid_density * (g.transform.position.y + collider.m_radius - m_fluid_level); //höjden från vätskeytan 
                    float pb = gravity.y * m_fluid_density * (g.transform.position.y - collider.m_radius - m_fluid_level); //height from fluid level to bottom

                    buoyancy_force.y = pt * collider.m_area  + pb * collider.m_area;
                }

                curr_script.ApplyForce(buoyancy_force);
            }
        }
    }
}
