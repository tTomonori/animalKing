using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMasDisplay : MasDisplay {
    [SerializeField] public TextMesh mName;
    [SerializeField] public Transform mFree;
    [SerializeField] public TextMesh mOccupy;
    [SerializeField] public Transform mOccupied;
    [SerializeField] public TextMesh mLooting;
    [SerializeField] public TextMesh mExpansion;
    [SerializeField] public SpriteRenderer mAttribute1;
    [SerializeField] public SpriteRenderer mAttribute2;
    [SerializeField] public SpriteRenderer mCrown1;
    [SerializeField] public SpriteRenderer mCrown2;
    [SerializeField] public SpriteRenderer mCrown3;
    [SerializeField] public SpriteRenderer mEdge;

    public void initDisplay(LandMasStatus aStatus) {
        mName.text = aStatus.mName;
        mAttribute1.sprite = LandMasStatus.getLandAttributeImage(aStatus.mAttribute1);
        mAttribute2.sprite = LandMasStatus.getLandAttributeImage(aStatus.mAttribute2);

        updateDisplay(aStatus);
    }
    public void updateDisplay(LandMasStatus aStatus) {
        if (aStatus.mOwnerNumber < 0) {
            mFree.gameObject.SetActive(true);
            mOccupy.text = aStatus.mOccupyCost.ToString();
            mOccupied.gameObject.SetActive(false);
            mEdge.color = PlayerStatus.nonePlayerColor;
            mCrown1.color = PlayerStatus.nonePlayerColor;
            mCrown2.color = PlayerStatus.nonePlayerColor;
            mCrown3.color = PlayerStatus.nonePlayerColor;
        } else {
            mFree.gameObject.SetActive(false);
            mOccupied.gameObject.SetActive(true);
            mLooting.text = aStatus.mLootedCost.ToString();
            mExpansion.text = aStatus.mExpansionCost.ToString();
            mEdge.color = PlayerStatus.playerColor[aStatus.mOwnerNumber];
            mCrown1.color = PlayerStatus.playerColor[aStatus.mOwnerNumber];
            mCrown2.color = PlayerStatus.playerColor[aStatus.mOwnerNumber];
            mCrown3.color = PlayerStatus.playerColor[aStatus.mOwnerNumber];
        }
        mCrown1.gameObject.SetActive(aStatus.mExpansionLevel == 1);
        mCrown2.gameObject.SetActive(aStatus.mExpansionLevel == 2);
        mCrown3.gameObject.SetActive(aStatus.mExpansionLevel == 3);
    }
}
