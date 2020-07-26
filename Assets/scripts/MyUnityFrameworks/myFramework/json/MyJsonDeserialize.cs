using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static partial class MyJson {
    /// <summary>
    /// ファイルを読んでdeserialize
    /// </summary>
    /// <returns>deserialize結果</returns>
    /// <param name="filePath">"Assets/../" + filePath (拡張子必須)</param>
    static public Dictionary<string, object> deserializeFile(string filePath) {
        string jsonString = File.ReadAllText(filePath);
        return deserialize(jsonString);
    }
    /// <summary>
    /// ファイルを読んでdeserialize
    /// </summary>
    /// <returns>deserialize結果</returns>
    /// <param name="filePath">"resources/" + filePath (拡張子不要)</param>
    static public Dictionary<string, object> deserializeResourse(string filePath) {
        string jsonString = ((TextAsset)Resources.Load(filePath)).text;
        return deserialize(jsonString);
    }
    /// <summary>
    /// 引数文字列をdeserialize
    /// </summary>
    /// <returns>deserialize結果</returns>
    /// <param name="jsonString">Json形式文字列</param>
    static public Dictionary<string, object> deserialize(string jsonString) {
        return new Parser().parse(jsonString);
    }
    private class Parser {
        private StringReader reader;
        ///以降の数文字を読む(エラー出力用)
        private string following {
            get { return new string(new char[] { nextSense(), nextSense(), nextSense(), nextSense(), nextSense() }); }
        }
        ///次の文字を読む
        private char nextChar {
            get {
                checkEnd();
                return Convert.ToChar(reader.Read());
            }
        }
        ///現在の位置の文字を読む
        private char currentChar {
            get { return Convert.ToChar(reader.Peek()); }
        }
        ///位置文字読み進める
        private void readOneChar() {
            checkEnd();
            reader.Read();
        }
        //最後まで読む
        private string readToEnd() {
            return reader.ReadToEnd();
        }
        ///ファイルが読み終わっているか判定する(読み終わっていたらエラーを吐く)
        private void checkEnd() { if (reader.Peek() == -1) throw new Exception("不正なjson文字列 : fraudulent file end"); }
        ///次の、意味を持つcharの位置まで読み進める
        private void readToNextSense() {
            while (true) {
                switch (currentChar) {
                    case ' ': readOneChar(); continue;
                    case '\t': readOneChar(); continue;
                    case '\n': readOneChar(); continue;
                    case '/': reader.ReadLine(); continue;
                    default:
                        if (reader.Peek() == 13) {//改行文字
                            readOneChar(); continue;
                        }
                        return;
                }
            }
        }
        ///次の、意味を持つcharを読む(readerの位置はその次)
        private char nextSense() {
            readToNextSense();
            return nextChar;
        }
        ///次の、意味を持つcharを返す(readerの位置はその文字)
        private char currenSense() {
            readToNextSense();
            return currentChar;
        }
        ///次の、指定した文字の前の位置まで読む(readerは指定した文字の位置)
        private string readToNextSpecificChar(char c) {
            string readed = "";
            while (true) {
                if (currentChar == c) return readed;
                readed += nextChar;
            }
        }
        ///指定した文字の位置まで読み進める(次の、意味を持つcharが指定した文字でないならエラーを吐く)(readerの位置はその文字)
        private void search(char c) {
            readToNextSense();
            char sense = currentChar;
            if (sense != c)
                throw new Exception("不正なjson文字列 : invalid char 「" + sense + "」, expected 「" + c + "」, following「"+following+"」");
        }
        private void search(string s) {
            readToNextSense();
            char sense = currentChar;
            for (int i = 0; i < s.Length; i++) {
                if (sense == s[i]) return;
            }
            throw new Exception("不正なjson文字列 : invalid char 「" + sense + "」, expected 「" + s + "」, following「"+following+"」");
        }
        ///文字列を読む
        private string readString() {
            search('"');
            readOneChar();
            string s = "";
            while (true) {
                switch (currentChar) {
                    case '\\':
                        readOneChar();
                        s += nextChar;
                        break;
                    case '"':
                        readOneChar();
                        return s;
                    default:
                        s += nextChar;
                        break;
                }
            }
        }
        ///数字を読む
        private object readNumber() {
            search("0123456789+-");
            bool signFlag = false;
            bool pointFlag = false;
            string s = "";
            while (true) {
                switch (currentChar) {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        s += nextChar;
                        break;
                    case '+':
                    case '-':
                        if (signFlag)
                            throw new Exception("不正なjson文字列 : invalid number include 「" + currentChar + "」");
                        s += nextChar;
                        break;
                    case '.':
                        if (pointFlag)
                            throw new Exception("不正なjson文字列 : invalid number include two「.」");
                        pointFlag = true;
                        s += nextChar;
                        break;
                    default:
                        if (pointFlag) {
                            float f;
                            float.TryParse(s, out f);
                            return f;
                        } else {
                            int i;
                            int.TryParse(s, out i);
                            return i;
                        }
                }
                signFlag = true;
            }
        }
        ///boolを読む
        private object readBool() {
            search("tfTF");
            switch (currentChar) {
                case 't':
                    if (nextChar != 't') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'r') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'u') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'e') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    return true;
                case 'f':
                    if (nextChar != 'f') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'a') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'l') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 's') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'e') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    return false;
                case 'T':
                    if (nextChar != 'T') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'r') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'u') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    if (nextChar != 'e') throw new Exception("不正なjson文字列 : invalid value start from 「t」");
                    return true;
                case 'F':
                    if (nextChar != 'F') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'a') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'l') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 's') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    if (nextChar != 'e') throw new Exception("不正なjson文字列 : invalid value start from 「f」");
                    return false;
                default:
                    throw new Exception("想定不能なエラー : 「" + currentChar + "」 != t nor f");
            }
        }
        ///Vectorを読む
        private object readVector() {
            readOneChar();//Vを読む
            if (nextChar != 'e') throw new Exception("不正なjson文字列 : invalid text start from 「V」");
            if (nextChar != 'c') throw new Exception("不正なjson文字列 : invalid text start from 「Ve」");
            if (nextChar != 't') throw new Exception("不正なjson文字列 : invalid text start from 「Vec」");
            if (nextChar != 'o') throw new Exception("不正なjson文字列 : invalid text start from 「Vect」");
            if (nextChar != 'r') throw new Exception("不正なjson文字列 : invalid text start from 「Vecto」");
            switch (currentChar) {
                case '2'://Vector2
                    readOneChar();//2を読む
                    if (nextChar != '(') throw new Exception("不正なVectorデータ : 「 ( 」 is not found, following「"+following+"」");
                    string tTwoNumberString = readToNextSpecificChar(')');
                    readOneChar();//)を読む
                    string[] tTwoNumber = tTwoNumberString.Split(',');
                    return new Vector2(float.Parse(tTwoNumber[0]), float.Parse(tTwoNumber[1]));
                case '3'://Vector3
                    readOneChar();//3を読む
                    if (nextChar != '(') throw new Exception("不正なVectorデータ : 「 ( 」 is not found, following「"+following+"」");
                    string tThreeNumberString = readToNextSpecificChar(')');
                    readOneChar();//)を読む
                    string[] tThreeNumber = tThreeNumberString.Split(',');
                    return new Vector3(float.Parse(tThreeNumber[0]), float.Parse(tThreeNumber[1]), float.Parse(tThreeNumber[2]));
                default:
                    throw new Exception("不正なjson文字列 : invalid text start from 「Vector" + currentChar.ToString() + "」, following「"+following+"」");
            }
        }
        ///Enumを読む
        private object readEnum() {
            search("<");
            readOneChar();//<を読む
            //Enumの型名
            string type = readToNextSpecificChar('>');
            readOneChar();//>を読む
            char next = nextChar;
            if (next != '(') {
                throw new Exception("不正なjson文字列 : invalid char 「" + next + "」, expected 「 ( 」, following「"+following+"」");
            }
            //Enumの値
            string value = readToNextSpecificChar(')');
            next = nextChar;
            if (next != ')') {
                throw new Exception("不正なjson文字列 : invalid char 「" + next + "」, expected 「 ) 」, following「"+following+"」");
            }
            return Enum.Parse(Type.GetType(type), value);
        }
        ///Listを読む
        private object readList() {
            search('[');
            readOneChar();
            if (currenSense() == ']') {
                readOneChar();
                return new List<object>();
            }

            object o = readValue();
            Type firstElementType = o.GetType();
            Type listType = Type.GetType("System.Collections.Generic.List`1[" + firstElementType.ToString() + "]");
            IList valueList = null;
            if (listType == null) {
                //Activatorで対応できない型への対応
                switch (firstElementType.ToString()) {
                    case "UnityEngine.Vector2":
                        valueList = new List<Vector2>();
                        break;
                    case "UnityEngine.Vector3":
                        valueList = new List<Vector3>();
                        break;
                }
            } else {
                valueList = (IList)Activator.CreateInstance(listType);
            }
            valueList.Add(o);

            object e;
            while (true) {
                switch (currenSense()) {
                    case ',':
                        readOneChar();
                        e = readValue();
                        if (e.GetType() != firstElementType) goto readObjectList;
                        valueList.Add(e);
                        break;
                    case ']':
                        readOneChar();
                        return valueList;
                    default:
                        throw new Exception("不正なjson文字列 : invalid list separator 「" + currentChar + "」, following「"+following+"」");
                }
            }
        readObjectList:
            //リスト内の型が統一されていない
            List<object> objectList = new List<object>();
            foreach (object v in valueList) {
                objectList.Add(v);
            }
            objectList.Add(e);
            while (true) {
                switch (currenSense()) {
                    case ',':
                        readOneChar();
                        e = readValue();
                        objectList.Add(e);
                        break;
                    case ']':
                        readOneChar();
                        return objectList;
                    default:
                        throw new Exception("不正なjson文字列 : invalid list separator 「" + currentChar + "」, following「"+following+"」");
                }
            }
        }
        ///Dictionary(string,object)を読む
        private object readDictionaryOfObject() {
            search('{');
            readOneChar();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (currenSense() == '}') {
                readOneChar();//}を読み飛ばす
                return dic;
            }
            while (true) {
                string key = readString();
                //:を読み飛ばす
                search(':');
                readOneChar();
                object value = readValue();
                dic[key] = value;
                if (currenSense() == ',') {
                    readOneChar();//,を読み飛ばす
                } else if (currenSense() == '}') {
                    readOneChar();//}を読み飛ばす
                    return dic;
                } else {
                    throw new Exception("不正なjson文字列 : not found value separator「" + currenSense() + "」, following「"+following+"」");
                }
            }
        }
        ///Dictionary(string,)を読む
        private object readDictionary() {
            search('{');
            readOneChar();
            if (currenSense() == '}') {
                readOneChar();
                return new Dictionary<string, object>();
            }
            string firstKey = readString();
            //:を読み飛ばす
            search(':');
            readOneChar();
            object firstValue = readValue();
            //Dictionary<string,T>生成
            Type firstValueType = firstValue.GetType();
            Type dicType = Type.GetType("System.Collections.Generic.Dictionary`2[System.String," + firstValueType.ToString() + "]");
            IDictionary valueDic = (IDictionary)Activator.CreateInstance(dicType);
            valueDic[firstKey] = firstValue;
            //Dictionary<string,object>生成
            Dictionary<string, object> objectDic = new Dictionary<string, object>();
            objectDic[firstKey] = firstValue;

            bool inconsistencyFlag = false;
            while (true) {
                if (currenSense() == ',') {
                    readOneChar();//,を読み飛ばす
                } else if (currenSense() == '}') {
                    readOneChar();//}を読み飛ばす
                    return (inconsistencyFlag) ? objectDic : valueDic;
                } else {
                    throw new Exception("不正なjson文字列 : not found value separator, instedad 「" + currenSense() + "」 was readed, following「"+following+"」");
                }
                string key = readString();
                //:を読み飛ばす
                search(':');
                readOneChar();
                object value = readValue();
                if (inconsistencyFlag) {
                    //valueの型が不統一
                    objectDic[key] = value;
                } else {
                    //valueの型が統一
                    if (value.GetType() == firstValueType) {
                        valueDic[key] = value;
                        objectDic[key] = value;
                    } else {
                        inconsistencyFlag = true;
                        objectDic[key] = value;
                    }
                }
            }
        }
        ///valueを取得
        private object readValue() {
            switch (currenSense()) {
                case '"'://文字列
                    return readString();
                case '{'://Dictionary
                    //return readDictionary();
                    return readDictionaryOfObject();
                case '['://配列
                    return readList();
                case 't'://bool
                case 'f':
                case 'T':
                case 'F':
                    return readBool();
                case '0'://数字
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '+':
                case '-':
                    return readNumber();
                case 'V'://Vector2 or Vector3
                    return readVector();
                case '<'://Enum
                    return readEnum();
                default://不正
                    throw new Exception("不正なjson文字列 : invalid value start from 「" + currenSense() + "」, following「"+following+"」");
            }
        }
        public Dictionary<string, object> parse(string jsonString) {
            reader = new StringReader(jsonString);
            return (Dictionary<string, object>)readDictionaryOfObject();
        }
    }
}
