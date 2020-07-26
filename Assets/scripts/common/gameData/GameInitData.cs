using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitData {
    public PlayerData[] mPlayerDataList;
    public int mInitialFood;
    public int mLooting;
    public bool mAcquisition;
    public string mStagePath;
    public struct PlayerData {
        public string mAnimalName;
        public bool mIsPlayer;
    }
}
