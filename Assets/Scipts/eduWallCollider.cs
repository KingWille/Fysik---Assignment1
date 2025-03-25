using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduWallCollider : MonoBehaviour
{
    public float m_density;

    public eduRigidBody rb;

    public Vector2 m_normal;
    public enum WallType
    {
        Left, Right, Bottom, Top
    }

    public WallType m_wall_type;

    private void Start()
    {
        rb.m_mass = m_density * transform.lossyScale.x * transform.lossyScale.y;
    }
    // Update is called once per frame
    void Update()
    {
        switch(m_wall_type)
        {
            case WallType.Left:
                Debug.DrawLine(transform.position, transform.position + Vector3.right);
                m_normal = (Vector2)transform.position + Vector2.right;
                break;
            case WallType.Right:
                Debug.DrawLine(transform.position, transform.position + Vector3.left);
                m_normal = (Vector2)transform.position + Vector2.left;
                break;
            case WallType.Bottom:
                Debug.DrawLine(transform.position, transform.position + Vector3.up);
                m_normal = (Vector2)transform.position + Vector2.up;
                break;
            case WallType.Top:
                Debug.DrawLine(transform.position, transform.position + Vector3.down);
                m_normal = (Vector2)transform.position + Vector2.down;
                break;
        }
    }
}
