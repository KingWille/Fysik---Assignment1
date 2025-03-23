using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduCollisionDetection : MonoBehaviour
{
    GameObject[] circleColliders;
    GameObject[] wallColliders;
    GameObject[] planeColliders;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CicleCircleCollision();
        CircleWallCollision();
        CirclePlaneCollision();
    }

    private void Init()
    {
        circleColliders = GameObject.FindGameObjectsWithTag("Player");
        //wallColliders = GameObject.FindGameObjectsWithTag("Wall");
        //planeColliders = GameObject.FindGameObjectsWithTag("Plane");
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

                if (Vector2.Distance(c1.transform.position, c2.transform.position) <= radius1 + radius2)
                    EvaluateCollision(c1, c2, radius1, radius2);
            }
        }
    }

    private void CircleWallCollision()
    {

    }

    private void CirclePlaneCollision()
    {

    }

    private void EvaluateCollision(GameObject obj1, GameObject obj2, float radius1, float radius2)
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
        float ERP = 0.8f;

        float penetration = (radius2 + radius1) - Vector2.Distance(obj1.transform.position, obj1.transform.position);


        float P = ERP * massAverage * penetration;

        Vector2 correction1 = rb1.m_is_static ? Vector3.zero : -P / rb1.m_mass * collisionNormal;
        Vector2 correction2 = rb2.m_is_static ? Vector3.zero : P / rb2.m_mass * collisionNormal;


        obj1.transform.position += (Vector3)correction1;
        obj2.transform.position += (Vector3)correction2;
    }
}
