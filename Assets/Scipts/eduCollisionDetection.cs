using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduCollisionDetection : MonoBehaviour
{
    GameObject[] circleColliders;
    GameObject[] wallColliders;
    GameObject[] planeColliders;

    float distance;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CicleCircleCollision();
        //CircleWallCollision();
        CirclePlaneCollision();
    }

    private void Init()
    {
        circleColliders = GameObject.FindGameObjectsWithTag("Player");
        //wallColliders = GameObject.FindGameObjectsWithTag("Wall");
        planeColliders = GameObject.FindGameObjectsWithTag("Plane");
    }

    private void CicleCircleCollision()
    {
        GameObject c1;
        GameObject c2;

        float radius1;  
        float radius2;  

        for (int i = 0; i < circleColliders.Length - 1; i++) //Första loopen assignar bara vilken som är första cirkeln och vilken radie den har
        {
            c1 = circleColliders[i];
            radius1 = circleColliders[i].GetComponent<eduCircleCollider>().m_radius;

            for(int j = i + 1; j < circleColliders.Length; j++) //Andra loopen assignar andra cirkeln och dens radie, samt kolla avståndet mellan c1 och c2
            {
                c2 = circleColliders[j];
                radius2 = circleColliders[j].GetComponent<eduCircleCollider>().m_radius;
                distance = Vector2.Distance(c1.transform.position, c2.transform.position);

                if ( distance <= radius1 + radius2)
                    EvaluateCollisionCircle(c1, c2, radius1, radius2);
            }
        }
    }

    private void CirclePlaneCollision()
    {
        GameObject plane;
        GameObject circle;

        float radius;
        float thickness;

        for (int i = 0; i < circleColliders.Length; i++)
        {
            circle = circleColliders[i];
            radius = circle.GetComponent<eduCircleCollider>().m_radius;

            for(int j = 0; j < planeColliders.Length; j++)
            {
                plane = planeColliders[j];
                thickness = Mathf.Min(plane.transform.lossyScale.x, plane.transform.lossyScale.y);

                if (ClosestPoint(plane, circle, radius, thickness))
                    EvaluateCollisionPlane(plane, circle, radius, thickness);
            }
        }
    }
    #region WALL_COLLISIONS
    //private void CircleWallCollision()
    //{
    //    GameObject wall;
    //    GameObject circle;
    //    eduWallCollider collider;

    //    float radius;
    //    float thickness;

    //    for (int i = 0; i < circleColliders.Length; i++)
    //    {
    //        circle = circleColliders[i];
    //        radius = circle.GetComponent<eduCircleCollider>().m_radius;

    //        for(int j = 0; j < wallColliders.Length; j++)
    //        {
    //            wall = wallColliders[j];
    //            thickness = Mathf.Min(wall.transform.lossyScale.x, wall.transform.lossyScale.y);
    //            collider = wall.GetComponent<eduWallCollider>();

    //            distance = collider.m_wall_type == eduWallCollider.WallType.Top || collider.m_wall_type == eduWallCollider.WallType.Bottom 
    //                ? Vector2.Distance(new Vector2(circle.transform.position.x, wall.transform.position.y), circle.transform.position) //jämför endast y positionen på cirkel och vägg
    //                : Vector2.Distance(new Vector2(wall.transform.position.x, circle.transform.position.y), circle.transform.position); //Jämför endast x position == " ==

    //            if(distance <= radius + thickness) 
    //                EvaluateCollisionWall(wall, circle, radius, thickness);
    //        }
    //    }

    //}

    //private void EvaluateCollisionWall(GameObject wall, GameObject circle, float radius, float thickness)
    //{
    //    eduRigidBody rb1 = wall.GetComponent<eduRigidBody>();
    //    eduRigidBody rb2 = circle.GetComponent<eduRigidBody>();
    //    eduWallCollider collider = wall.GetComponent<eduWallCollider>();

    //    //Impuls beräkning
    //    Vector2 collisionNormal;

    //    //Hämta hem korrekt normal för väggen
    //    if (collider.m_wall_type == eduWallCollider.WallType.Top || collider.m_wall_type == eduWallCollider.WallType.Bottom)
    //        collisionNormal = new Vector2(0, circle.transform.position.y - wall.transform.position.y);
    //    else
    //        collisionNormal = new Vector2(circle.transform.position.x - wall.transform.position.x, 0);

    //    collisionNormal = collisionNormal.normalized;

    //    float massAverage = rb1.m_mass * rb2.m_mass / (rb1.m_mass + rb2.m_mass);

    //    float CAverage = (rb1.m_restitution_coef + rb2.m_restitution_coef) / 2;

    //    float relVel = Vector2.Dot(rb2.m_velocity - rb1.m_velocity, collisionNormal);

    //    float impulseMagnitude = massAverage * (1 + CAverage) * relVel;

    //    rb2.ApplyImpulse(-(impulseMagnitude / rb2.m_mass) * collisionNormal);

    //    //Positions rättning
    //    float ERP = 1f;

    //    float penetration = radius + thickness / 2 - distance;

    //    if (penetration > 0)
    //    {
    //        float P = ERP * massAverage * penetration;

    //        Vector2 correction = rb2.m_is_static ? Vector3.zero : P / rb2.m_mass * collisionNormal;

    //        circle.transform.position += (Vector3)correction;
    //    }
    //}
    #endregion
    private void EvaluateCollisionCircle(GameObject obj1, GameObject obj2, float radius1, float radius2)
    {
        eduRigidBody rb1 = obj1.GetComponent<eduRigidBody>();
        eduRigidBody rb2 = obj2.GetComponent<eduRigidBody>();

        //Impuls beräkning
        Vector2 collisionNormal = (rb2.m_position - rb1.m_position).normalized;

        float massAverage = rb1.m_mass * rb2.m_mass / (rb1.m_mass + rb2.m_mass);

        float CAverage = (rb1.m_restitution_coef + rb2.m_restitution_coef) / 2;

        float relVel = Vector2.Dot(rb2.m_velocity - rb1.m_velocity, collisionNormal);

        float impulseMagnitude = massAverage * (1 + CAverage) * relVel;

        rb1.ApplyImpulse((impulseMagnitude / rb1.m_mass) * collisionNormal);
        rb2.ApplyImpulse(-(impulseMagnitude / rb2.m_mass) * collisionNormal);

        //Positions rättning
        float ERP = 1f;

        float penetration = (radius2 + radius1) - Vector2.Distance(obj1.transform.position, obj2.transform.position);

        if (penetration > 0)
        {
            float P = ERP * massAverage * penetration;

            Vector2 correction1 = rb1.m_is_static ? Vector3.zero : -P / rb1.m_mass * collisionNormal;
            Vector2 correction2 = rb2.m_is_static ? Vector3.zero : P / rb2.m_mass * collisionNormal;


            obj1.transform.position += (Vector3)correction1;
            obj2.transform.position += (Vector3)correction2;
        }
    }

    private void EvaluateCollisionPlane(GameObject plane, GameObject circle, float radius1, float thickness)
    {
        eduRigidBody rb1 = plane.GetComponent<eduRigidBody>();
        eduRigidBody rb2 = circle.GetComponent<eduRigidBody>();

        //Impuls beräkning
        Vector2 collisionNormal = plane.GetComponent<eduPlaneCollider>().m_normal;

        float massAverage = rb1.m_mass * rb2.m_mass / (rb1.m_mass + rb2.m_mass);

        float CAverage = (rb1.m_restitution_coef + rb2.m_restitution_coef) / 2;

        float relVel = Vector2.Dot(rb2.m_velocity - rb1.m_velocity, collisionNormal);

        float impulseMagnitude = massAverage * (1 + CAverage) * relVel;

        rb2.ApplyImpulse(-(impulseMagnitude / rb2.m_mass) * collisionNormal);

        //Positions rättning
        float ERP = 1f;

        float penetration = radius1 + thickness / 2 - distance;

        if (penetration > 0)
        {
            float P = ERP * massAverage * penetration;

            Vector2 correction = rb2.m_is_static ? Vector3.zero : P / rb2.m_mass * collisionNormal;

            circle.transform.position += (Vector3)correction;
        }
    }

    private bool ClosestPoint(GameObject plane, GameObject circle, float radius, float thickness)
    {
        float len = Mathf.Max(plane.transform.lossyScale.x, plane.transform.lossyScale.y); //Hittar längden på planet

        Vector2 posLeft = plane.transform.position - plane.transform.right * len / 2; // hämtar koordinaterna för vänstra extrempunkten
        Vector2 posRight = plane.transform.position + plane.transform.right * len / 2; // == "" == högra extrempunkten

        Vector2 lineDirection = (posRight - posLeft).normalized;
        Vector2 circleDirection = (Vector2)circle.transform.position - posLeft;

        float dot = Vector2.Dot(circleDirection, lineDirection);

        dot = Mathf.Clamp(dot, 0, len);

        Vector2 closest = posLeft + lineDirection * dot; //Kollar närmsta punkten på linjen innan för linjens gränser

        distance = Vector2.Distance(closest, circle.transform.position);

        return distance <= radius + thickness / 2;

    }
}
