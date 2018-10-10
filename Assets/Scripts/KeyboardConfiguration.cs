using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyboardConfiguration {

    public static Dictionary<NumpadKey, KeyType> GetKeyTypes(Mode mode)
    {
        switch (mode)
        {
            case Mode.Battle:
                Dictionary<NumpadKey, KeyType> keyTypes = new Dictionary<NumpadKey, KeyType> {
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
                return keyTypes;
        }
        return null;
    }

    public static KeyType[][] GetValidSequences(Mode mode) {
        switch (mode)
        {
            case Mode.Battle:
                KeyType[][] validSequences = {
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
                return validSequences;
        }
        return null;

    }
}
