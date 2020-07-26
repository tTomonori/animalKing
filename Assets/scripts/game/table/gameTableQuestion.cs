using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GameTable {
    [SerializeField] public TableQuestionForm mQuestionForm;
    public void question(string aText) {
        mQuestionForm.mText.text = aText;
        mQuestionForm.gameObject.SetActive(true);
    }
    public void hideQuestionForm() {
        mQuestionForm.gameObject.SetActive(false);

        MyButton[] tButton = mQuestionForm.GetComponentsInChildren<MyButton>();
        for (int i = 0; i < 2; i++) {
            Transform tTrans = tButton[i].findChild<Transform>("buttonContents");
            if (tTrans != null) tTrans.localScale = new Vector3(1, 1, 1);
        }
    }
}
