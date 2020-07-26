using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

public partial class MyBehaviour : MonoBehaviour {
    /// <summary>
    /// レイヤーを変更する
    /// </summary>
    /// <param name="aLayer">レイヤーの番号</param>
    /// <param name="withChildren">子要素も同時にレイヤーを変更する</param>
    public void changeLayer(int aLayer, bool withChildren = true) {
        if (!withChildren) {
            gameObject.layer = aLayer;
            return;
        }
        foreach (Transform tObject in gameObject.GetComponentsInChildren<Transform>()) {
            tObject.gameObject.layer = aLayer;
        }
    }
    /// <summary>
    /// 指定したComponentもつGameObjectを生成
    /// </summary>
    /// <returns>生成されたGameObjectがもつComponent</returns>
    /// <typeparam name="T">取り付けるComponent</typeparam>
    static public T create<T>() where T : Component {
        return new GameObject().AddComponent<T>();
    }
    /// <summary>
    /// 指定したパスのプレハブを生成
    /// </summary>
    /// <returns>生成したプレハブがもつComponent</returns>
    /// <param name="aFilePath">プレハブへのパス("resources/prefabs/" + X)</param>
    /// <typeparam name="Type">取得するComponent</typeparam>
    public static Type createObjectFromPrefab<Type>(string aFilePath) {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("prefabs/" + aFilePath);
        // プレハブからインスタンスを生成
        return Instantiate(prefab).GetComponent<Type>();
    }
    /// <summary>
    /// 指定したパスのプレハブを生成
    /// </summary>
    /// <returns>生成したプレハブがもつComponent</returns>
    /// <param name="aFilePath">プレハブへのパス("resources/" + X)</param>
    /// <typeparam name="Type">取得するComponent</typeparam>
    public static Type createObjectFromResources<Type>(string aFilePath) {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load(aFilePath);
        // プレハブからインスタンスを生成
        return Instantiate(prefab).GetComponent<Type>();
    }
    public GameObject createChild(string name) {
        GameObject child = new GameObject();
        child.transform.SetParent(this.transform, false);
        child.name = name;
        return child;
    }
    public Type createChild<Type>() where Type : Component {
        Type child = new GameObject().AddComponent<Type>();
        child.transform.SetParent(this.transform, false);
        return child;
    }
    public Type createChild<Type>(string name) where Type : Component {
        Type child = new GameObject().AddComponent<Type>();
        child.transform.SetParent(this.transform, false);
        child.name = name;
        return child;
    }
    /// <summary>
    /// 指定した名前の子要素を取得
    /// </summary>
    /// <returns>取得したGameObject(存在しなければNULL)</returns>
    /// <param name="name">取得する要素の名前</param>
    public GameObject findChild(string name) {
        foreach (Transform tObject in GetComponentsInChildren<Transform>()) {
            if (tObject.name == name) {
                return tObject.gameObject;
            }
        }
        return null;
    }
    /// <summary>
    /// 指定した名前の子要素を取得
    /// </summary>
    /// <returns>取得したGameObject(存在しなければNULL)</returns>
    /// <param name="name">取得する要素の名前</param>
    /// <typeparam name="T">Component</typeparam>
    public T findChild<T>(string name) where T : Component {
        foreach (Transform tObject in GetComponentsInChildren<Transform>()) {
            if (tObject.name == name) {
                return tObject.GetComponent<T>();
            }
        }
        return null;
    }
    /// <summary>
    /// GetComponentsInChildrenで自分自身を含まないようにする
    /// </summary>
    /// <returns>The components in children without self.</returns>
    /// <typeparam name="T">Component</typeparam>
    public T[] GetComponentsInChildrenWithoutSelf<T>() where T : Component {
        return GetComponentsInChildren<T>().Where(c => this.gameObject != c.gameObject).ToArray();
    }
    /// <summary>
    /// GetComponentInChildrenで自分自身を含まないようにする
    /// </summary>
    /// <returns>The component in children without self.</returns>
    /// <typeparam name="T">Component</typeparam>
    public T GetComponentInChildrenWithoutSelf<T>() where T : Component {
        return (GetComponentsInChildren<T>().Where(c => this.gameObject != c.gameObject).ToArray())[0];
    }
    /// <summary>
    /// aSecond秒後にaFunctionを実行
    /// </summary>
    /// <param name="aSeconds">aFunctionを実行するまでの時間(秒)</param>
    /// <param name="aFunction">実行する関数</param>
    public void setTimeout(float aSeconds, Action aFunction) {
        StartCoroutine(waite(aSeconds, aFunction));
    }
    public static void setTimeoutToIns(float aSeconds, Action aFunction) {
        ins.StartCoroutine(ins.waite(aSeconds, aFunction));
    }
    private IEnumerator waite(float aSeconds, Action aFunction) {
        yield return new WaitForSeconds(aSeconds);
        aFunction();
    }
    ///子ルーチン実行
    static public void runCoroutine(IEnumerator aCoroutine) {
        ins.StartCoroutine(aCoroutine);
    }
    public Coroutine runCoroutine(Action aClosure) {
        return StartCoroutine(createCroutine(aClosure));
    }
    private IEnumerator createCroutine(Action aClosure) {
        while (true) {
            aClosure();
            yield return null;
        }
    }
    ///削除する
    public void delete() {
        Destroy(gameObject);
    }
    //<summary>子要素を全て削除</summary>
    public void deleteChildren() {
        foreach (Transform tChild in transform) {
            if (tChild == this) continue;
            Destroy(tChild.gameObject);
        }
    }
    //<summary>指定したコンポーネントをもつオブジェクトのみを取り出す</summary>
    static public List<T> selectComponent<T>(List<Component> aList) where T : Component {
        List<T> tList = new List<T>();
        foreach (Component tComponent in aList) {
            T tTarget = tComponent.GetComponent<T>();
            if (tTarget == null) continue;
            tList.Add(tTarget);
        }
        return tList;
    }
    ///座標
    public Vector3 position {
        get { return gameObject.transform.localPosition; }
        set { gameObject.transform.localPosition = value; }
    }
    public float positionX {
        get { return gameObject.transform.localPosition.x; }
        set {
            Vector3 tPosition = gameObject.transform.localPosition;
            gameObject.transform.localPosition = new Vector3(value, tPosition.y, tPosition.z);
        }
    }
    public float positionY {
        get { return gameObject.transform.localPosition.y; }
        set {
            Vector3 tPosition = gameObject.transform.localPosition;
            gameObject.transform.localPosition = new Vector3(tPosition.x, value, tPosition.z);
        }
    }
    public float positionZ {
        get { return gameObject.transform.localPosition.z; }
        set {
            Vector3 tPosition = gameObject.transform.localPosition;
            gameObject.transform.localPosition = new Vector3(tPosition.x, tPosition.y, value);
        }
    }
    ///座標(２次元)
    public Vector2 position2D {
        get { return new Vector2(positionX, positionY); }
        set { gameObject.transform.localPosition = new Vector3(value.x, value.y, positionZ); }
    }
    //ワールド座標(2次元)
    public Vector2 worldPosition2D {
        get { return new Vector2(gameObject.transform.position.x, gameObject.transform.position.y); }
        set {
            gameObject.transform.position = new Vector3(value.x, value.y, gameObject.transform.position.z);
        }
    }
    //ワールド座標
    public Vector3 worldPosition {
        get { return gameObject.transform.position; }
        set {
            gameObject.transform.position = value;
        }
    }
    //スケール
    public Vector3 scale {
        get { return gameObject.transform.localScale; }
        set { gameObject.transform.localScale = value; }
    }
    public float scaleX {
        get { return gameObject.transform.localScale.x; }
        set {
            Vector3 tScale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(value, tScale.y, tScale.z);
        }
    }
    public float scaleY {
        get { return gameObject.transform.localScale.y; }
        set {
            Vector3 tScale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(tScale.x, value, tScale.z);
        }
    }
    public float scaleZ {
        get { return gameObject.transform.localScale.z; }
        set {
            Vector3 tScale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(tScale.x, tScale.y, value);
        }
    }
    public Vector2 scale2D {
        get { return new Vector2(scaleX, scaleY); }
        set { this.transform.localScale = new Vector3(value.x, value.y, scaleZ); }
    }
    //回転
    public Vector3 rotate {
        get { return gameObject.transform.localRotation.eulerAngles; }
        set { gameObject.transform.localRotation = Quaternion.Euler(value.x, value.y, value.z); }
    }
    public float rotateX {
        get { return gameObject.transform.localRotation.eulerAngles.x; }
        set {
            Vector3 tRotate = gameObject.transform.localRotation.eulerAngles;
            gameObject.transform.localRotation = Quaternion.Euler(value, tRotate.y, tRotate.z);
        }
    }
    public float rotateY {
        get { return gameObject.transform.localRotation.eulerAngles.y; }
        set {
            Vector3 tRotate = gameObject.transform.localRotation.eulerAngles;
            gameObject.transform.localRotation = Quaternion.Euler(tRotate.x, value, tRotate.z);
        }
    }
    public float rotateZ {
        get { return gameObject.transform.localRotation.eulerAngles.z; }
        set {
            Vector3 tRotate = gameObject.transform.localRotation.eulerAngles;
            gameObject.transform.localRotation = Quaternion.Euler(tRotate.x, tRotate.y, value);
        }
    }


