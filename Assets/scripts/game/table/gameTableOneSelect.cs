using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameTable : MyBehaviour {
    [SerializeField] private TableOneSelectForm mOneSelectForm;
    public void showOneSelectForm(string aText, string aButtonText) {
        mOneSelectForm.mText.text = aText;
        mOneSelectForm.mButtonText.text = aButtonText;
        mOneSelectForm.gameObject.SetActive(true);
    }
    public void hideOneSelectForm() {
        mOneSelectForm.gameObject.SetActive(false);

        MyButton[] tButton = mQuestionForm.GetComponentsInChildren<MyButton>();
        for (int i = 0; i < 2; i++) {
            Transform tTrans = tButton[i].findChild<Transform>("buttonContents");
            if (tTrans != null) tTrans.localScale = new Vector3(1, 1, 1);
        }
    }
}
