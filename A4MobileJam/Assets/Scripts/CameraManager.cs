using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] float _onTargetHeight;
    [SerializeField] AnimationCurve _onTargetPosCurve;
    [SerializeField] AnimationCurve _onTargetRotCurve;
    [SerializeField] float _onTargetCurveSpeed;

    bool _isMoving = false;

    void Start()
    {

    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(1)) SetToTarget();   
    }

    public void SetToTarget()
    {
        Vector3 sPos = transform.position;
        Quaternion sRot = transform.rotation;

        Vector3 fPos = GameManager.Instance.CurrentLevel.TargetPosition;
        fPos.y += _onTargetHeight;
        Quaternion fRot = Quaternion.Euler(new Vector3(90, 0, 0));

        StartCoroutine(MoveTo(sPos, fPos, sRot, fRot, _onTargetCurveSpeed, _onTargetPosCurve, _onTargetRotCurve, null));
    }

    public void MoveToSet(Vector3 sP, Vector3 fP, Quaternion sR, Quaternion fR, float speed, AnimationCurve posCurve, AnimationCurve rotCurve, Func<bool> onFinish, float waitStart = 0.0f, float waitEnd = 0.0f)
    {
        StartCoroutine(MoveTo(sP, fP, sR, fR, speed, posCurve, rotCurve, onFinish, waitStart, waitEnd));
    }

    IEnumerator MoveTo(Vector3 sP, Vector3 fP, Quaternion sR, Quaternion fR, float speed, AnimationCurve posCurve, AnimationCurve rotCurve, Func<bool> onFinish, float waitStart = 0.0f, float waitEnd = 0.0f)
    {
        yield return new WaitForSeconds(waitStart);
        _isMoving = true;
        float t = 0;

        while (t < 1.0f)
        {
            transform.position = Vector3.Lerp(sP, fP, (posCurve != null) ? posCurve.Evaluate(t) : t);
            transform.rotation = Quaternion.Lerp(sR, fR, (rotCurve != null) ? rotCurve.Evaluate(t) : t);
            yield return null;
            t += Time.deltaTime * speed;
        }
        yield return null;
        transform.position = fP;
        transform.rotation = fR;
        _isMoving = false;
        yield return new WaitForSeconds(waitEnd);

        if (onFinish != null) onFinish();
    }
}
