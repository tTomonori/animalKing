using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlacer {
    public static PlayerStatusDisplay[] placePlayerStatus(StageData aData) {
        //プレイヤー情報の入れ物
        MyBehaviour tStatusContainer = MyBehaviour.create<MyBehaviour>();
        tStatusContainer.name = "playerStatusContainer";
        tStatusContainer.positionZ = -10;

        List<Vector2> tPositions = aData.mPlayerStatusPosition;
        PlayerStatusDisplay[] tDisplay = new PlayerStatusDisplay[tPositions.Count];
        PlayerStatusDisplay tPrefab = Resources.Load<PlayerStatusDisplay>("prefab/game/playerStatus");
        for (int i = 0; i < tPositions.Count; i++) {
            tDisplay[i] = GameObject.Instantiate<PlayerStatusDisplay>(tPrefab);
            tDisplay[i].position2D = tPositions[i];
            tDisplay[i].positionZ = i;
            tDisplay[i].transform.SetParent(tStatusContainer.transform, false);
        }
        return tDisplay;
    }
    public static MasDisplay[] placeMas(StageData aData) {
        //ルート(白い線)の入れ物
        MyBehaviour tRouteContainer = MyBehaviour.create<MyBehaviour>();
        tRouteContainer.positionZ = 0.5f;
        MyBehaviour tRoute;
        tRouteContainer.name = "routeContainer";
        //マスの入れ物
        MyBehaviour tMasContainer = MyButton.create<MyBehaviour>();
        tMasContainer.name = "masContainer";

        List<StageData.Mas> tMasDataList = aData.mMas;
        MasDisplay[] tDisplay = new MasDisplay[tMasDataList.Count];
        MasDisplay tPrefab = Resources.Load<MasDisplay>("prefab/game/mas/land");
        for (int i = 0; i < tMasDataList.Count; i++) {
            switch (tMasDataList[i].mType) {
                case MasStatus.MasType.land:
                    tDisplay[i] = GameObject.Instantiate<MasDisplay>(tPrefab);
                    break;
                case MasStatus.MasType.start:
                    tDisplay[i] = GameObject.Instantiate<MasDisplay>(Resources.Load<MasDisplay>("prefab/game/mas/start"));
                    break;
                case MasStatus.MasType.accident:
                    tDisplay[i]= GameObject.Instantiate<MasDisplay>(Resources.Load<MasDisplay>("prefab/game/mas/"+((StageData.Accident)tMasDataList[i]).mAccidentType));
                    break;
                default:
                    throw new System.Exception("GamePlacer : 不正な土地属性");
            }
            tDisplay[i].position2D = tMasDataList[i].position;
            tDisplay[i].transform.parent = tMasContainer.transform;
            //ルート配置
            if (i != 0) {
                tRoute = placeRoute(tDisplay[i - 1].position2D, tDisplay[i].position2D);
                tRoute.transform.SetParent(tRouteContainer.transform, false);
            }
        }
        //最初のマスと最後のマスのルート配置
        tRoute = placeRoute(tDisplay[tMasDataList.Count - 1].position2D, tDisplay[0].position2D);
        tRoute.transform.parent = tRouteContainer.transform;
        return tDisplay;
    }
    public static MyBehaviour placeRoute(Vector2 aPosition1, Vector2 aPosition2) {
        MyBehaviour tRoute = MyBehaviour.create<MyBehaviour>();
        SpriteRenderer tRenderer = tRoute.gameObject.AddComponent<SpriteRenderer>();
        tRoute.name = "route";
        tRenderer.sprite = Resources.Load<Sprite>("tile");
        tRoute.scaleX = Vector2.Distance(aPosition1, aPosition2) * 10;
        tRoute.position2D = (aPosition1 + aPosition2) / 2.0f;
        tRoute.rotateZ = new Vector2(1, 0).corner(aPosition2 - aPosition1);
        return tRoute;
    }
    public static GameTable placeTable(StageData aData) {
        GameTable tTable = Resources.Load<GameTable>("prefab/game/table");
        tTable = GameObject.Instantiate<GameTable>(tTable);
        tTable.position2D = aData.mTablePosition;
        return tTable;
    }
    public static PlayerPiece[] placePiece(PlayerStatus[] aStatus,Vector2 aStartPosition) {
        //コマの入れ物
        MyBehaviour tPieceContainer = MyBehaviour.create<MyBehaviour>();
        tPieceContainer.name = "piceContainer";
        tPieceContainer.positionZ = -5;

        PlayerPiece[] tPieces = new PlayerPiece[aStatus.Length];
        PlayerPiece tPrefab = Resources.Load<PlayerPiece>("prefab/game/playerPiece");
        for (int i = 0; i < aStatus.Length; i++) {
            PlayerPiece tPiece = GameObject.Instantiate<PlayerPiece>(tPrefab);
            tPiece.mRenderer.sprite = Animal.getPieceImage(aStatus[i].mAnimalName);
            tPiece.mRenderer.color = PlayerStatus.playerColor[i];
            tPiece.setRelativePosition(PlayerPiece.mPieceRelativePosition[i]);
            tPiece.position2D = aStartPosition;
            tPiece.transform.SetParent(tPieceContainer.transform, false);
            tPieces[i] = tPiece;
        }
        return tPieces;
    }
}
