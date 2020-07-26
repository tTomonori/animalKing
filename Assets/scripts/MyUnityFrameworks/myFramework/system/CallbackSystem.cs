using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CallbackSystem {
    //全callbackの数
    private int mCounterNum = 0;
    //callbackを受け取った数
    private int mCount = 0;
    //全callbackを受け取った時に実行するcallback
    private Action mCallback;
    public Action getCounter(){
        mCounterNum++;
        return () => { 
            //callbackカウント
            mCount++;
            //全calbackを受け取った
            if (mCounterNum == mCount){
                if (mCallback != null) mCallback();
                return;
            }
        };
    }
    public void then(Action aCallback){
        mCallback = aCallback;
        if(mCounterNum==mCount){
            mCallback();
            return;
        }
    }
}
