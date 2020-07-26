using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private class MyScrollViewWaitState : MyScrollViewOperateState {
        public MyScrollViewWaitState(MyScrollView aParent) : base(aParent){}
        public override void update(){
            MyTouchInfo[] tTouches = MyTouchInput.getTouch(parent.mCamera);
            if (tTouches.Length == 0){//タッチ操作なし
                parent.scrollByWheel();
                parent.remediateScrollPosition();
                parent.updateElement();
                return;
            }
            //タッチ操作開始
            if (tTouches[0].state != MyTouchState.begin) return;
            if (!parent.targeting(tTouches[0].position)) return;
            parent.changeState(new MyScrollViewLongTapState(parent));
        }
    }
}