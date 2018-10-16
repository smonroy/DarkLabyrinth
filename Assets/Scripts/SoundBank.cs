#region usings

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

public static class SoundBank {

    public static Dictionary<String, AudioClip> audioDictionary;

    public static void LoadAudioDictionary() {
        audioDictionary = new Dictionary<string, AudioClip>();

        Object[] audioClips = Resources.LoadAll("Audio", typeof(AudioClip));
        foreach (Object audioClip in audioClips) audioDictionary.Add(audioClip.name, (AudioClip) audioClip);
    }

    public static AudioClip GetSound(string clip) {
        if (audioDictionary.ContainsKey(clip)) {
            return audioDictionary[clip];
        }
        Debug.Log("Failed to find sound " + clip);
        return null;
    }

}