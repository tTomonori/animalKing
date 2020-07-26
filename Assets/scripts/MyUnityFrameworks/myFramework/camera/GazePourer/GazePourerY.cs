using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazePourerY : GazePourer {

	void Update () {
        if (mTarget == null) return;
        Vector3 tPosition = mTarget.transform.position;
        tPosition.y = positionY;
        transform.LookAt(tPosition);
        if (mReverse) rotateY += 180;
	}
}
