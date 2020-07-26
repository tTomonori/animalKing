using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyFlagItem {
    /// <summary>文字列に変換する</summary>
    public abstract string toString();
    /// <summary>文字列からFlagItemを生成</summary>
    static public MyFlagItem create(string a) {
        if (a == "") return null;
        if (a.StartsWith("<", System.StringComparison.Ordinal)) {
            MyTag tTag = new MyTag(a);
            if (tTag.mTagName == "and") {
                MyFlagAndItems andItem = new MyFlagAndItems();
                andItem.mItems = new MyFlagSingleItem[tTag.mArguments.Length];
                for (int i = 0; i < tTag.mArguments.Length; ++i) {
                    andItem.mItems[i] = (MyFlagSingleItem)create(tTag.mArguments[i]);
                }
                return andItem;
            } else if (tTag.mTagName == "or") {
                MyFlagOrItems orItem = new MyFlagOrItems();
                orItem.mItems = new MyFlagSingleItem[tTag.mArguments.Length];
                for (int i = 0; i < tTag.mArguments.Length; ++i) {
                    orItem.mItems[i] = (MyFlagSingleItem)create(tTag.mArguments[i]);
                }
                return orItem;
            } else {
                throw new System.Exception("MyFlagItem : FlagItemの生成失敗「" + a + "」");
            }
        } else {
            string[] tElements = a.Split(':');
            //bool
            if (tElements[1] == "true" || tElements[1] == "True") {
                MyFlagBoolItem bItem = new MyFlagBoolItem();
                bItem.mPath = tElements[0];
                bItem.mValue = true;
                return bItem;
            }
            if (tElements[1] == "false" || tElements[1] == "False") {
                MyFlagBoolItem bItem = new MyFlagBoolItem();
                bItem.mPath = tElements[0];
                bItem.mValue = false;
                return bItem;
            }
            //float
            if (tElements.Length == 3) {
                MyFlagFloatItem fItem = new MyFlagFloatItem();
                fItem.mPath = tElements[0];
                fItem.mValue = float.Parse(tElements[1]);
                fItem.mCompare = EnumParser.parse<MyFlagFloatItem.Compare>(tElements[2]);
                return fItem;
            }
            //string
            MyFlagStringItem sItem = new MyFlagStringItem();
            sItem.mPath = tElements[0];
            sItem.mValue = tElements[1];
            return sItem;
        }
    }
}
public abstract class MyFlagSingleItem : MyFlagItem {
    public string mPath;
    public MyFlagSingleItem copy() {
        return (MyFlagSingleItem)MemberwiseClone();
    }
}
public class MyFlagBoolItem : MyFlagSingleItem {
    public bool mValue;
    public override string toString() {
        if (mValue) return mPath + ":true";
        else return mPath + ":false";
    }
}
public class MyFlagStringItem : MyFlagSingleItem {
    public string mValue;
    public override string toString() {
        return mPath + ":" + mValue;
    }
}
public class MyFlagFloatItem : MyFlagSingleItem {
    public float mValue;
    /// <summary>フラグの値に対してこの値が</summary>
    public Compare mCompare;
    public enum Compare {
        equal, lessThan, moreThan, orLess, orMore
    }
    public override string toString() {
        return mPath + ":" + mValue.ToString() + ":" + mCompare.ToString();
    }
}
public class MyFlagAndItems : MyFlagItem {
    public MyFlagSingleItem[] mItems;
    public override string toString() {
        string s = "<and";
        foreach (MyFlagItem tItem in mItems) {
            s += "," + tItem.toString();
        }
        return s + ">";
    }
    public MyFlagAndItems copy() {
        MyFlagAndItems tAnd = new MyFlagAndItems();
        tAnd.mItems = new MyFlagSingleItem[mItems.Length];
        for (int i = 0; i < mItems.Length; ++i) {
            tAnd.mItems[i] = mItems[i].copy();
        }
        return tAnd;
    }
}
public class MyFlagOrItems : MyFlagItem {
    public MyFlagSingleItem[] mItems;
    public override string toString() {
        string s = "<or";
        foreach (MyFlagItem tItem in mItems) {
            s += "," + tItem.toString();
        }
        return s + ">";
    }
    public MyFlagOrItems copy() {
        MyFlagOrItems tOr = new MyFlagOrItems();
        tOr.mItems = new MyFlagSingleItem[mItems.Length];
        for (int i = 0; i < mItems.Length; ++i) {
            tOr.mItems[i] = mItems[i].copy();
        }
        return tOr;
    }
}


public partial class MyFlag {
    public bool check(MyFlagItem aItem) {
        switch (aItem) {
            case MyFlagBoolItem bItem:
                object bO = getFlag(bItem.mPath);
                if (bO is bool && (bool)bO == bItem.mValue)
                    return true;
                else
                    return false;
            case MyFlagStringItem sItem:
                object sO = getFlag(sItem.mPath);
                if (sO is string && (string)sO == sItem.mValue)
                    return true;
                else
                    return false;
            case MyFlagFloatItem fItem:
                object fO = getFlag(fItem.mPath);
                float f;
                if (fO is int) f = (int)fO;
                else if (fO is float) f = (float)fO;
                else return false;
                switch (fItem.mCompare) {
                    case MyFlagFloatItem.Compare.equal:
#pragma warning disable RECS0018 // 等値演算子による浮動小数点値の比較
                        return fItem.mValue == (float)f;
#pragma warning restore RECS0018 // 等値演算子による浮動小数点値の比較
                    case MyFlagFloatItem.Compare.lessThan:
                        return fItem.mValue < (float)f;
                    case MyFlagFloatItem.Compare.moreThan:
                        return fItem.mValue > (float)f;
                    case MyFlagFloatItem.Compare.orLess:
                        return fItem.mValue <= (float)f;
                    case MyFlagFloatItem.Compare.orMore:
                        return fItem.mValue >= (float)f;
                }
                throw new System.Exception("MyFlag : FloatItem未定義の比較手法「" + fItem.mCompare.ToString() + "」");
            case MyFlagAndItems andItem:
                foreach (MyFlagItem tItem in andItem.mItems) {
                    if (!check(tItem)) return false;
                }
                return true;
            case MyFlagOrItems orItem:
                foreach (MyFlagItem tItem in orItem.mItems) {
                    if (check(tItem)) return true;
                }
                return false;
        }
        throw new System.Exception("MyFlag : 未定義のフラグCheck「" + aItem.GetType().ToString() + "」");
    }
}