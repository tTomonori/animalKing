using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazePourerCamera : GazePourer {
    
	void Update () {
        if (mTarget == null) return;
        transform.rotation = mTarget.transform.rotation;
        if (!mReverse) rotateY += 180;
	}
}
