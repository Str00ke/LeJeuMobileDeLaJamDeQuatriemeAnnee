using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Ball))]
public class DebugBallArrow : MonoBehaviour
{
    Ball _ball;

    private void Awake()
    {
        _ball = GetComponent<Ball>();
    }

    void Update()
    {
        Vector3 vec = _ball.transform.forward;
        //vec = Quaternion.AngleAxis(_ball.ShootAngle, transform.right) * vec;
        DrawArrow.ForDebug(transform.position, vec * _ball.ShootPower, Color.red);
    }
}
