using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GameMaster : MyBehaviour {
    private void occurAccident(PlayerStatus aPlayer, AccidentMasStatus aAccident, Action aCallback) {
        switch (aAccident.mAccidentKey) {
            case "heart":
                occurHeartAccident(aPlayer, aCallback);
                break;
            case "bat":
                occurBatAccident(aPlayer, aCallback);
                break;
            case "god":
                occurGodAccident(aPlayer, aCallback);
                break;
            case "gear":
                occurGearAccident(aPlayer, aCallback);
                break;
            case "question":
                occurQuestionAccident(aPlayer, aCallback);
                break;
        }
    }
    private void occurHeartAccident(PlayerStatus aPlayer, Action aCallback) {
        int tNum = UnityEngine.Random.Range(0, 3);
        switch (tNum) {
            case 0:
                int tFood = UnityEngine.Random.Range(80, 120) + aPlayer.mLap * 7;
                mElement.mTable.showMessage("果樹園を見つけた\n食糧+" + tFood.ToString());
                waitTap(() => {
                    mElement.mTable.hideMessage();
                    getFood(aPlayer, tFood, aCallback);
                });
                break;
            case 1:
                mElement.mTable.showMessage("獲物を狩って食糧ゲット\n食糧+200");
                waitTap(() => {
                    mElement.mTable.hideMessage();
                    getFood(aPlayer, 200, aCallback);
                });
                break;
            case 2:
                mElement.mTable.showMessage("仲間を派遣して\n好きな土地を\n占領できます");
                waitTap(() => {
                    mElement.mTable.hideMessage();
                    occupySelectedLand(aPlayer, aCallback);
                });
                break;
        }
    }
    private void occurBatAccident(PlayerStatus aPlayer, Action aCallback) {
        int tNum = UnityEngine.Random.Range(0, 2);
        switch (tNum) {
            case 0:
                int tFood = UnityEngine.Random.Range(3, 7) + aPlayer.mLap / 5;
                mElement.mTable.showMessage("内戦勃発\n縄張りの" + tFood.ToString() + "%の食糧損失");
                waitTap(() => {
                    mElement.mTable.hideMessage();
                    lostFood(aPlayer, (int)((tFood / 100f) * aPlayer.mTerritory), aCallback);
                });
                break;
            case 1:
                mElement.mTable.showMessage("野盗に襲われる\n食糧-150");
                waitTap(() => {
                    mElement.mTable.hideMessage();
                    lostFood(aPlayer, 150, aCallback);
                });
                break;
        }
    }
    private void occurGodAccident(PlayerStatus aPlayer, Action aCallback) {
        int tFood = 0;
        if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.2f) {
            tFood = UnityEngine.Random.Range(5, 10) + aPlayer.mLap / 5;
            mElement.mTable.showMessage("全世界で地震が発生\n縄張りの"+tFood.ToString()+"%の食糧損失");
            damageLandAttribute(tFood, LandMasStatus.LandAttribute.all, aCallback);
        } else {
            List<LandMasStatus.LandAttribute> tList = getAllLandAttribute();
            tFood = UnityEngine.Random.Range(5, 10) + aPlayer.mLap / 5;
            int i = UnityEngine.Random.Range((int)0, (int)(tList.Count));
            switch (tList[i]) {
                case LandMasStatus.LandAttribute.north:
                    mElement.mTable.showMessage("北部地方を竜巻が襲った\n縄張りの"+tFood.ToString()+"%の食糧損失");
                    damageLandAttribute(tFood, LandMasStatus.LandAttribute.north, aCallback);
                    break;
                case LandMasStatus.LandAttribute.east:
                    mElement.mTable.showMessage("東部地方を竜巻が襲った\n縄張りの"+tFood.ToString()+"%の食糧損失");
                    damageLandAttribute(tFood, LandMasStatus.LandAttribute.east, aCallback);
                    break;
                case LandMasStatus.LandAttribute.south:
                    mElement.mTable.showMessage("南部地方を竜巻が襲った\n縄張りの"+tFood.ToString()+"%の食糧損失");
                    damageLandAttribute(tFood, LandMasStatus.LandAttribute.south, aCallback);
                    break;
                case LandMasStatus.LandAttribute.west:
                    mElement.mTable.showMessage("西部地方を竜巻が襲った\n縄張りの"+tFood.ToString()+"%の食糧損失");
                    damageLandAttribute(tFood, LandMasStatus.LandAttribute.west, aCallback);
                    break;
                case LandMasStatus.LandAttribute.center:
                    mElement.mTable.showMessage("中部地方を竜巻が襲った\n縄張りの" + tFood.ToString() + "%の食糧損失");
                    damageLandAttribute(tFood, LandMasStatus.LandAttribute.center, aCallback);
                    break;
                case LandMasStatus.LandAttribute.woods:
                    mElement.mTable.showMessage("森林地帯を酸性雨が襲った\n縄張りの"+tFood.ToString()+"%の食糧損失");
                    damageLandAttribute(tFood, LandMasStatus.LandAttribute.woods, aCallback);
                    break;
                case LandMasStatus.LandAttribute.waterside:
                    mElement.mTable.showMessage("水辺地帯で水質汚濁が発生\n縄張りの"+tFood.ToString()+"%の食糧損失");
                    damageLandAttribute(tFood, LandMasStatus.LandAttribute.waterside, aCallback);
                    break;
            }
        }
    }
    private void occurGearAccident(PlayerStatus aPlayer, Action aCallback) {

    }
    private void occurQuestionAccident(PlayerStatus aPlayer, Action aCallback) {

    }
    //土地属性に被害
    private void damageLandAttribute(int aParcent, LandMasStatus.LandAttribute aAttribute, Action aCallback) {
        waitTap(() => {
            int[] tLostFoodList = new int[mElement.mPlayerStatus.Length];
            for (int i = 0; i < mElement.mMasStatus.Length; i++) {
                if (!(mElement.mMasStatus[i] is LandMasStatus)) continue;
                LandMasStatus tLand = (LandMasStatus)mElement.mMasStatus[i];
                if (tLand.mOwnerNumber < 0) continue;
                if (!(tLand.mAttribute1 == aAttribute || tLand.mAttribute2 == aAttribute || aAttribute == LandMasStatus.LandAttribute.all)) continue;
                tLostFoodList[tLand.mOwnerNumber] += tLand.mTotalValue * aParcent / 100;
            }
            int j = 0;
            Action tLost = () => { };
            tLost = () => {
                if (j >= mTurn.Length) {
                    mElement.mTable.hideMessage();
                    aCallback();
                    return;
                }
                PlayerStatus tPlayer = mElement.mPlayerStatus[mTurn[j]];
                if (tPlayer.mTotalResource < 0) {
                    j++;
                    tLost();
                    return;
                }
                mElement.mTable.showMessage((tPlayer.mPlayerNumber + 1) + "Pは\n食糧-" + tLostFoodList[tPlayer.mPlayerNumber].ToString() + "の損失");
                lostFood(tPlayer, tLostFoodList[tPlayer.mPlayerNumber], () => {
                    j++;
                    tLost();
                });
            };
            tLost();
        });
    }
    //プレイヤが画面に触れるのを待つ
    private void waitTap(Action aCallback) {
        Subject.addObserver(new Observer("accidentWateTap", (aMessage) => {
            if (aMessage.name != "touchBack") return;
            Subject.removeObserver("accidentWateTap");
            aCallback();
        }));
    }
    //ステージに含まれる土地属性を全て返す
    private List<LandMasStatus.LandAttribute> getAllLandAttribute() {
        List<LandMasStatus.LandAttribute> tList = new List<LandMasStatus.LandAttribute>();
        for (int i = 0; i < mElement.mMasStatus.Length; i++) {
            if (!(mElement.mMasStatus[i] is LandMasStatus)) continue;
            LandMasStatus tLand = (LandMasStatus)mElement.mMasStatus[i];

            bool tContain1 = false;
            bool tContain2 = false;
            foreach (LandMasStatus.LandAttribute aAttribute in tList) {
                if (aAttribute == tLand.mAttribute1) tContain1 = true;
                if (aAttribute == tLand.mAttribute2) tContain2 = true;
            }
            if (!tContain1 && tLand.mAttribute1 != LandMasStatus.LandAttribute.none) tList.Add(tLand.mAttribute1);
            if (!tContain2 && tLand.mAttribute2 != LandMasStatus.LandAttribute.none) tList.Add(tLand.mAttribute2);
        }
        return tList;
    }
    //土地を選択して占領
    private void occupySelectedLand(PlayerStatus aPlayer, Action aCallback) {
        List<LandMasStatus> tFreeList = mElement.getAllFreeLand();
        LandMasStatus[] tFree = tFreeList.ToArray();
        //価値が安い順にソート
        Array.Sort(tFree, (x, y) => { return x.mOccupyCost - y.mOccupyCost; });
        if (tFree.Length == 0 || aPlayer.mFood < tFree[0].mOccupyCost) {
            mElement.mTable.showMessage("占領できる土地が\nありません");
            waitTap(() => {
                mElement.mTable.hideMessage();
                aCallback();
            });
            return;
        }
        //占領できる土地がある
        aPlayer.mController.selectOccupyLand(aPlayer, mElement, (aLand) => {
            if (aLand == null) {//占領しない
                if (!(aPlayer.mController is PlayerController)) {
                    mElement.mTable.showMessage("占領しませんでした");
                    waitTap(() => {
                        mElement.mTable.hideMessage();
                        aCallback();
                    });
                } else {
                    aCallback();
                }
                return;
            }
            //占領する
            occupyLand(aPlayer, aLand);
            aCallback();
        });
    }
}
