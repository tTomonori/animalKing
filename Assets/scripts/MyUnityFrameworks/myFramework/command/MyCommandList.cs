using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCommandList{
    //操作のリスト
    private List<MyCommand> mList;
    //操作リストの現在位置
    private int mIndex;
    public MyCommandList(){
        reset();
    }
    /// <summary>
    /// 履歴を削除
    /// </summary>
    public void reset(){
        mList = new List<MyCommand>();
        mIndex = -1;
    }
    /// <summary>
    /// Commandを実行
    /// </summary>
    /// <param name="aCommand">実行するコマンド</param>
    public void run(MyCommand aCommand){
        if(mIndex+1 != mList.Count)//取り消した操作あり
            mList.RemoveRange(mIndex + 1, mList.Count - mIndex - 1);//取り消したcommandを全て破棄

        mList.Add(aCommand);
        mIndex++;
        aCommand.run();
    }
    /// <summary>
    /// Commandを一つ取り消す
    /// </summary>
    public void undo(){
        if (mIndex < 0) return;
        mList[mIndex].undo();
        mIndex--;
    }
    /// <summary>
    /// 取り消したCommandを一つ実行し直す
    /// </summary>
    public void redo(){
        if (mIndex + 1 == mList.Count) return;
        mIndex++;
        mList[mIndex].run();
    }
}
