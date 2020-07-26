using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Reflection;

public class MyVirtualPadTransparentPush : MyVirtualPad {
    [SerializeField] private List<Camera> mCameraList;
    [SerializeField] private List<CameraType> mCameraTypeList;
    public void addCamera(Camera aCamera,CameraType aType){
        mCameraList.Add(aCamera);
        mCameraTypeList.Add(aType);
    }
    public void removeCamera(Camera aCamera){
        for (int i = 0; i < mCameraList.Count; i++){
            if (mCameraList[i] != aCamera) continue;
            mCameraList.RemoveAt(i);
            mCameraTypeList.RemoveAt(i);
        }
    }
    public void resetCamera(){
        mCameraList.Clear();
        mCameraTypeList.Clear();
    }
    protected override void OnMouseUp(){
        mouseUp();
        if (!mIsTapped) return;

        //pushイベントを後ろへ透過
        for (int i = 0; i < mCameraList.Count;i++){
            if (mCameraTypeList[i] == CameraType.threeD){
                if (transmissionTo3d(mCameraList[i])) return;
            }else{
                if (transmissionTo2d(mCameraList[i])) return;
            }
        }
    }
    //3dのシーンのオブジェクトにpushイベントを転送(転送できたらtrue)
    private bool transmissionTo3d(Camera aCamera){
        Ray tRay = aCamera.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入
        RaycastHit[] tHits3d = Physics.RaycastAll(tRay);//hitした3dオブジェクト
        if (tHits3d.Length == 0) return false;
        transmission(tHits3d[0].collider.gameObject);
        return true;
    }
    //2dのシーンのオブジェクトにpushイベントを転送(転送できたらtrue)
    private bool transmissionTo2d(Camera aCamera){
        MyTouchInfo[] tInfo = MyTouchInput.getTouch(aCamera);
        if (tInfo.Length == 0) return false;
        Collider2D[] tColliders = Physics2D.OverlapPointAll(tInfo[0].position);
        if (tColliders.Length == 0) return false;
        bool tFound = false;
        foreach(Collider2D tCollider in tColliders){
            if(tFound){
                transmission(tCollider.gameObject);
                return true;
            }
            if (tCollider.gameObject == this.gameObject)
                tFound = true;
        }
        return false;
    }
    //pushイベントを転送する
    private void transmission(GameObject aObject){
        foreach(MonoBehaviour tObject in aObject.GetComponents<MonoBehaviour>()){
            // メソッドの有無を確認する
            MethodInfo tInfo = tObject.GetType().GetMethod("OnMouseUpAsButton");
            if (tInfo == null) continue;
            object[] tParams = new object[0];
            tInfo.Invoke(tObject, tParams);
        }
    }
    public enum CameraType{
        twoD,threeD
    }
}