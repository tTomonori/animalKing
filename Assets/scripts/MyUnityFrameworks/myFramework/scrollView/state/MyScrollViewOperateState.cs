using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyScrollView{
    private abstract class MyScrollViewOperateState{
        protected MyScrollView parent;
        public MyScrollViewOperateState(MyScrollView aParent){
            parent = aParent;
        }
        public virtual void update(){}
        public virtual void enter(){}
        public virtual void exit(){}
    }
}