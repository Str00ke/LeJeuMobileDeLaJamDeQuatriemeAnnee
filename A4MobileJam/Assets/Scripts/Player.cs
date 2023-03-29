using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    LineRenderer _lr;

    Vector2 _inputPosition;

    SlingshotDrawer _drawer;

    public Action<Vector3> _onRelease;

    private Ball _ball;

    private int _turnNbr;

    private Camera _cam;

    public Ball Ball
    {
        get => _ball; 
        set => _ball = value;
    }

    public Vector3 DebugDir { get; private set; }

    public bool IsOnTarget { get; set; }

    public bool IsPlaying { get; set; }

    public Camera Cam => _cam;

    private void Awake()
    {
        DebugDir = Vector3.zero;
        IsOnTarget = false;
    }

    void Start()
    {
        _ball._onStopRolling += EndTurn;
        _cam = _ball.GetComponentInChildren<Camera>();
    }

    private void OnDestroy()
    {
        _ball._onStopRolling -= EndTurn;
    }

    void Update()
    {

        if (!IsPlaying) return;
        if (Input.GetMouseButtonDown(0) && _ball.IsStop) CreateSling();
        if (Input.GetMouseButtonUp(0) && _drawer != null) DestroySling();

        _drawer?.Update();

        if (_drawer != null)
        {
            Vector3 d = _drawer.CurrPos - _drawer.StartPos;
            d *= -1;
            //Vector3 dx = d - _ball.gameObject.transform.position;
            Vector3 v = new Vector3(d.x, 0, d.y);
            Debug.Log(Vector3.Magnitude(v));

            DebugDir = v;
        }
    }

    void CreateSling()
    {
        _drawer = new SlingshotDrawer(this, GameManager.Instance.SlingshotOrigin.position, GameManager.Instance.Canvas);
    }

    void DestroySling()
    {
        Vector3 d = _drawer.CurrPos - _drawer.StartPos;
        d *= -1;
        //Vector3 dx = d - _ball.gameObject.transform.position;
        Vector3 v = new Vector3(d.x, 0, d.y);

        _onRelease.Invoke(Vector3.Normalize(v));
        //_onRelease.Invoke(v);
        _drawer = null;
    }

    public void StartTurn()
    {
        _turnNbr++;
        IsPlaying = true;
    }


    public void EndTurn()
    {
        GameManager.Instance.DoTurn();
    }

}
