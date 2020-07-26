using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Animal {
    /// <summary>動物の名前リスト</summary>
    static public string[] getAnimalList { get { return new string[5] { "dog", "cat", "mouse", "sheep", "rabbit" }; } }

    /// <summary>画像から動物名を取得</summary>
    static public string getName(Sprite aSprite) {
        return aSprite.name.Split('_')[0];
    }

    /// <summary>指定した動物の顔画像を取得</summary>
    static public Sprite getFaceImage(string aAnimalName) {
        return Resources.Load<Sprite>("image/animal/" + aAnimalName + "/" + aAnimalName + "_face");
    }
    static public Sprite getPieceImage(string aAnimalName) {
        return Resources.Load<Sprite>("image/animal/" + aAnimalName + "/" + aAnimalName + "_piece");
    }
}
