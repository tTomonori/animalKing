using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorCalculator {
    /// <summary>
    /// ベクトルを成分分解する(成分方向ベクトルに垂直な成分を返す)
    /// </summary>
    /// <returns>成分方向ベクトルに垂直な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector2 disassembleOrthogonal(this Vector2 aVector, Vector2 aComponent) {
        //(aComponent・Returns = 0) && (k*aComponent + Returns = aVector) ←方程式を解く
        float k = -(aComponent.x * aVector.x + aComponent.y * aVector.y) / (aComponent.x * aComponent.x + aComponent.y * aComponent.y);
        return k * aComponent + aVector;
    }
    /// <summary>
    /// ベクトルを成分分解する(成分方向ベクトルに平行な成分を返す)
    /// </summary>
    /// <returns>成分方向ベクトルに平行な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector2 disassembleParallel(this Vector2 aVector, Vector2 aComponent) {
        float k = -(aComponent.x * aVector.x + aComponent.y * aVector.y) / (aComponent.x * aComponent.x + aComponent.y * aComponent.y);
        return -k * aComponent;
    }
    /// <summary>
    /// ランダムな方向の単位ベクトルを返す
    /// </summary>
    static public Vector2 randomVector() {
        Vector2 vector = new Vector2(0, 1);
        return Quaternion.Euler(0, 0, Random.Range(0, 359)) * vector;
    }


    /// <summary>
    /// 直行判定
    /// </summary>
    /// <returns>垂直ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isOrthogonal(this Vector2 v1, Vector2 v2) {
        return Equals(Vector2.Dot(v1, v2), 0.0f);
    }
    /// <summary>
    /// 直行判定(誤差を考慮する)
    /// </summary>
    /// <returns>垂直ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isOrthogonalConsiderdError(this Vector2 v1, Vector2 v2) {
        float a = Vector2.Dot(v1, v2);
        return (-0.01f < a) && (a < 0.01f);
    }
    /// <summary>
    /// 平行判定
    /// </summary>
    /// <returns>平行ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isParallel(this Vector2 v1, Vector2 v2) {
        return Equals(v1.x * v2.y - v1.y * v2.x, 0.0f);
    }
    /// <summary>
    /// 平行判定(誤差を考慮する)
    /// </summary>
    /// <returns>平行ならtrue</returns>
    /// <param name="v1">vector 1</param>
    /// <param name="v2">vector 2</param>
    public static bool isParallelConsiderdError(this Vector2 v1, Vector2 v2) {
        float a = v1.x * v2.y - v1.y * v2.x;
        return (-0.01f < a) && (a < 0.01f);
    }
    /// <summary>
    /// 2つのベクトルのなす角(0 ~ 180)
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns>2つのベクトルのなす角(単位は度)</returns>
    public static float cornerAbs(this Vector2 v1, Vector2 v2) {
        float tRad = Mathf.Atan2(v2.x * v1.y - v1.x * v2.y, v1.x * v2.x + v1.y * v2.y);
        tRad = Mathf.Abs(tRad);
        return 180f * tRad / Mathf.PI;
    }
    /// <summary>
    /// 2つのベクトルのなす角(-180 ~ 180)(V1からV2への角度,反時計周りが正)
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns>2つのベクトルのなす角(単位は度)</returns>
    public static float corner(this Vector2 v1, Vector2 v2) {
        float tRad = Mathf.Atan2(v1.x * v2.y - v2.x * v1.y, v2.x * v1.x + v2.y * v1.y);
        return 180f * tRad / Mathf.PI;
    }
    /// <summary>
    /// 2つのベクトルのなす角(-PI ~ PI)(V1からV2への角度,反時計周りが正)
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns>2つのベクトルのなす角(単位はrad)</returns>
    public static float cornerRad(this Vector2 v1, Vector2 v2) {
        return Mathf.Atan2(v1.x * v2.y - v2.x * v1.y, v2.x * v1.x + v2.y * v1.y);
    }
    /// <summary>
    /// 引数の長さのベクトルを返す
    /// </summary>
    /// <returns>引数長の同じ向きのベクトル</returns>
    /// <param name="aVector">向き</param>
    /// <param name="aLength">長さ</param>
    public static Vector2 matchLength(this Vector2 aVector, float aLength) {
        return aVector * (aLength / aVector.magnitude);
    }
    /// <summary>Vector2に変換する</summary>
    public static Vector2 toVector2(this Vector3 aVector) {
        return new Vector2(aVector.x, aVector.y);
    }





    /// <summary>
    /// ベクトルを成分分解する(成分方向ベクトルに垂直な成分を返す)
    /// </summary>
    /// <returns>成分方向ベクトルに垂直な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector3 disassembleOrthogonal(this Vector3 aVector, Vector3 aComponent) {
        //(aComponent・Returns = 0) && (k*aComponent + Returns = aVector) ←方程式を解く
        float k = -(aComponent.x * aVector.x + aComponent.y * aVector.y + aComponent.z * aVector.z) / (aComponent.x * aComponent.x + aComponent.y * aComponent.y + aComponent.z * aComponent.z);
        return k * aComponent + aVector;
    }
    /// <summary>
    /// ベクトルを成分分解する(成分方向ベクトルに平行な成分を返す)
    /// </summary>
    /// <returns>成分方向ベクトルに平行な成分</returns>
    /// <param name="aVector">分解するベクトル</param>
    /// <param name="aComponent">成分方向ベクトル</param>
    static public Vector3 disassembleParallel(this Vector3 aVector, Vector3 aComponent) {
        float k = -(aComponent.x * aVector.x + aComponent.y * aVector.y + aComponent.z * aVector.z) / (aComponent.x * aComponent.x + aComponent.y * aComponent.y + aComponent.z * aComponent.z);
        return -k * aComponent;
    }
    /// <summary>
    /// 引数の長さのベクトルを返す
    /// </summary>
    /// <returns>引数長の同じ向きのベクトル</returns>
    /// <param name="aVector">向き</param>
    /// <param name="aLength">長さ</param>
    public static Vector3 matchLength(this Vector3 aVector, float aLength) {
        return aVector * (aLength / aVector.magnitude);
    }
}
