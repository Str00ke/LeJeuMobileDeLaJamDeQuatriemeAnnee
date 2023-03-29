using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector3 pos;

    public GameObject obj;

    RectTransform rt;

    Vector3 startPos;
    Vector3 currPos;

    void Start()
    {
        rt = obj.GetComponent<RectTransform>();
    }

    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            obj.transform.parent.position = startPos;
        }
        currPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        float x = (startPos.x + currPos.x) / 2;
        float y = (startPos.y + currPos.y) / 2;
        obj.transform.position = new Vector3(x, y, 0);
        float dx = Mathf.Abs(currPos.x - startPos.x);
        float dy = Mathf.Abs(currPos.y - startPos.y);
        rt.sizeDelta = new Vector2(Mathf.Max(dx, dy), 100);
        //Debug.Log(Vector2.ClampMagnitude(new Vector2(dx, dy), 100));
        

        mouse_pos = Input.mousePosition;
        mouse_pos.z = -10;
        object_pos = obj.transform.transform.parent.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        obj.transform.parent.rotation = Quaternion.Euler(0, 0, angle);
    }

}
