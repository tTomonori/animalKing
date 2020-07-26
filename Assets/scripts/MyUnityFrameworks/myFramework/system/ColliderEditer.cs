using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderEditer {
    // ** サイズ **
    /// <summary>最小外接矩形のサイズを返す</summary>
    public static Vector3 minimumCircumscribedCube(this BoxCollider aCollider) {
        return aCollider.size;
    }
    /// <summary>最小外接矩形のサイズを返す</summary>
    public static Vector3 minimumCircumscribedCube(this SphereCollider aCollider) {
        return new Vector3(aCollider.radius, aCollider.radius, aCollider.radius);
    }
    /// <summary>最小外接矩形のサイズを返す</summary>
    public static Vector3 minimumCircumscribedCube(this MeshCollider aCollider) {
        CubeEndPoint tEndPoint = aCollider.minimumCircumscribedCubeEndPoint();
        return new Vector3(tEndPoint.right - tEndPoint.left, tEndPoint.top - tEndPoint.bottom, tEndPoint.back - tEndPoint.front);
    }
    /// <summary>最小外接矩形のサイズを返す</summary>
    public static Vector3 minimumCircumscribedCube(this Collider aCollider) {
        switch (aCollider) {
            case BoxCollider box:
                return minimumCircumscribedCube(box);
            case SphereCollider sphere:
                return minimumCircumscribedCube(sphere);
            case MeshCollider mesh:
                return minimumCircumscribedCube(mesh);
        }
        throw new System.Exception("ColliderEditer : 最小外接矩形サイズ計算未定義「" + aCollider.GetType().ToString() + "」");
    }


    // ** 最小外接矩形サイズ ローカル **
    /// <summary>最小外接矩形の上下左右の座標(ローカル座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPoint(this BoxCollider aCollider) {
        CubeEndPoint tPoints = new CubeEndPoint();
        tPoints.top = aCollider.size.y / 2f + aCollider.center.y;
        tPoints.bottom = -aCollider.size.y / 2f + aCollider.center.y;
        tPoints.back = aCollider.size.z / 2f + aCollider.center.z;
        tPoints.front = -aCollider.size.z / 2f + aCollider.center.z;
        tPoints.left = -aCollider.size.x / 2f + aCollider.center.x;
        tPoints.right = aCollider.size.x / 2f + aCollider.center.x;
        return tPoints;
    }
    /// <summary>最小外接矩形の上下左右の座標(ローカル座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPoint(this SphereCollider aCollider) {
        CubeEndPoint tPoints = new CubeEndPoint();
        tPoints.top = aCollider.radius + aCollider.center.y;
        tPoints.bottom = -aCollider.radius + aCollider.center.y;
        tPoints.back = aCollider.radius + aCollider.center.z;
        tPoints.front = -aCollider.radius + aCollider.center.z;
        tPoints.left = -aCollider.radius + aCollider.center.x;
        tPoints.right = aCollider.radius + aCollider.center.x;
        return tPoints;
    }
    /// <summary>最小外接矩形の上下左右の座標(ローカル座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPoint(this MeshCollider aCollider) {
        CubeEndPoint tPoints = new CubeEndPoint();
        tPoints.top = aCollider.sharedMesh.vertices[0].y;
        tPoints.bottom = aCollider.sharedMesh.vertices[0].y;
        tPoints.back = aCollider.sharedMesh.vertices[0].z;
        tPoints.front = aCollider.sharedMesh.vertices[0].z;
        tPoints.left = aCollider.sharedMesh.vertices[0].x;
        tPoints.right = aCollider.sharedMesh.vertices[0].x;
        foreach (Vector3 tPoint in aCollider.sharedMesh.vertices) {
            //上下
            if (tPoints.top < tPoint.y) tPoints.top = tPoint.y;
            else if (tPoint.y < tPoints.bottom) tPoints.bottom = tPoint.y;
            //奥手前
            if (tPoints.back < tPoint.z) tPoints.back = tPoint.z;
            else if (tPoint.z < tPoints.front) tPoints.front = tPoint.z;
            //左右
            if (tPoints.right < tPoint.x) tPoints.right = tPoint.x;
            else if (tPoint.x < tPoints.left) tPoints.left = tPoint.x;
        }
        return tPoints;
    }
    /// <summary>最小外接矩形の上下左右の座標(ローカル座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPoint(this Collider aCollider) {
        switch (aCollider) {
            case BoxCollider box:
                return minimumCircumscribedCubeEndPoint(box);
            case SphereCollider sphere:
                return minimumCircumscribedCubeEndPoint(sphere);
            case MeshCollider mesh:
                return minimumCircumscribedCubeEndPoint(mesh);
        }
        throw new System.Exception("ColliderEditer : 最小外接矩形端座標計算未定義「" + aCollider.GetType().ToString() + "」");
    }

    // ** 最小外接矩形サイズ ワールド **
    /// <summary>最小外接矩形の上下左右の座標(ワールド座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPointWorld(this BoxCollider aCollider) {
        Vector3[] tPoints = new Vector3[8] {
            aCollider.transform.rotation*(aCollider.center+new Vector3(-aCollider.size.x/2f,-aCollider.size.y/2f,-aCollider.size.z/2f))+aCollider.transform.position,
            aCollider.transform.rotation*(aCollider.center+new Vector3(aCollider.size.x/2f,-aCollider.size.y/2f,-aCollider.size.z/2f))+aCollider.transform.position,
            aCollider.transform.rotation*(aCollider.center+new Vector3(-aCollider.size.x/2f,-aCollider.size.y/2f,aCollider.size.z/2f))+aCollider.transform.position,
            aCollider.transform.rotation*(aCollider.center+new Vector3(aCollider.size.x/2f,-aCollider.size.y/2f,aCollider.size.z/2f))+aCollider.transform.position,
            aCollider.transform.rotation*(aCollider.center+new Vector3(-aCollider.size.x/2f,aCollider.size.y/2f,-aCollider.size.z/2f))+aCollider.transform.position,
            aCollider.transform.rotation*(aCollider.center+new Vector3(aCollider.size.x/2f,aCollider.size.y/2f,-aCollider.size.z/2f))+aCollider.transform.position,
            aCollider.transform.rotation*(aCollider.center+new Vector3(-aCollider.size.x/2f,aCollider.size.y/2f,aCollider.size.z/2f))+aCollider.transform.position,
            aCollider.transform.rotation*(aCollider.center+new Vector3(aCollider.size.x/2f,aCollider.size.y/2f,aCollider.size.z/2f))+aCollider.transform.position
        };
        return extractCubeEndPoint(tPoints);
    }
    /// <summary>最小外接矩形の上下左右の座標(ワールド座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPointWorld(this SphereCollider aCollider) {
        CubeEndPoint tPoints = new CubeEndPoint();
        tPoints.top = aCollider.radius + aCollider.center.y + aCollider.transform.position.y;
        tPoints.bottom = -aCollider.radius + aCollider.center.y + aCollider.transform.position.y;
        tPoints.back = aCollider.radius + aCollider.center.z + aCollider.transform.position.z;
        tPoints.front = -aCollider.radius + aCollider.center.z + aCollider.transform.position.z;
        tPoints.left = -aCollider.radius + aCollider.center.x + aCollider.transform.position.x;
        tPoints.right = aCollider.radius + aCollider.center.x + aCollider.transform.position.x;
        return tPoints;
    }
    /// <summary>最小外接矩形の上下左右の座標(ワールド座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPointWorld(this MeshCollider aCollider) {
        Vector3[] tVertices = aCollider.sharedMesh.vertices;
        int tLength = tVertices.Length;
        Vector3[] tPoints = new Vector3[tLength];
        Quaternion tRotation = aCollider.transform.rotation;
        Vector3 tPosition = aCollider.transform.position;
        for (int i = 0; i < tLength; ++i) {
            tPoints[i] = tRotation * tVertices[i] + tPosition;
        }
        return extractCubeEndPoint(tPoints);
    }
    /// <summary>最小外接矩形の上下左右の座標(ワールド座標)を返す</summary>
    public static CubeEndPoint minimumCircumscribedCubeEndPointWorld(this Collider aCollider) {
        switch (aCollider) {
            case BoxCollider box:
                return minimumCircumscribedCubeEndPointWorld(box);
            case SphereCollider sphere:
                return minimumCircumscribedCubeEndPointWorld(sphere);
            case MeshCollider mesh:
                return minimumCircumscribedCubeEndPointWorld(mesh);
        }
        throw new System.Exception("ColliderEditer : 最小外接矩形端ワールド座標計算未定義「" + aCollider.GetType().ToString() + "」");
    }
    public static CubeEndPoint extractCubeEndPoint(Vector3[] aPoints) {
        CubeEndPoint tPoints = new CubeEndPoint();
        tPoints.top = aPoints[0].y;
        tPoints.bottom = aPoints[0].y;
        tPoints.back = aPoints[0].z;
        tPoints.front = aPoints[0].z;
        tPoints.left = aPoints[0].x;
        tPoints.right = aPoints[0].x;
        foreach (Vector3 tPoint in aPoints) {
            //上下
            if (tPoints.top < tPoint.y) tPoints.top = tPoint.y;
            else if (tPoint.y < tPoints.bottom) tPoints.bottom = tPoint.y;
            //奥手前
            if (tPoints.back < tPoint.z) tPoints.back = tPoint.z;
            else if (tPoint.z < tPoints.front) tPoints.front = tPoint.z;
            //左右
            if (tPoints.right < tPoint.x) tPoints.right = tPoint.x;
            else if (tPoint.x < tPoints.left) tPoints.left = tPoint.x;
        }
        return tPoints;
    }
    /// <summary>上下左右の四つの値</summary>
    public class CubeEndPoint {
        public float top;
        public float bottom;
        public float back;
        public float front;
        public float left;
        public float right;
        public CubeEndPoint copy() {
            CubeEndPoint tCopy = new CubeEndPoint();
            tCopy.top = top;
            tCopy.bottom = bottom;
            tCopy.back = back;
            tCopy.front = front;
            tCopy.left = left;
            tCopy.right = right;
            return tCopy;
        }
    }
    /// <summary>
    /// 二つのcolliderの最小外接矩形のXZ平面上の距離を返す(引数2から引数1へ向かうベクトル)(重なっている場合はゼロベクトル)
    /// </summary>
    /// <returns>最小外接矩形のXZ平面上の距離(引数2から引数1へ向かうベクトル)</returns>
    /// <param name="aCollider1">A collider1.</param>
    /// <param name="aCollider2">A collider2.</param>
    public static Vector2 planeDistance(Collider aCollider1, Collider aCollider2) {
        CubeEndPoint tEnd1 = aCollider1.minimumCircumscribedCubeEndPoint();
        CubeEndPoint tEnd2 = aCollider2.minimumCircumscribedCubeEndPoint();
        if (tEnd1.right <= tEnd2.left) {
            //1が左
            if (tEnd1.back <= tEnd2.front) {
                //1が下
                return new Vector2(tEnd1.right - tEnd2.left, tEnd1.back - tEnd2.front);
            } else if (tEnd2.back <= tEnd1.back) {
                //1が上
                return new Vector2(tEnd1.right - tEnd2.left, tEnd1.front - tEnd2.back);
            } else {
                //Y方向重なり
                return new Vector2(tEnd1.right - tEnd2.left, 0);
            }
        } else if (tEnd2.right <= tEnd1.left) {
            //1が右
            if (tEnd1.back <= tEnd2.front) {
                //1が下
                return new Vector2(tEnd1.left - tEnd2.right, tEnd1.back - tEnd2.front);
            } else if (tEnd2.back <= tEnd1.back) {
                //1が上
                return new Vector2(tEnd1.left - tEnd2.right, tEnd1.front - tEnd2.back);
            } else {
                //Y方向重なり
                return new Vector2(tEnd1.left - tEnd2.right, 0);
            }
        } else {
            //X方向重なり
            if (tEnd1.back <= tEnd2.front) {
                //1が下
                return new Vector2(0, tEnd1.back - tEnd2.front);
            } else if (tEnd2.back <= tEnd1.back) {
                //1が上
                return new Vector2(0, tEnd1.front - tEnd2.back);
            } else {
                //Y方向重なり
                return new Vector2(0, 0);
            }
        }
    }
    public static Vector3 getCenter(this Collider aCollider) {
        switch (aCollider) {
            case BoxCollider box:
                return box.center;
            case SphereCollider sphere:
                return sphere.center;
        }
        throw new System.Exception("ColliderEditer : 未定義の中心座標取得「" + aCollider.GetType() + "」");
    }
}