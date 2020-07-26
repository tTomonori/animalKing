using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter {
    private float mCount = 0;
    private float _TimerTime;
    public float mTimerTime{
        get{ return _TimerTime; }
        set{
            if (value <= 0) return;
            _TimerTime = value;
        }
    }
    public TimeCounter(float aTimerTime){
        _TimerTime = aTimerTime;
        if (_TimerTime <= 0) _TimerTime = 1;
    }
    //<summary>deltaTime分カウント timerTime以上経過していた場合は経過回数を通知</summary>
    public int count(){
        mCount += Time.deltaTime;
        int tCount = (int)(mCount / _TimerTime);
        mCount = mCount % _TimerTime;
        return tCount;
    }
    //<summary>カウントをリセット</summary>
    public void reset(){
        mCount = 0;
    }
}
