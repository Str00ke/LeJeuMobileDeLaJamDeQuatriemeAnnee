using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProtoMiniMap : MonoBehaviour
{
    RawImage _rw;
    [SerializeField] Camera _cam;

    private void Awake()
    {
        _rw = GetComponent<RawImage>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _rw.texture = _cam.targetTexture;
    }
}
