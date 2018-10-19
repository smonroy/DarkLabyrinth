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
    bool lowPriority;           // if the clip is low priority any other not low-priority clip can interrupt it automatically

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

    void Load() {
        SoundBank.LoadAudioDictionary();
        //foreach (KeyValuePair<string, AudioClip> keyValuePair in SoundBank.audioDictionary) {
        //    centerAudioSource.clip = keyValuePair.Value;
        //    centerAudioSource.Play();
        //}
    }

    void Awake() {
        Position = AudioPosition.Center;
        lastPositionQueued = Position;
        lowPriority = false;
        Load();
        audioQueue = new Queue<string>();
        StartCoroutine(PlayQueue());
    }

    IEnumerator PlayQueue() {
        while (true) {
            bool areThereClipsHighPriority = false; 
            foreach (string clip in audioQueue.ToArray()) {
                if (clip.Substring(0, 1) != "[") {
                    if (clip.Substring(0, 1) != "*") {
                        areThereClipsHighPriority = true;
                        break;
                    }
                }
            }

            if (audioQueue.Count != 0 && (!source.isPlaying || (lowPriority && areThereClipsHighPriority))) {
                if(source.isPlaying) {
                    source.Stop();
                }
                string nextSound = audioQueue.Dequeue();
                switch (nextSound) {
                    case "[front]": Position = AudioPosition.Front; break;
                    case "[left]": Position = AudioPosition.Left; ; break;
                    case "[right]": Position = AudioPosition.Right; break;
                    case "[center]": Position = AudioPosition.Center; break;
                    case "[space]": yield return new WaitForSeconds(0.1f); break;
                    default:
                        lowPriority = (nextSound.Substring(0, 1) == "*"); // if the clip is low priority any other not low-priority clip can interrupt it automatically
                        if (lowPriority) { 
                            nextSound = nextSound.Substring(1);
                        } 
                        source.clip = SoundBank.GetSound(nextSound);
                        if (source.clip != null){
                            source.Play();
                            //if (!lowPriority) {
                            //    yield return new WaitForSeconds(source.clip.length);
                            //}
                        }
                        break;
                }
            }
            yield return null;
        }
    }

    private string GetNextClipString() {
        foreach(string clip in audioQueue.ToArray()) {
            if(clip.Substring(0,1) != "[") {
                return clip;
            }
        }
        return null;
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

    public void Play(string clips, string message = "", bool interrupt = false, AudioPosition pos = AudioPosition.Center) {
        bool lowP = false;
        if(message != "") {
            Debug.Log(message);
        } else {
            Debug.Log(clips.Replace("-", " ").Replace("_","").Replace("*",""));
        }
        if (interrupt) Interrupt();
        if(lastPositionQueued != pos) {
            switch (pos) {
                case AudioPosition.Center: audioQueue.Enqueue("[center]"); break;
                case AudioPosition.Front: audioQueue.Enqueue("[front]"); break;
                case AudioPosition.Left: audioQueue.Enqueue("[left]"); break;
                case AudioPosition.Right: audioQueue.Enqueue("[right]"); break;
            }
            lastPositionQueued = pos;
        }
        lowP = (clips.Substring(0, 1) == "*");
        if(lowP) clips = clips.Substring(1);

        List<string> playable = new List<string>(clips.Split(','));
        foreach (string s in playable) SplitSpaces(s.Trim(), lowP);
    }

    void SplitSpaces(string s, bool lowP) {
        List<string> thing = new List<string>(s.Split(' '));
        foreach (string s1 in thing) {
            int val;
            if (Int32.TryParse(s1, out val)) {
                ReadNumbers(val, lowP);
            }
            else {
                audioQueue.Enqueue((lowP ? "*": "") + s1);
                audioQueue.Enqueue("[space]");
            }
        }
        audioQueue.Enqueue("[space]");
    }

    void ReadNumbers(int val, bool lowP) {
        string valstring = val.ToString();
        string tens, units;
        if (valstring.Length == 3) {
            string hundreds = valstring.Substring(0, 1) + "00";
            audioQueue.Enqueue((lowP ? "*" : "") + hundreds);
            valstring = valstring.Substring(1, 2);
            val = val % 100;
        }
        if (val > 0) {
            valstring = val.ToString();
            if (val <= 20) {
                audioQueue.Enqueue((lowP ? "*" : "") + valstring);
            } else {
                tens = valstring.Substring(0, 1) + "0";
                units = valstring.Substring(1, 1);
                audioQueue.Enqueue((lowP ? "*" : "") + tens);
                if (units != "0") {
                    audioQueue.Enqueue((lowP ? "*" : "") + units);
                }
            }
        }
        audioQueue.Enqueue("[space]");
    }

    public void Interrupt() {
        audioQueue.Clear();
        source.Stop();
    }
}