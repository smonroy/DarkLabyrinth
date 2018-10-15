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
                    {NumpadKey.N1Key, KeyType.StartNewGame},
                    {NumpadKey.ConfirmationKey, KeyType.Confirmation},
                };
                return menuTypes;
        }
        return null;
    }

    public static KeyType[][] GetValidSequences(Mode mode) {

        switch (mode)
        {
            case Mode.Battle:
                KeyType[][] battleSequences = {
                    new KeyType[] {KeyType.Menu, KeyType.Menu},
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
                    new KeyType[] {KeyType.StartNewGame, KeyType.Confirmation},
                };
                return menuSequences;

        }
        return null;

    }

    public static GameActions[] GetActions(Mode mode) {

        switch (mode)
        {
            case Mode.Battle:
                GameActions[] battleActions = { 
                    GameActions.GoToMenuMode,
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
                    GameActions.GoToBattleMode,
                };
                return menuActions;

        }
        return null;

    }

}
