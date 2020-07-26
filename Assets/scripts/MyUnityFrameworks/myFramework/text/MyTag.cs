using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <タグ名,引数1,引数2,...>
/// </summary>
public class MyTag {
    //<summary>タグの名前</summary>
    public string mTagName;
    //<summary>タグに含まれていた引数</summary>
    public string[] mArguments;

    public MyTag(string aTagText) {
        //タグ内の文字
        string tTagContents = aTagText.Substring(1, aTagText.Length - 2);
        //,で分割
        string[] tSplited = tTagContents.Split(',');
        mTagName = tSplited[0];
        mArguments = new string[tSplited.Length - 1];
        for (int i = 1; i < tSplited.Length; i++) {
            mArguments[i - 1] = tSplited[i];
        }
    }
}
