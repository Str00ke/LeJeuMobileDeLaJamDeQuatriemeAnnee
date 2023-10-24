using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectBall : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Populate(List<Sprite> balls)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Sprite sprite = balls[i];
            GameObject go = new GameObject(sprite.name, typeof(RectTransform));
            go.transform.parent = transform.GetChild(0);
            go.GetComponent<RectTransform>().localScale = Vector3.one;
            PlayerSetIndex ind = go.transform.AddComponent<PlayerSetIndex>();
            ind.SetIndex(i);
            Image img = go.transform.AddComponent<Image>();
            img.sprite = sprite;
            Button btn = go.transform.AddComponent<Button>();
            btn.onClick.AddListener(ind.CloseSelectBall);
        }
    }
}
