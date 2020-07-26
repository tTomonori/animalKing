using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPillarMeshCollider : MyMeshCollider {
    /// <summary>底面の頂点座標</summary>
    [SerializeField] public Vector3[] mPoints;
    /// <summary>colliderの長さ(mPointsの0~2番目の頂点がなす法線の逆方向に伸ばす)</summary>
    [SerializeField] public float mLength;
    public override Mesh createMesh() {
        if (mPoints == null || mPoints.Length < 3) return null;
        Mesh tMesh = new Mesh();
        tMesh.name = "myPillar";

        List<Vector3> tVerticeList = new List<Vector3>();
        List<int> tTriangleList = new List<int>();

        Vector3 tLength = Vector3.Cross(mPoints[2] - mPoints[0], mPoints[1] - mPoints[0]).normalized * mLength;
        int tPointNum = mPoints.Length;
        for (int i = 0; i < tPointNum; ++i) {
            int tIndex1 = i;
            int tIndex2 = (i + 1) % tPointNum;
            //頂点
            tVerticeList.Add(mPoints[tIndex1]);
            tVerticeList.Add(mPoints[tIndex1] + tLength);

            //側面のtriangles
            tTriangleList.Add(tIndex1 * 2);
            tTriangleList.Add(tIndex1 * 2 + 1);
            tTriangleList.Add(tIndex2 * 2);

            tTriangleList.Add(tIndex2 * 2);
            tTriangleList.Add(tIndex1 * 2 + 1);
            tTriangleList.Add(tIndex2 * 2 + 1);
        }
        for (int i = 1; i < tPointNum - 1; ++i) {
            //底面のtriangles
            tTriangleList.Add(0);
            tTriangleList.Add(i * 2);
            tTriangleList.Add((i + 1) * 2);
            //上面のtriangles
            tTriangleList.Add(1);
            tTriangleList.Add((i + 1) * 2 + 1);
            tTriangleList.Add(i * 2 + 1);
        }

        Vector3[] tVertices = tVerticeList.ToArray();
        Vector2[] tUvs = new Vector2[0];
        int[] tTriangles = tTriangleList.ToArray();

        tMesh.vertices = tVertices;
        tMesh.uv = tUvs;
        tMesh.triangles = tTriangles;
        return tMesh;
    }
}
