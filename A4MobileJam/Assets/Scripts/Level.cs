using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level")]
public class Level : ScriptableObject
{
    [SerializeField] private string _levelName;

    [SerializeField] private GameObject _levelGeometry;

    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private float _targetRadius;

    [SerializeField] private Vector3 _playerStartPosition;

    [SerializeField] List<TargetPart> _targetParts = new List<TargetPart>();

    public Vector3 TargetPosition
    {
        get { return _targetPosition; }
        set { _targetPosition = value; }
    }

    public Vector3 PlayerStartPosition
    {
        get { return _playerStartPosition; }
        set { _playerStartPosition = value; }
    }

    public string LevelName
    {
        get { return _levelName; }
        set { _levelName = value; }
    }

    public GameObject LevelGeometry
    {
        get { return _levelGeometry; }
        set { _levelGeometry = value; }
    }
}
