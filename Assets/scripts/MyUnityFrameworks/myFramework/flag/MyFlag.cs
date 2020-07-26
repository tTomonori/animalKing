using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MyFlag {
    private Dictionary<string, object> mFlags;
    /// <summary>
    /// ファイルを読み込む
    /// </summary>
    /// <returns>読み込んだflag</returns>
    /// <param name="aFilePath">"Assets/../" + filePath (拡張子必須)</param>
    static public MyFlag load(string aFilePath) {
        MyFlag tFlag = new MyFlag();
        tFlag.mFlags = MyJson.deserializeFile(aFilePath);
        return tFlag;
    }
    /// <summary>
    /// シリアライズしてファイルに保存
    /// </summary>
    /// <param name="aFilePath">Assets/../ + filePath (必要であれば拡張子も入力する)</param>
    /// <param name="aLineFeedCode">trueならjson文字列中に改行を自動で挿入する</param>
    public void write(string aFilePath, bool aLineFeedCode = true) {
        MyJson.serializeToFile(mFlags, aFilePath, true);
    }
    public MyFlag() {
        mFlags = new Dictionary<string, object>();
    }
    private void setFlag(string aPath, object aValue) {
        string[] tPath = toPath(aPath);
        Dictionary<string, object> tDic = mFlags;
        int tLength = tPath.Length;
        for (int i = 0; i < tLength - 1; ++i) {
            string key = tPath[i];
            if (!tDic.ContainsKey(key)) {
                tDic[key] = new Dictionary<string, object>();
            } else {
                if (!(tDic[key] is IDictionary)) {
                    Debug.LogWarning("MyFlag : SET:ValueをDirectoryで上書きしました Path:" + toString(tPath, i));
                    tDic[key] = new Dictionary<string, object>();
                }
            }
            tDic = (Dictionary<string, object>)tDic[key];
        }
        tDic[tPath[tLength - 1]] = aValue;
    }
    private object getFlag(string aPath) {
        string[] tPath = toPath(aPath);
        Dictionary<string, object> tDic = mFlags;
        int tLength = tPath.Length;
        for (int i = 0; i < tLength - 1; ++i) {
            string key = tPath[i];
            if (!tDic.ContainsKey(key)) {
                return null;
            } else {
                if (!(tDic[key] is IDictionary)) {
                    Debug.LogWarning("MyFlag : GET:Pathの途中にValueが存在しました Path:" + toString(tPath, i));
                    return null;
                }
            }
            tDic = (Dictionary<string, object>)tDic[key];
        }
        if (!tDic.ContainsKey(tPath[tLength - 1]))
            return null;
        return tDic[tPath[tLength - 1]];
    }
    public string[] toPath(string aPath) {
        string[] tSplit = aPath.Split('/');
        string[] tPath = new string[tSplit.Length];
        int tCount = 0;
        foreach (string key in tSplit) {
            if (key == "..") {
                if (tCount == 0) continue;
                --tCount;
                continue;
            }
            tPath[tCount] = key;
            ++tCount;
        }
        Array.Resize<string>(ref tPath, tCount);
        return tPath;
    }
    public string toString(string[] aPath, int aIndex) {
        string s = aPath[0];
        for (int i = 1; i <= aIndex; ++i) {
            s += "/";
            s += aPath[i];
        }
        return s;
    }
    //<summary>フラグセット</summary>
    public void set(string aFlagName, bool aValue) {
        setFlag(aFlagName, aValue);
    }
    //<summary>フラグセット</summary>
    public void set(string aFlagName, string aValue) {
        setFlag(aFlagName, aValue);
    }
    //<summary>フラグセット 注)stringとして保存</summary>
    public void set(string aFlagName, DateTime aValue) {
        setFlag(aFlagName, aValue.Ticks.ToString());
    }
    //<summary>フラグセット</summary>
    public void set(string aFlagName, float aValue) {
        setFlag(aFlagName, aValue);
    }
    /// <summary>フラグ取得(null → false, bool以外 → true)</summary>
    public bool getBool(string aPath) {
        object o = getFlag(aPath);
        if (o == null) return false;
        else if (o is bool) return (bool)o;
        return true;
    }
    /// <summary>フラグ取得(null → "", string以外 → ToString())</summary>
    public string getString(string aPath) {
        object o = getFlag(aPath);
        if (o == null) return "";
        else if (o is string) return (string)o;
        return o.ToString();
    }
    /// <summary>フラグ取得(DateTime以外 → DateTime.MinValue)</summary>
    public DateTime getDateTime(string aPath) {
        object o = getFlag(aPath);
        if (o is string) {
            long l;
            if (long.TryParse((string)o, out l)) {
                return new DateTime(l);
            }
        }
        return DateTime.MinValue;
    }
    /// <summary>フラグ取得(null → 0, string以外 → 0)</summary>
    public float getFloat(string aPath) {
        object o = getFlag(aPath);
        if (o == null) return 0;
        else if (o is float) return (float)o;
        else if (o is int) return (int)o;
        return 0;
    }
    //<summary>フラグ削除</summary>
    public void delete(string aPath) {
        string[] tPath = toPath(aPath);
        Dictionary<string, object> tDic = mFlags;
        int tLength = tPath.Length;
        for (int i = 0; i < tLength - 1; ++i) {
            string key = tPath[i];
            if (!tDic.ContainsKey(key)) {
                return;
            } else {
                if (!(tDic[key] is IDictionary)) {
                    Debug.LogWarning("MyFlag : DELETE:Pathの途中にValueが存在しました Path:" + toString(tPath, i));
                    tDic.Remove(key);
                    return;
                }
            }
            tDic = (Dictionary<string, object>)tDic[key];
        }
        tDic.Remove(tPath[tLength - 1]);
    }
}
