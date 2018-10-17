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
    Queue<string> audioQueue;
    AudioPosition lastPositionQueued;
    AudioPosition position;

    private AudioPosition Position {
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
        Position = AudioPosition.Center;
        lastPositionQueued = Position;
        Load();
        audioQueue = new Queue<string>();
        StartCoroutine(PlayQueue());
    }

    IEnumerator PlayQueue() {
        while (true) {
            if (audioQueue.Count != 0 && !source.isPlaying) {
                string nextSound = audioQueue.Dequeue();
                switch (nextSound) {
                    case "[front]": Position = AudioPosition.Front; break;
                    case "[left]": Position = AudioPosition.Left; ; break;
                    case "[right]": Position = AudioPosition.Right; break;
                    case "[center]": Position = AudioPosition.Center; break;
                    case "[space]": yield return new WaitForSeconds(0.2f); break;
                    default:
                        source.clip = SoundBank.GetSound(nextSound);
                        if (source.clip != null){
                            source.Play();
                            yield return new WaitForSeconds(source.clip.length);
                        }
                        break;
                }
            }
            yield return null;
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

    public void QueuePlay(string clips, AudioPosition pos = AudioPosition.Center, bool interrupt = true) {
        if (interrupt) {
            audioQueue.Clear();
            source.Stop();
        }
        if(lastPositionQueued != pos) {
            switch (pos) {
                case AudioPosition.Center: audioQueue.Enqueue("[center]"); break;
                case AudioPosition.Front: audioQueue.Enqueue("[front]"); break;
                case AudioPosition.Left: audioQueue.Enqueue("[left]"); break;
                case AudioPosition.Right: audioQueue.Enqueue("[right]"); break;
            }
            lastPositionQueued = pos;
        }
        List<string> playable = new List<string>(clips.Split(','));
        foreach (string s in playable) SplitSpaces(s);
    }

    void SplitSpaces(string s) {
        List<string> thing = new List<string>(s.Split(' '));
        foreach (string s1 in thing) {
            int val;
            if (Int32.TryParse(s1, out val)) {
                ReadNumbers(val);
            }
            else {
                audioQueue.Enqueue(s1);
                audioQueue.Enqueue("[space]");
            }
        }
        audioQueue.Enqueue("[space]");
    }

    void ReadNumbers(int val) {
        if (val <= 20) {
            audioQueue.Enqueue(val.ToString());
        }
        else {
            string valstring = val.ToString();
            string tens, units;
            if (valstring.Length == 3) {
                string hundreds = valstring.Substring(0, 1) + "00";
                tens = valstring.Substring(1, 1) + "0";
                units = valstring.Substring(2, 1);
                audioQueue.Enqueue(hundreds);
            }
            else {
                tens = valstring.Substring(0, 1) + "0";
                units = valstring.Substring(1, 1);
            }
            audioQueue.Enqueue(tens);
            audioQueue.Enqueue(units);
        }
        audioQueue.Enqueue("[space]");
    }

}