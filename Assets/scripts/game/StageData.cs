using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData{
    private Arg mData;
    public StageData(string aFilePath) {
        mData = new Arg(MyJson.deserializeResourse("stage/data/" + aFilePath));
        //マスデータ
        List<Mas> tMasList = new List<Mas>();
        List<Arg> tMasData = mData.get<List<Arg>>("mas");
        foreach(Arg tData in tMasData) {
            Mas tMas = Mas.create(tData);
            tMasList.Add(tMas);
        }
        mData.set("mas",tMasList);
    }

    public string mName { get { return mData.get<string>("name"); } }
    public int mInitialFood { get { return mData.get<int>("initialFood"); } }
    public List<Vector2> mPlayerStatusPosition { get { return mData.get<List<Vector2>>("playerStatusPosition"); } }
    public Vector2 mTablePosition { get { return mData.get<Vector2>("tablePosition"); } }
    public List<Mas> mMas { get { return mData.get<List<Mas>>("mas"); } }
    public class Mas {
        public MasStatus.MasType mType;
        public Vector2 position;
        public static Mas create(Arg aData) {
            switch (aData.get<string>("type")) {
                case "land":
                    Land tLand = new Land();
                    tLand.mType = MasStatus.MasType.land;
                    tLand.position = aData.get<Vector2>("position");
                    tLand.mName = aData.get<string>("name");
                    tLand.mValue = aData.get<int>("value");
                    List<string> tAttribute = aData.get<List<string>>("attribute");
                    tLand.mAttribute1 = EnumParser.parse<LandMasStatus.LandAttribute>(tAttribute[0]);
                    tLand.mAttribute2 = EnumParser.parse<LandMasStatus.LandAttribute>(tAttribute[1]);
                    return tLand;
                case "start":
                    Start tStart = new Start();
                    tStart.mType = MasStatus.MasType.start;
                    tStart.position = aData.get<Vector2>("position");
                    return tStart;
                case "heart":
                case "bat":
                case "god":
                case "gear":
                case "question":
                    Accident tAccident = new Accident();
                    tAccident.mType = MasStatus.MasType.accident;
                    tAccident.position = aData.get<Vector2>("position");
                    tAccident.mAccidentType = aData.get<string>("type");
                    return tAccident;
                default:
                    throw new System.Exception("StageData-Mas : 不正なマスタイプ「"+aData.get<string>("type")+"」");
            }
        }
    }
    public class Land : Mas {
        public string mName;
        public int mValue;
        public LandMasStatus.LandAttribute mAttribute1;
        public LandMasStatus.LandAttribute mAttribute2;
    }
    public class Start : Mas {

    }
    public class Accident : Mas {
        public string mAccidentType;
    }
}
