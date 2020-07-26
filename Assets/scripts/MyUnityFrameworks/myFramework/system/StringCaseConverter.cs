using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//参考 http://mag.autumn.org/Content.modf?id=20130930161420
public static class StringCaseConverter{
    //片仮名にする
    public static string ToKatakana(this string s){
        return new string(s.Select(c => (c >= 'ぁ' && c <= 'ゖ') ? (char)(c + 'ァ' - 'ぁ') : c).ToArray());
    }
    //平仮名にする
    public static string ToHiragana(this string s){
        return new string(s.Select(c => (c >= 'ァ' && c <= 'ヶ') ? (char)(c + 'ぁ' - 'ァ') : c).ToArray());
    }
    //大文字にする
    public static string ToUpper(this string s){
        return new string(s.Select(c => (c >= 'a' && c <= 'z') ? (char)(c + 'A' - 'a') : c).ToArray());
    }
    //小文字にする
    public static string ToLower(this string s){
        return new string(s.Select(c => (c >= 'A' && c <= 'Z') ? (char)(c + 'a' - 'A') : c).ToArray());
    }
}