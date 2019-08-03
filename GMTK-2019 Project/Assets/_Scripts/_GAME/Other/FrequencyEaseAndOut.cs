using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FrequencyEaseAndOut
{
    [SerializeField, Tooltip("curve, default: time: 0 to X; value: 0 to 1")]
    private AnimationCurve _animationCurveIn;
    [SerializeField, Tooltip("curve, default: time: 0 to X; value: 0 to 1")]
    private AnimationCurve _animationCurveOut;

    private float _previousTimeFrame;
    [SerializeField]
    private float _currentTime;
    [SerializeField]
    private bool _startIn = false;
    [SerializeField]
    private bool _startOut = false;
    [SerializeField]
    private float percentIn;
    [SerializeField]
    private float percentOut;
    [SerializeField]
    private float _currentValue;

    public float EvaluateEveryFrameIn()
    {
        //if this is the first time we ease
        if (!_startIn)
        {
            //if we were previously in out
            if (_startOut)
            {
                percentIn = ExtMathf.MirrorFromInterval(percentOut, 0, 1);
                _currentTime = _animationCurveIn.Evaluate(percentIn);
            }
            else
            {
                _currentTime = 0;
            }

            _startIn = true;
            _startOut = false;
            _previousTimeFrame = Time.fixedTime;
        }
        else
        {
            float amountToAdd = Time.fixedTime - _previousTimeFrame;
            _currentTime += amountToAdd;
        }

        _currentTime = Mathf.Clamp(_currentTime, _animationCurveIn.keys[0].time, _animationCurveIn.keys[_animationCurveIn.keys.Length - 1].time);

        float timeMax = _animationCurveIn.keys[_animationCurveIn.keys.Length - 1].time;
        if (timeMax == 0)
        {
            Debug.LogError("max limit");
            return (0);
        }
        percentIn = _currentTime * 1f / timeMax;

        _currentValue = _animationCurveIn.Evaluate(_currentTime);
        return (_currentValue);
    }

    public void Stop()
    {
        _startIn = false;
        _startOut = false;
    }

    public float EvaluateEveryFrameOut()
    {
        if (!_startOut)
        {
            //if we were previously in out
            if (_startIn)
            {
                percentOut = ExtMathf.MirrorFromInterval(percentIn, 0, 1);
                _currentTime = _animationCurveIn.Evaluate(percentOut);
            }
            else
            {
                _currentTime = 0;
            }

            _startOut = true;
            _startIn = false;
            _previousTimeFrame = Time.fixedTime;
        }
        else
        {
            float amountToAdd = Time.fixedTime - _previousTimeFrame;
            _currentTime += amountToAdd;
        }

        _currentTime = Mathf.Clamp(_currentTime, _animationCurveOut.keys[0].time, _animationCurveOut.keys[_animationCurveOut.keys.Length - 1].time);
        float timeMax = _animationCurveOut.keys[_animationCurveOut.keys.Length - 1].time;
        if (timeMax == 0)
        {
            Debug.LogError("max limit");
            return (0);
        }
        percentOut = _currentTime * 1f / timeMax;

        _currentValue = _animationCurveOut.Evaluate(_currentTime);
        return (_currentValue);
    }
}
