using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceButton : MyButton {
    /// <summary>現在表示している動物名を返す</summary>
    public string getAnimalName() {
        SpriteRenderer tFace = findChild<SpriteRenderer>("faceImage");
        return Animal.getName(tFace.sprite);
    }

    protected override void pushed() {
        string[] tAnimalList = Animal.getAnimalList;
        string tCurrentAnimalName = getAnimalName();
        //現在表示している動物のインデックス
        int tCurrentIndex;
        for(tCurrentIndex=0; tCurrentIndex < tAnimalList.Length - 1; tCurrentIndex++) {
            if (tCurrentAnimalName == tAnimalList[tCurrentIndex]) break;
        }
        //被りがないように変更後の動物を探す
        int tToIndex = (tCurrentIndex + 1) % tAnimalList.Length;
        for(; ; ) {
            bool tIsOvercoat = false;
            for(int i = 1; i < 5; i++) {
                GameObject tConfig = GameObject.Find("player_config" + i);
                FaceButton tFaceButton = tConfig.GetComponentInChildren<FaceButton>();
                if (tAnimalList[tToIndex] == tFaceButton.getAnimalName()) {
                    tIsOvercoat = true;
                    break;
                }
            }
            if (!tIsOvercoat) break;
            tToIndex = (tToIndex + 1) % tAnimalList.Length;
        }
        findChild<SpriteRenderer>("faceImage").sprite = Animal.getFaceImage(tAnimalList[tToIndex]);
    }
}
