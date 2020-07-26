using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private class MyScrollViewDragScrollState : MyScrollViewOperateState {
        public MyScrollViewDragScrollState(MyScrollView aParent) : base(aParent){}
        public override void enter(){
            parent.scrollByDrag();
            parent.remediateScrollPosition();
            parent.updateElement();
        }
        public override void update(){
            if(MyTouchInput.getTouch()[0].state== MyTouchState.ended){//終了判定
                parent.changeState(new MyScrollViewWaitState(parent));
                return;
            }
            //ドラッグスクロール
            parent.scrollByDrag();
            parent.remediateScrollPosition();
            parent.updateElement();
        }
    }
}