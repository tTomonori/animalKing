using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageView : MyBehaviour {
    private MyBehaviour mMasContainer;
    private MyBehaviour mRouteContainer;
    public StageData mStageData;
    [SerializeField] private MyBehaviour mStage;

    public void showStage(string aFilePath) {
        if (mMasContainer != null) {
            mMasContainer.name = "deleted";
            mMasContainer.delete();
            mRouteContainer.name = "deleted";
            mRouteContainer.delete();
        }

        StageData tStageData = new StageData(aFilePath);
        mStageData = tStageData;
        GamePlacer.placeMas(tStageData);
        mMasContainer = GameObject.Find("masContainer").GetComponent<MyBehaviour>();
        mMasContainer.transform.SetParent(mStage.transform, false);
        mRouteContainer = GameObject.Find("routeContainer").GetComponent<MyBehaviour>();
        mRouteContainer.transform.SetParent(mStage.transform, false);
    }
}
