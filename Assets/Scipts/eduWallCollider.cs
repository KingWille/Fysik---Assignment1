using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eduWallCollider : MonoBehaviour
{
    enum WallType
    {
        Left, Right, Bottom, Top
    }

    WallType wall_type;
    // Update is called once per frame
    void Update()
    {
        switch(wall_type)
        {
            case WallType.Left:
                Debug.DrawLine(transform.position, -transform.right);
                break;
            case WallType.Right:
                Debug.DrawLine(transform.position, transform.right);
                break;
            case WallType.Bottom:
                Debug.DrawLine(transform.position, -transform.up);
                break;
            case WallType.Top:
                Debug.DrawLine(transform.position, transform.up);
                break;
        }
    }
}
