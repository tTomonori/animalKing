using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private class MyScrollViewImmediatelyAfterClickingState : MyScrollViewWaitState {
        public MyScrollViewImmediatelyAfterClickingState(MyScrollView aParent) : base(aParent){}
        //最後にタップを終えてからの時間
        private float mAloneTime = 0;
        //タップした要素
        private ElementTuple mTuple;
        public override void enter(){
            mTuple = parent.getElement(MyTouchInput.getTouch(parent.mCamera)[0].position);
            if(mTuple==null){//要素がない部分をタップした
                parent.changeState(new MyScrollViewWaitState(parent));
                return;
            }
            if(!parent.mOption.doubleTap){//ダブルタップ機能offならすぐに状態遷移
                tappedElement();//要素をタップした
                return;
            }
        }
        public override void update(){
            MyTouchInfo[] tTouches = MyTouchInput.getTouch(parent.mCamera);
            if (tTouches.Length == 0){//タッチ操作なし
                mAloneTime += Time.deltaTime;
                if(parent.mOption.doubleTapTime<mAloneTime){//ダブルタップ判定終了
                    tappedElement();//要素をタップした
                    return;
                }
                base.update();
                return;
            }
            //ダブルタップ判定
            if (tTouches[0].state != MyTouchState.begin) return;
            if (!parent.targeting(tTouches[0].position)) return;
            ElementTuple tTuple = parent.getElement(tTouches[0].position);
            if(mTuple.index==tTuple.index){
                //１回目と２回目で同じ要素をタップ
                parent.changeState(new MyScrollViewSecondLongTapState(parent));
                return;
            }else{
                //１回目と２回目で違う要素をタップ
                parent.changeState(new MyScrollViewImmediatelyAfterClickingState(parent));
                return;
            }
        }
        //要素をタップした
        private void tappedElement(){
            parent.mDataList.tapped(mTuple.elementNumber);
            parent.changeState(new MyScrollViewWaitState(parent));
        }
    }
}