using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ProtoSceneGUI : MonoBehaviour
{
    Ball _ball;
    Player _player;

    [SerializeField] float _arrowLenght;

    void Start()
    {
        _ball = FindObjectOfType<Ball>();
        _player = FindObjectOfType<Player>();
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 15;
        style.normal.textColor = Color.white;

        GUI.BeginGroup(new Rect(0, 0, 400, 300));
        EditorGUILayout.Space(10);


        GUI.Box(new Rect(0, 0, 400, 300), "Infos");
        Rigidbody rb = (Rigidbody)typeof(Ball).GetField("_rb", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("=== RIGHT MOUSE BUTTON TO MOVE SLIDERS ===", style);

        EditorGUILayout.Space(5);


        _arrowLenght = EditorGUILayout.Slider("Debug Arrow Lenght", _arrowLenght, 1, 100);

        EditorGUILayout.Space(5);

        _ball.ShootPower = EditorGUILayout.Slider("Ball Shoot Power", _ball.ShootPower, 0, 30);

        EditorGUILayout.Space(5);

        rb.drag = EditorGUILayout.Slider("Ball air drag", rb.drag, 0, 5);

        EditorGUILayout.Space(10);

        //BallVelocity
        EditorGUILayout.LabelField("Ball Velocity", style);
        EditorGUILayout.LabelField(rb.velocity.x.ToString(), style);
        EditorGUILayout.LabelField(rb.velocity.y.ToString(), style);
        EditorGUILayout.LabelField(rb.velocity.z.ToString(), style);
        EditorGUILayout.Space(5);

        float maxv = (float)typeof(Ball).GetField("_maxAngleShoot", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);
        float minv = (float)typeof(Ball).GetField("_minAngleShoot", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);
        AnimationCurve v = (AnimationCurve)typeof(Ball).GetField("_shootAngleRatio", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);
        float maxS = (float)typeof(Ball).GetField("_maxShootPower", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);

        float t = Mathf.Min(Vector3.Magnitude(_player.DebugDir) / maxS, 1);
        float baseAngle = Mathf.Max(Mathf.Lerp(minv, maxv, v.Evaluate(t)), 0);
        EditorGUILayout.LabelField("Current Angle", style);
        EditorGUILayout.LabelField(baseAngle.ToString(), style);
        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Spacebar to zero vel", style);


        GUI.EndGroup();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        //CurrDirArrow
        //Vector3 v = (Vector3)typeof(Player).GetField("DebugDir", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_player);
        DrawArrow.ForGizmo(_ball.transform.position, _player.DebugDir, Color.blue);

        //MaxAngleArrow
        float maxv = (float)typeof(Ball).GetField("_maxAngleShoot", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);
        Vector3 maxVec = Quaternion.AngleAxis(-maxv, transform.right) * transform.forward;
        DrawArrow.ForGizmo(_ball.transform.position, maxVec * _arrowLenght, Color.green);

        //MinAngleArrow
        float minv = (float)typeof(Ball).GetField("_minAngleShoot", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);
        Vector3 minVec = Quaternion.AngleAxis(-minv, transform.right) * transform.forward;
        DrawArrow.ForGizmo(_ball.transform.position, minVec * _arrowLenght, Color.yellow);

        //CurrAngleArrow
        AnimationCurve v = (AnimationCurve)typeof(Ball).GetField("_shootAngleRatio", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);
        float maxS = (float)typeof(Ball).GetField("_maxShootPower", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ball);

        float t = Mathf.Min(Vector3.Magnitude(_player.DebugDir) / maxS, 1);
        float baseAngle = Mathf.Max(Mathf.Lerp(minv, maxv, v.Evaluate(t)), 0);
        Vector3 dir = Quaternion.AngleAxis(-baseAngle, transform.right) * transform.forward;
        DrawArrow.ForGizmo(_ball.transform.position, dir * _arrowLenght, Color.red);
    }
#endif
}
