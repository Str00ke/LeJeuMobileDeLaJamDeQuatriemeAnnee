using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    LineRenderer _lr;

    Vector2 _inputPosition;

    SlingshotDrawer _drawer;

    public Action<Vector3> _onRelease;

    private Ball _ball;

    private int _turnNbr;

    private Camera _cam;

    //[SerializeField] string _name;
    //[SerializeField] int _Points;

    public int Points { get; private set; }
    public Image Img { get; private set; }
    public Sprite Spr { get; private set; }
    public string Name { get; private set; }

    public Ball Ball
    {
        get => _ball; 
        set => _ball = value;
    }

    public Vector3 DebugDir { get; private set; }

    public bool IsOnTarget { get; set; }

    public bool IsPlaying { get; set; }
    public bool HasFinished { get; private set; }

    public Camera Cam => _cam;

    public float CurrentDepth { get; set; }

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

    public void Init(Image img, string name, Sprite spr)
    {
        Img = img;
        Spr = spr;
        Name = name;
    }

    private void OnDestroy()
    {
        _ball._onStopRolling -= EndTurn;
    }

    void Update()
    {
        if (_cam.depth != CurrentDepth) _cam.depth = CurrentDepth;
        if (!IsPlaying) return;

        if (GameManager.Instance.TouchType == GameManager.ETouchType.MOUSE)
        {
            if (Input.GetMouseButtonDown(0) && _ball.IsStop) CreateSling();
            if (Input.GetMouseButtonUp(0) && _drawer != null) DestroySling();
        }
        else if (GameManager.Instance.TouchType == GameManager.ETouchType.TOUCH)
        {
            if (Input.touchCount > 0 && _ball.IsStop) CreateSling();
            if (Input.touchCount <= 0 && _drawer != null) DestroySling();
        }
            

        _drawer?.Update();

        if (_drawer != null)
        {
            Vector3 d = _drawer.CurrPos - _drawer.StartPos;
            d *= -1;
            //Vector3 dx = d - _ball.gameObject.transform.position;
            Vector3 v = new Vector3(d.x, 0, d.y);
            //Debug.Log(Vector3.Normalize(v) * Mathf.Min(Vector3.Magnitude(v), 15));
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

        _onRelease.Invoke(v);
        //_onRelease.Invoke(v);
        _drawer = null;
    }

    public void StartTurn(bool isFirst, Player prevPlayer)
    {
        CurrentDepth = 1;
        _turnNbr++;
        _ball.EnableMeshRender();
        if (!isFirst) PlaceCamera(prevPlayer.Cam.transform);
        else SetupDone();
    }

    void PlaceCamera(Transform start)
    {
        Transform tmp = _cam.transform;
        _cam.transform.GetComponent<CameraManager>().MoveToSet(start.position, tmp.position, start.rotation, tmp.rotation, 1, null, null, SetupDone);
    }

    bool SetupDone()
    {
        IsPlaying = true;
        Ball.ToggleInscription(true);
        return true;
    }

    public void GetPoints(Target tar)
    {
        Points = tar.AttribPoints(this);
        //Debug.Log(Points);
    }

    public void EndTurn()
    {
        if (IsOnTarget) HasFinished = true;
        CurrentDepth = 0;
        IsPlaying = false;
        Ball.ToggleInscription(false);
        GameManager.Instance.DoTurn();
    }

}
