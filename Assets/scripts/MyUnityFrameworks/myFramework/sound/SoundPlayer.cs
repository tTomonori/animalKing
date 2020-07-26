using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer{
    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="aFileName">ファイルへのパス("resources/sound/se" + X)</param>
    static public void playSe(string aFileName){
        //音声ロード
        AudioSource tAudio = MyBehaviour.create<AudioSource>();
        tAudio.name = "SE : " + aFileName;
        tAudio.clip = Resources.Load<AudioClip>("sound/se/"+aFileName);
        if (tAudio.clip == null){
            //音声のロードに失敗
            Debug.Log("音声のロードに失敗 : " + "「sound/se/" + aFileName + "」");
            return;
        }
        tAudio.Play();
        //再生終了したら削除
        MyBehaviour tBehaviour = tAudio.gameObject.AddComponent<MyBehaviour>();
        Coroutine tCoroutine = null;
        tCoroutine = tBehaviour.runCoroutine(() =>{
            if (!tAudio.isPlaying){
                tBehaviour.StopCoroutine(tCoroutine);
                tBehaviour.delete();
            }
        });


    }
}
