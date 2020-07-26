using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMasStatus : MasStatus {
    public static float mLootingRate;
    public string mName;
    public int mValue;
    public LandAttribute mAttribute1;
    public LandAttribute mAttribute2;
    public int mOwnerNumber;
    public int mExpansionLevel;

    public enum LandAttribute {
        none, all, north, east, south, west, center, woods, waterside
    }
    public static Sprite getLandAttributeImage(LandAttribute aAttribute) {
        switch (aAttribute) {
            case LandAttribute.none: return null;
            case LandAttribute.north: return Resources.Load<Sprite>("image/attribute/north");
            case LandAttribute.east: return Resources.Load<Sprite>("image/attribute/east");
            case LandAttribute.south: return Resources.Load<Sprite>("image/attribute/south");
            case LandAttribute.west: return Resources.Load<Sprite>("image/attribute/west");
            case LandAttribute.center: return Resources.Load<Sprite>("image/attribute/center");
            case LandAttribute.woods: return Resources.Load<Sprite>("image/attribute/woods");
            case LandAttribute.waterside: return Resources.Load<Sprite>("image/attribute/waterside");
        }
        return null;
    }

    public int getExpansionCost(int aExpansionLevel) {
        return mValue / 2 * (int)Mathf.Pow(2, aExpansionLevel);
    }

    public int mOccupyCost { get { return mValue; } }
    public int mLootedCost { get { return (int)(mLootingRate * mValue / 10 * Mathf.Pow(3, mExpansionLevel)); } }
    public int mExpansionCost { get { return getExpansionCost(mExpansionLevel); } }
    public int mTotalValue {
        get {
            int tTotal = mValue;
            for (int i = 0; i < mExpansionLevel; i++) {
                tTotal += getExpansionCost(i);
            }
            return tTotal;
        }
    }
}
