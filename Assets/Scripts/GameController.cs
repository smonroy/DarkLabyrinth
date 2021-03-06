﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    private AudioManager audioManager;

    // game object collection
    [Serializable]
    public struct KeyObjectStructure { public NumpadKey numpadKey; public GameObject go; }
    public KeyObjectStructure[] keyGameObjects;

    public Text modeLabel;
    public Mode mode;

    Dictionary<NumpadKey, GameObject> keyMap;   // to map numpadKey to GameObject

    Queue<NumpadKey> sequenceKeys;
    Queue<KeyType> sequenceType;

    KeyType[][] validSequences;
    GameActions[] gamesActions;
    Dictionary<NumpadKey, KeyType> keyTypes;

    private int level;
    private Room roomSelected;
    private Character[] allies;
    private Battle battle;
    private Path path;
    private Menu menu;


    // Use this for initialization
    void Start () {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioManager>();

        keyMap = new Dictionary<NumpadKey, GameObject>();
        foreach (KeyObjectStructure ko in keyGameObjects) {
            keyMap.Add(ko.numpadKey, ko.go);
        }
        sequenceKeys = new Queue<NumpadKey>();
        sequenceType = new Queue<KeyType>();
        menu = new Menu(audioManager);
        audioManager.Play("*game-introduction", "Welcome to the game");
        ChangeMode(Mode.Menu);
    }

    // Update is called once per frame
    void Update () {
        if(Input.anyKeyDown) {
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
    }



    void ReleaseAllKeys()
    {
        foreach (NumpadKey numpadKey in sequenceKeys) {
            keyMap[numpadKey].GetComponent<KeyController>().Release();
        }
        sequenceKeys.Clear();
        sequenceType.Clear();
    }

    private void PressKey(NumpadKey numpadKey) {
        if (!keyTypes.ContainsKey(numpadKey)) {
            return;
        }
        if (isEmpty(numpadKey)) {
            return;
        }

        GameActions gameAction;
        sequenceKeys.Enqueue(numpadKey);
        sequenceType.Enqueue(keyTypes[numpadKey]);

        bool complete;
        if (IsSequenceValid(out complete, out gameAction)) {
            keyMap[numpadKey].GetComponent<KeyController>().PressDown(!complete);
        } else {
            ReleaseAllKeys();   // delete the current sequence
            sequenceKeys.Enqueue(numpadKey);
            sequenceType.Enqueue(keyTypes[numpadKey]);

            bool valid = IsSequenceValid(out complete, out gameAction);
            keyMap[numpadKey].GetComponent<KeyController>().PressDown(valid && !complete);
        }

        if(!complete) {
            GetToast(numpadKey);
        }
        if (complete) {
            DoAction(gameAction);
            ReleaseAllKeys();
        }

    }

    private bool isEmpty(NumpadKey numpadKey) {
        switch(mode) {
            case Mode.Battle:
                return battle.isEmpty(numpadKey);
            case Mode.Path:
                return path.isEmpty(numpadKey);
        }
        return false;
    }

    private bool IsSequenceValid(out bool complete, out GameActions gameAction)
    {
        complete = false;
        gameAction = GameActions.NoAction;
        for (int j = 0; j < validSequences.Length; j++) {
            int i;
            for (i = 0; i < sequenceType.Count && i < validSequences[j].Length; i++) {
                if (sequenceType.ToArray()[i] != validSequences[j][i]) {
                    break;
                }
            }
            if (i == sequenceType.Count) { // is valid
                gameAction = gamesActions[j];
                complete = (validSequences[j].Length == sequenceType.Count); // is complete
                return true;
            }
        }
        return false;
    }

    private void GetToast(NumpadKey numpadKey) {
        if(mode == Mode.Battle) {
            battle.GetToast(numpadKey);
        }
        if (mode == Mode.Path) {
            path.GetToast(numpadKey);
        }
        if (mode == Mode.Menu) {
            menu.GetToast(numpadKey);
        }
    }

    private void DoAction(GameActions action) {
        audioManager.Interrupt();
        switch (action) {
            case GameActions.GoToMenu:
                menu.previousMode = mode;
                ChangeMode(Mode.Menu);
                break;
            case GameActions.NewGame:
                audioManager.Play("*new-game-started");
                level = 1;
                allies = new Character[3];
                path = new Path(this.level, allies, audioManager);
                ChangeMode(Mode.Path);
                break;
            case GameActions.ResumeGame:
                if (menu.previousMode != Mode.Menu) {
                    audioManager.Play("previous-game-resumed");
                    ChangeMode(menu.previousMode);
                }
                break;
            case GameActions.EnterNewBattleRoom:
                roomSelected = path.GetRoom(sequenceKeys);
                battle = new Battle(this.allies, roomSelected.GetEnemies(), audioManager);
                ChangeMode(Mode.Battle);
                break;
            case GameActions.AttackEnemy:
                battle.AttackEnemy(sequenceKeys.ToArray());
                TryFinishOfBattle();
                break;
            case GameActions.Recovering:
                battle.RecoverAlly(sequenceKeys.ToArray()[0]);
                TryFinishOfBattle();
                break;
            case GameActions.DeffendAllies:
                battle.DefendAllies(sequenceKeys.ToArray()[0]);
                TryFinishOfBattle();
                break;
            case GameActions.NoAction:
                GetToast(sequenceKeys.ToArray()[0]);
                break;
        }
    }

    private void TryFinishOfBattle(){
        if (battle.isTeamDied(battle.allies)) {
            audioManager.Play("game-over");
            menu.previousMode = Mode.Menu; // No game resume any more
            ChangeMode(Mode.Menu);
            return;
        }
        if (battle.isTeamDied(battle.enemies)) {
            audioManager.Play("all-enemies-are-defeated", "", true);
            this.allies = battle.allies;
            if (roomSelected.isOpen())
            {
                level++;
                path = new Path(this.level, this.allies, audioManager);
            } else {
                roomSelected.Explore();
                audioManager.Play("no-path", "There is no exit in this path, you need to return to the same level");
            }
            battle.ResetAllies();
            ChangeMode(Mode.Path);
        }
    }

    public void ChangeMode(Mode mode)
    {
        this.mode = mode;
        switch (mode) {
            case Mode.Battle:
                audioManager.PlayBGM("battleTheme");
                modeLabel.text = "Battle Mode";
                if(level == 1) {
                    audioManager.Play("*you-are-in-a-battle");
                } else {
                    audioManager.Play("_you-are-in-a-battle");
                }
                break;
            case Mode.Menu:
                audioManager.PlayBGM("exploringTheme");
                modeLabel.text = "Menu Mode";
                audioManager.Play("*you-are-in-the-menu");
                break;
            case Mode.Path:
                audioManager.PlayBGM("gameOverTheme");
                modeLabel.text = "Path Selection Mode";
                if (level == 13) {
                    audioManager.Play("end-demo", "", true);
                } else {
                    audioManager.Play("*you-are-in-the-level " + level);
                }
                break;
        }
        validSequences = KeyboardConfiguration.GetValidSequences(mode);
        keyTypes = KeyboardConfiguration.GetKeyTypes(mode);
        gamesActions = KeyboardConfiguration.GetActions(mode);
    }

}
