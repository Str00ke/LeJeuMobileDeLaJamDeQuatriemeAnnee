using UnityEngine;
using UnityEngine.UI;


public class SlingshotDrawer
{
    private LineRenderer _lineRenderer;

    private Player _player;

    GameObject line;
    public SlingshotDrawer(Player player, Vector3 startPos, GameObject canvas)
    {
        _player = player;

        Init(startPos, canvas);
    }

    public void Init(Vector3 startPos, GameObject canvas)
    {
        _startPos = startPos;
        GameObject rotate = new GameObject("Rotate");
        rotate.AddComponent<RectTransform>();
        rotate.transform.parent = canvas.transform;
        line = new GameObject("Size");
        line.transform.parent = rotate.transform;
        line.AddComponent<Image>();
        line.GetComponent<Image>().color = Color.red;
        rt = line.GetComponent<RectTransform>();
        _player._onRelease += OnDestroy;
        line.transform.parent.position = startPos;
        //Vector3 pPos = _player.Cam.transform.parent.position;
        //pPos.z += 1;
        //_initialPosition = pPos;

    }


    
   


    public Vector3 pos;

    RectTransform rt;

    Vector3 _startPos;
    Vector3 currPos;

    public Vector3 StartPos => _startPos;
    public Vector3 CurrPos => currPos;

    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;
    public void Update()
    {
        if (GameManager.Instance.TouchType == GameManager.ETouchType.MOUSE)
        {
            currPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            mouse_pos = Input.mousePosition;
        }
        else if (GameManager.Instance.TouchType == GameManager.ETouchType.TOUCH)
        {
            currPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);
            mouse_pos = Input.GetTouch(0).position;
        }
        float x = (_startPos.x + currPos.x) / 2;
        float y = (_startPos.y + currPos.y) / 2;
        line.transform.position = new Vector3(x, y, 0);
        float dx = Mathf.Abs(currPos.x - _startPos.x);
        float dy = Mathf.Abs(currPos.y - _startPos.y);
        rt.sizeDelta = new Vector2(Mathf.Max(dx, dy), 50);
        line.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, Vector3.Magnitude(rt.sizeDelta) / 250);

        mouse_pos.z = -10;
        object_pos = line.transform.transform.parent.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        line.transform.parent.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnDestroy(Vector3 dir)
    {
        _player._onRelease -= OnDestroy;
        GameObject.Destroy(line);
    }
}