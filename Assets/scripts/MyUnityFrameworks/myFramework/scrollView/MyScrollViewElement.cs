using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScrollViewElement : MyBehaviour {
    //クリックを開始した
    public virtual void push(){
        scaleBy(new Vector3(-0.2f, -0.2f, 0), 0.1f);
    }
    //クリックを終了した
    public virtual void pull(){
        scaleBy(new Vector3(0.2f, 0.2f, 0), 0.1f);
    }
    //ソートを始めた
    public virtual void grab(){
        rotateBy(30,0.1f);
    }
    //ソートを終えた
    public virtual void release(){
        rotateBy(-30, 0.1f);

    }
}
