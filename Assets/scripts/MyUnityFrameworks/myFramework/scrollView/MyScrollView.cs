using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public partial class MyScrollView : MyBehaviour {
    //このオブジェクトを写しているカメラ
    private Camera mCamera;
    //設定
    private Option mOption;
    //操作状態
    private MyScrollViewOperateState mState;
    //表示領域
    private MyBehaviour mContent;
    //collider
    private BoxCollider2D mCollider;
    //データリスト
    private MyScrollViewElementDataList mDataList;
    //行の空き部分を埋めた時の最後の要素のindex
    private int squareLastIndex{
        get { return mDataList.lastNum + (elementNumInRow - mDataList.length % elementNumInRow) % elementNumInRow; }
    }
    //行の空き部分を埋めた時のデータの数
    private int squareDataLength{
        get { return squareLastIndex + 1; }
    }
    //表示している要素
    private List<ElementTuple> mElements;
    //要素のスクロール方向の大きさ
    public float elementColLength{
        get { return bidirectional<float>(mOption.elementSize.y, mOption.elementSize.x); }
    }
    //要素のスクロール方向に直角方向の大きさ
    public float elementRowLength{
        get { return bidirectional<float>(mOption.elementSize.x, mOption.elementSize.y); }
    }
    //表示領域のスクロール方向の大きさ
    public float contentColLength{
        get { return bidirectional<float>(mOption.contentSize.y, mOption.contentSize.x); }
    }
    //表示領域のスクロール方向に直角方向の大きさ
    public float contentRowLength{
        get { return bidirectional<float>(mOption.contentSize.x, mOption.contentSize.y); }
    }
    //スクロール方向によって値を選択
    public T bidirectional<T>(T verticalCase,T horizontalCase){
        return (mOption.scrollDirection == scrollDirection.vertical) ? verticalCase : horizontalCase;
    }
    //スクロール量
    public float scrollPosition{
        get { return bidirectional<float>(mContent.positionY, -mContent.positionX); }
        set{
            if (mOption.scrollDirection == scrollDirection.vertical)
                mContent.positionY = value;
            else
                mContent.positionX = -value;
        }
    }
    //スクロール量のベクトル
    public Vector2 scrollPositionVector{
        get { return new Vector2(-mContent.positionX, mContent.positionY); }
        set{
            if (mOption.scrollDirection == scrollDirection.vertical)
                mContent.positionY = value.y;
            else
                mContent.positionX = -value.x;
        }
    }
    //ループスクロール不可の場合の末尾スクロール量
    public float scrollTail{
        get{
            float tTail = (lastRowNum + 1) * elementColLength - contentColLength;
            return (tTail < 0) ? 0 : tTail;
        }
    }
    //一行に表示する要素の数
    public int elementNumInRow{
        get { return Mathf.FloorToInt(contentRowLength / elementRowLength); }
    }
    //末尾の行番号
    public int lastRowNum{
        get { return Mathf.FloorToInt(mDataList.lastNum / elementNumInRow); }
    }
    //表示できる要素の先頭のindex
    public int topElementIndex{
        get { return (mOption.infinitScrool) ? int.MinValue : 0; }
    }
    //表示できる要素の末尾のindex
    public int tailElementIndex{
        get { return (mOption.infinitScrool) ? int.MaxValue : mDataList.lastNum; }
    }
    private void Awake(){
        foreach(GameObject tObject in gameObject.scene.GetRootGameObjects()){
            Camera tCamera = tObject.GetComponentInChildren<Camera>();
            if (tCamera == null)continue;
            mCamera = tCamera;
            return;
        }
    }
    //状態遷移
    private void changeState(MyScrollViewOperateState aState){
        mState.exit();
        mState = aState;
        mState.enter();
    }
    public void init(MyScrollViewElementDataList aDataList,Option aOption){
        if (mOption != null) return;
        mOption = aOption;
        //オプション調整
        if (mOption.infinitScrool) mOption.sortable = false;//無限スクロールがonならソート機能は使えない
        if (!mOption.sortable) mOption.doubleTapSort = false;//ソート機能がoffならダブルタップでのソート機能もoff
        if(!mOption.doubleTap){//ダブルタップがoff
            mOption.doubleTapTime = 0;
            mOption.doubleTapSort = false;
        }
        //マスク
        gameObject.AddComponent<RectMask2D>();
        RectTransform tRect = gameObject.GetComponent<RectTransform>();
        tRect.sizeDelta = mOption.contentSize;
        tRect.pivot = new Vector2(0, 1);

        mDataList = aDataList;
        mState = new MyScrollViewWaitState(this);
        //可視部分collider
        MyBehaviour tColliderBehaviour = MyBehaviour.create<MyBehaviour>();
        tColliderBehaviour.name = "myScrollViewContentCollider";
        tColliderBehaviour.transform.parent = this.transform;
        tColliderBehaviour.position = new Vector3(0, 0, 0);
        tColliderBehaviour.scale = new Vector3(1, 1, 1);
        mCollider = tColliderBehaviour.gameObject.AddComponent<BoxCollider2D>();
        mCollider.size = mOption.contentSize;
        mCollider.offset = new Vector2(mOption.contentSize.x / 2, -mOption.contentSize.y / 2);
        //表示領域
        mContent = MyBehaviour.create<MyBehaviour>();
        mContent.name = "myScrollViewContent";
        mContent.transform.parent = this.gameObject.transform;
        mContent.position = new Vector3(0, 0, -0.1f);
        mContent.scale = new Vector3(1, 1, 1);
        //要素リスト
        mElements = new List<ElementTuple>();

        initElements();
    }
    private void Update(){
        if (mOption == null) return;
        mState.update();
    }
    //マウスホイールによるスクロール
    private void scrollByWheel(){
        Vector3 tMousePosition = mCamera.ScreenToWorldPoint(Input.mousePosition);
        if (!overlap(new Vector2(tMousePosition.x, tMousePosition.y))) return;
        //カーソルがビュー上にある時のみ適用
        scrollPositionVector -= Input.mouseScrollDelta;
    }
    //ドラッグによるスクロール
    private void scrollByDrag(){
        MyTouchInfo[] tInfos = MyTouchInput.getTouch(mCamera);
        if (tInfos.Count() == 0) return;
        if (tInfos[0].state != MyTouchState.moved) return;
        //ドラッグ操作した時のみ適用
        scrollPositionVector += new Vector2(-tInfos[0].deltaPosition.x, tInfos[0].deltaPosition.y);
    }
    //スクロールしすぎた時の調整
    private void remediateScrollPosition(){
        if (!mOption.infinitScrool){
            if (scrollPosition < 0)
                scrollPosition = 0;
            else if (scrollTail < scrollPosition)
                scrollPosition = scrollTail;
        }
    }
    //要素の表示更新
    private void updateElement(){
        List<int> tDisplayIndex = getDisplayIndex();
        int tFirstIndex = tDisplayIndex.First<int>();
        int tLastIndex = tDisplayIndex.Last<int>();
        int tListTopIndex = mElements.First<ElementTuple>().index;
        int tListTailIndex = mElements.Last<ElementTuple>().index;
        //先頭側修正
        if(tFirstIndex<=tListTopIndex){//先頭追加
            for (int i = tListTopIndex-1; tFirstIndex <= i; i--){
                createElement(i);
            }
        }else{//先頭削除
            while(true){
                if (mElements.Count == 0) break;
                ElementTuple tTuple = mElements.First<ElementTuple>();
                if (tFirstIndex <= tTuple.index) break;
                mElements.Remove(tTuple);
                tTuple.element.delete();
            }
        }
        //末尾側修正
        if(tListTailIndex<=tLastIndex){//末尾追加
            for (int i = Math.Max(tListTailIndex + 1,tFirstIndex); i <= tLastIndex;i++){
                createElement(i);
            }
        }else{//末尾削除
            while(true){
                ElementTuple tTuple = mElements.Last<ElementTuple>();
                if (tTuple.index <= tLastIndex) break;
                mElements.Remove(tTuple);
                tTuple.element.delete();
            }
        }
    }
    //要素の表示初期化
    private void initElements(){
        //要素全て削除
        foreach(ElementTuple tTuple in mElements){
            tTuple.element.delete();
        }
        mElements = new List<ElementTuple>();
        //生成し直し
        List<int> tDisplayIndex = getDisplayIndex();
        foreach (int tIndex in tDisplayIndex){
            createElement(tIndex);
        }
    }
    //表示する要素の番号取得
    private List<int> getDisplayIndex(){
        //表示候補のindex
        int tTop = Mathf.FloorToInt(scrollPosition / elementColLength) * elementNumInRow;
        int tTail = Mathf.CeilToInt((scrollPosition + contentColLength) / elementColLength) * elementNumInRow;
        //表示可能なindex
        int tTopIndex = topElementIndex;
        int tTailIndex = tailElementIndex;
        List<int> tIndexArray = new List<int>();
        for (int i = 0; i < tTail-tTop;i++){
            if (tTop + i < tTopIndex) continue;
            if (tTailIndex < tTop + i) break;
            tIndexArray.Add(tTop + i);
        }
        return tIndexArray;
    }
    //要素を表示する座標
    private Vector3 calculateElementPosition(int aIndex){
        int tRowNum = ((aIndex < 0) ? aIndex - 1 : aIndex) / elementNumInRow;
        int tColNum = aIndex % elementNumInRow;
        if (tColNum < 0) tColNum += elementNumInRow;
        float tRowPosition = tRowNum * elementColLength + elementColLength / 2;
        float tColPosition = tColNum * elementRowLength + elementRowLength / 2;
        if (mOption.scrollDirection == scrollDirection.vertical)
            return new Vector3(tColPosition, -tRowPosition, 0);
        else
            return new Vector3(tRowPosition, -tColPosition, 0);
    }
    //要素を生成
    private ElementTuple createElement(int aIndex){
        if (aIndex < topElementIndex) return null;
        if (tailElementIndex < aIndex) return null;
        int? tNum = indexToNum(aIndex);
        if (tNum == null) return null;
        int tElementNum = (int)tNum;
        //要素生成
        ElementTuple tTuple = new ElementTuple(aIndex, tElementNum, mDataList.createElement(tElementNum));
        tTuple.element.name = "myScrollViewElement " + aIndex.ToString();
        //シーンに追加
        tTuple.element.transform.parent = mContent.transform;
        tTuple.element.transform.SetParent(mContent.transform, false);
        tTuple.element.position = calculateElementPosition(aIndex);
        //リストの先頭か末尾に追加
        if (mElements.Count == 0)
            mElements.Add(tTuple);
        else if (tTuple.index < mElements.First<ElementTuple>().index)
            mElements.Insert(0, tTuple);
        else
            mElements.Add(tTuple);
        
        return tTuple;
    }
    //要素のindexを番号に変換
    private int? indexToNum(int aIndex){
        int tNum = aIndex % squareDataLength;
        tNum = (tNum < 0) ? tNum + squareDataLength : tNum;
        if (mDataList.lastNum < tNum) return null;
        return tNum;
    }
    //指定した座標がこのオブジェクトに衝突している
    private bool targeting(Vector2 aPosition){
        Collider2D tCollider = Physics2D.OverlapPoint(aPosition);
        return (tCollider == mCollider);
    }
    //指定した座標がこのオブジェクトに含まれている
    private bool overlap(Vector2 aPosition){
        if (aPosition.x < this.transform.position.x) return false;
        if (this.transform.position.x + mOption.contentSize.x < aPosition.x) return false;
        if (aPosition.y < this.transform.position.y-mOption.contentSize.y) return false;
        if (this.transform.position.y < aPosition.y) return false;
        return true;
    }
    //指定したワールド座標に配置しているelementを取得
    private ElementTuple getElement(Vector2 aPosition){
        Vector2 tRelPosition = worldPositionToContentPosition(aPosition);
        //要素index判定
        int? tIndex = contentPositionToIndex(tRelPosition);
        if (tIndex == null) return null;
        //要素番号判定
        int? tNum = indexToNum((int)tIndex);
        if (tNum == null) return null;
        //要素を探す
        foreach(ElementTuple tTuple in mElements){
            if (tTuple.index == (int)tIndex)
                return tTuple;
        }
        return null;
    }
    //ワールド座標をcontentの相対座標に変換
    private Vector2 worldPositionToContentPosition(Vector2 aPosition){
        //MyScrollViewからの相対座標
        Vector2 tRes = new Vector2(aPosition.x - this.transform.position.x, aPosition.y - this.transform.position.y);
        return new Vector2(tRes.x - mContent.positionX, tRes.y - mContent.positionY);
    }
    //指定した座標を要素のindexに変換
    private int? contentPositionToIndex(Vector2 aPosition){
        int tRowNum = Mathf.FloorToInt(bidirectional<float>(-aPosition.y, aPosition.x) / elementColLength);
        int tColNum = Mathf.FloorToInt(bidirectional<float>(aPosition.x, -aPosition.y) / elementRowLength);
        if (tColNum < 0 || elementNumInRow <= tColNum) return null;//範囲外
        int tNum = tRowNum * elementNumInRow + tColNum;
        if (tNum < topElementIndex) return null;//範囲外
        if (tailElementIndex < tNum) return null;//範囲外
        return tNum;
    }
    //要素を並び替える(データの順番のみで表示はそのまま)
    private void sortElement(int aIndex1,int aIndex2,out ElementTuple oNewTupleOfIndex1,out ElementTuple oNewTupleOfIndex2){
        int tIndexOfElements1 = -1;
        int tIndexOfElements2 = -1;
        for (int i = 0; i < mElements.Count;i++){
            ElementTuple tTuple = mElements[i];
            if(tTuple.index==aIndex1){
                tIndexOfElements1 = i;
                continue;
            }
            if(tTuple.index==aIndex2){
                tIndexOfElements2 = i;
                continue;
            }
        }
        if(tIndexOfElements1<0||tIndexOfElements2<0){//並び替える要素が見つからなかった
            oNewTupleOfIndex1 = null;
            oNewTupleOfIndex2 = null;
            return;
        }
        //並び替え
        mDataList.sort(aIndex1, aIndex2);
        ElementTuple tTuple1 = mElements[tIndexOfElements1];
        ElementTuple tTuple2 = mElements[tIndexOfElements2];
        mElements[tIndexOfElements1] = new ElementTuple(tTuple1.index, tTuple1.elementNumber, tTuple2.element);
        mElements[tIndexOfElements2] = new ElementTuple(tTuple2.index, tTuple2.elementNumber, tTuple1.element);
        oNewTupleOfIndex1 = mElements[tIndexOfElements2];
        oNewTupleOfIndex2 = mElements[tIndexOfElements1];
        return;
    }
    //要素を正しい位置へ移動
    private void moveToCorrectPosition(ElementTuple aTuple){
        aTuple.element.moveBy(calculateElementPosition(aTuple.index) - aTuple.element.position, 0.1f);
    }
    public class Option{
        /// <summary>要素のサイズ</summary>
        public Vector2 elementSize = new Vector2(1, 1);
        /// <summary>表示領域のサイズ</summary>
        public Vector2 contentSize = new Vector2(5, 5);
        /// <summary>スクロール方向</summary>
        public scrollDirection scrollDirection = scrollDirection.vertical;
        /// <summary>無限スクロール</summary>
        public bool infinitScrool = false;
        /// <summary>要素の並び替え機能</summary>
        public bool sortable = false;
        /// <summary>長押し判定とするまでの時間</summary>
        public float longTapTime = 0.7f;
        /// <summary>ダブルタップ機能</summary>
        public bool doubleTap = true;
        /// <summary>ダブルタップとするまでの時間</summary>
        public float doubleTapTime = 0.2f;
        /// <summary>ダブルタップでソート機能を起動</summary>
        public bool doubleTapSort = false;
    }
    public enum scrollDirection{
        vertical,horizontal
    }
    private class ElementTuple{
        public int index;
        public int elementNumber;
        public MyScrollViewElement element;
        public ElementTuple(int aIndex,int aElementNumber,MyScrollViewElement aElement){
            index = aIndex;
            elementNumber = aElementNumber;
            element = aElement;
        }
    }
}