using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyMeshCollider : MyBehaviour {
    public abstract Mesh createMesh();
    private void OnValidate() {
        createCollider();
    }
    public void createCollider() {
        MeshCollider tCollider = GetComponent<MeshCollider>();
        if (tCollider == null) {
            tCollider = gameObject.AddComponent<MeshCollider>();
            tCollider.convex = true;
        }
        tCollider.sharedMesh = createMesh();
    }
}
