using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/// <summary>
/// タグ入りの文字列を順に読む
/// 開始タグ : <タグ名,引数,引数,...>
/// 終了タグ : </タグ名>
/// タグ回避 : \X　(Xはタグに関係ない1文字として扱う)
/// (例) あい<test,1,ん>うえ</test>お
/// </summary>
public class TagReader {
    private StringReader mReader;
    //<summary>次の1文字</summary>
    public char mNext {
        get { return Convert.ToChar(mReader.Peek()); }
    }
    public TagReader(string aText) {
        mReader = new StringReader(aText);
    }
    //読む文字列を追加
    public void add(string aText) {
        mReader = new StringReader(mReader.ReadToEnd() + aText);
    }
    //次の1文字またはタグを読む
    public Element read() {
        //終了判定
        if (isEnd()) return new Empty();
        //次を読む
        char tNext = Convert.ToChar(mReader.Read());
        //タグ回避文字
        if (tNext == '\\') return new OneChar(Convert.ToChar(mReader.Read()).ToString());
        //文字1文字
        if (tNext != '<') return new OneChar(tNext.ToString());
        //終了タグ
        if (Convert.ToChar(mReader.Peek()) == '/') {
            mReader.Read();
            return new EndTag(readTo('>'));
        }
        //開始タグ
        //タグ内の文字
        string tTagContents = readTo('>');
        string[] tSplited = tTagContents.Split(',');
        string tTagName = tSplited[0];
        string[] tParams = new string[tSplited.Length - 1];
        for(int i = 1; i < tSplited.Length; i++) {
            tParams[i - 1] = tSplited[i];
        }
        return new StartTag(tTagName, tParams);
    }
    //指定した文字まで読み,指定した文字の前までの文字列を返す
    private string readTo(char c) {
        string tRes = "";
        char tNext;
        while (true) {
            tNext = Convert.ToChar(mReader.Read());
            if (tNext == c) break;
            tRes += tNext;
        }
        return tRes;
    }
    //全て読み終えているならtrue
    public bool isEnd() {
        return mReader.Peek() == -1;
    }


    //<summary>読んだ情報</summary>
    public class Element {}

    //<summary>文字1文字</summary>
    public class OneChar : Element {
        //<summary>1文字</summary>
        public string mChar;
        public OneChar(string aChar) {
            mChar = aChar;
        }
    }

    //<summary>開始タグ</summary>
    public class StartTag : Element {
        //<summary>タグの名前</summary>
        public string mTagName;
        //<summary>タグに含まれていた引数</summary>
        public string[] mArguments;
        public StartTag(string aTagName,string[] aArguments) {
            mTagName = aTagName;
            mArguments = aArguments;
        }
        //<summary>元の文字列</summary>
        public string mOriginalString {
            get {
                string tTag = "<" + mTagName;
                foreach(string tArgument in mArguments) {
                    tTag += "," + tArgument;
                }
                return tTag + ">";
            }
        }
    }

    //<summary>終了タグ</summary>
    public class EndTag : Element {
        //<summary>タグの名前</summary>
        public string mTagName;
        public EndTag(string aTagName) {
            mTagName = aTagName;
        }
        //<summary>元の文字列</summary>
        public string mOriginalString {
            get { return "</" + mTagName + ">"; }
        }
    }

    //<summary>読み込み失敗</summary>
    public class Empty : Element {}
}
