using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOutFloatSystem {
    private float mValue;
    private float mParSecond;
    private float mRemaining;
    public bool isEmpty{
        get { return mRemaining <= 0; }
    }
    public TakeOutFloatSystem(float aValue,float aParSecond){
        mValue = aValue;
        mParSecond = aParSecond;
        mRemaining = mValue;
    }
    public float takeOut(){
        float tOut = mParSecond * Time.deltaTime;
        if(mRemaining<=tOut){
            tOut = mRemaining;
            mRemaining = 0;
            return tOut;
        }
        mRemaining -= tOut;
        return tOut;
    }
}
