using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NumberCard : MyBehaviour {
    private bool _IsFront = false;
    public bool mIsFront { get { return _IsFront; } }
    [SerializeField] public SpriteRenderer mCard;
    [SerializeField] public SpriteRenderer mPattern;
    [SerializeField] public TextMesh mNumber;
    public int mParsedNumber { get { return int.Parse(mNumber.text); } }
    public void showFront() {
        mPattern.gameObject.SetActive(false);
        mNumber.gameObject.SetActive(true);
        _IsFront = true;
    }
    public void showBack() {
        mPattern.gameObject.SetActive(true);
        mNumber.gameObject.SetActive(false);
        _IsFront = false;
    }
    public void flip(bool aToFront, Action aCallback = null) {
        MySoundPlayer.playSe("dealing_cards1", true);
        _IsFront = aToFront;
        this.scaleToWithSpeed(new Vector2(0, 1), 5, () => {
            if (aToFront) showFront();
            else showBack();
            this.scaleToWithSpeed(new Vector2(1, 1), 5, () => {
                if (aCallback != null)
                    aCallback();
            });
        });
    }
    public void flipAndExpand(Action aCallback = null) {
        CallbackSystem tSystem = new CallbackSystem();
        Action tCounter = tSystem.getCounter();
        this.opacityBy(-0.9f, 1f, tSystem.getCounter());
        this.scaleToWithSpeed(new Vector2(0, 1.5f), 3, () => {
            mNumber.text = "";
            showFront();
            this.scaleToWithSpeed(new Vector2(2, 2), 3, () => {
                tCounter();
            });
        });
        tSystem.then(() => { if (aCallback != null) aCallback(); });
    }
}
