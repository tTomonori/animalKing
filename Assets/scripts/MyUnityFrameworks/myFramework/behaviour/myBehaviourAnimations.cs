using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Reflection;

public partial class MyBehaviour : MonoBehaviour {
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <param name="delta">変化量</param>
    /// <param name="duration">変化時間</param>
    /// <param name="callback">変化終了時関数</param>
    public Coroutine scaleBy(Vector3 delta, float duration, Action callback = null) {
        return StartCoroutine(scaleDelta(delta, duration, callback));
    }
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <param name="delta">変化先</param>
    /// <param name="duration">変化時間</param>
    /// <param name="callback">変化終了時間数</param>
    public Coroutine scaleTo(Vector3 goal, float duration, Action callback = null) {
        return StartCoroutine(scaleDelta(goal - scale, duration, callback));
    }
    public Coroutine scaleTo(Vector2 goal, float duration, Action callback = null) {
        return StartCoroutine(scaleDelta(new Vector3(goal.x - scaleX, goal.y - scaleY, 0), duration, callback));
    }
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="delta">変化量</param>
    /// <param name="speed">変化速度(/s)</param>
    /// <param name="callback">変化終了時関数</param>
    public Coroutine scaleByWithSpeed(Vector3 delta, float speed, Action callback = null) {
        return StartCoroutine(scaleDelta(delta, delta.magnitude / speed, callback));
    }
    /// <summary>
    /// scaleを変化させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="goal">変化先</param>
    /// <param name="speed">変化速度(/s)</param>
    /// <param name="callback">変化終了時関数</param>
    public Coroutine scaleToWithSpeed(Vector3 goal, float speed, Action callback = null) {
        Vector3 tDelta = goal - scale;
        return StartCoroutine(scaleDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    public Coroutine scaleToWithSpeed(Vector2 goal, float speed, Action callback = null) {
        Vector2 tDelta = goal - scale2D;
        return StartCoroutine(scaleDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    private IEnumerator scaleDelta(Vector3 delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                scale += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            scale += delta * Time.deltaTime / duration;
            yield return null;
        }
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <param name="delta">移動量</param>
    /// <param name="duration">移動時間</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveBy(Vector3 delta, float duration, Action callback = null) {
        return StartCoroutine(moveDelta(delta, duration, callback));
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <param name="goal">移動先の座標(メソッド呼び出し時の座標からの相対座標分移動)</param>
    /// <param name="duration">移動時間</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveTo(Vector3 goal, float duration, Action callback = null) {
        return StartCoroutine(moveDelta(goal - position, duration, callback));
    }
    public Coroutine moveTo(Vector2 goal, float duration, Action callback = null) {
        return StartCoroutine(moveDelta(new Vector3(goal.x - positionX, goal.y - positionY, 0), duration, callback));
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="delta">移動量</param>
    /// <param name="speed">移動速度(/s)</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveByWithSpeed(Vector3 delta, float speed, Action callback = null) {
        return StartCoroutine(moveDelta(delta, delta.magnitude / speed, callback));
    }
    /// <summary>
    /// 移動させる
    /// </summary>
    /// <returns>コルーチン</returns>
    /// <param name="goal">目標地点</param>
    /// <param name="speed">移動速度(/s)</param>
    /// <param name="callback">移動終了時関数</param>
    public Coroutine moveToWithSpeed(Vector3 goal, float speed, Action callback = null) {
        Vector3 tDelta = goal - position;
        return StartCoroutine(moveDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    public Coroutine moveToWithSpeed(Vector2 goal, float speed, Action callback = null) {
        Vector2 tDelta = goal - position2D;
        return StartCoroutine(moveDelta(tDelta, tDelta.magnitude / speed, callback));
    }
    private IEnumerator moveDelta(Vector3 delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                position += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            position += delta * Time.deltaTime / duration;
            yield return null;
        }
    }
    /// <summary>
    /// 回転させる
    /// </summary>
    /// <param name="delta">回転量</param>
    /// <param name="duration">回転時間</param>
    /// <param name="callback">回転終了時関数</param>
    public Coroutine rotateBy(float delta, float duration, Action callback = null) {
        return StartCoroutine(rotateDelta(delta, duration, callback));
    }
    private IEnumerator rotateDelta(float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if (tElapsedTime + Time.deltaTime >= duration) {//完了
                rotateZ += delta * (duration - tElapsedTime) / duration;
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            rotateZ += delta * Time.deltaTime / duration;
            yield return null;
        }

    }
    /// <summary>
    /// 回転させ続ける
    /// </summary>
    /// <param name="speed">回転速度(度/s)</param>
    public Coroutine rotateForever(float speed) {
        return StartCoroutine(rotateForeverDelta(speed));
    }
    private IEnumerator rotateForeverDelta(float speed) {
        while (true) {
            rotateZ += speed * Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    /// 透明度を変化させる
    /// </summary>
    /// <param name="delta">変化量</param>
    /// <param name="duration">変化時間</param>
    /// <param name="callback">完了時コールバック</param>
    public void opacityBy(float delta, float duration, Action callback = null) {
        CallbackSystem system = new CallbackSystem();
        foreach (SpriteRenderer tC in GetComponentsInChildren<SpriteRenderer>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        foreach (TextMesh tC in GetComponentsInChildren<TextMesh>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        foreach (Image tC in GetComponentsInChildren<Image>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        foreach (Text tC in GetComponentsInChildren<Text>()) StartCoroutine(opacityDelta(tC, delta, duration, system.getCounter()));
        system.then(callback);
    }
    private IEnumerator opacityDeltad(SpriteRenderer obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
    private IEnumerator opacityDelta(TextMesh obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
    private IEnumerator opacityDelta(Image obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
    private IEnumerator opacityDelta(Text obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
    private IEnumerator opacityDelta(SpriteRenderer obj, float delta, float duration, Action callback) {
        float tElapsedTime = 0;
        while (true) {
            if ((float)tElapsedTime + Time.deltaTime >= (float)duration) {//完了
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * (duration - tElapsedTime) / duration);
                if (callback != null) callback();
                yield break;
            }
            tElapsedTime += Time.deltaTime;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + delta * Time.deltaTime / duration);
            yield return null;
        }
    }
}
