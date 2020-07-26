using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private class MyScrollViewDragSortState : MyScrollViewOperateState {
        public MyScrollViewDragSortState(MyScrollView aParent) : base(aParent){}
        //並びかえ対象の要素
        private ElementTuple mSortingElement;
        public override void enter(){
            mSortingElement = parent.getElement(MyTouchInput.getTouch(parent.mCamera)[0].position);
            if (mSortingElement==null){//並び替える要素がない
                parent.changeState(new MyScrollViewWaitState(parent));
                return;
            }
            if(!parent.mDataList.isCanSort(mSortingElement.elementNumber)){//並び替え不可の要素
                mSortingElement = null;
                parent.changeState(new MyScrollViewWaitState(parent));
                return;
            }
            mSortingElement.element.grab();
        }
        public override void exit(){
            if (mSortingElement != null){
                mSortingElement.element.release();
                parent.moveToCorrectPosition(mSortingElement);
            }
        }
        public override void update(){
            MyTouchInfo[] tTouches = MyTouchInput.getTouch(parent.mCamera);
            if(tTouches.Length==0){//タッチ操作なし
                parent.changeState(new MyScrollViewWaitState(parent));
                return;
            }
            mSortingElement.element.position = parent.worldPositionToContentPosition(tTouches[0].position);
            if (tTouches[0].state == MyTouchState.stationary){//タッチ状態変化なし
                return;
            }
            if(tTouches[0].state == MyTouchState.moved){//ドラッグ移動
                ElementTuple tTuple = parent.getElement(tTouches[0].position);
                if (tTuple == null) return;
                if (mSortingElement.element == tTuple.element) return;//並び替え相手として自分を選択
                if (!parent.mDataList.isCanSort(tTuple.elementNumber)) return;//並び替え不可の要素
                //並び替える
                parent.sortElement(mSortingElement.index, tTuple.index, out mSortingElement, out tTuple);
                parent.moveToCorrectPosition(tTuple);
                return;
            }
            parent.changeState(new MyScrollViewWaitState(parent));
        }
    }
}