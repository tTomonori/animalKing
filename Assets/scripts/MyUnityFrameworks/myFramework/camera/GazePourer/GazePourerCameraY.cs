using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazePourerCameraY : GazePourer {
	
	void Update () {
        if (mTarget == null) return;
        rotateY = mTarget.transform.eulerAngles.y;
        if (!mReverse) rotateY += 180;
	}
}
