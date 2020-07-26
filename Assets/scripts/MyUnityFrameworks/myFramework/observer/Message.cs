using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message {
    ///メッセージ名
    private string mName;
    ///メッセージが所属するグループ
    private string[] mGroup;
    ///メッセージが持つ変数
    private Arg mParameters;
    ///メッセージ名
    public string name{
        get { return mName; }
    }
    ///メッセージがもつパラメータ
    public Arg parameters{
        get { return mParameters; }
    }
    public Message(string aName, Arg aParameters, string aGroup=null){
        mName = aName;
        mGroup = (aGroup==null) ? new string[0] : new string[1] { aGroup };
        mParameters = aParameters;
    }
    public Message(string aName,Arg aParameters, string[] aGroup){
        mName = aName;
        mGroup = aGroup;
        mParameters = aParameters;
    }
    ///指定したグループに含まれているか
    public bool isMemberOf(string aGroupName){
        foreach(string tGroupName in mGroup){
            if (tGroupName == aGroupName)
                return true;
        }
        return false;
    }
    ///パラメータを受け取る
    public T getParameter<T>(string aKey){
        return mParameters.get<T>(aKey);
    }
}
