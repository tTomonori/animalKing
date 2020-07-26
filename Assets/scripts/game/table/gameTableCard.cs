using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class GameTable {
    public NumberCard mCard1;
    public NumberCard mCard2;
    public NumberCard mCard3;
    [SerializeField] public TextMesh mText;
    public int[] mCard1Numbers { get { return new int[3] { 1, 3, 5 }; } }
    public int[] mCard2Numbers { get { return new int[4] { 0, 2, 4, 6 }; } }
    public int[] mCard3Numbers { get { return new int[2] { 0, 1 }; } }
    public int mRandomNumber1 { get { return mCard1Numbers[UnityEngine.Random.Range((int)0, (int)3)]; } }
    public int mRandomNumber2 { get { return mCard2Numbers[UnityEngine.Random.Range((int)0, (int)4)]; } }
    public int mRandomNumber3 { get { return mCard3Numbers[UnityEngine.Random.Range((int)0, (int)2)]; } }
    public int mTotalNumber { get { return int.Parse(mText.text); } }

    public void setNumberCards(Color aColor) {
        mText.text = "0";
        NumberCard tPrefab = Resources.Load<NumberCard>("prefab/game/numberCard");
        mCard1 = GameObject.Instantiate<NumberCard>(tPrefab);
        mCard2 = GameObject.Instantiate<NumberCard>(tPrefab);
        mCard3 = GameObject.Instantiate<NumberCard>(tPrefab);
        mCard1.mPattern.color = aColor;
        mCard2.mPattern.color = aColor;
        mCard3.mPattern.color = aColor;
        mCard1.mPattern.sprite = Resources.Load<Sprite>("image/card/card_meet");
        mCard2.mPattern.sprite = Resources.Load<Sprite>("image/card/card_fruits");
        mCard3.mPattern.sprite = Resources.Load<Sprite>("image/card/card_fish");
        mCard1.transform.SetParent(this.transform, false);
        mCard2.transform.SetParent(this.transform, false);
        mCard3.transform.SetParent(this.transform, false);
        mCard1.position = new Vector3(-1.5f, 0, 0);
        mCard2.position = new Vector3(0, 0, 0);
        mCard3.position = new Vector3(1.5f, 0, 0);
        mCard1.showBack();
        mCard2.showBack();
        mCard3.showBack();
    }
    public void removeNumberCards() {
        mCard1.delete();
        mCard2.delete();
        mCard3.delete();
    }
    public void setStarCard(Color aColor) {
        NumberCard tPrefab = Resources.Load<NumberCard>("prefab/game/numberCard");
        mCard1 = GameObject.Instantiate<NumberCard>(tPrefab);
        mCard1.mPattern.color = aColor;
        mCard1.mPattern.sprite = Resources.Load<Sprite>("image/card/card_star");
        mCard1.transform.SetParent(this.transform, false);
        mCard1.position = new Vector3(0, 0, 0);
        mCard1.showBack();
    }
    public void removeText() {
        mText.text = "";
    }
    public void flipNumberCard(Action aCallback = null) {
        if (!mCard1.mIsFront) {
            mCard1.mNumber.text = mRandomNumber1.ToString();
            mCard1.flip(true, () => {
                mText.text = mCard1.mNumber.text;
                if (aCallback != null) aCallback();
            });
            return;
        }
        if (!mCard2.mIsFront) {
            mCard2.mNumber.text = mRandomNumber2.ToString();
            mCard2.flip(true, () => {
                mText.text = (mCard1.mParsedNumber + mCard2.mParsedNumber).ToString();
                if (aCallback != null) aCallback();
            });
            return;
        }
        if (!mCard3.mIsFront) {
            mCard3.mNumber.text = mRandomNumber3.ToString();
            mCard3.flip(true, () => {
                mText.text = (mCard1.mParsedNumber + mCard2.mParsedNumber + mCard3.mParsedNumber).ToString();
                if (aCallback != null) aCallback();
            });
            return;
        }
    }
    public void flipStarCard(Action aCallback) {
        mCard1.flipAndExpand(() => {
            mCard1.delete();
            aCallback();
        });
    }
}
