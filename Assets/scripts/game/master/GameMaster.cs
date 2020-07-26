using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public partial class GameMaster : MyBehaviour {
    public GameElementData mElement;
    public GameMain mMain;

    private int mTurnNumber = -1;
    private int[] mTurn;
    public PlayerStatus mTurnPlayer { get { return mElement.mPlayerStatus[mTurn[mTurnNumber]]; } }

    public void start() {
        shufflePlayerTurn(() => {
            startTurn();
        });
    }
    private void startTurn() {
        Action tNext = () => { };
        tNext = () => {
            if (judgeEnd()) {
                endGame();
                return;
            }
            mTurnNumber = (mTurnNumber + 1) % mElement.mPlayerStatus.Length;
            while (mTurnPlayer.mFood < 0) {//絶滅したプレイヤの手番は飛ばす
                mTurnNumber = (mTurnNumber + 1) % mElement.mPlayerStatus.Length;
            }
            nextPlayerTurn(() => { tNext(); });
        };
        tNext();
    }
    //終了判定trueで終了
    private bool judgeEnd() {
        PlayerStatus tSurvivor = null;
        for(int i = 0; i < mElement.mPlayerStatus.Length; i++) {
            if (mElement.mPlayerStatus[i].mTotalResource < 0) continue;
            if (tSurvivor != null) return false;
            tSurvivor = mElement.mPlayerStatus[i];
        }
        return true;
    }
    /// <summary>ゲーム終了</summary>
    private void endGame() {
        //勝者を探す
        PlayerStatus tPlayer = null;
        for(int i = 0; i < mElement.mPlayerStatus.Length; i++) {
            if (mElement.mPlayerStatus[i].mTotalResource >= 0) {
                tPlayer = mElement.mPlayerStatus[i];
                break;
            }
        }
        //メッセージ表示
        if (tPlayer != null) {
            mElement.mTable.showMessage((tPlayer.mPlayerNumber + 1) + "Pが生き残りました\n" + (tPlayer.mPlayerNumber + 1) + "Pの勝利です");
        } else {
            mElement.mTable.showMessage("皆絶滅しました...");
        }
        //画面タップでタイトルへ
        setTimeout(1.5f, () => {
            Subject.addObserver(new Observer("gameMasterEnd", (aMessage) => {
                if (aMessage.name != "touchBack") return;
                Subject.removeObserver("gameMasterEnd");
                MySceneManager.changeScene("title");
             }));
        });
    }
    /// <summary>プレイヤの手番の順番をシャッフル</summary>
    private void shufflePlayerTurn(Action aCallback) {
        int[] tTurn = new int[mElement.mPlayerStatus.Length];
        for (int i = 0; i < mElement.mPlayerStatus.Length; i++) {
            tTurn[i] = i;
        }
        tTurn = tTurn.OrderBy(i => Guid.NewGuid()).ToArray();
        GameAnimation.playerShuffle(mElement.mPlayerStatusDisplay, mElement.mPlayerStatus, tTurn, () => {
            for (int i = 0; i < mElement.mPlayerStatus.Length; i++) {
                //結果をstatusに適用
                mElement.mPlayerStatus[tTurn[i]].mTurn = i;
                mTurn = tTurn;
            }
            aCallback();
        });
    }
    /// <summary>次のプレイヤの手番を開始</summary>
    private void nextPlayerTurn(Action aCallback) {
        runFlipCardPhase(() => { runMoveMasPhase(() => { runEventPhase(() => { aCallback(); }); }); });
    }
    //手番でカードをめくる処理
    private void runFlipCardPhase(Action aCallback) {
        //カードをめくる
        mElement.mTable.setNumberCards(PlayerStatus.playerColor[mTurnPlayer.mPlayerNumber]);
        int tFlippedCardNum = 0;
        Action tFlipControl = () => {
            switch (tFlippedCardNum) {
                case 0:
                    tFlippedCardNum++;
                    mElement.mTable.flipNumberCard();
                    break;
                case 1:
                    tFlippedCardNum++;
                    mElement.mTable.flipNumberCard();
                    break;
                case 2:
                    tFlippedCardNum++;
                    mTurnPlayer.mController.endFlip();
                    mElement.mTable.flipNumberCard(() => {
                        aCallback();
                    });
                    break;
            }
        };
        mTurnPlayer.mController.flipCard(tFlipControl);
    }
    //手番でマスを移動する処理
    private void runMoveMasPhase(Action aCallback) {
        int tMoveNum = mElement.mTable.mTotalNumber;
        moveMas(tMoveNum, mTurnPlayer, () => {
            mElement.mTable.removeNumberCards();
            mElement.mTable.removeText();
            aCallback();
        });
    }
    //マスを移動させる
    private void moveMas(int aNum, PlayerStatus aPlayer, Action aCallback) {
        int tCount = 0;
        Action tMove = () => { };
        tMove = () => {
            int tNextMasNumber = (aPlayer.mCurrentMasNumber + 1) % mElement.mMasStatus.Length;
            MasStatus tNextMas = mElement.mMasStatus[tNextMasNumber];
            MasDisplay tNextDisplay = mElement.mMasDisplay[tNextMasNumber];
            mElement.mPlayerPieces[aPlayer.mPlayerNumber].moveToWithSpeed(tNextDisplay.position2D, 10, () => {
                aPlayer.mCurrentMasNumber = tNextMasNumber;
                tCount++;
                //通過イベント
                runPassEvent(aPlayer, () => {
                    if (tCount < aNum) {
                        tMove();
                        return;
                    }
                    aCallback();
                });
            });
        };
        tMove();
    }
    //全てのプレイヤの情報表示を更新する
    private void updatePlayerDisplay() {
        for (int i = 0; i < mElement.mPlayerStatus.Length; i++) {
            mElement.mPlayerStatusDisplay[i].updateDisplay(mElement.mPlayerStatus[i]);
        }
    }
    //プレイヤの順位を更新する
    private void sortPlayerRank() {
        PlayerStatus[] tPlayers = new PlayerStatus[mElement.mPlayerStatus.Length];
        for (int i = 0; i < mElement.mPlayerStatus.Length; i++) {
            tPlayers[i] = mElement.mPlayerStatus[i];
        }
        Array.Sort(tPlayers, (x, y) => { return y.mTotalResource - x.mTotalResource; });
        tPlayers[0].mRank = 1;
        for(int i = 1; i < tPlayers.Length; i++) {
            if (tPlayers[i].mTotalResource == tPlayers[i - 1].mTotalResource) tPlayers[i].mRank = tPlayers[i - 1].mRank;
            else tPlayers[i].mRank = i + 1;
        }
        updatePlayerDisplay();
    }
}
