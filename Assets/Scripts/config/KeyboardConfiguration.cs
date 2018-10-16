using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class KeyboardConfiguration {

    public static Dictionary<NumpadKey, KeyType> GetKeyTypes(Mode mode)
    {
        switch (mode)
        {
            case Mode.Battle:
                Dictionary<NumpadKey, KeyType> battleTypes = new Dictionary<NumpadKey, KeyType> {
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
                    {NumpadKey.HelpKey, KeyType.ExtraInformation}
                };
                return battleTypes;
            case Mode.Menu:
                Dictionary<NumpadKey, KeyType> menuTypes = new Dictionary<NumpadKey, KeyType> {
                    {NumpadKey.N1Key, KeyType.MenuOptionNewGame},
                    {NumpadKey.N2Key, KeyType.MenuOptionResumeGame},
                    {NumpadKey.ConfirmationKey, KeyType.Confirmation},
                };
                return menuTypes;
            case Mode.Path:
                Dictionary<NumpadKey, KeyType> pathTypes = new Dictionary<NumpadKey, KeyType> {
                    {NumpadKey.TopKey, KeyType.Menu},
                    {NumpadKey.N1Key, KeyType.Ally},
                    {NumpadKey.N2Key, KeyType.Ally},
                    {NumpadKey.N3Key, KeyType.Ally},
                    {NumpadKey.N7Key, KeyType.RoomSelection},
                    {NumpadKey.N8Key, KeyType.RoomSelection},
                    {NumpadKey.N9Key, KeyType.RoomSelection},
                    {NumpadKey.N5Key, KeyType.Exploration},
                    {NumpadKey.ConfirmationKey, KeyType.Confirmation},
                };
                return pathTypes;
        }
        return null;
    }

    public static KeyType[][] GetValidSequences(Mode mode) {

        switch (mode)
        {
            case Mode.Battle:
                KeyType[][] battleSequences = {
                    new KeyType[] {KeyType.Menu, KeyType.Confirmation},
                    new KeyType[] {KeyType.Ally, KeyType.TargectedAction, KeyType.Enemy, KeyType.Confirmation},
                    new KeyType[] {KeyType.Ally, KeyType.UntargetedAction, KeyType.Confirmation},
                    new KeyType[] {KeyType.ExtraInformation, KeyType.Ally},
                    new KeyType[] {KeyType.ExtraInformation, KeyType.TargectedAction},
                    new KeyType[] {KeyType.ExtraInformation, KeyType.UntargetedAction},
                    new KeyType[] {KeyType.ExtraInformation, KeyType.Enemy},
                    new KeyType[] {KeyType.ExtraInformation, KeyType.Confirmation},
                    new KeyType[] {KeyType.ExtraInformation, KeyType.ExtraInformation},
                    new KeyType[] {KeyType.ExtraInformation, KeyType.Menu},
                };
                return battleSequences;
            case Mode.Menu:
                KeyType[][] menuSequences = {
                    new KeyType[] {KeyType.MenuOptionNewGame, KeyType.Confirmation},
                    new KeyType[] {KeyType.MenuOptionResumeGame, KeyType.Confirmation},
                };
                return menuSequences;
            case Mode.Path:
                KeyType[][] pathSequences = {
                    new KeyType[] {KeyType.Menu, KeyType.Confirmation},
                    new KeyType[] {KeyType.RoomSelection, KeyType.Confirmation},
                    new KeyType[] {KeyType.Exploration},
                    new KeyType[] {KeyType.Ally}
                };
                return pathSequences;

        }
        return null;

    }

    public static GameActions[] GetActions(Mode mode) {

        switch (mode)
        {
            case Mode.Battle:
                GameActions[] battleActions = { 
                    GameActions.GoToMenu,
                    GameActions.AttackEnemy,
                    GameActions.UntargetAction,
                    GameActions.GetExtraInformation,
                    GameActions.GetExtraInformation,
                    GameActions.GetExtraInformation,
                    GameActions.GetExtraInformation,
                    GameActions.GetExtraInformation,
                    GameActions.GetExtraInformation,
                    GameActions.GetExtraInformation,
                };
                return battleActions;
            case Mode.Menu:
                GameActions[] menuActions = {
                    GameActions.NewGame,
                    GameActions.ResumeGame
                };
                return menuActions;
            case Mode.Path:
                GameActions[] pathActions = {
                    GameActions.GoToMenu,
                    GameActions.EnterNewBattleRoom,
                    GameActions.NoAction,
                    GameActions.NoAction
                };
                return pathActions;

        }
        return null;

    }

}
