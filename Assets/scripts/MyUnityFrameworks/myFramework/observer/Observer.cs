using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SendMessage(Message message);
public class Observer {
    ///オブザーバ名
    private string mName;
    ///オブザーバが所属するグループ
    private string[] mGroup;
    ///メッセージの送信先
    private SendMessage mReceive;
    ///オブザーバ名
    public string name{
        get { return mName; }
    }
    public Observer(string aName, SendMessage aReceive, string aGroup = null){
        mName = aName;
        mGroup = (aGroup == null) ? new string[0] : new string[1] { aGroup };
        mReceive = aReceive;
    }
    public Observer(string aName, SendMessage aReceive, string[] aGroup){
        mName = aName;
        mGroup = aGroup;
        mReceive = aReceive;
    }
    ///指定したグループに含まれているか
    public bool isMemberOf(string aGroupName){
        foreach(string tGroup in mGroup){
            if (tGroup == aGroupName)
                return true;
        }
        return false;
    }
    ///メッセージ送信
    public void send(Message aMessage){
        mReceive(aMessage);
    }
}
