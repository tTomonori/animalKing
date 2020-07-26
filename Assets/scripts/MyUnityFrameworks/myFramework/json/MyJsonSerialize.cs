using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static partial class MyJson {
    /// <summary>
    /// シリアライズしてファイルに保存
    /// </summary>
    /// <param name="data">jsonに変換するデータ</param>
    /// <param name="filePath">Assets/../ + filePath (必要であれば拡張子も入力する)</param>
    /// <param name="lineFeedCode">trueならjson文字列中に改行を自動で挿入する</param>
    static public void serializeToFile(IDictionary data, string filePath, bool lineFeedCode = false) {
        string tString = serialize(data, lineFeedCode);
        File.WriteAllText(filePath, tString);
    }
    //シリアライズ
    static public string serialize(IDictionary data, bool lineFeedCode = false) {
        return new Serializer().serialize(data, lineFeedCode);
    }
    private class Serializer {
        private bool mLienFeedCode;
        private int mNest = 0;
        public string serialize(IDictionary data, bool lineFeedCode) {
            mLienFeedCode = lineFeedCode;
            string tOut;
            if (dictionaryToString(data, out tOut, true))
                return tOut;
            else
                return "";
        }
        //改行を入れる状態なら改行文字を返す
        private string newLineChar(bool aSecondFlag = true) {
            if (mLienFeedCode && aSecondFlag)
                return "\n";
            return "";
        }
        //ネスト分だけスペースを返す
        private string getNext(bool aSecondFlag = true, int aNestOffset = 0) {
            if (mLienFeedCode && aSecondFlag) {
                string tNest = "";
                for (int i = 0; i < mNest + aNestOffset; ++i) {
                    tNest += "  ";
                }
                return tNest;
            }
            return "";
        }
        //改行を入れる状態なら１文字取り消す
        private string backLine(string s, bool aSecondFlag = true) {
            if (mLienFeedCode && aSecondFlag)
                return s.Remove(s.Length - 1);
            return s;
        }
        //改行を入れる状態ならネストを取り消す
        private string backNest(string s, bool aSecondFlag = true, int aNestOffset = 0) {
            if (mLienFeedCode && aSecondFlag)
                return s.Remove(s.Length - 2 * (mNest + aNestOffset));
            return s;
        }
        //１文字取り消す
        private string back(string s) {
            return s.Remove(s.Length - 1);
        }
        //dictionaryをstringに
        private bool dictionaryToString(IDictionary aDic, out string oOut, bool aLineFeedCode) {
            ++mNest;
            string tOut = "";
            //一つ以上要素を書き込めたか
            bool tWritten = false;
            //不正な要素があったか
            bool tContainsError = false;
            foreach (DictionaryEntry tEntry in aDic) {
                //key書き出し
                string tKeyString;
                if (!toString(tEntry.Key, out tKeyString, aLineFeedCode)) {
                    tContainsError = true;
                    continue;
                }
                //value書き出し
                string tValueString;
                if (!toString(tEntry.Value, out tValueString, aLineFeedCode)) {
                    tContainsError = true;
                    continue;
                }
                //keyもvalueも書き込める
                tOut += tKeyString;
                tOut += ":";
                tOut += tValueString;
                tOut += ",";
                tOut += newLineChar(aLineFeedCode);
                tOut += getNext(aLineFeedCode, 0);
                tWritten = true;
            }
            if (!tWritten && tContainsError) {//一つも要素がない かつ 不正な要素があった
                oOut = "";
                return false;
            }
            //一つ以上要素を書き込める
            if (tWritten) {
                tOut = backNest(tOut, aLineFeedCode);//ネスト削除
                tOut = backLine(tOut, aLineFeedCode);//改行削除
                tOut = back(tOut);//コンマ削除
            }

            oOut = "{" + newLineChar(aLineFeedCode) + getNext(aLineFeedCode, 0) + tOut + newLineChar(aLineFeedCode) + getNext(aLineFeedCode, -1) + "}";
            --mNest;
            return true;
        }
        //listをstringに
        private bool listToString(IList aList, out string oOut) {
            ++mNest;
            string tOut = "";
            //一つ以上要素を書き込めたか
            bool tWritten = false;
            //不正な要素があったか
            bool tContainsError = false;
            bool tIsMass = false;//要素がListかDictionaryの時のみ改行を入れる
            foreach (object tObject in aList) {
                string tElement;
                if (!toString(tObject, out tElement, false)) {
                    tContainsError = true;
                    continue;
                }
                tIsMass = (tObject is IDictionary) || (tObject is IList) || (tObject is Arg);
                tOut += tElement;
                tOut += ",";
                tOut += newLineChar(tIsMass);
                tOut += getNext(tIsMass, 0);
                tWritten = true;
            }
            if (!tWritten && tContainsError) {//一つも要素がない かつ 不正な要素があった
                oOut = "";
                return false;
            }
            //一つ以上要素を書き込める
            if (tWritten) {
                tOut = backNest(tOut, tIsMass);//ネスト削除
                tOut = backLine(tOut, tIsMass);//改行削除
                tOut = back(tOut);//コンマ削除
            }
            bool tFirstIsMass = tWritten && ((aList[0] is IDictionary) || (aList[0] is IList) || (aList[0] is Arg));//先頭の要素がListかDictionaryの時のみ改行を入れる
            oOut = "[" + newLineChar(tFirstIsMass) + getNext(tFirstIsMass, 0) + tOut + newLineChar(tIsMass) + getNext(tIsMass, -1) + "]";
            --mNest;
            return true;
        }
        //型を見てStringにする
        private bool toString(object aObject, out string oOut, bool aSecondFlag) {
            if (aObject is string) {
                //「"」は「\"」に置換する
                oOut = '"' + (string)((string)aObject).Replace("\"", "\\\"") + '"';
                return true;
            } else if (aObject is float) {
                oOut = ((float)aObject).ToString();
                return true;
            } else if (aObject is double) {
                oOut = ((double)aObject).ToString();
                return true;
            } else if (aObject is int) {
                oOut = ((int)aObject).ToString();
                return true;
            } else if (aObject is bool) {
                oOut = ((bool)aObject).ToString().ToLower();
                return true;
            } else if (aObject is Vector2) {
                oOut = "Vector2(" + ((Vector2)aObject).x.ToString() + "," + ((Vector2)aObject).y.ToString() + ")";
                return true;
            } else if (aObject is Vector3) {
                oOut = "Vector3(" + ((Vector3)aObject).x.ToString() + "," + ((Vector3)aObject).y.ToString() + "," + ((Vector3)aObject).z.ToString() + ")";
                return true;
            } else if (aObject is IDictionary) {
                return dictionaryToString((IDictionary)aObject, out oOut, aSecondFlag);
            } else if (aObject is IList) {
                return listToString((IList)aObject, out oOut);
            } else if (aObject is Enum) {
                oOut = "<" + aObject.GetType().ToString() + ">(" + ((Enum)aObject).ToString() + ")";
                return true;
            } else if (aObject is Arg) {
                return dictionaryToString(((Arg)aObject).dictionary, out oOut, aSecondFlag);
            } else if (aObject == null) {
                Debug.LogWarning("MyJsonSerialize : Nullが含まれている");
                oOut = "";
                return false;
            } else {
                Debug.LogWarning("MyJsonSerialize : サポートしていない型 「" + aObject.GetType() + "」");
                oOut = "";
                return false;
            }
        }
    }
}