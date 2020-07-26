using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class DragMonitor : MyPad {
    private void Update(){
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) mouseDown();
        else if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) mouseDrag();
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) mouseUp();
    }
}
