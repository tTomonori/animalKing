using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GameMaster : MyBehaviour {
    //手番で止まったマスのイベントの処理
    private void runEventPhase(Action aCallback) {
        MasStatus tMas = mElement.mMasStatus[mTurnPlayer.mCurrentMasNumber];
        switch (tMas) {
            case LandMasStatus tLand:
                runLandEventPhase(mTurnPlayer, tLand, aCallback);
                break;
            case AccidentMasStatus tAccident:
                runAccident(mTurnPlayer, tAccident, aCallback);
                break;
            case MasStatus t:
                aCallback();
                break;
        }
    }
    private void runAccident(PlayerStatus aPlayer, AccidentMasStatus aAccident, Action aCallback) {
        mElement.mTable.setStarCard(PlayerStatus.playerColor[aPlayer.mPlayerNumber]);
        aPlayer.mController.flipCard(() => {
            aPlayer.mController.endFlip();
            mElement.mTable.flipStarCard(() => {
                occurAccident(aPlayer, aAccident, aCallback);
            });
        });
    }
    //マスの通過イベント処理
    private void runPassEvent(PlayerStatus aPlayer, Action aCallback) {
        //通過するマス
        MasStatus tMas = mElement.mMasStatus[aPlayer.mCurrentMasNumber];
        switch (tMas) {
            case StartMasStatus tStart:
                aPlayer.mLap += 1;
                //mElement.mTable.showMessage("周回ボーナス\n食糧+150");
                getFood(aPlayer, 150, () => {
                    mElement.mTable.hideMessage();
                    aCallback();
                });
                break;
            default:
                aCallback();
                break;
        }
    }
    //土地に止まった時のイベント
    private void runLandEventPhase(PlayerStatus aPlayer, LandMasStatus aLand, Action aCallback) {
        if (aLand.mOwnerNumber < 0) {
            //未占領だった場合
            bool tRuned = false;
            if (aPlayer.mFood >= aLand.mValue) {
                //食糧が足りる
                aPlayer.mController.occupyLand(aPlayer, aLand, mElement, (a) => {
                    if (tRuned) return;//既に回答済
                    tRuned = true;
                    if (!a) { //占領しない
                        setTimeout(0.3f, () => { aCallback(); });
                        return;
                    }
                    occupyLand(aPlayer, aLand);
                    setTimeout(0.5f, () => { aCallback(); });
                });
            } else {
                //食糧が足りない
                setTimeout(0.3f, () => { aCallback(); });
            }
        } else {
            PlayerStatus tOwerPlayer = mElement.mPlayerStatus[aLand.mOwnerNumber];
            if (aPlayer == tOwerPlayer) {
                //自分の所有地だった場合
                bool tRuned = false;
                if (aPlayer.mFood >= aLand.mExpansionCost && aLand.mExpansionLevel < 3) {
                    //食糧が足りる
                    aPlayer.mController.expandLand(aPlayer, aLand, mElement, (a) => {
                        if (tRuned) return;//既に回答済
                        tRuned = true;
                        if (!a) {//拡大しない
                            setTimeout(0.3f, () => { aCallback(); });
                            return;
                        }
                        expandLand(aPlayer, aLand);
                        setTimeout(0.5f, () => { aCallback(); });
                    });
                } else {
                    //食糧が足りない
                    setTimeout(0.3f, () => { aCallback(); });
                }
            } else {
                //他人の所有地だった場合
                moveFood(mElement.mPlayerStatus[aLand.mOwnerNumber], aPlayer, aLand.mLootedCost, aCallback);
            }
        }

    }
    //占領する
    private void occupyLand(PlayerStatus aPlayer, LandMasStatus aLand) {
        MySoundPlayer.playSe("decision7");
        aPlayer.mFood -= aLand.mValue;
        aPlayer.mTerritory += aLand.mValue;
        mElement.mPlayerStatusDisplay[aPlayer.mPlayerNumber].updateDisplay(aPlayer);

        aLand.mOwnerNumber = aPlayer.mPlayerNumber;
        ((LandMasDisplay)mElement.mMasDisplay[aLand.mMasNumber]).updateDisplay(aLand);
    }
    //拡大する
    private void expandLand(PlayerStatus aPlayer, LandMasStatus aLand) {
        MySoundPlayer.playSe("decision7");
        aPlayer.mFood -= aLand.mExpansionCost;
        aPlayer.mTerritory += aLand.mExpansionCost;
        mElement.mPlayerStatusDisplay[aPlayer.mPlayerNumber].updateDisplay(aPlayer);

        aLand.mExpansionLevel += 1;
        ((LandMasDisplay)mElement.mMasDisplay[aLand.mMasNumber]).updateDisplay(aLand);
    }
    //解放する
    private void freeLand(LandMasStatus aLand, Action aCallback) {
        MySoundPlayer.playSe("cancel1");
        PlayerStatus tOwner = mElement.mPlayerStatus[aLand.mOwnerNumber];
        tOwner.mFood += aLand.mFreeCost;
        aLand.mOwnerNumber = -1;
        tOwner.mTerritory -= aLand.mTotalValue;
        mElement.mPlayerStatusDisplay[tOwner.mPlayerNumber].updateDisplay(tOwner);
        ((LandMasDisplay)mElement.mMasDisplay[aLand.mMasNumber]).updateDisplay(aLand);
        setTimeout(1, aCallback);
    }
    //所持食糧が負の数になった
    private void endanger(PlayerStatus aPlayer, Action aCallback) {
        MySoundPlayer.playSe("cursor6");
        mElement.mTable.showMessage((aPlayer.mPlayerNumber + 1) + "Pは絶滅の危機です");
        setTimeout(1, () => {
            Action tAskFree = () => { };
            tAskFree = () => {
                //絶滅判定
                if (aPlayer.mTerritory <= 0) {
                    endangered(aPlayer, aCallback);
                    return;
                }
                //解放する土地を選択する
                bool tAnsered = false;
                aPlayer.mController.freeLand(aPlayer, mElement, (aLand) => {
                    if (tAnsered == true) return;
                    tAnsered = true;
                    freeLand(aLand, () => {
                        if (aPlayer.mFood < 0) {
                            tAskFree();//まだ負の数
                        } else {
                            mElement.mTable.hideMessage();
                            aCallback();
                        }
                    });
                });
            };
            tAskFree();
        });
    }
    //絶滅した
    private void endangered(PlayerStatus aPlayer, Action aCallback) {
        mElement.mPlayerPieces[aPlayer.mPlayerNumber].delete();
        mElement.mTable.showMessage((aPlayer.mPlayerNumber + 1) + "Pは絶滅しました");
        setTimeout(2, () => {
            mElement.mTable.hideMessage();
            aCallback();
        });
    }
    private void moveFood(PlayerStatus aGetter, PlayerStatus aPayer, int aFood, Action aCallback) {
        MySoundPlayer.playSe("decision14");
        aGetter.mFood += aFood;
        aPayer.mFood -= aFood;
        sortPlayerRank();
        CallbackSystem tSystem = new CallbackSystem();
        GameAnimation.getFood(mElement.mPlayerStatusDisplay[aGetter.mPlayerNumber], aFood, tSystem.getCounter());
        GameAnimation.getFood(mElement.mPlayerStatusDisplay[aPayer.mPlayerNumber], -aFood, tSystem.getCounter());
        tSystem.then(() => {
            if (aPayer.mFood < 0) {
                endanger(aPayer, aCallback);
                return;
            }
            aCallback();
        });
    }
    private void getFood(PlayerStatus aPlayer, int aFood, Action aCallback) {
        if (aFood != 0)
            MySoundPlayer.playSe("decision22");
        aPlayer.mFood += aFood;
        sortPlayerRank();
        GameAnimation.getFood(mElement.mPlayerStatusDisplay[aPlayer.mPlayerNumber], aFood, aCallback);
    }
    private void lostFood(PlayerStatus aPlayer, int aFood, Action aCallback) {
        if (aFood != 0)
            MySoundPlayer.playSe("decision14");
        aPlayer.mFood -= aFood;
        sortPlayerRank();
        GameAnimation.getFood(mElement.mPlayerStatusDisplay[aPlayer.mPlayerNumber], -aFood, () => {
            if (aPlayer.mFood >= 0) {
                aCallback();
            } else {
                endanger(aPlayer, aCallback);
            }
        });
    }
}
