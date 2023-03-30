using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;

    bool _isPause = false;
    bool _isFirst = true;
    bool _start = true;


    public enum ETouchType
    {
        MOUSE,
        TOUCH
    }

    [SerializeField] private ETouchType _touchType;
    [SerializeField] private FinalScoreManager _scoreManager;

    [SerializeField] private int _debugPlayerNbr;

    public ETouchType TouchType => _touchType; 
    
    List<Player> _gamePlayersList = new List<Player>();
    int _currPTurn = 0;
    
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] int _maxPlayerTurnNbr;
    int _currPlayerTurnNbr;
    Player _currPlayer;

    Level _currentLevel;

    [SerializeField] RectTransform _slingshotOrigin;
    [SerializeField] Material _slingshotMaterial;
    [SerializeField] private Material _targetMaterial;
    [SerializeField] private GameObject _target;
    [SerializeField] private List<Level> _levelsPool = new List<Level>();

    [SerializeField] private GameObject _canvas;

    [SerializeField] private GameObject _pTurnTxt;
    [SerializeField] private AnimationCurve _pTurnTxtAC;
    [SerializeField] private float _pTurnTxtSpeed;
    [SerializeField] private Vector3 _pTurnTxtPosUp;
    [SerializeField] private Vector3 _pTurnTxtPosDown;

    [SerializeField] private AnimationCurve _startCamACPos;
    [SerializeField] private AnimationCurve _startCamACRot;
    [SerializeField] private float _startCamSpeed;

    [SerializeField] Camera _startCam;


    GlobalManager _globalMan;

    Target _currTarget;
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
        _globalMan = FindObjectOfType<GlobalManager>();
        if (_globalMan.Sets.Count <= 0) StartGame(_debugPlayerNbr);
        else StartGame(_globalMan.Sets.Count);
    }

    private void Update()
    {
        if (_start)
        {
            if (_startCam == null) _startCam = GameObject.Find("StartCamera").GetComponent<Camera>();
            Vector3 start = _currTarget.transform.position;
            start.y += 350;
            Vector3 end = _gamePlayersList[0].Cam.transform.position;

            Quaternion startRot = Quaternion.Euler(new Vector3(90, 0, 0));
            Quaternion endRot = _gamePlayersList[0].Cam.transform.rotation;

            _startCam.depth = 50;
            _startCam.transform.GetComponent<CameraManager>().MoveToSet(start, end, startRot, endRot, _startCamSpeed, _startCamACPos, _startCamACRot, CallNextRound, 1.75f, 1.5f);
            _start = false;
        }
    }
    public void Pause()
    {
        _isPause = !_isPause;
    }

    IEnumerator MoveText(RectTransform txt, Vector3 sP, Vector3 fP, float speed, AnimationCurve posCurve, Func<bool> onFinished = null, float waitStart = 0.0f, float waitEnd = 0.0f)
    {
        yield return new WaitForSeconds(waitStart);

        float t = 0;

        while (t < 1.0f)
        {
            txt.localPosition = Vector3.Lerp(sP, fP, (posCurve != null) ? posCurve.Evaluate(t) : t);
            yield return null;
            t += Time.deltaTime * speed;
        }
        yield return null;
        yield return new WaitForSeconds(waitEnd);

        if (onFinished != null) onFinished();
    }

    void StartGame(int pNbr)
    {
        _currPTurn = -1;
        _currPlayerTurnNbr = 0;

        _currentLevel = _levelsPool[UnityEngine.Random.Range(0, _levelsPool.Count)];
        

        GameObject go = Instantiate(_currentLevel.LevelGeometry);

        //Spawn Target
        GameObject tar = Instantiate(_target, _currentLevel.TargetPosition, Quaternion.identity);
        _currTarget = tar.GetComponent<Target>();
        _currTarget.Init(_currentLevel.TargetParts);

        for (int i = 0; i < pNbr; i++)
        {
            GameObject pGo = Instantiate(_playerPrefab);
            GameObject pBall = Instantiate(_ballPrefab, _currentLevel.PlayerStartPosition, Quaternion.identity);
            pGo.GetComponent<Player>().Ball = pBall.transform.GetChild(1).GetComponent<Ball>();
            if (_globalMan.Sets.Count > 0)
            {
                pGo.GetComponent<Player>().Init(_globalMan.Sets[i]._img, _globalMan.Sets[i]._strName);
                pGo.GetComponent<Player>().Ball.BallMat = _globalMan.Sets[i]._mat;
            }
            else
                pGo.GetComponent<Player>().Init(null, "Player_" + (i+1).ToString());
            pBall.transform.GetChild(1).GetComponent<Ball>().Player = pGo.GetComponent<Player>();
            if (i == 0)
            {
                pBall.transform.GetChild(1).GetComponent<Ball>().EnableMeshRender();
            }
            _gamePlayersList.Add(pGo.GetComponent<Player>());
        }

    }

    public void DoTurn()
    {
        if (CheckEnd())
        {
            EndGame();
            return;
        }
        else
        {
            if (_currPTurn + 1 >= _gamePlayersList.Count) NextRound();
            else
            {
                _currPTurn++;
                if (_gamePlayersList[_currPTurn].HasFinished)
                {
                    DoTurn();
                    return;
                }
                else
                {
                    StartCoroutine(MoveText(_pTurnTxt.GetComponent<RectTransform>(), _pTurnTxtPosUp, _pTurnTxtPosDown, _pTurnTxtSpeed, _pTurnTxtAC, PlayerStartTurn, 0, 0.25f));
                    _gamePlayersList[_currPTurn].StartTurn(_isFirst, _currPlayer);
                    _currPlayer = _gamePlayersList[_currPTurn];
                    _currPlayer.IsPlaying = false;
                    _pTurnTxt.GetComponent<Text>().text = _currPlayer.Name + " turn";
                    _currTarget.CurrPlayer = _currPlayer;                
                    if (_isFirst) _isFirst = false;
                }
            }
        } 
    }

    bool PlayerStartTurn()
    {
        _currPlayer.IsPlaying = true;
        return true;
    }

    bool CallNextRound()
    {
        _startCam.depth = -50;
        NextRound();
        return true;
    }
   
    void NextRound()
    {
        _currPlayerTurnNbr++;
        if (_currPlayerTurnNbr > _maxPlayerTurnNbr) EndGame();
        else
        {
            _currPTurn = -1;
            StartCoroutine(MoveText(_pTurnTxt.GetComponent<RectTransform>(), _pTurnTxtPosUp, _pTurnTxtPosDown, _pTurnTxtSpeed, _pTurnTxtAC, CallDoTurn, 1, 1));
            _pTurnTxt.GetComponent<Text>().text = "Round " + _currPlayerTurnNbr;
        }
    }

    bool CallDoTurn()
    {
        DoTurn();
        return true;
    }

    public bool CheckEnd()
    {
        bool res = true;

        foreach (Player p in _gamePlayersList)
        {
            if (!p.HasFinished) res = false;
        }
        return res;
    }

    void EndGame()
    {
        foreach (Player p in _gamePlayersList)
        {
            p.IsPlaying = false;
            p.GetPoints(_currTarget);
        }
        _scoreManager.ShowScore(_gamePlayersList);
    }
}
