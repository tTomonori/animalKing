using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElementData {
    public GameInitData mInitData;
    public StageData mStageData;
    public PlayerStatus[] mPlayerStatus;
    public PlayerStatusDisplay[] mPlayerStatusDisplay;
    public MasStatus[] mMasStatus;
    public MasDisplay[] mMasDisplay;
    public GameTable mTable;
    public PlayerPiece[] mPlayerPieces;

    /// <summary>指定したプレイヤが占領している土地を全て返す</summary>
    public List<LandMasStatus> getAllOccupiedLand(PlayerStatus aPlayer) {
        List<LandMasStatus> tOccupied = new List<LandMasStatus>();
        for (int i = 0; i < mMasStatus.Length; i++) {
            if (!(mMasStatus[i] is LandMasStatus)) continue;
            LandMasStatus tLand = (LandMasStatus)mMasStatus[i];
            if (tLand.mOwnerNumber != aPlayer.mPlayerNumber) continue;
            tOccupied.Add(tLand);
        }
        return tOccupied;
    }
    /// <summary>占領されていない土地を全て返す</summary>
    public List<LandMasStatus> getAllFreeLand() {
        List<LandMasStatus> tFree = new List<LandMasStatus>();
        for (int i = 0; i < mMasStatus.Length; i++) {
            if (!(mMasStatus[i] is LandMasStatus)) continue;
            LandMasStatus tLand = (LandMasStatus)mMasStatus[i];
            if (tLand.mOwnerNumber >= 0) continue;
            tFree.Add(tLand);
        }
        return tFree;
    }
}
