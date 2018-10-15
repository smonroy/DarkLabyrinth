using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SoundBank {

    public static Dictionary<String, AudioClip> audioDictionary;

    public static void LoadAudioDictionary() {
        Object[] audioClips;
        audioDictionary = new Dictionary<string, AudioClip>();

        audioClips = Resources.LoadAll("Audio", typeof(AudioClip));
        foreach (Object audioClip in audioClips) {
            audioDictionary.Add(audioClip.name, (AudioClip)audioClip);
        }
    }

}