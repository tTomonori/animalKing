using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath {
    public static float vecToRad(Vector2 aVec){
        float tRad = Mathf.Acos(aVec.x / Mathf.Sqrt(aVec.x * aVec.x + aVec.y * aVec.y));
        return tRad;
    }
    public static float vecToDeg(Vector2 aVec){
        float tRad = vecToRad(aVec);
        float tDeg = (tRad / Mathf.PI) * 180;
        if (aVec.y < 0) tDeg = 360 - tDeg;
        return tDeg;
    }
}