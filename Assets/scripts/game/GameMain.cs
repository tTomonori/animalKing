using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameMain : MonoBehaviour {
    private GameElementData mElement;
    private GameMaster mMaster;

    // Start is called before the first frame update
    void Start() {
        Arg tArg = MySceneManager.getArg("game");
        MySoundPlayer.playBgm("Morning_2", MySoundPlayer.LoopType.normalConnect, 0.7f);

        mElement = new GameElementData();

        mElement.mInitData = tArg.get<GameInitData>("data");
        mElement.mStageData = new StageData(mElement.mInitData.mStagePath);
        LandMasStatus.mLootingRate = mElement.mInitData.mLooting / 100.0f;

        initPlace();
        initStatus();
        //piece
        mElement.mPlayerPieces = GamePlacer.placePiece(mElement.mPlayerStatus, mElement.mMasDisplay[0].position2D);

        mMaster = MyBehaviour.create<GameMaster>();
        mMaster.name = "GameMaster";
        mMaster.mElement = mElement;
        mMaster.mMain = this;

        mMaster.start();
    }

    // Update is called once per frame
    void Update() {

    }

    private void initPlace() {
        //playerStatus
        mElement.mPlayerStatusDisplay = GamePlacer.placePlayerStatus(mElement.mStageData);
        //mas
        mElement.mMasDisplay = GamePlacer.placeMas(mElement.mStageData);
        //table
        mElement.mTable = GamePlacer.placeTable(mElement.mStageData);
    }
    private void initStatus() {
        //player
        //データ生成
        mElement.mPlayerStatus = new PlayerStatus[mElement.mInitData.mPlayerDataList.Length];
        for (int i = 0; i < mElement.mInitData.mPlayerDataList.Length; i++) {
            mElement.mPlayerStatus[i] = new PlayerStatus();
            mElement.mPlayerStatus[i].mAnimalName = mElement.mInitData.mPlayerDataList[i].mAnimalName;
            mElement.mPlayerStatus[i].mPlayerNumber = i;
            mElement.mPlayerStatus[i].mController = mElement.mInitData.mPlayerDataList[i].mIsPlayer ? (GameController)new PlayerController() : (GameController)new AiController();
            mElement.mPlayerStatus[i].mRank = 1;
            mElement.mPlayerStatus[i].mLap = 0;
            mElement.mPlayerStatus[i].mReverseLap = 0;
            mElement.mPlayerStatus[i].mFood = mElement.mStageData.mInitialFood * mElement.mInitData.mInitialFood / 100;
            mElement.mPlayerStatus[i].mTerritory = 0;
            mElement.mPlayerStatus[i].mCurrentMasNumber = 0;
            mElement.mPlayerStatus[i].mTurn = i;
        }
        //データ表示
        for (int i = 0; i < mElement.mInitData.mPlayerDataList.Length; i++) {
            mElement.mPlayerStatusDisplay[i].initDisplay(mElement.mPlayerStatus[i]);
        }
        //mass
        //データ生成
        List<StageData.Mas> tMasDataList = mElement.mStageData.mMas;
        mElement.mMasStatus = new MasStatus[tMasDataList.Count];
        for (int i = 0; i < tMasDataList.Count; i++) {
            StageData.Mas tMasData = tMasDataList[i];
            MasStatus tMas = null;
            switch (tMasData.mType) {
                case MasStatus.MasType.land:
                    StageData.Land tLandData = tMasData as StageData.Land;
                    LandMasStatus tLand = new LandMasStatus();
                    tLand.mName = tLandData.mName;
                    tLand.mValue = tLandData.mValue;
                    tLand.mAttribute1 = tLandData.mAttribute1;
                    tLand.mAttribute2 = tLandData.mAttribute2;
                    tLand.mOwnerNumber = -1;
                    tLand.mExpansionLevel = 0;
                    tMas = tLand;
                    break;
                case MasStatus.MasType.start:
                    tMas = new StartMasStatus();
                    break;
                case MasStatus.MasType.accident:
                    AccidentMasStatus tAccident = new AccidentMasStatus();
                    tAccident.mAccidentKey = ((StageData.Accident)tMasData).mAccidentType;
                    tMas = tAccident;
                    break;
            }
            tMas.mType = tMasData.mType;
            tMas.mMasNumber = i;
            mElement.mMasDisplay[i].mEventListener.mMessage = "touchMas";
            mElement.mMasDisplay[i].mEventListener.mArg = new Arg(new Dictionary<string, object>() { { "number", i } });
            mElement.mMasStatus[i] = tMas;
        }
        //データ表示
        for (int i = 0; i < mElement.mMasStatus.Length; i++) {
            if (mElement.mMasStatus[i].mType == MasStatus.MasType.land) {
                ((LandMasDisplay)mElement.mMasDisplay[i]).initDisplay((LandMasStatus)mElement.mMasStatus[i]);
            }
        }
    }
}
