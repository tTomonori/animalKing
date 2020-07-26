using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//new Arg(new Dictionary<string, object>() { { "key", "value" } })
public class Arg {
    private IDictionary arg;
    public IDictionary dictionary {
        get { return arg; }
    }
    public Arg(IDictionary a) {
        arg = a;
    }
    public Arg(object a = null) {
        arg = new Dictionary<string, object>();
    }
    static public Arg loadJson(string fileName) {
        return new Arg(MyJson.deserializeFile(fileName));
    }
    public T get<T>(string key) {
        if (arg[key] is T)
            return (T)arg[key];
        if (!(this is T)) {
            if ((float)1 is T) {
                object o = (float)(int)arg[key];
                return (T)o;
            } else if ((int)1 is T) {
                object o = (int)(float)arg[key];
                Debug.LogWarning("Arg : float型の値をint型に変換しちゃってるけどいいの？");
                return (T)o;
            } else if (new List<Arg>() is T) {
                List<Arg> o = new List<Arg>();
                foreach (object d in (IList)arg[key]) {
                    o.Add(new Arg((IDictionary)d));
                }
                return (T)(object)o;
            }
            if (arg[key] == null)
                throw new Exception("key:「" + key + "」はNullだよ");
            throw new Exception("key:「" + key + "」" + arg[key].GetType().ToString() + "型を指定した型にキャストできないよ");
        }
        Arg a = new Arg((IDictionary)arg[key]);
        //arg[key] = a;
        //return (T)arg[key];
        return (T)(object)a;
    }
    public bool ContainsKey(string key) {
        return arg.Contains(key);
        //return arg.ContainsKey(key);
    }
    public void set(string key, object value) {
        arg[key] = value;
    }
    public void remove(object key) {
        dictionary.Remove(key);
    }
}