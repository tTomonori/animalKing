using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GifAnimator : MonoBehaviour {
    private SpriteRenderer mRenderer;
    [SerializeField] private Sprite[] mSprites;
    //アニメーションに使う画像が変更された
    private bool mChangedSprites = false;
    //<summary>画像を表示する順番</summary>
    public int[] mOrder=new int[0];
    //<summary>画像を変更する間隔</summary>
    public float mInterval = 0.2f;
    //<summary>trueならイベントループで自動更新</summary>
    public bool mIsPlayed = false;
    //<summary>表示中の画像のindex</summary>
    public int mOrderIndex = 0;
    //<summary>最後に画像を変更してから経過した時間</summary>
    private float mDeltaTime = 0;

    private void Awake(){
        mRenderer = gameObject.GetComponent<SpriteRenderer>(); 
    }

    void Update () {
        //画像の順番が未設定なら、indexの順に設定
        if (mOrder.Length == 0){
            mOrder = new int[mSprites.Length];
            for (int i = 0; i < mSprites.Length; i++)
                mOrder[i] = i;
        }

        if(mChangedSprites){
            //画像が変更された場合
            mChangedSprites = false;
            mOrderIndex = mOrderIndex % mOrder.Length;
            mRenderer.sprite = mSprites[mOrder[mOrderIndex]];
        }

        if (!mIsPlayed) return;
        //更新
        updateImage();
	}
    //<summary>画像を更新</summary>
    public void updateImage(){
        //不正な時間間隔設定
        if (mInterval <= 0) return;
        //経過時間
        mDeltaTime += Time.deltaTime;
        //画像番号決定
        if (mOrder.Length == 0) return;
        mOrderIndex = (mOrderIndex + Mathf.FloorToInt(mDeltaTime / mInterval)) % mOrder.Length;
        mDeltaTime = mDeltaTime % mInterval;
        //画像変更
        mRenderer.sprite = mSprites[mOrder[mOrderIndex]];
    }
    //<summary>画像を変更</summary>
    public void setSprites(Sprite[] aSprites){
        mSprites = aSprites;
        mChangedSprites = true;
    }
}
