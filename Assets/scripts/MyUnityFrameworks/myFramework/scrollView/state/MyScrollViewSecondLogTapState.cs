using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private class MyScrollViewSecondLongTapState : MyScrollViewLongTapState {
        public MyScrollViewSecondLongTapState(MyScrollView aParent) : base(aParent){}
        protected override void endTap(){
            doubleTappedElement();
        }
        //要素をダブルタップした
        private void doubleTappedElement(){
            if(parent.mOption.doubleTapSort){
                parent.changeState(new MyScrollViewTapSortState(parent));
            }
            else{
                parent.mDataList.doubleTapped(mTuple.elementNumber);
                parent.changeState(new MyScrollViewWaitState(parent));
            }
        }
    }
}