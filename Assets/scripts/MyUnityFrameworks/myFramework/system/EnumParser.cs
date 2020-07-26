using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

static public class EnumParser {
    /// <summary>
    /// 文字列をEnumの値に変換する
    /// </summary>
    /// <returns>変換結果</returns>
    /// <param name="aString">変換する文字列</param>
    /// <typeparam name="T">Enumの型</typeparam>
    static public T parse<T>(string aString){
        return (T)Enum.Parse(typeof(T), aString, true);
    }
}
