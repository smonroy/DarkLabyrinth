using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    // game object collection
    [Serializable]
    public struct KeyObjectStructure { public NumpadKey numpadKey; public GameObject go; }
    public KeyObjectStructure[] keyGameObjects;

    public Text modeLabel;
    public Mode mode;

    private Dictionary<NumpadKey, GameObject> keyMap;   // to map numpadKey to GameObject

    [Serializable]
    private struct KeySeqStructure { public NumpadKey numpadKey; public KeyType keyType; }
    private Queue<KeySeqStructure> sequence;            // active sequence

    private KeyType[][] validSequences;
    private Dictionary<NumpadKey, KeyType> keyTypes;


    // Use this for initialization
    void Start () {
        keyMap = new Dictionary<NumpadKey, GameObject>();
        foreach (KeyObjectStructure ko in keyGameObjects)
        {
            keyMap.Add(ko.numpadKey, ko.go);
        }
        sequence = new Queue<KeySeqStructure>();
        ChangeMode(Mode.Battle);
    }

    // Update is called once per frame
    void Update () {
        // numpad numbers
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

        // Alpha numbers
        if (Input.GetKeyDown(KeyCode.Alpha0)) { PressKey(NumpadKey.N0Key); }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { PressKey(NumpadKey.N1Key); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { PressKey(NumpadKey.N2Key); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { PressKey(NumpadKey.N3Key); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { PressKey(NumpadKey.N4Key); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { PressKey(NumpadKey.N5Key); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { PressKey(NumpadKey.N6Key); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { PressKey(NumpadKey.N7Key); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { PressKey(NumpadKey.N8Key); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { PressKey(NumpadKey.N9Key); }
        if (Input.GetKeyDown(KeyCode.Period)) { PressKey(NumpadKey.Period); }

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

    public void ChangeMode(Mode mode) {
        this.mode = mode;
        validSequences = KeyboardConfiguration.GetValidSequences(mode);
        keyTypes = KeyboardConfiguration.GetKeyTypes(mode);
        switch (mode) {
            case Mode.Battle:
                modeLabel.text = "Battle Mode";
                break;
            case Mode.Menu:
                modeLabel.text = "Menu Mode";
                break;
            case Mode.Path:
                modeLabel.text = "Path Selection Mode";
                break;
        }
    }

    private void PressKey(NumpadKey numpadKey) {
        KeySeqStructure seqStep = new KeySeqStructure(); // for a new sequence step
        seqStep.numpadKey = numpadKey;
        seqStep.keyType = keyTypes[numpadKey];

        sequence.Enqueue(seqStep);  // add the new step to the current sequece
        bool complete;
        if (IsSequenceValid(out complete))
        { 
            keyMap[numpadKey].GetComponent<KeyController>().PressDown(!complete);
        }
        else
        {
            ReleaseAllKeys();   // delete the current sequence
            sequence.Enqueue(seqStep);
            bool valid = IsSequenceValid(out complete);
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
