using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collider2DCreator {
    /// <summary>
    /// Tagのデータを元にCollider2Dを生成
    /// </summary>
    /// <returns>生成したCollider</returns>
    /// <param name="aObject">Colliderを追加するオブジェクト</param>
    /// <param name="aTag">Colliderのデータのタグ</param>
    static public Collider2D addCollider(GameObject aObject, MyTag aTag) {
        switch (aTag.mTagName) {
            case "box":
                BoxCollider2D tBox = aObject.AddComponent<BoxCollider2D>();
                tBox.size = new Vector2(float.Parse(aTag.mArguments[0]), float.Parse(aTag.mArguments[1]));
                if (2 < aTag.mArguments.Length) {
                    tBox.offset = new Vector2(float.Parse(aTag.mArguments[2]), float.Parse(aTag.mArguments[3]));
                }
                return tBox;
        }
        Debug.LogWarning("Collider2DCreator : 形状「" + aTag.mTagName + "」のColliderの生成に失敗しました");
        return null;
    }
    static public void addCollider(GameObject aObject, Arg aData) {
        switch (aData.get<string>("type")) {
            case "box":
                BoxCollider2D tBox = aObject.AddComponent<BoxCollider2D>();
                tBox.size = new Vector2(aData.get<float>("width"), aData.get<float>("height"));
                break;
        }
    }
}
