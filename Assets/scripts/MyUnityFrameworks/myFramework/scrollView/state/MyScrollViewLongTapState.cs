using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private class MyScrollViewLongTapState : MyScrollViewOperateState {
        public MyScrollViewLongTapState(MyScrollView aParent) : base(aParent){}
        //タップし続けた時間
        protected float mTappingTime = 0;
        //タップした要素
        protected ElementTuple mTuple;
        public override void enter(){
            mTuple = parent.getElement(MyTouchInput.getTouch(parent.mCamera)[0].position);
            if(mTuple==null){//要素がない部分をタップした
                parent.changeState(new MyScrollViewDragScrollState(parent));
                return;
            }
            mTuple.element.push();
        }
        public override void exit(){
            if (mTuple != null)
                mTuple.element.pull();
        }
        public override void update(){
            MyTouchInfo tTouch = MyTouchInput.getTouch()[0];
            if(tTouch.state== MyTouchState.moved){//ドラッグ開始
                parent.changeState(new MyScrollViewDragScrollState(parent));
                return;
            }
            if(tTouch.state== MyTouchState.stationary){//長押し判定
                if (!parent.mOption.sortable) return;//ソート機能offの場合はreturn
                mTappingTime += Time.deltaTime;
                if(parent.mOption.longTapTime<mTappingTime){
                    parent.changeState(new MyScrollViewDragSortState(parent));
                }
                return;
            }
            //タップ終了
            endTap();
        }
        protected virtual void endTap(){
            parent.changeState(new MyScrollViewImmediatelyAfterClickingState(parent));
        }
    }
}