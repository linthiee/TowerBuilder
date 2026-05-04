using System;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField] GameObject rope;
    [SerializeField] Transform pivot;

    private float gravity = 9.81f;
    private float amplitude = 15.0f;
    void Start()
    {

    }

    private void FixedUpdate()
    {
        float sin = Mathf.Sin(Time.time) * amplitude;

        float acceleration = -(gravity / rope.transform.localScale.y) * sin;

        Vector3 euler = pivot.localEulerAngles;
        euler.z = acceleration;
        pivot.eulerAngles = euler;
    }
    void Update()
    {

    }
}
