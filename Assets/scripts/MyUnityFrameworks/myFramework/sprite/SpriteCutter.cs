using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteCutter {
    static public Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit = 100.0f,
                                uint extrude = 0, SpriteMeshType meshType = SpriteMeshType.FullRect,
                                Vector4 border = new Vector4(), bool generateFallbackPhysicsShape = false) {
        Rect tRect = new Rect(rect.xMin + 1f, rect.yMin + 1f, rect.width - 2f, rect.height - 2f);
        return Sprite.Create(texture, tRect, pivot, pixelsPerUnit - 2f, extrude, meshType, border, generateFallbackPhysicsShape);
    }
    static public Sprite[][] split(Texture2D aTexture, Vector2 aPartsSize, Vector2 aPivot) {
        Vector2 tPivot = new Vector2(0.5f, 0.5f);
        int tXSize = Mathf.FloorToInt(aTexture.width / aPartsSize.x);
        int tYSize = Mathf.FloorToInt(aTexture.height / aPartsSize.y);
        Sprite[][] tSprites = new Sprite[tYSize][];
        for (int y = 0; y < tYSize; y++) {
            tSprites[tYSize - y - 1] = new Sprite[tXSize];
            for (int x = 0; x < tXSize; x++) {
                tSprites[tYSize - y - 1][x] = Sprite.Create(aTexture, new Rect(new Vector2(aPartsSize.x * x, aPartsSize.y * y), aPartsSize), tPivot);
            }
        }
        return tSprites;
    }
}
