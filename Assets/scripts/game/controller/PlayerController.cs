using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : GameController {
    public override void flipCard(Action aFlipThen) {
        Subject.addObserver(new Observer("playerFlip", (aMessage) => {
            if (aMessage.name == "touchBack") aFlipThen();
        }));
    }
    public override void endFlip() {
        Subject.removeObserver("playerFlip");
    }
    public override void occupyLand(PlayerStatus aMyself, LandMasStatus aLand, GameElementData aElement, Action<bool> aAnser) {
        Subject.addObserver(new Observer("playerController", (aMessage) => {
            if (aMessage.name == "yes") {
                aAnser(true);
            }else if (aMessage.name == "no") {
                aAnser(false);
            } else {
                return;
            }
            Subject.removeObserver("playerController");
            aElement.mTable.hideQuestionForm();
        }));
        aElement.mTable.question("食糧" + aLand.mValue + "で占領しますか？");
    }
    public override void expandLand(PlayerStatus aMyself, LandMasStatus aLand, GameElementData aElement, Action<bool> aAnser) {
        Subject.addObserver(new Observer("playerController", (aMessage) => {
            if (aMessage.name == "yes") {
                aAnser(true);
            } else if (aMessage.name == "no") {
                aAnser(false);
            } else {
                return;
            }
            Subject.removeObserver("playerController");
            aElement.mTable.hideQuestionForm();
        }));
        aElement.mTable.question("食糧" + aLand.mExpansionCost + "で拡大しますか？");
    }
    public override void freeLand(PlayerStatus aMyself, GameElementData aElement, Action<LandMasStatus> aAnser) {
        aElement.mTable.showMessage("解放する土地を\n選択してください");
        LandMasStatus tSelected = null;
        //マスタップ監視
        Subject.addObserver(new Observer("playerControllerTouchMas", (aMessage) => {
            if (aMessage.name != "touchMas") return;//監視外メッセージ
            MasStatus tMas = aElement.mMasStatus[aMessage.parameters.get<int>("number")];
            if (!(tMas is LandMasStatus)) return;//土地のマス以外
            LandMasStatus tLand = (LandMasStatus)tMas;
            if (tLand.mOwnerNumber != aMyself.mPlayerNumber) return;//自分以外の土地
            tSelected = tLand;
            aElement.mTable.hideMessage();
            aElement.mTable.question(tSelected.mName+"を解放しますか？\n食糧+"+tLand.mFreeCost);
        }));
        //はい,いいえ選択監視
        Subject.addObserver(new Observer("playerControllerTableQuestion", (aMessage) => {
            if (aMessage.name == "yes") {
                Subject.removeObserver("playerControllerTouchMas");
                Subject.removeObserver("playerControllerTableQuestion");
                aElement.mTable.hideQuestionForm();
                aAnser(tSelected);
            } else if (aMessage.name == "no") {
                aElement.mTable.hideQuestionForm();
                tSelected = null;
                aElement.mTable.showMessage("解放する土地を\n選択してください");
            }
        }));
    }
    public override void selectOccupyLand(PlayerStatus aMyself, GameElementData aElement, Action<LandMasStatus> aAnser) {
        aElement.mTable.showOneSelectForm("占領する土地を\n選択してください", "占領しない");
        LandMasStatus tSelected = null;
        //マスタップ監視
        Subject.addObserver(new Observer("playerControllerTouchMas", (aMessage) => {
            if (aMessage.name != "touchMas") return;//監視外メッセージ
            MasStatus tMas = aElement.mMasStatus[aMessage.parameters.get<int>("number")];
            if (!(tMas is LandMasStatus)) return;//土地のマス以外
            LandMasStatus tLand = (LandMasStatus)tMas;
            if (tLand.mOwnerNumber >= 0) return;//占領済マス
            if (aMyself.mFood < tLand.mOccupyCost) return;//所持食糧を超えるコストのマス
            tSelected = tLand;
            aElement.mTable.hideMessage();
            aElement.mTable.hideOneSelectForm();
            aElement.mTable.question(tSelected.mName + "を占領しますか？\n食糧-" + tLand.mOccupyCost);
        }));
        //はい,いいえ,占領しない選択監視
        Subject.addObserver(new Observer("playerControllerTableQuestion", (aMessage) => {
            if (aMessage.name == "yes") {
                Subject.removeObserver("playerControllerTouchMas");
                Subject.removeObserver("playerControllerTableQuestion");
                aElement.mTable.hideQuestionForm();
                aAnser(tSelected);
            } else if (aMessage.name == "no") {
                aElement.mTable.hideQuestionForm();
                tSelected = null;
                aElement.mTable.showOneSelectForm("占領する土地を\n選択してください", "占領しない");
            } else if (aMessage.name == "oneSelect") {
                Subject.removeObserver("playerControllerTouchMas");
                Subject.removeObserver("playerControllerTableQuestion");
                aElement.mTable.hideOneSelectForm();
                aAnser(null);
            }
        }));
    }
}
