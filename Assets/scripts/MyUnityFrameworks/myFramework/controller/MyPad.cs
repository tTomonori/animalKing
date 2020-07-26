using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPad : MyBehaviour {
    public ControllerType mControllerType = ControllerType.shortTail;
    private Vector2 mDragOriginPoint;
    //1フレーム前のマウス座標
    private Vector2 mPreMousePoint;
    //ドラッグした方向
    private Vector2 _TailVec;
    //タップしたフラグ
    private bool _IsTapped = false;
    //Padへの触れ方(右クリックor左クリック)
    private DragType _DragType = DragType.none;
    //1フレーム内でのドラッグベクトル
    private Vector2 _Delta;
    //ドラッグ中か
    private bool _IsDragging = false;
    //<summary>テールの最大長(shortTailモードの時のみ有効)</summary>
    public float mMaxTailLength = 100;
    //<summary>ドラッグした判定とする最低距離</summary>
    public float mThresholdDrag = 10;
    //<summary>ドラッグした方向</summary>
    public Vector2 mTailVec{
        get { return _TailVec; }
    }
    //<summary>ドラッグした方向の単位ベクトル</summary>
    public Vector2 mNormalized{
        get { return _TailVec.normalized; }
    }
    //<summary>(ドラッグせずに)タップした</summary>
    public bool mIsTapped{
        get { return _IsTapped; }
    }
    //<summary>VirtualPadへの触れ方(右クリックor左クリック)</summary>
    public DragType mDragType{
        get { return _DragType; }
    }
    //<summary>1フレーム内でのドラッグベクトル</summary>
    public Vector2 mDelta{
        get { return _Delta; }
    }
    //<summary>ドラッグ中か</summary>
    public bool mIsDragging{
        get { return _IsDragging; }
    }
    protected void mouseDrag(){
        Vector2 tMousePosition = Input.mousePosition;
        Vector2 tTailVec = tMousePosition - mDragOriginPoint;
        //前のフレームからのドラッグ距離
        _Delta = tMousePosition - mPreMousePoint;
        mPreMousePoint = tMousePosition;
        //ドラッグ距離が短いならドラッグしていない扱い
        if (tTailVec.magnitude < mThresholdDrag) return;
        //ドラッグした
        _IsDragging = true;
        _TailVec = tTailVec;
        //テールの長さ調整
        if(mControllerType==ControllerType.shortTail){
            if(mMaxTailLength<_TailVec.magnitude){
                //ドラッグの基点修正
                mDragOriginPoint = tMousePosition + (mDragOriginPoint - tMousePosition).normalized * mMaxTailLength;
                _TailVec = tMousePosition - mDragOriginPoint;
            }
        }
    }
    protected void mouseDown(){
        mDragOriginPoint = Input.mousePosition;
        mPreMousePoint = mDragOriginPoint;
        //右クリックか左クリックか
        if (Input.GetMouseButtonDown(1)) _DragType = DragType.typeTwo;
        else _DragType = DragType.typeOne;
    }
    protected void mouseUp(){
        _TailVec = Vector2.zero;
        if (!_IsDragging)
            _IsTapped = true;
        _DragType = DragType.none;
        _Delta = Vector2.zero;
        _IsDragging = false;
    }
    private void FixedUpdate(){
        _IsTapped = false;
    }
    public enum ControllerType{
        shortTail,longTail
    }
    public enum DragType{
        none,
        typeOne,//左クリック
        typeTwo//右クリック
    }
}
