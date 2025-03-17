using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class eduRigidBody : MonoBehaviour
{
    public Vector2 m_velocity;
    public Vector2 m_force;

    public float m_ang_velocity;
    public float m_torque;
    public float m_mass;
    public float m_inertia;
    public float m_restitution_coef;

    public int m_frame_skip;
    int frame_skip_counter;

    bool inertia_zero;

    readonly Vector3 rotation_axis = new Vector3(0, 0, 1);

    //Variabler för analytisk linje
    Vector2 draw_line_pos = new Vector2(0, 0);
    Vector2 draw_line_new_pos;
    Vector2 draw_line_Vel = new Vector2(10, 10);
    float time_passed = 0;

    private void FixedUpdate()
    {
        //Skippar frames baserat på m_frame_skip
        if(frame_skip_counter < m_frame_skip)
        {
            frame_skip_counter++;
            m_force = Vector2.zero;
            m_torque = 0;
            return;
        }

        //Beräknar hastigtheten före positionen som semi implicit Euler kräver
        m_velocity += (m_force / m_mass) * Time.fixedDeltaTime * Mathf.Max(1, m_frame_skip);

        transform.position += (Vector3)(m_velocity * Time.fixedDeltaTime * Mathf.Max(1, m_frame_skip));

        //Beräknar vinkelhastigheten
        transform.Rotate(rotation_axis, m_ang_velocity);

        m_ang_velocity += m_torque * Mathf.Pow(m_inertia, -1) * Time.fixedDeltaTime * Mathf.Max(1, m_frame_skip);

        //Återställer variabler
        m_force = Vector2.zero;
        m_torque = 0.0f;
        frame_skip_counter = 0;

        //Analytisk linje
        if (m_frame_skip == 0)
        {
            draw_line_new_pos = draw_line_Vel * time_passed + (new Vector2(0, -9.82f) * Mathf.Pow(time_passed, 2)) / 2;
            Debug.DrawLine(draw_line_pos, draw_line_new_pos, Color.white, 10f);
            draw_line_pos = draw_line_new_pos;
        }

        //För uppgift 1.4
        if (m_ang_velocity <= 0 && !inertia_zero)
        {
            Debug.Log(time_passed + " " + name);
            inertia_zero = true;
        }

        //Variabel för tid som har passerat
        time_passed += Time.fixedDeltaTime * Mathf.Max(1, m_frame_skip);
    }

    public void ApplyForce(Vector2 f)
    {
        m_force += f;
    }

    public void ApplyTorque(float t)
    {
        m_torque += t;
    }

    public void ApplyImpulse(Vector2 j)
    {

    }
}
