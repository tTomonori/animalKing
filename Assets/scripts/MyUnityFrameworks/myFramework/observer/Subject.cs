using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Subject {
    ///登録されたオブザーバ
    static private List<Observer> mObservers = new List<Observer>();
    ///登録されたフィルタ
    static private List<MessageFilter> mFilters = new List<MessageFilter>();
    ///オブザーバを登録
    static public void addObserver(Observer aObserver){
        mObservers.Add(aObserver);
    }
    ///オブザーバを削除
    static public void removeObserver(string aName){
        for (int i = 0; i < mObservers.Count;i++){
            Observer tObserver = mObservers[i];
            if (tObserver.name != aName)
                continue;
            mObservers.RemoveAt(i);
            return;
        }
    }
    ///フィルタを登録
    static public void addFilter(MessageFilter aFilter){
        mFilters.Add(aFilter);
    }
    ///フィルタを削除
    static public void removeFilter(string aName){
        for (int i = 0; i < mFilters.Count;i++){
            MessageFilter tFilter = mFilters[i];
            if (tFilter.name != aName)
                continue;
            mFilters.RemoveAt(i);
            return;
        }
    }
    ///指定したグループのオブザーバを全て削除
    static public void removeObserverGroup(string aGroupName){
        foreach(Observer tObserver in mObservers){
            if(tObserver.isMemberOf(aGroupName))
                mObservers.Remove(tObserver);
        }
    }
    ///メッセージ送信
    static public void sendMessage(Message aMessage){
        if (!filter(aMessage))
            return;
        sendToObservers(aMessage);
    }
    ///フィルタを通す
    static private bool filter(Message aMessage){
        foreach(MessageFilter tFilter in mFilters){
            if (!tFilter.through(aMessage))
                return false;
        }
        return true;
    }
    ///全てのオブザーバにメッセージ送信
    static private void sendToObservers(Message aMessage){
        List<Observer> tObservers = new List<Observer>(mObservers);
        foreach(Observer tObserver in tObservers){
            tObserver.send(aMessage);
        }
    }
}
