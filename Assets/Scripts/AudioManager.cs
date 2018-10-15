using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource leftAudioSource;
    public AudioSource rightAudioSource;
    public AudioSource centerAudioSource;
    public AudioSource frontAudioSource;

    public Object[] audioClips;

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

}
