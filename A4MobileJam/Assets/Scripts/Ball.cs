using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody _rb;
    Collider _col;
    MeshRenderer _meshRenderer;
    bool _mrToggle = false;

    public MeshRenderer mr
    {
        get { return _meshRenderer; }
        set { _meshRenderer = value; }
    }

    Vector3 _dirVec;

    bool _isRolling = false;

    private Player _player; //Associated player

    [SerializeField] GameObject _ball;

    [SerializeField] private float _shootPowerCoeff;
    //[SerializeField] private float _shootAngle;

    [SerializeField] [Range(0.0f, 90f)] private float _minAngleShoot;
    [SerializeField] [Range(0.0f, 90f)] private float _maxAngleShoot;
    [SerializeField]                    private float _maxShootPower;
    [SerializeField]                    private AnimationCurve _shootAngleRatio;
    [SerializeField]                    private float _minVelBeforeStop;

    //public float ShootPower => _shootPowerCoeff;
    //public float ShootAngle => _shootAngle;

    public System.Action _onStopRolling;

    bool _isBegin = true;

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public float ShootPower
    {
        get { return _shootPowerCoeff; }
        set { _shootPowerCoeff = value; }
    }


    public bool IsStop => _rb.velocity == Vector3.zero;

    void Start()
    {
        _rb = _ball.GetComponent<Rigidbody>();
        _col = _ball.GetComponent<SphereCollider>();
        _meshRenderer = _ball.GetComponent<MeshRenderer>();
        _col.enabled = false;
        _meshRenderer.enabled = false;
        _rb.useGravity = false;

        _player.Ball = this;
    }

    private void OnDestroy()
    {
        ToggleInscription(false);
    }

    void Update()
    {
        //if (_player.IsPlaying && !_meshRenderer.enabled) _meshRenderer.enabled = true;
        if (_mrToggle && _meshRenderer != null && !_meshRenderer.enabled) _meshRenderer.enabled = true;

        if (Input.GetKeyDown(KeyCode.Space)) StopBallVel();

        if (!_player.IsOnTarget) transform.position = _ball.transform.position;

        if (Vector3.Magnitude(_rb.velocity) < _minVelBeforeStop) StopBallVel();
    }

    Vector3 GetShootDirection(Vector3 dir)
    {

        _dirVec = dir / _shootPowerCoeff;//Vector3.Normalize(dir) * Mathf.Min(Vector3.Magnitude(dir), _maxShootPower);
        float t = Mathf.Min((Vector3.Magnitude(dir) / _shootPowerCoeff) / _maxShootPower, 1);
        float baseAngle = Mathf.Max(Mathf.Lerp(_minAngleShoot, _maxAngleShoot, _shootAngleRatio.Evaluate(t)), 0);
        _dirVec = Quaternion.AngleAxis(-baseAngle, transform.right) * _dirVec;
        return _dirVec;
        //return Vector3.Normalize(_dirVec);
    }

    void Shoot(Vector3 dir)
    {
        if (_isBegin)
        {
            _col.enabled = true;
            _rb.useGravity = true;
            _meshRenderer.enabled = true;
            _isBegin = false;
        }
        _rb.velocity = GetShootDirection(dir)/* * _shootPowerCoeff*/;
        _isRolling = true;
    }

    public void EnableMeshRender()
    {
        _mrToggle = true;
    }


    public void ToggleInscription(bool play)
    {
        if (play) _player._onRelease += Shoot;
        else _player._onRelease -= Shoot;
    }

    private void StopBallVel()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.Sleep();
        if (_isRolling)
        {
            _isRolling = false;
            _onStopRolling.Invoke();
        }

    }
}
