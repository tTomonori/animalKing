using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetCheck : MyButton {
    [SerializeField] private bool _Checked;
    public bool mIsChecked {
        get { return _Checked; }
        set { _Checked = !value; pushed(); }
    }
    // Start is called before the first frame update
    void Start() {
        SpriteRenderer tRenderer = GetComponent<SpriteRenderer>();
        if (_Checked) {
            tRenderer.sprite = Resources.Load<Sprite>("image/ui/checked");
        } else {
            tRenderer.sprite = Resources.Load<Sprite>("image/ui/notChecked");
        }
    }

    // Update is called once per frame
    void Update() {

    }

    protected override void pushed() {
        _Checked = !_Checked;
        SpriteRenderer tRenderer = GetComponent<SpriteRenderer>();
        if (_Checked) {
            tRenderer.sprite = Resources.Load<Sprite>("image/ui/checked");
        } else {
            tRenderer.sprite = Resources.Load<Sprite>("image/ui/notChecked");
        }
    }
}
