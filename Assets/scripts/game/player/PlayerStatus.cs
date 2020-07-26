using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus {
    /// <summary>動物名</summary>
    public string mAnimalName;
    /// <summary>プレイヤ番号</summary>
    public int mPlayerNumber;
    /// <summary>プレイヤかCPUか</summary>
    public GameController mController;
    /// <summary>順位</summary>
    public int mRank;
    /// <summary>周回数(最初は0)</summary>
    public int mLap;
    /// <summary>逆走周回数</summary>
    public int mReverseLap;
    /// <summary>食糧</summary>
    public int mFood;
    /// <summary>縄張り</summary>
    public int mTerritory;
    /// <summary>合計資源</summary>
    public int mTotalResource { get { return mFood + mTerritory; } }
    /// <summary>現在いるマスの番号</summary>
    public int mCurrentMasNumber;
    /// <summary>手番の順番</summary>
    public int mTurn;

    public static Color[] playerColor {
        get { return new Color[4] { new Color(0.3f, 0.7f, 1), new Color(1, 0.3f, 0.3f), new Color(0.4f, 1, 0.4f), new Color(1, 1, 0.3f) }; }
    }
    public static Color nonePlayerColor { get { return new Color(0.5f, 0.5f, 0.5f); } }
}
