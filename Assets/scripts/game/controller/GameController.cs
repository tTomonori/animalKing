using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class GameController {
    //カードをめくるタイミング指示
    abstract public void flipCard(Action aFlipThen);
    abstract public void endFlip();
    //土地を占領するか
    abstract public void occupyLand(PlayerStatus aMyself, LandMasStatus aLand, GameElementData aElement, Action<bool> aAnser);
    //土地を拡大するか
    abstract public void expandLand(PlayerStatus aMyself, LandMasStatus aLand, GameElementData aElement, Action<bool> aAnser);
    //どの土地を解放するか
    abstract public void freeLand(PlayerStatus aMyself, GameElementData aElement, Action<LandMasStatus> aAnser);
    //どの土地を占領するか
    abstract public void selectOccupyLand(PlayerStatus aMyself, GameElementData aElement, Action<LandMasStatus> aAnser);
}
