using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AiController : GameController {
    public override void flipCard(Action aFlipThen) {
        MyBehaviour.setTimeoutToIns(0.4f, () => { aFlipThen(); });
        MyBehaviour.setTimeoutToIns(0.8f, () => { aFlipThen(); });
        MyBehaviour.setTimeoutToIns(1.2f, () => { aFlipThen(); });
    }
    public override void endFlip() {

    }
    public override void occupyLand(PlayerStatus aMyself, LandMasStatus aLand, GameElementData aElement, Action<bool> aAnser) {
        aAnser(true);
    }
    public override void expandLand(PlayerStatus aMyself, LandMasStatus aLand, GameElementData aElement, Action<bool> aAnser) {
        aAnser(true);
    }
    public override void freeLand(PlayerStatus aMyself, GameElementData aElement, Action<LandMasStatus> aAnser) {
        List<LandMasStatus> tOccupiedList = aElement.getAllOccupiedLand(aMyself);
        LandMasStatus[] tOccupied = tOccupiedList.ToArray();
        //略奪コストが最も安い土地を解放
        Array.Sort(tOccupied, (x, y) => { return x.mLootedCost - y.mLootedCost; });
        aAnser(tOccupied[0]);
    }
    public override void selectOccupyLand(PlayerStatus aMyself, GameElementData aElement, Action<LandMasStatus> aAnser) {
        List<LandMasStatus> tFreeList = aElement.getAllFreeLand();
        LandMasStatus[] tFree = tFreeList.ToArray();
        //最も価値が安い土地を占領
        Array.Sort(tFree, (x, y) => { return x.mOccupyCost - y.mOccupyCost; });
        foreach(LandMasStatus tLand in tFree) {
            if (tLand.mOccupyCost <= aMyself.mFood) {
                aAnser(tLand);
                return;
            }
        }
        aAnser(null);
        return;
    }
}
