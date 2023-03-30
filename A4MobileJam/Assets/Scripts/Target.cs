using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[Serializable]
public struct TargetPart
{
    public float _outerRange;
    public float _innerRange;
    public int _points;
    public Color _col;
}

[Serializable]
public class Target : MonoBehaviour
{
    [SerializeField] List<TargetPart> _parts = new List<TargetPart>();

    private Material _targetMaterial;

    [SerializeField] float _minRadiusEngage;

    Player _currPlayer;

    public Player CurrPlayer
    {
        get => _currPlayer;
        set => _currPlayer = value;
    }

    private void Start()
    {
        _currPlayer = FindObjectOfType<Player>();
    }

    public void Init(List<TargetPart> list)
    {
        _parts = list;
        //_targetMaterial = new Material(GameManager.Instance.TargetMaterial);
        //_targetMaterial.set
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _currPlayer.Ball.transform.position) < _minRadiusEngage)
        {
            _currPlayer.IsOnTarget = true;
            _currPlayer.Ball.GetComponentInChildren<CameraManager>().SetToTarget();
        }
        //Debug.Log(AttribPoints(_currPlayer));
    }

    public int AttribPoints(Player p)
    {
        float dist = Vector3.Distance(transform.position, p.Ball.transform.parent.GetChild(0).position/*transform.TransformPoint(p.Ball.transform.position)*/);
        for (int i = _parts.Count - 1; i >= 0; --i)
        {
            //Debug.Log("ForP: " + _parts[i]._points + " " + dist + " " + _parts[i]._outerRange + " " + (dist <= _parts[i]._outerRange));
            if (dist <= _parts[i]._outerRange) return _parts[i]._points;
        }
        return 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _parts.Count; i++)
        {
            //Gizmos.color = _parts[i]._col;
            //Gizmos.DrawSphere(transform.position, _parts[i]._outerRange);
            UnityEditor.Handles.color = _parts[i]._col;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, _parts[i]._outerRange);
        }
    }

#endif
}
