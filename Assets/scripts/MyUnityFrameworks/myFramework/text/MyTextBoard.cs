using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// 利用可能なタグリスト
/// <size,2></size>文字サイズ変更
/// <color,red></color>文字カラー変更
/// <color,0></color>
/// <color,0,0,1></color>
/// <color,0,0,1,1></color>
/// <br>改行
/// <rotate,180></rotate>回転
/// <tremble,10></tremble>揺れる
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class MyTextBoard : MyBehaviour{
    //文字オブジェクトのscale
    static float kScale = 100;
    //<summary>デフォルト文字サイズ</summary>
    [SerializeField] public int mFontSize = 1;
    //<summary>デフォルト文字カラー</summary>
    [SerializeField] public Color mFontColor = new Color(1, 1, 1, 1);
    //<summary>デフォルトフォント</summary>
    [SerializeField] public Font mFont;
    //<summary>行間隔</summary>
    [SerializeField] public float mLineSpacing = 10;
    //<summary>trueならこのオブジェクトの幅に合わせて自動で改行する</summary>
    [SerializeField] public bool mAutoBreakLine = true;
    //<summary>表示する文字列</summary>
    [SerializeField,TextArea(5,10)] private string mDisplayedText = "";
    //<summary>表示する文字列</summary>
    public string mText{
        get { return mDisplayedText; }
        set {
            mDisplayedText = value;
            updateBoard();
        }
    }
    //行のオブジェクト
    private List<Line> mLines = new List<Line>();
    //タグで設定されているフォントサイズ
    private List<float> mCustomFontSize = new List<float>();
    //タグで設定されているフォントカラー
    private List<Color> mCustomFontColor = new List<Color>();
    //タグで設定されている特殊演出
    private Dictionary<string,CustomProductionData> mCustomProductions = new Dictionary<string,CustomProductionData>();
    //タグで設定する特殊演出のデータ
    private struct CustomProductionData {
        public string mName;
        public List<Action<MyBehaviour>> mProduction;//2重にタグを使用した場合は最も内側にあるものだけ適用する
    }

    //現在適用中のフォントサイズ
    private float mCurrentFontSize {
        get {
            int tLength = mCustomFontSize.Count;
            if (tLength == 0) return mFontSize;
            return mCustomFontSize[tLength - 1];
        }
    }
    //現在適用中のフォントカラー
    private Color mCurrentFontColor {
        get {
            int tLength = mCustomFontColor.Count;
            if (tLength == 0) return mFontColor;
            return mCustomFontColor[tLength - 1];
        }
    }

    //自分のtransform
    private RectTransform mRect;
    private void Awake(){
        mRect = transform as RectTransform;
        updateBoard();
    }

    //<summary>表示を更新する</summary>
    public void updateBoard(){
        clear();
        write(mDisplayedText);
    }
    //<summary>末尾に文字列を追加する</summary>
    public void addText(string aText){
        mDisplayedText += aText;
        write(aText);
    }
    //引数の文字列を表示する
    private void write(string aText) {
        TagReader tReader = new TagReader(aText);
        while (!tReader.isEnd()) {
            TagReader.Element tElement = tReader.read();
            //1文字
            if(tElement is TagReader.OneChar) {
                addChar(((TagReader.OneChar)tElement).mChar);
                continue;
            }
            //開始タグ
            if(tElement is TagReader.StartTag) {
                applyStartTag(((TagReader.StartTag)tElement));
                continue;
            }
            //終了タグ
            if(tElement is TagReader.EndTag) {
                applyEndTag(((TagReader.EndTag)tElement));
                continue;
            }
            Debug.LogWarning("MyTextBoard : 文字読み込み失敗　次の文字「"+tReader.mNext.ToString()+"」");
        }
    }
    //開始タグ適用
    private void applyStartTag(TagReader.StartTag aTag) {
        switch (aTag.mTagName) {
            case "size"://文字サイズ
                mCustomFontSize.Add(float.Parse(aTag.mArguments[0]));
                return;
            case "color"://文字色
                mCustomFontColor.Add(readColor(aTag));
                return;
            case "br"://改行
                createNewLine();
                return;
            case "rotate"://回転
                addTextProduction((obj) => {
                    obj.rotateForever(float.Parse(aTag.mArguments[0]));
                }, "rotate");
                return;
            case "tremble"://震える
                addTextProduction((obj) => {
                    Vector2 tInitialPosition = obj.position2D;
                    Action tTremble = () => { };
                    tTremble = () => {
                        obj.moveTo(tInitialPosition + VectorCalculator.randomVector() * float.Parse(aTag.mArguments[0]), 0.5f, tTremble);
                    };
                    tTremble();
                }, "tremble");
                return;
            default:
                Debug.LogWarning("MyTextBoard : 不明な開始タグ「" + aTag.mTagName + "」");
                return;
        }
    }
    //終了タグ適用
    private void applyEndTag(TagReader.EndTag aTag) {
        switch (aTag.mTagName) {
            case "size"://文字サイズ
                int mSizeLength = mCustomFontSize.Count;
                if (mSizeLength == 0) {
                    Debug.LogWarning("MyTextBoard : 不正なsize終了タグ");
                    return;
                }
                mCustomFontSize.RemoveAt(mSizeLength - 1);
                return;
            case "color"://文字色
                int mColorLength = mCustomFontColor.Count;
                if (mColorLength == 0) {
                    Debug.LogWarning("MyTextBoard : 不正なcolor終了タグ");
                    return;
                }
                mCustomFontColor.RemoveAt(mColorLength - 1);
                return;
            case "rotate"://回転
                removeTextProduction("rotate");
                return;
            case "tremble"://震える
                removeTextProduction("tremble");
                return;
            default:
                Debug.LogWarning("MyTextBoard : 不明な終了タグ「" + aTag.mTagName + "」");
                return;
        }
    }
    //colorタグからColorインスタンスを生成
    private Color readColor(TagReader.StartTag aTag) {
        switch (aTag.mArguments.Length) {
            case 0://引数なし
                return new Color(0, 0, 0, 1);

            case 1:
            case 2:
                //キーワード指定
                switch (aTag.mArguments[0]) {
                    case "red": return new Color(1, 0, 0);
                    case "green": return new Color(0, 1, 0);
                    case "blue": return new Color(0, 0, 1);
                    case "black": return new Color(0, 0, 0);
                    case "white": return new Color(1, 1, 1);
                    case "yellow": return new Color(1, 1, 0);
                }
                //rgb一括設定
                float rgb = float.Parse(aTag.mArguments[0]);
                return new Color(rgb, rgb, rgb);

            case 3://rgb設定
                return new Color(float.Parse(aTag.mArguments[0]), float.Parse(aTag.mArguments[1]), float.Parse(aTag.mArguments[2]));

            case 4://rgba設定
                return new Color(float.Parse(aTag.mArguments[0]), float.Parse(aTag.mArguments[1]),
                 float.Parse(aTag.mArguments[2]), float.Parse(aTag.mArguments[3]));
            default:
                return new Color(float.Parse(aTag.mArguments[0]), float.Parse(aTag.mArguments[1]),
                 float.Parse(aTag.mArguments[2]), float.Parse(aTag.mArguments[3]));
        }
    }
    //特殊演出追加
    private void addTextProduction(Action<MyBehaviour> aProduction,string aName) {
        if (!mCustomProductions.ContainsKey(aName)) {
            //dictionaryにない場合は新たにstructを生成
            CustomProductionData tCreatedData = new CustomProductionData();
            tCreatedData.mName = aName;
            tCreatedData.mProduction = new List<Action<MyBehaviour>>();
            mCustomProductions.Add(aName, tCreatedData);
        }
        //production追加
        mCustomProductions[aName].mProduction.Add(aProduction);
    }
    //特殊演出削除
    private void removeTextProduction(string aName) {
        CustomProductionData tData = mCustomProductions[aName];
        int tLength = tData.mProduction.Count;
        tData.mProduction.RemoveAt(tLength - 1);
        //同名の演出が全て削除された場合はdictionaryから削除
        if (tLength == 1) {
            mCustomProductions.Remove(aName);
        }
    }
    //文字のオブジェクトを全て削除する
    private void clear(){
        foreach(MyBehaviour tLine in mLines){
            tLine.delete();
        }
        mLines.Clear();
        mCustomFontSize.Clear();
        mCustomFontColor.Clear();
    }
    //文字を表示(引数は１文字)
    private void addChar(string aChar){
        //改行文字は無視
        if (aChar == "\n") return;
        //文字オブジェクト生成
        Text tText = createText();
        tText.text = aChar;

        if (mLines.Count == 0) {//初回(行オブジェクトが1つも生成されていない)
            createNewLine();
            mLines[0].addText(tText);
        } else {//行オブジェクトが既に生成されている
            Line tLine = mLines[mLines.Count - 1];
            //追加するとはみ出る場合は改行
            if (mAutoBreakLine && !tLine.canAdd(tText.preferredWidth)) {
                tLine = createNewLine();
            }
            //文字を追加
            tLine.addText(tText);
        }

        //特殊演出適用
        if (mCustomProductions.Count != 0) {
            MyBehaviour tBehaviour = tText.gameObject.AddComponent<MyBehaviour>();
            foreach(KeyValuePair<string,CustomProductionData> tPair in mCustomProductions) {
                tPair.Value.mProduction[tPair.Value.mProduction.Count - 1](tBehaviour);
            }
        }
    }
    //テキストのオブジェクトを生成
    private Text createText() {
        Text tText = MyBehaviour.create<Text>();
        tText.rectTransform.sizeDelta = new Vector2(1, 1);
        tText.rectTransform.localScale = new Vector3(1 / kScale, 1 / kScale, 1 / kScale);
        tText.rectTransform.pivot = new Vector2(0, 0);
        tText.fontSize = (int)(mCurrentFontSize * kScale);
        tText.color = mCurrentFontColor;
        tText.horizontalOverflow = HorizontalWrapMode.Overflow;
        tText.verticalOverflow = VerticalWrapMode.Overflow;
        tText.font = mFont;
        tText.gameObject.layer = gameObject.layer;
        return tText;
    }
    //改行
    private Line createNewLine() {
        int tLineNum = mLines.Count;
        float tHeadPosition = (tLineNum == 0) ?
            mRect.sizeDelta.y / 2 :
            mLines[tLineNum - 1].mBottomPosition - mLineSpacing / kScale;
        Line tNewLine = MyBehaviour.create<Line>();
        tNewLine.name = "Line" + tLineNum.ToString();
        //パラメータ設定
        tNewLine.mHeadPosition = tHeadPosition;
        tNewLine.mLeftPosition = -mRect.sizeDelta.x / 2;
        tNewLine.mWidth = mRect.sizeDelta.x;
        //親ノード設定
        tNewLine.setParent(this.transform);
        //リストに追加
        mLines.Add(tNewLine);
        return tNewLine;
    }
    //行のオブジェクト
    private class Line : MyBehaviour {
        //最も大きい文字の頭の座標
        public float mHeadPosition;
        //最も左の文字の左端の座標
        public float mLeftPosition;
        //文字を置ける幅
        public float mWidth;
        //文字の下端の座標
        public float mBottomPosition {
            get { return positionY; }
        }
        //置いた文字の幅の合計
        public float mCurrentWidth = 0;
        //親ノード設定(パラメータ設定後に呼ぶ)
        public void setParent(Transform aParent) {
            this.transform.parent = aParent;
            this.position = new Vector3(mLeftPosition, mHeadPosition, 0);
        }
        //指定幅の文字を追加できるか
        public bool canAdd(float aWidth) {
            return mCurrentWidth + aWidth / MyTextBoard.kScale <= mWidth;
        }
        //文字を追加
        public void addText(Text aText) {
            //左下を文字の原点に設定
            aText.alignment = TextAnchor.MiddleCenter;
            //子ノードに設定
            aText.transform.SetParent(this.transform, false);
            //位置設定
            aText.transform.localPosition = new Vector3(mCurrentWidth + aText.preferredWidth / MyTextBoard.kScale / 2, aText.preferredHeight / MyTextBoard.kScale / 2, 0);
            mCurrentWidth += aText.preferredWidth / MyTextBoard.kScale;
            //高さ調整
            if (mHeadPosition < mBottomPosition + aText.preferredHeight / MyTextBoard.kScale) {
                positionY = mHeadPosition - aText.preferredHeight / MyTextBoard.kScale;
            }
        }
    }
}
