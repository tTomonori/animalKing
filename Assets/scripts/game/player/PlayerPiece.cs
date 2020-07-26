using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MyBehaviour {
    [SerializeField] public SpriteRenderer mRenderer;
    [SerializeField] public MyBehaviour mPiece;

    public static Vector2[] mPieceRelativePosition { get { return new Vector2[4] { new Vector2(-0.3f, 0.3f), new Vector2(0.3f, 0.3f), new Vector2(-0.3f, -0.1f), new Vector2(0.3f, -0.1f) }; } }

    public void setRelativePosition(Vector2 aPosition) {
        mPiece.position2D = aPosition;
    }
}
