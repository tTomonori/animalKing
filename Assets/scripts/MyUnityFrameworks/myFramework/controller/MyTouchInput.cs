using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyTouchInput {
    static private TouchGetter mGetter;
    static private float mLastUpdateTime;
    static MyTouchInput(){
        switch(Application.platform){
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                mGetter = new MobileTouchGetter();
                break;
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                mGetter = new PCTouchGetter();
                break;
        }
    }
    static public MyTouchInfo[] getTouch(Camera aCamera = null){
        //タッチ情報更新
        if(mLastUpdateTime!=Time.time){
            mGetter.updateTouch();
            mLastUpdateTime = Time.time;
        }
        //タッチ情報渡し
        if (aCamera == null) return mGetter.getTouches();
        else return mGetter.getTouches(aCamera);
    }

    private abstract class TouchGetter{
        protected MyTouchInfo[] mInfos = new MyTouchInfo[0];
        public abstract void updateTouch();//タッチ状態更新
        public abstract MyTouchInfo[] getTouches();//タッチ状態取得
        public abstract MyTouchInfo[] getTouches(Camera aCamera);//タッチ状態をcamera座標に変換して取得
    }
    //スマホの時
    private class MobileTouchGetter:TouchGetter{
        public override void updateTouch(){
            MyTouchInfo[] tInfos = new MyTouchInfo[Input.touchCount];
            for (int i = 0; i < Input.touchCount; i++){
                Touch tTouch = Input.GetTouch(i);
                MyTouchInfo tMyInfo = new MyTouchInfo();
                tMyInfo.state = (MyTouchState)((int)tTouch.phase);
                tMyInfo.position = tTouch.position;
                tMyInfo.deltaPosition = tTouch.deltaPosition;
                tInfos[i] = tMyInfo;
            }
            mInfos = tInfos;
        }
        public override MyTouchInfo[] getTouches(){
            return mInfos;
        }
        public override MyTouchInfo[] getTouches(Camera aCamera){
            return mInfos;
        }
    }
    //PCの時
    private class PCTouchGetter:TouchGetter{
        static private Vector3 mLastMousePosition;
        static private Vector3 mCurrentMousePosition;
        public override void updateTouch(){
            //最後のカーソル座標更新
            mLastMousePosition = mCurrentMousePosition;
            mCurrentMousePosition = Input.mousePosition;

            MyTouchInfo tInfo = new MyTouchInfo();
            //タッチ状態
            if (Input.GetMouseButtonDown(0)){
                tInfo.state = MyTouchState.begin;
            }
            else if (Input.GetMouseButton(0)){
                tInfo.state = (mLastMousePosition == Input.mousePosition) ? MyTouchState.stationary : MyTouchState.moved;
            }
            else if (Input.GetMouseButtonUp(0)){
                tInfo.state = MyTouchState.ended;
            }
            else{
                mLastMousePosition = Input.mousePosition;
                mInfos = new MyTouchInfo[0];
                return;
            }
            //タッチ座標
            tInfo.position = mCurrentMousePosition;
            tInfo.deltaPosition = mCurrentMousePosition - mLastMousePosition;
            mInfos = new MyTouchInfo[1] { tInfo };
        }
        public override MyTouchInfo[] getTouches(){
            return mInfos;
        }
        public override MyTouchInfo[] getTouches(Camera aCamera){
            if (mInfos.Length == 0) return new MyTouchInfo[0];
            MyTouchInfo tInfo = mInfos[0].clone();
            tInfo.position = aCamera.ScreenToWorldPoint(tInfo.position);
            Vector3 tLastCameraPosition = aCamera.ScreenToWorldPoint(mLastMousePosition);
            tInfo.deltaPosition = new Vector2(tInfo.position.x - tLastCameraPosition.x, tInfo.position.y - tLastCameraPosition.y);
            return new MyTouchInfo[1] { tInfo };
        }
    }
}
public class MyTouchInfo{
    public MyTouchState state;
    public Vector2 position;
    public Vector2 deltaPosition;
    public MyTouchInfo clone(){
        MyTouchInfo tInfo = new MyTouchInfo();
        tInfo.state = this.state;
        tInfo.position = new Vector2(this.position.x, this.position.y);
        tInfo.deltaPosition = new Vector2(this.deltaPosition.x, this.deltaPosition.y);
        return tInfo;
    }
}
public enum MyTouchState{
    begin,moved,stationary,ended,canceld,none
}