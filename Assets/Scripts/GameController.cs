using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// remplazar por NumpadKey
public enum KeyId {
    TopLeft, TopCenter, TopRight,
    UpLeft, UpCenter, UpRight,
    MiddleLeft, MiddleCenter, MiddleRight,
    DownLeft, DownCenter, DownRight,
    BottomLeft, BottomRight,
    ExtremeRightBottom, ExtremeRightCenter, ExtremeRightTop, 
}

public enum KeyType { Ally, TargectedAction, Enemy, Confirmation, ExtraInformation, All, UntargetedAction, Menu}


public class GameController : MonoBehaviour {


    public static Dictionary<NumpadKey, KeyType>[] ModeTypes = {
        new Dictionary<NumpadKey, KeyType> {
            {NumpadKey.TopKey, KeyType.Menu},
            {NumpadKey.N1Key, KeyType.Ally},
            {NumpadKey.N2Key, KeyType.Ally},
            {NumpadKey.N3Key, KeyType.Ally},
            {NumpadKey.N4Key, KeyType.TargectedAction},
            {NumpadKey.N5Key, KeyType.TargectedAction},
            {NumpadKey.N6Key, KeyType.TargectedAction},
            {NumpadKey.N7Key, KeyType.Enemy},
            {NumpadKey.N8Key, KeyType.Enemy},
            {NumpadKey.N9Key, KeyType.Enemy},
            {NumpadKey.N0Key, KeyType.UntargetedAction},
            {NumpadKey.Period, KeyType.UntargetedAction},
            {NumpadKey.ConfirmationKey, KeyType.Confirmation},
            {NumpadKey.HelpKey, KeyType.ExtraInformation},
        }
    };

    //  TODO: multimode
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

    [Serializable]
    public struct KeyObjectStructure { public NumpadKey numpadKey; public GameObject go; }
    public KeyObjectStructure[] keyGameObjects;         // to configure all the key gameobjects

    public Mode mode;

    private Dictionary<NumpadKey, GameObject> keyMap;   // to map numpadKey to GameObject

    [Serializable]
    public struct KeySeqStructure { public NumpadKey numpadKey; public KeyType keyType; }
    private Queue<KeySeqStructure> sequence;            // active sequence


    // Use this for initialization
    void Start () {
        keyMap = new Dictionary<NumpadKey, GameObject>();
        foreach (KeyObjectStructure ko in keyGameObjects)
        {
            keyMap.Add(ko.numpadKey, ko.go);
        }
        sequence = new Queue<KeySeqStructure>();
        mode = Mode.Battle;
    }

    // Update is called once per frame
    void Update () {
        // numeric keys
        if (Input.GetKeyDown(KeyCode.Keypad0)) { PressKey(NumpadKey.N0Key); }
        if (Input.GetKeyDown(KeyCode.Keypad1)) { PressKey(NumpadKey.N1Key); }
        if (Input.GetKeyDown(KeyCode.Keypad2)) { PressKey(NumpadKey.N2Key); }
        if (Input.GetKeyDown(KeyCode.Keypad3)) { PressKey(NumpadKey.N3Key); }
        if (Input.GetKeyDown(KeyCode.Keypad4)) { PressKey(NumpadKey.N4Key); }
        if (Input.GetKeyDown(KeyCode.Keypad5)) { PressKey(NumpadKey.N5Key); }
        if (Input.GetKeyDown(KeyCode.Keypad6)) { PressKey(NumpadKey.N6Key); }
        if (Input.GetKeyDown(KeyCode.Keypad7)) { PressKey(NumpadKey.N7Key); }
        if (Input.GetKeyDown(KeyCode.Keypad8)) { PressKey(NumpadKey.N8Key); }
        if (Input.GetKeyDown(KeyCode.Keypad9)) { PressKey(NumpadKey.N9Key); }
        if (Input.GetKeyDown(KeyCode.KeypadPeriod)) { PressKey(NumpadKey.Period); }

        // confirmation key
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) { PressKey(NumpadKey.ConfirmationKey); }
        if (Input.GetKeyDown(KeyCode.Return)) { PressKey(NumpadKey.ConfirmationKey); }

        // helpkey
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) { PressKey(NumpadKey.HelpKey); }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) { PressKey(NumpadKey.HelpKey); }
        if (Input.GetKeyDown(KeyCode.Backspace)) { PressKey(NumpadKey.HelpKey); }

        // topkey
        if (Input.GetKeyDown(KeyCode.KeypadDivide)) { PressKey(NumpadKey.TopKey); }
        if (Input.GetKeyDown(KeyCode.KeypadMultiply)) { PressKey(NumpadKey.TopKey); }
        if (Input.GetKeyDown(KeyCode.Tab)) { PressKey(NumpadKey.TopKey); }
        if (Input.GetKeyDown(KeyCode.Numlock)) { PressKey(NumpadKey.TopKey); }
    }

    private void PressKey(NumpadKey numpadKey) {
        KeySeqStructure seqStep = new KeySeqStructure();
        seqStep.numpadKey = numpadKey;
        seqStep.keyType = ModeTypes[(int)mode][numpadKey];

        sequence.Enqueue(seqStep);
        bool complete;
        if (IsSequenceValid(out complete))
        { // check the sequence adding the new key
            keyMap[numpadKey].GetComponent<KeyController>().PressDown(!complete);
        }
        else
        {
            ReleaseAllKeys();
            sequence.Enqueue(seqStep);
            bool valid = IsSequenceValid(out complete); // new check only the new key as a new sequence
            keyMap[numpadKey].GetComponent<KeyController>().PressDown(valid && !complete);
        }
        if (complete)
        {
            string cad = "";
            foreach (KeySeqStructure ks in sequence)
            {
                cad += ks.numpadKey.ToString() + " (" + ks.keyType.ToString() + "), ";
            }
            Debug.Log(cad);
            ReleaseAllKeys();
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
        foreach(KeySeqStructure seqStep in sequence) {
            keyMap[seqStep.numpadKey].GetComponent<KeyController>().Release();
        }
        sequence.Clear();
    }

}
