#region usings

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class AudioManager : MonoBehaviour {

    #region Variables and Properties

    public AudioSource leftAudioSource;
    public AudioSource rightAudioSource;
    public AudioSource centerAudioSource;
    public AudioSource frontAudioSource;

    public AudioSource musicAudioSource;

    AudioSource source;

    #endregion

    public void Load() {
        SoundBank.LoadAudioDictionary();
        foreach (KeyValuePair<string, AudioClip> keyValuePair in SoundBank.audioDictionary) {
            centerAudioSource.clip = keyValuePair.Value;
            centerAudioSource.Play();
        }
    }

    void Start() {
        Load();
    }

    IEnumerator PlaySequenceCoroutine(string[] clips, AudioPosition position) {
        switch (position) {
            case AudioPosition.Left:
                source = leftAudioSource;
                break;
            case AudioPosition.Center:
                source = centerAudioSource;
                break;
            case AudioPosition.Right:
                source = rightAudioSource;
                break;
            case AudioPosition.Front:
                source = frontAudioSource;
                break;
            default:
                source = centerAudioSource;
                break;
        }
        foreach (string clip in clips) {
            source.clip = SoundBank.audioDictionary[clip];
            source.Play();
            while (source.isPlaying) yield return null;
        }
    }

    void PlayBGM(string clip) {
        StartCoroutine(PlayBGMCoroutine(clip));
    }

    IEnumerator PlayBGMCoroutine(string clip) {
        musicAudioSource.clip = SoundBank.audioDictionary[clip];
        while (true) {
            if (!musicAudioSource.isPlaying) musicAudioSource.Play();
            if (musicAudioSource.clip == null) StopCoroutine("PlayBGMCoroutine");
        }
    }

    void PlaySequence(string[] clips, AudioPosition position = AudioPosition.Center) {
        StartCoroutine(PlaySequenceCoroutine(clips, position));
    }

    void PlaySound(string clip, AudioPosition position = AudioPosition.Center) {
        string[] clipArray = {clip};
    }

    void Interrupt() {
        StopCoroutine("PlaySequenceCoroutine");
        source.Stop();
    }

}