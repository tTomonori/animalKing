using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MySoundPlayer {
    private static AudioSource mBgm;
    private static Dictionary<string, SoundSet> mSe;
    private static Coroutine mBgmFadeCoroutine;
    private static Coroutine mBgmLoopCoroutine;
    private static LoopType mBgmLoopType;
    public enum LoopType {
        notLoop, normalConnect
    }
    public struct SoundSet {
        public AudioSource mSound;
        public Coroutine mCoroutine;
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="aFilePath">resources/sound/bgm/ + aFilePath</param>
    public static void playBgm(string aFilePath, LoopType aLoopType = LoopType.normalConnect, float aVolume = 1) {
        if (mBgm == null) {
            mBgm = MyBehaviour.create<AudioSource>();
            mBgm.name = "BGM AudioSource";
            mBgm.gameObject.AddComponent<MyBehaviour>();
            GameObject.DontDestroyOnLoad(mBgm);
        }

        mBgm.clip = Resources.Load<AudioClip>("sound/bgm/" + aFilePath);
        mBgm.time = 0;
        mBgm.volume = aVolume;
        mBgm.Play();
        MyBehaviour tBehaviour = mBgm.gameObject.GetComponent<MyBehaviour>();
        if (mBgmLoopCoroutine != null)
            tBehaviour.StopCoroutine(mBgmLoopCoroutine);
        switch (aLoopType) {
            case LoopType.normalConnect:
                mBgmLoopType = aLoopType;
                mBgmLoopCoroutine = tBehaviour.StartCoroutine(runNormalConnect(mBgm));
                break;
        }
    }
    private static IEnumerator runNormalConnect(AudioSource aAudio) {
        while (true) {
            if (!aAudio.isPlaying) aAudio.Play();
            yield return null;
        }
    }
    /// <summary>
    /// BGMを停止する
    /// </summary>
    public static void stopBgm() {
        mBgm.Pause();
    }
    /// <summary>
    /// BGMをフェードする
    /// </summary>
    /// <param name="aDuration">フェード時間(s)</param>
    /// <param name="aVolume">フェード後のボリューム</param>
    public static Coroutine fadeBgm(float aDuration, float aVolume, Action aCallback = null) {
        MyBehaviour tBehaviour = mBgm.GetComponent<MyBehaviour>();
        if (tBehaviour == null) tBehaviour = mBgm.gameObject.AddComponent<MyBehaviour>();

        if (mBgmFadeCoroutine != null)
            tBehaviour.StopCoroutine(mBgmFadeCoroutine);
        mBgmFadeCoroutine= tBehaviour.StartCoroutine(fadeBgmCoroutine(mBgm, aVolume, aDuration, aCallback));
        return mBgmFadeCoroutine;
    }
    private static IEnumerator fadeBgmCoroutine(AudioSource aAudio, float aVolume, float aDuration, Action aCallback) {
        float tInitial = aAudio.volume;
        float tElapsedTime = 0;
        while (true) {
            tElapsedTime += Time.deltaTime;
            if (tElapsedTime > aDuration) tElapsedTime = aDuration;
            if (tElapsedTime == aDuration) {//フェード完了
                aAudio.volume = aVolume;
                if (aCallback != null) aCallback();
                yield break;
            }
            aAudio.volume = tInitial + (aVolume - tInitial) * (tElapsedTime / aDuration);
            yield return null;
        }
    }
    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="aFilePath">resources/sound/se/ + aFilePath</param>
    /// <param name="aAllowOverlap">falseの場合は同じSEを同時に鳴らそうとすると前のSEを停止する</param>
    public static void playSe(string aFilePath, bool aAllowOverlap = false, float aVolume = 1) {
        if (mSe == null) mSe = new Dictionary<string, SoundSet>();
        AudioSource tAudio;
        MyBehaviour tBehaviour;
        if (aAllowOverlap) {
            //同時再生可
            tAudio = MyBehaviour.create<AudioSource>();
            tAudio.clip = Resources.Load<AudioClip>("sound/se/" + aFilePath);
            tAudio.name = "SE : " + aFilePath;
            tAudio.volume = aVolume;
            tAudio.Play();
            tBehaviour = tAudio.gameObject.AddComponent<MyBehaviour>();
            tBehaviour.StartCoroutine(deleteSe(tAudio, ""));
            return;
        } else {
            //同時再生不可
            if (mSe.ContainsKey(aFilePath)) {
                //同時再生しようとした
                SoundSet tPlayedSet = mSe[aFilePath];
                tPlayedSet.mSound.clip = Resources.Load<AudioClip>("sound/se/" + aFilePath);
                tPlayedSet.mSound.time = 0;
                tPlayedSet.mSound.Play();
                return;
            } else {
                tAudio = MyBehaviour.create<AudioSource>();
                tAudio.clip = Resources.Load<AudioClip>("sound/se/" + aFilePath);
                tAudio.name = "SE : " + aFilePath;
                tAudio.volume = aVolume;
                tAudio.Play();
                tBehaviour = tAudio.gameObject.AddComponent<MyBehaviour>();
                SoundSet tSet = new SoundSet();
                tSet.mSound = tAudio;
                tSet.mCoroutine = tBehaviour.StartCoroutine(deleteSe(tAudio, aFilePath));
                mSe.Add(aFilePath, tSet);
                GameObject.DontDestroyOnLoad(tAudio);
            }
        }
    }
    private static IEnumerator deleteSe(AudioSource aAudio, string aKey) {
        while (true) {
            if (!aAudio.isPlaying) {
                GameObject.Destroy(aAudio.gameObject);
                mSe.Remove(aKey);
                yield break;
            }
            yield return null;
        }
    }
}
