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

    public void Populate(List<Sprite> balls, List<int> ballsPossessed)
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

            bool possessed = false;
            for (int j = 0; j < ballsPossessed.Count; j++)
            {
                if (ballsPossessed[j] == i) possessed = true;
            }

            Button btn = go.transform.AddComponent<Button>();
            if (!possessed) 
            {
                img.color = new Color(1f, 1f, 1f, 0.25f);
                btn.onClick.AddListener(ind.TryBuyBall);
            }
            else
                btn.onClick.AddListener(ind.CloseSelectBall);


        }
    }
}
