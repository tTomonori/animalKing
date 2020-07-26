using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageList : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Arg aData = new Arg(MyJson.deserializeResourse("stage/stage"));
        List<string> tStageList = aData.get<List<string>>("stageList");
        MyButton tPrefab = Resources.Load<MyButton>("prefab/selectStage/element");
        for (int i = 0; i < tStageList.Count; i++) {
            string tStagePath = tStageList[i];
            MyButton tButton = GameObject.Instantiate(tPrefab);
            tButton.mParameters = new Arg(new Dictionary<string, object> { { "filePath", tStagePath } });
            tButton.position = new Vector3(0, -i, -10);
            tButton.transform.SetParent(this.transform, false);
            StageData tStageData = new StageData(tStagePath);
            tButton.GetComponentInChildren<TextMesh>().text = tStageData.mName;
        }
    }

}
