using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigMain : MonoBehaviour {
    private string mStagePath;
    [SerializeField] public MyBehaviour[] mPlayers;
    [SerializeField] public MyListButton mMeetList;
    [SerializeField] public MyListButton mLootingList;
    [SerializeField] public MeetCheck mAcquisitionCheck;
    // Start is called before the first frame update
    void Start() {
        mStagePath = "standard";
        Subject.addObserver(new Observer("gameConfigMain", (a) => {
            if (a.name == "start") {
                GameInitData tData = getGameData();
                MySceneManager.changeScene("game", new Arg(new Dictionary<string, object> { { "data", tData } }));
            } else if (a.name == "back") {
                MySceneManager.changeScene("title");
            } else if (a.name == "selectStage") {
                MySceneManager.openScene("selectStage", new Arg(new Dictionary<string, object> { { "path", mStagePath } }), null, (aArg) => {
                    TextMesh tText = GameObject.Find("stageName").GetComponent<TextMesh>();
                    tText.text = aArg.get<string>("stageName");
                    mStagePath = aArg.get<string>("stageFilePath");
                });
            }
        }));
    }

    // Update is called once per frame
    void Update() {

    }

    private GameInitData getGameData() {
        GameInitData tData = new GameInitData();
        tData.mPlayerDataList = new GameInitData.PlayerData[4];
        for (int i = 0; i < 4; i++) {
            GameInitData.PlayerData tPlayer = new GameInitData.PlayerData();
            tPlayer.mAnimalName = mPlayers[i].GetComponentInChildren<FaceButton>().getAnimalName();
            tPlayer.mIsPlayer = mPlayers[i].findChild<MeetCheck>("playerCheck").mIsChecked;
            tData.mPlayerDataList[i] = tPlayer;
        }
        tData.mInitialFood = int.Parse(mMeetList.mSelected.Substring(0, mMeetList.mSelected.Length - 1));
        tData.mLooting = int.Parse(mLootingList.mSelected.Substring(0, mLootingList.mSelected.Length - 1));
        tData.mAcquisition = mAcquisitionCheck;
        tData.mStagePath = mStagePath;
        return tData;
    }

    private void OnDestroy() {
        Subject.removeObserver("gameConfigMain");
    }
}
