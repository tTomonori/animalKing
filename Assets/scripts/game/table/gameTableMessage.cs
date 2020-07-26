using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameTable : MyBehaviour {
    [SerializeField] public TextMesh tMessageText;
    public void showMessage(string aText) {
        tMessageText.text = aText;
        tMessageText.gameObject.SetActive(true);
    }
    public void hideMessage() {
        tMessageText.gameObject.SetActive(false);
    }
}
