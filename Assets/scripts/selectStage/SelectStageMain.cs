using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageMain : MonoBehaviour {
    public string mStageName;
    public string mStageFilePath;
    [SerializeField] public StageView mStageView;
    // Start is called before the first frame update
    void Start() {
        string tPath= MySceneManager.getArg("selectStage").get<string>("path");
        mStageView.showStage(tPath);
        mStageName = mStageView.mStageData.mName;
        mStageFilePath = tPath;
        Subject.addObserver(new Observer("selectStageMain", (aMessage) => {
            if (aMessage.name == "ok") {
                Subject.removeObserver("selectStageMain");
                MySceneManager.closeScene("selectStage",
                 new Arg(new Dictionary<string, object> { { "stageName", mStageName }, { "stageFilePath", mStageFilePath } }), null);
            } else if (aMessage.name == "selectStageName") {
                string tFilePath = aMessage.parameters.get<string>("filePath");
                mStageView.showStage(tFilePath);
                mStageName = mStageView.mStageData.mName;
                mStageFilePath = tFilePath;
            }
        }));
    }
}
