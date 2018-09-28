using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum KeyId {
    TopLeft, TopCenter, TopRight,
    UpLeft, UpCenter, UpRight,
    MiddleLeft, MiddleCenter, MiddleRight,
    DownLeft, DownCenter, DownRight,
    BottomLeft, BottomRight,
    ExtremeRightBottom, ExtremeRightCenter, ExtremeRightTop, 

}

public enum Mode { Battle, Menu }

public enum KeyType { Ally, TargectedAction, Enemy, Confirmation, ExtraInformation, All, UntargetedAction, Menu}


public class KeyboardDetector : MonoBehaviour {

    [Serializable]
    public struct KeyObjectStructure
    {
        public KeyId keyId;
        public GameObject go;
    }

    [Serializable]
    public struct KeyCodesStructure {
        public KeyCode code;
        public KeyId keyId;
    }

    [Serializable]
    public struct KeySeqStructure
    {
        public KeyId keyId;
        public KeyType keyType;
    }

    public static Dictionary<KeyId, KeyType>[] ModeTypes = {
        new Dictionary<KeyId, KeyType> {
            {KeyId.TopCenter, KeyType.Menu},
            {KeyId.DownLeft, KeyType.Ally},
            {KeyId.DownCenter, KeyType.Ally},
            {KeyId.DownRight, KeyType.Ally},
            {KeyId.MiddleLeft, KeyType.TargectedAction},
            {KeyId.MiddleCenter, KeyType.TargectedAction},
            {KeyId.MiddleRight, KeyType.TargectedAction},
            {KeyId.UpLeft, KeyType.Enemy},
            {KeyId.UpCenter, KeyType.Enemy},
            {KeyId.UpRight, KeyType.Enemy},
            {KeyId.BottomLeft, KeyType.UntargetedAction},
            {KeyId.BottomRight, KeyType.UntargetedAction},
            {KeyId.ExtremeRightBottom, KeyType.Confirmation},
            {KeyId.ExtremeRightCenter, KeyType.ExtraInformation},
        }
    };

    public static KeyType[][] validSequences = {
        new KeyType[] {KeyType.Ally, KeyType.TargectedAction, KeyType.Enemy, KeyType.Confirmation},
        new KeyType[] {KeyType.Ally, KeyType.UntargetedAction, KeyType.Confirmation},
        new KeyType[] {KeyType.ExtraInformation, KeyType.Ally},
        new KeyType[] {KeyType.ExtraInformation, KeyType.TargectedAction},
        new KeyType[] {KeyType.ExtraInformation, KeyType.UntargetedAction},
        new KeyType[] {KeyType.ExtraInformation, KeyType.Enemy},
        new KeyType[] {KeyType.ExtraInformation, KeyType.Confirmation},
        new KeyType[] {KeyType.ExtraInformation, KeyType.ExtraInformation},
        new KeyType[] {KeyType.ExtraInformation, KeyType.Menu},
        new KeyType[] {KeyType.Ally, KeyType.UntargetedAction, KeyType.Ally, KeyType.Confirmation},
        new KeyType[] {KeyType.Menu, KeyType.Menu}
    };

    public KeyObjectStructure[] keyObjects;
    public KeyCodesStructure[] keyCodes;


    private Dictionary<KeyId, GameObject> keyMap;
    private Queue<KeySeqStructure> sequence;


    // Use this for initialization
    void Start () {
        keyMap = new Dictionary<KeyId, GameObject>();
        foreach (KeyObjectStructure ko in keyObjects)
        {
            keyMap.Add(ko.keyId, ko.go);
        }
        sequence = new Queue<KeySeqStructure>();
    }

    // Update is called once per frame
    void Update () {
        foreach(KeyCodesStructure kc in keyCodes) {
            if(Input.GetKeyDown(kc.code)) {

                KeySeqStructure kss = new KeySeqStructure();
                kss.keyId = kc.keyId;
                kss.keyType = ModeTypes[(int)Mode.Battle][kc.keyId];

                sequence.Enqueue(kss);
                bool complete;
                if(IsSequenceValid(out complete)) { // check the sequence adding the new key
                    keyMap[kc.keyId].GetComponent<KeyController>().PressDown(!complete);
                } else {
                    ReleaseAllKeys();
                    sequence.Enqueue(kss);
                    bool valid = IsSequenceValid(out complete); // new check only the new key as a new sequence
                    keyMap[kc.keyId].GetComponent<KeyController>().PressDown(valid && !complete);
                }
                if (complete) {
                    string cad = "";
                    foreach(KeySeqStructure ks in sequence) {
                        cad += ks.keyId.ToString() + " (" + ks.keyType.ToString() + "), ";
                    }
                    Debug.Log(cad);
                    ReleaseAllKeys();
                }
            }
        }
	}

    bool IsSequenceValid(out bool complete) {
        complete = false;
        foreach (KeyType[] seq in validSequences) {
            int i;
            for(i = 0; i < sequence.Count && i < seq.Length; i++) {
                if(sequence.ToArray()[i].keyType != seq[i]) {
                    break;
                }
            }
            if(i == sequence.Count) { // is valid
                complete = (seq.Length == sequence.Count); // is complete
                return true;
            }
        }
        return false;
    }

    void ReleaseAllKeys(){
        foreach(KeySeqStructure kss in sequence) {
            keyMap[kss.keyId].GetComponent<KeyController>().Release();
        }
        sequence.Clear();
    }

}
