using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyScrollViewElementDataList {
    protected IList mDataList;
    public int length{
        get { return mDataList.Count; }
    }
    public int lastNum{
        get { return mDataList.Count - 1; }
    }
    public abstract MyScrollViewElement createElement(int aIndex);
    public virtual bool sort(int aIndex1,int aIndex2){
        object tData = mDataList[aIndex1];
        mDataList[aIndex1] = mDataList[aIndex2];
        mDataList[aIndex2] = tData;
        return true;
    }
    /// <summary>
    /// 指定した番号の要素が並び替え可能か
    /// </summary>
    /// <returns>並び替え可能ならtrue</returns>
    /// <param name="aIndex">要素のindex</param>
    public virtual bool isCanSort(int aIndex){
        return true;
    }
    public virtual void tapped(int aIndex){
        
    }
    public virtual void doubleTapped(int aIndex){
        
    }
}
