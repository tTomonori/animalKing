using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private class MyScrollViewTapSortState : MyScrollViewOperateState {
        public MyScrollViewTapSortState(MyScrollView aParent) : base(aParent){}
        //並びかえ対象の要素
        private ElementTuple mSortingElement;
        public override void enter(){
            mSortingElement = parent.getElement(MyTouchInput.getTouch(parent.mCamera)[0].position);
            if (mSortingElement == null){//並び替える要素がない
                parent.changeState(new MyScrollViewWaitState(parent));
                return;
            }
            if (!parent.mDataList.isCanSort(mSortingElement.elementNumber)){//並び替え不可の要素
                mSortingElement = null;
                parent.changeState(new MyScrollViewWaitState(parent));
                return;
            }
            mSortingElement.element.grab();
        }
        public override void exit(){
            if (mSortingElement != null)
                mSortingElement.element.release();
        }
        public override void update(){
            MyTouchInfo[] tTouches = MyTouchInput.getTouch(parent.mCamera);
            if (tTouches.Length == 0)//タッチ操作なし
                return;

            //並び替え相手選択
            if(tTouches[0].state == MyTouchState.ended){
                ElementTuple tTuple = parent.getElement(tTouches[0].position);
                if(tTuple==null || tTuple.element==mSortingElement.element){//選択した要素なし or 相手として自分を選択
                    endSort();
                    return;
                }
                if(!parent.mDataList.isCanSort(tTuple.elementNumber)){//並び替え不可の要素
                    endSort();
                    return;
                }
                //並び替え
                parent.sortElement(mSortingElement.index, tTuple.index, out mSortingElement, out tTuple);
                parent.moveToCorrectPosition(mSortingElement);
                parent.moveToCorrectPosition(tTuple);
                endSort();
            }
        }
        private void endSort(){
            parent.changeState(new MyScrollViewWaitState(parent));
        }
    }
}