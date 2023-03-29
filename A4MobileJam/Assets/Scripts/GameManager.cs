using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;

    bool _isPause = false;

    
    List<Player> _gamePlayersList = new List<Player>();
    int _currPTurn = 0;
    
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] int _maxPlayerTurnNbr;
    int _currPlayerTurnNbr;

    Level _currentLevel;

    [SerializeField] RectTransform _slingshotOrigin;
    [SerializeField] Material _slingshotMaterial;
    [SerializeField] private Material _targetMaterial;
    [SerializeField] private GameObject _target;
    [SerializeField] private List<Level> _levelsPool = new List<Level>();

    [SerializeField] private GameObject _canvas;
    public RectTransform SlingshotOrigin
    {
        get => _slingshotOrigin;
    }

    public Material SlingshotMaterial
    {
        get => _slingshotMaterial;
    }
    public GameObject Target
    {
        get => _target;
    }

    public GameObject Canvas
    {
        get => _canvas;
    }

    public Level CurrentLevel
    {
        get => _currentLevel;
    }
    private void Awake()
    {
        instance = this; 
    }

    static public GameManager Instance
    {
        get => instance;
    }

    void Start()
    {
        StartGame(1);
    }

    void Update()
    {
        
    }


    public void Pause()
    {
        _isPause = !_isPause;
    }

    void StartGame(int pNbr)
    {
        _currPTurn = -1;
        _currPlayerTurnNbr = 1;

        _currentLevel = _levelsPool[Random.Range(0, _levelsPool.Count)];
        GameObject go = Instantiate(_currentLevel.LevelGeometry);

        //Spawn Target
        GameObject tar = Instantiate(_target, _currentLevel.TargetPosition, Quaternion.identity);

        for (int i = 0; i < pNbr; i++)
        {
            GameObject pGo = Instantiate(_playerPrefab);
            GameObject pBall = Instantiate(_ballPrefab, _currentLevel.PlayerStartPosition, Quaternion.identity);
            pGo.GetComponent<Player>().Ball = pBall.transform.GetChild(1).GetComponent<Ball>();
            pBall.transform.GetChild(1).GetComponent<Ball>().Player = pGo.GetComponent<Player>();
            _gamePlayersList.Add(pGo.GetComponent<Player>());
        }

        DoTurn();
    }

    public void DoTurn()
    {
        //Debug.Log("DoTurn");
        if (_currPTurn + 1 >= _gamePlayersList.Count) NextRound();
        else
        {
            _currPTurn++;
            _gamePlayersList[_currPTurn].StartTurn();
        } 
    }

    void NextRound()
    {
        //Debug.Log("NextRound");
        _currPlayerTurnNbr++;
        if (_currPlayerTurnNbr > _maxPlayerTurnNbr) EndGame();
        else
        {
            _currPTurn = -1;
            DoTurn();
        }
    }

    void EndGame()
    {
        Debug.Log("GAME END");
    }
}
