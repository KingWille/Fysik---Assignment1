using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduExplosion : MonoBehaviour
{
    public float m_max_force;
    public float m_interval;

    float radius;
    float buffer;

    bool allow_coroutine;

    eduCircleCollider curr_script;
    GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        allow_coroutine = true;

        players = GameObject.FindGameObjectsWithTag("Player");
        radius = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (allow_coroutine)
            StartCoroutine(DetermineObjects());
    }

    IEnumerator DetermineObjects() //Coroutine som k�rs med intervallet. 
    {
        Debug.Log("Entered coroutine");

        allow_coroutine = false;

        foreach(GameObject obj in players)
        {
            curr_script = obj.GetComponent<eduCircleCollider>();

            buffer = radius + curr_script.m_radius;

            if (Vector3.Distance(obj.transform.position, gameObject.transform.position) < buffer) //Kollar avst�ndet mellan en cirkelt och explosionen
                AddExplosionForce(obj.GetComponent<eduRigidBody>(), obj);
        }

        yield return new WaitForSeconds(m_interval);

        allow_coroutine = true;
    }

    void AddExplosionForce(eduRigidBody curr_rb, GameObject circle)
    {
        Debug.Log("Entered Explosion");

        float multiplier = Vector3.Distance(circle.transform.position, gameObject.transform.position) / buffer; //F�r ut i procent hur l�ngt ifr�n circkeln �r, d�r 1.0 motsvarar att cirkeln �r precis p� radie + radie avst�nd

        Vector3 direction = (circle.transform.position - transform.position).normalized;

        curr_rb.ApplyImpulse((1 - multiplier) * m_max_force * direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(transform.position, radius);
    }
}
