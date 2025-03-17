using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduPlaneCollider : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.up);
    }
}
