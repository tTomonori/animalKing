using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool FilterMessage(Message message);
public class MessageFilter {
    ///フィルタ名
    private string mName;
    ///フィルタが所属するグループ
    private string[] mGroup;
    ///フィルタ関数
    private FilterMessage mThrough;
    ///フィルタ名
    public string name{
        get { return mName; }
    }
    public MessageFilter(string aName, FilterMessage aFilter, string aGroup = null){
        mName = aName;
        mGroup = (aGroup == null) ? new string[0] : new string[1] { aGroup };
        mThrough = aFilter;
    }
    public MessageFilter(string aName, FilterMessage aFilter, string[] aGroup){
        mName = aName;
        mGroup = aGroup;
        mThrough = aFilter;
    }
    ///指定したグループに含まれているか
    public bool isMemberOf(string aGroupName){
        foreach (string tGroup in mGroup){
            if (tGroup == aGroupName)
                return true;
        }
        return false;
    }
    ///フィルタをかける
    public bool through(Message aMessage){
        return mThrough(aMessage);
    }
}
