using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEventListener : MyBehaviour {
    [SerializeField] public string mMessage;
    public Arg mArg = null;
    private void OnMouseUpAsButton() {
        Subject.sendMessage(new Message(mMessage, mArg));
    }
}
