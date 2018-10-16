#region usings

using System;
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
    Queue<AudioClip> audioQueue;
    AudioPosition position;

    public AudioPosition Position {
        get { return position; }
        set {
            position = value;
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
        }
    }

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
        audioQueue = new Queue<AudioClip>();
        StartCoroutine(PlayQueue());
    }

    IEnumerator PlaySequenceCoroutine(string[] clips, AudioPosition position) {
        foreach (string clip in clips) {
            source.clip = SoundBank.GetSound(clip);
            if (source.clip != null)
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

    void QueuePlay(string clips, AudioPosition pos, bool interrrupt = false) {
        Position = pos;


        List<string> playable = new List<string>(clips.Split(','));
        foreach (string s in playable) SplitSpaces(s);
        audioQueue.Enqueue(SoundBank.GetSound("space"));
    }

    void SplitSpaces(string s) {
        List<string> thing = new List<string>(s.Split(' '));
        foreach (string s1 in thing) {
            int val;
            if (Int32.TryParse(s1, out val)) {
                ReadNumbers(val);
            }
            else {
                audioQueue.Enqueue(SoundBank.GetSound(s1));
                audioQueue.Enqueue(SoundBank.GetSound("space"));
            }
        }
        audioQueue.Enqueue(SoundBank.GetSound("space"));
    }

    void ReadNumbers(int val) {
        if (val <= 20) {
            audioQueue.Enqueue(SoundBank.GetSound(val.ToString()));
        }
        else {
            string valstring = val.ToString();
            string tens, units;
            if (valstring.Length == 3) {
                string hundreds = valstring.Substring(0, 1) + "00";
                tens = valstring.Substring(1, 1) + "0";
                units = valstring.Substring(2, 1);
                audioQueue.Enqueue(SoundBank.GetSound(hundreds));
            }
            else {
                tens = valstring.Substring(0, 1) + "0";
                units = valstring.Substring(1, 1);
            }
            audioQueue.Enqueue(SoundBank.GetSound(tens));
            audioQueue.Enqueue(SoundBank.GetSound(units));
        }
        audioQueue.Enqueue(SoundBank.GetSound("space"));
    }

    void PlaySound(string clip, AudioPosition position = AudioPosition.Center) {
        string[] clipArray = {clip};
    }

    void Interrupt() {
        StopCoroutine("PlaySequenceCoroutine");
        source.Stop();
    }

}