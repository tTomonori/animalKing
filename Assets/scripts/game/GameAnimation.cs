using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAnimation {
    /// <summary>プレイヤの順番シャッフル</summary>
    public static void playerShuffle(PlayerStatusDisplay[] aDisplay, PlayerStatus[] aStatus, int[] aTurn, Action aCallback) {
        //手番順に座標を配列に格納
        Vector2[] tTurnPosition = new Vector2[aDisplay.Length];
        for (int i = 0; i < aDisplay.Length; i++) {
            int tTurn = aStatus[i].mTurn;
            tTurnPosition[tTurn] = aDisplay[i].position2D;
        }
        //シャッフル結果の座標をプレイヤ番号順に格納
        Vector2[] tResultPosition = new Vector2[aDisplay.Length];
        for (int i = 0; i < aDisplay.Length; i++) {
            int tPlayerNum = aTurn[i];
            tResultPosition[tPlayerNum] = tTurnPosition[i];
        }
        //全てのdisplayの中央座標
        Vector2 tCenter = new Vector2(0, 0);
        for (int i = 0; i < aDisplay.Length; i++) {
            tCenter += tTurnPosition[i];
        }
        tCenter /= aDisplay.Length;
        //アニメーション
        MyBehaviour.setTimeoutToIns(1, () => {
            //中央に集める
            CallbackSystem tSystem = new CallbackSystem();
            for (int i = 0; i < aDisplay.Length; i++) {
                aDisplay[i].moveTo(tCenter, 0.2f, tSystem.getCounter());
            }
            tSystem.then(() => {
                MyBehaviour.setTimeoutToIns(0.5f, () => {
                    //それぞれの位置へ移動
                    CallbackSystem tSystem2 = new CallbackSystem();
                    for (int i = 0; i < aDisplay.Length; i++) {
                        aDisplay[i].moveTo(tResultPosition[i], 0.2f, tSystem2.getCounter());
                    }
                    tSystem2.then(aCallback);
                });
            });
        });
    }
    /// <summary>食糧取得or損失アニメーション</summary>
    public static void getFood(PlayerStatusDisplay aDisplay, int aFood, Action aCallback) {
        TextMesh tMesh = MyBehaviour.create<TextMesh>();
        tMesh.text = (aFood >= 0 ? "+" : "") + aFood.ToString();
        tMesh.name = "getFoodMesh : " + tMesh.text;
        tMesh.fontSize = 100;
        tMesh.characterSize = 0.04f;
        tMesh.anchor = TextAnchor.MiddleLeft;
        tMesh.transform.position = new Vector3(aDisplay.positionX - 0.6f, aDisplay.positionY - 0.1f, -20);
        Vector2 tDirection;
        if (aFood >= 0) {
            //取得
            tMesh.color = new Color(0, 1, 1);
            tDirection = new Vector2(0, 0.3f);
        } else {
            //損失
            tMesh.color = new Color(1, 0, 0);
            tDirection = new Vector2(0, -0.3f);
        }
        MyBehaviour tBehaviour = tMesh.gameObject.AddComponent<MyBehaviour>();
        tBehaviour.moveBy(tDirection, 1.2f, () => {
            tBehaviour.delete();
            aCallback();
        });
    }
}
