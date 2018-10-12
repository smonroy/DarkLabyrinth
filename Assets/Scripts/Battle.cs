using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle {

    public Character[] enemies;
    public Character[] allies;

    public Battle(int room, Character[] allies) {
        this.allies = allies;
        this.enemies = LevelsConfiguration.GetEnemies(room);
    }

    public void GetToast(Queue<NumpadKey> sequenceKeys) {
        string toast = "";
        int ally = 0;
        foreach (NumpadKey numpadKey in sequenceKeys)
        {

            switch (numpadKey)
            {
                case NumpadKey.N1Key:
                    if (allies[0] != null) { toast += allies[0].GetToast(); ally = 0; }
                    break;
                case NumpadKey.N2Key:
                    if (allies[1] != null) { toast += allies[1].GetToast(); ally = 1; }
                    break;
                case NumpadKey.N3Key:
                    if (allies[2] != null) { toast += allies[2].GetToast(); ally = 2; }
                    break;

                case NumpadKey.N4Key:
                    if (allies[ally].actions[0] != null) { toast += " " + allies[ally].GetActionToast(Character.Side.Left); }
                    break;
                case NumpadKey.N5Key:
                    if (allies[ally].actions[1] != null) { toast += " " + allies[ally].GetActionToast(Character.Side.Center); }
                    break;
                case NumpadKey.N6Key:
                    if (allies[ally].actions[2] != null) { toast += " " + allies[ally].GetActionToast(Character.Side.Right); }
                    break;

                case NumpadKey.N7Key:
                    if (enemies[0] != null) { toast += " " + enemies[0].GetToast(); }
                    break;
                case NumpadKey.N8Key:
                    if (enemies[1] != null) { toast += " " + enemies[1].GetToast(); }
                    break;
                case NumpadKey.N9Key:
                    if (enemies[2] != null) { toast += " " + enemies[2].GetToast(); }
                    break;
            }
        }
        if(toast.Length > 0) {
            Debug.Log(toast);
        }
    }
}
