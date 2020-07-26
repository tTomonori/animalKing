using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GazePourer : MyBehaviour{
    [SerializeField]
    //<summary>視線を送る対象</summary>
    public GameObject mTarget;
    //<summary>前後を反転する</summary>
    public bool mReverse = false;
}