    //Behaviourの機能をstaticで使えるようにする
    static private MyBehaviourInstance instance;
    static private MyBehaviourInstance ins {
        get {
            if (instance == null) {
                instance = MyBehaviour.create<MyBehaviourInstance>();
                instance.name = "MyBehaviourInstance";
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }
    private class MyBehaviourInstance : MyBehaviour {
        private List<UpdateFunctionDate> mFunctions = new List<UpdateFunctionDate>();
        private void Update() {
            foreach (UpdateFunctionDate tData in mFunctions) {
                tData.function();
            }
        }
        public void addUpdate(Action aFunction, string aName) {
            UpdateFunctionDate tData = new UpdateFunctionDate();
            tData.function = aFunction;
            tData.name = aName;
            mFunctions.Add(tData);
        }
        public void removeUpdate(string aName) {
            foreach (UpdateFunctionDate tData in mFunctions) {
                if (tData.name != aName) continue;
                mFunctions.Remove(tData);
                return;
            }
        }
        private class UpdateFunctionDate {
            public Action function;
            public string name;
        }
    }
    public void addUpdateFunction(Action aFunction, string aName) {
        ins.addUpdateFunction(aFunction, aName);
    }
    public void removeUpdateFunction(string aName) {
        ins.removeUpdateFunction(aName);
    }
#if UNITY_EDITOR
    /// <summary>edit mode でdestroyを呼び出す</summary>
    public void deleteOnEditMode() {
        EditorApplication.delayCall += deleteOnEditModeDelegate;
    }
    private void deleteOnEditModeDelegate() {
        EditorApplication.delayCall -= deleteOnEditModeDelegate;
        DestroyImmediate(gameObject);
    }
#endif
}
