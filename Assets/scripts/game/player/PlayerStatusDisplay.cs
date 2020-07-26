using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusDisplay : MyBehaviour {
    [SerializeField] public SpriteRenderer mTile;
    [SerializeField] public TextMesh mPlayerNumber;
    [SerializeField] public SpriteRenderer mAnimalFace;
    [SerializeField] public TextMesh mRank;
    [SerializeField] public TextMesh mLap;
    [SerializeField] public TextMesh mFood;
    [SerializeField] public TextMesh mTerritory;
    [SerializeField] public TextMesh mTotalResource;

    public void initDisplay(PlayerStatus aStatus) {
        mTile.color = PlayerStatus.playerColor[aStatus.mPlayerNumber];
        mPlayerNumber.text = (aStatus.mPlayerNumber + 1) + "P";
        mAnimalFace.sprite = Animal.getFaceImage(aStatus.mAnimalName);

        updateDisplay(aStatus);
    }
    public void updateDisplay(PlayerStatus aStatus) {
        mRank.text = aStatus.mRank + "位";
        mLap.text = aStatus.mLap + (aStatus.mReverseLap == 0 ? "" : aStatus.mReverseLap.ToString()) + "周";
        mFood.text = aStatus.mFood.ToString();
        mTerritory.text = aStatus.mTerritory.ToString();
        mTotalResource.text = (aStatus.mFood + aStatus.mTerritory).ToString();
    }
}
