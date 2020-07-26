using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyListButton : MyButton {
    /// <summary>選択肢のリスト</summary>
    [SerializeField] private string[] mSelections;
    /// <summary>選択されている選択肢の文字列</summary>
    public string mSelected{
        get { return mSelections[mSelectionIndex]; }
    }
    /// <summary>選択されている選択肢の番号</summary>
    private int mSelectionIndex = 0;
    /// <summary>選択肢を表示するText</summary>
    [SerializeField] private TextMesh mText;

    private void Awake(){
        if (mSelections.Length == 0)
            mSelections = new string[1] { "null" }; 
    }
    private void Start(){
        //初期表示の文字列が選択肢に含まれているか調べる
        for(int i = 0; i < mSelections.Length; i++) {
            if (mText.text == mSelections[i]) {
                mSelectionIndex = i;
                break;
            }
        }
        mText.text = mSelections[mSelectionIndex];
    }
    /// <summary>指定した選択肢を選択している状態にする</summary>
    public void select(string aChoice) {
        for (int i = 0; i < mSelections.Length; i++) {
            if (mSelections[i] != aChoice) continue;
            mSelectionIndex = i;
            mText.text = mSelections[mSelectionIndex];
        }
    }


    protected override void pushed(){
        mSelectionIndex = (mSelectionIndex + 1) % mSelections.Length;
        mText.text = mSelections[mSelectionIndex];
        mParameters.set("selected", mSelections[mSelectionIndex]);
    }
}
