﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    AudioManager audioManager;

    // game object collection
    [Serializable]
    public struct KeyObjectStructure { public NumpadKey numpadKey; public GameObject go; }
    public KeyObjectStructure[] keyGameObjects;

    public Text modeLabel;
    public Mode mode;
    public GameActions gameAction;

    Dictionary<NumpadKey, GameObject> keyMap;   // to map numpadKey to GameObject

    Queue<NumpadKey> sequenceKeys;
    Queue<KeyType> sequenceType;

    KeyType[][] validSequences;
    GameActions[] gamesActions;
    Dictionary<NumpadKey, KeyType> keyTypes;

    int room;
    Character[] allies;
    Battle battle;


    // Use this for initialization
    void Start () {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioManager>();

        keyMap = new Dictionary<NumpadKey, GameObject>();
        foreach (KeyObjectStructure ko in keyGameObjects)
        {
            keyMap.Add(ko.numpadKey, ko.go);
        }
        sequenceKeys = new Queue<NumpadKey>();
        sequenceType = new Queue<KeyType>();
        room = 1;
        allies = new Character[3];
        allies[0] = LevelsConfiguration.GetNewAlly(this.room);
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
        switch (mode) {
            case Mode.Battle:
                modeLabel.text = "Battle Mode";
                battle = new Battle(this.room, this.allies);
                break;
            case Mode.Menu:
                modeLabel.text = "Menu Mode";
                break;
            case Mode.Path:
                modeLabel.text = "Path Selection Mode";
                break;
        }
        validSequences = KeyboardConfiguration.GetValidSequences(mode);
        keyTypes = KeyboardConfiguration.GetKeyTypes(mode);
        gamesActions = KeyboardConfiguration.GetActions(mode);
    }

    void PressKey(NumpadKey numpadKey) {
        if (keyTypes.ContainsKey(numpadKey))
        {
            sequenceKeys.Enqueue(numpadKey);
            sequenceType.Enqueue(keyTypes[numpadKey]);

            bool complete;
            if (IsSequenceValid(out complete))
            {
                keyMap[numpadKey].GetComponent<KeyController>().PressDown(!complete);
            }
            else
            {
                ReleaseAllKeys();   // delete the current sequence
                sequenceKeys.Enqueue(numpadKey);
                sequenceType.Enqueue(keyTypes[numpadKey]);

                bool valid = IsSequenceValid(out complete);
                keyMap[numpadKey].GetComponent<KeyController>().PressDown(valid && !complete);
            }

            if (complete)
            {
                DoAction();
                ReleaseAllKeys();
            } else {
                GetToast();
            }
        }
    }

    void GetToast() {
        if(mode == Mode.Battle) {
            battle.GetToast(sequenceKeys);
        }
    }

    void DoAction() {
        Debug.Log(gameAction);
        switch (gameAction) {
            case GameActions.GoToMenuMode:
                ChangeMode(Mode.Menu);
                break;
            case GameActions.GoToBattleMode:
                ChangeMode(Mode.Battle);
                break;
        }
    }

    bool IsSequenceValid(out bool complete) {
        complete = false;
        for (int j = 0; j < validSequences.Length; j++) {
            int i;
            for(i = 0; i < sequenceType.Count && i < validSequences[j].Length; i++) {
                if(sequenceType.ToArray()[i] != validSequences[j][i]) {
                    break;
                }
            }
            if(i == sequenceType.Count) { // is valid
                gameAction = gamesActions[j];
                complete = (validSequences[j].Length == sequenceType.Count); // is complete
                return true;
            }
        }
        return false;
    }

    void ReleaseAllKeys(){
        foreach(NumpadKey numpadKey in sequenceKeys) {
            keyMap[numpadKey].GetComponent<KeyController>().Release();
        }
        sequenceKeys.Clear();
        sequenceType.Clear();
    }

}
