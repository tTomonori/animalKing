using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyCommand{
    /// <summary>
    /// 操作を実行
    /// </summary>
    abstract public void run();
    /// <summary>
    /// 操作を取り消す
    /// </summary>
    abstract public void undo();
}
