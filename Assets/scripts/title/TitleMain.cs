using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMain : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Subject.addObserver(new Observer("titleMain", (a) => {
            if (a.name == "new") {
                MySceneManager.changeScene("gameConfig");
            }else if (a.name == "continue") {

            }
        }));
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnDestroy() {
        Subject.removeObserver("titleMain");
    }
}
