using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    [SerializeField] List<GameObject> categories = new();
    [SerializeField] AnimationCurve _moveCatCurve;
    [SerializeField] float _moveCatCurveSpeed;
    [SerializeField] float _oldTravelYValue = 150f;

    bool _isMoving = false;
    int _currCategoryIndex = 0;

    void Start()
    {
        categories[0].SetActive(true);
        categories[0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        for (int i = 1; i < categories.Count; i++) 
        {
            categories[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            categories[i].SetActive(false);
        }
    }

    void Update()
    {
        
    }

    public void ShowCategory(int index)
    {
        if (_isMoving || index == _currCategoryIndex) return;
        categories[index].SetActive(true);

        Vector2 oldS = categories[_currCategoryIndex].GetComponent<RectTransform>().anchoredPosition;
        Vector2 oldE = new Vector2(oldS.x, oldS.y + _oldTravelYValue);
        Vector2 newS = new Vector2(oldS.x, oldS.y - _oldTravelYValue);
        Vector2 newE = categories[_currCategoryIndex].GetComponent<RectTransform>().anchoredPosition;
        StartCoroutine(MoveCategory(categories[_currCategoryIndex], categories[index], oldS, oldE, newS, newE, index));
    }

    IEnumerator MoveCategory(GameObject old, GameObject _new, Vector2 oldS, Vector2 oldE, Vector2 newS, Vector2 newE, int newIndex)
    {
        _isMoving = true;
        float t = 0;

        while (t < 1.0f)
        {
            old.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(oldS, oldE, (_moveCatCurve != null) ? _moveCatCurve.Evaluate(t) : t);
            _new.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(newS, newE, (_moveCatCurve != null) ? _moveCatCurve.Evaluate(t) : t);
            yield return null;
            t += Time.deltaTime * _moveCatCurveSpeed;
        }
        yield return null;
        old.GetComponent<RectTransform>().anchoredPosition = oldE;
        _new.GetComponent<RectTransform>().anchoredPosition = newE;
        _isMoving = false;
        categories[_currCategoryIndex].SetActive(false);
        _currCategoryIndex = newIndex;
    }
}
