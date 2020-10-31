using System;
using UnityEngine;

public class TestDot : MonoBehaviour
{
    public Transform A;
    public Transform B;

    public Transform C;
    public Transform D;

    public float size;
    
    public Vector3 a => A.position;
    public Vector3 b => B.position;

    public Vector3 c => C.position;
    public Vector3 d => D.position;

    public float Dot = 0f;
    public float Dotr = 0f;
    
    private void Update()
    {
        Vector3 x = (b - a).normalized;
        Vector3 y = (d - c).normalized;
        Dot = Vector3.Dot(x, y);
        Dotr = Vector3.Dot(y, x);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(a, b);
        
        Gizmos.DrawCube(b, size * Vector3.one);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(c, d);
        
        Gizmos.DrawCube(d, size * Vector3.one);
    }
}