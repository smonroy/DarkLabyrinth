using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle {

    public Character[] enemies;
    public Character[] allies;

    private Character lastAllyToasted;
    private AudioManager audioManager;

    public Battle(Character[] allies, Character[] enemies, AudioManager audioManager) {
        this.allies = allies;
        this.enemies = enemies;
        this.audioManager = audioManager;
    }

    public void AttackEnemy(NumpadKey[] sequenceKeys) {
        Character ally = null;
        CharacterAction weapon = null;
        Character enemy = null;
        foreach (NumpadKey numpadKey in sequenceKeys) {
            switch (numpadKey) {
                case NumpadKey.N1Key: ally = allies[0]; break;
                case NumpadKey.N2Key: ally = allies[1]; break;
                case NumpadKey.N3Key: ally = allies[2]; break;
                case NumpadKey.N4Key: weapon = ally.actions[0]; break;
                case NumpadKey.N5Key: weapon = ally.actions[1]; break;
                case NumpadKey.N6Key: weapon = ally.actions[2]; break;
                case NumpadKey.N7Key: enemy = enemies[0]; break;
                case NumpadKey.N8Key: enemy = enemies[1]; break;
                case NumpadKey.N9Key: enemy = enemies[2]; break;
            }
        }
        if(weapon.Use(enemy)) {
            if (TeamNothingToDo(allies)) {
                ResetAlliesMoved();
                EnemiesTurn();
                RecoverAllies();
            }
        }
    }

    private void EnemiesTurn() {
        foreach(Character enemy in enemies) {
            if (enemy != null && !enemy.IsDead()) {
                int weapon = Random.Range(0, enemy.actions.Length);
                Character ally = Character.GetRandomCharacter(allies);
                enemy.actions[weapon].Use(ally);
                if (isTeamDied(allies)) {
                    return;
                }
            }
        }
        
    }

    private bool TeamNothingToDo(Character[] team) {
        foreach (Character member in team) {
            if(member != null) {
                if (!member.IsDead() && !member.moved) {
                    return false;
                }
            }
        }
        return true;
    }

    public bool isTeamDied(Character[] group) {
        foreach (Character member in group) {
            if (member != null) {
                if (!member.IsDead()){
                    return false;
                }
            }
        }
        return true;
    }

    public void ResetAllies() {
        foreach (Character ally in allies) {
            if (ally != null) {
                ally.moved = false;
                ally.ResetStamina();
                ally.ResetHealth();
            }
        }
    }

    private void ResetAlliesMoved() {
        foreach (Character ally in allies) {
            if(ally != null && ally.moved) {
                ally.moved = false;
            }
        }
    }

    public void RecoverAllies() {
        foreach (Character ally in allies) {
            if (ally != null) {
                ally.RecoverStamina();
                ally.RecoverHealth();
            }
        }
    }

    public bool isEmpty(NumpadKey numpadKey) {
        switch (numpadKey) {
            case NumpadKey.N1Key: return allies[0] == null;
            case NumpadKey.N2Key: return allies[1] == null;
            case NumpadKey.N3Key: return allies[2] == null;
            case NumpadKey.N4Key: return lastAllyToasted == null || lastAllyToasted.actions[0] == null;
            case NumpadKey.N5Key: return lastAllyToasted == null || lastAllyToasted.actions[1] == null;
            case NumpadKey.N6Key: return lastAllyToasted == null || lastAllyToasted.actions[2] == null;
            case NumpadKey.N7Key: return enemies[0] == null;
            case NumpadKey.N8Key: return enemies[1] == null;
            case NumpadKey.N9Key: return enemies[2] == null;
        }
        return false;
    }

    public void GetToast(NumpadKey numpadKey) {
        Character newAllyToasted = null;
        if (isEmpty(numpadKey)) {
            Debug.Log("is empty");
        } else {
            audioManager.QueuePlay("confirm confirm", AudioPosition.Left);
            audioManager.QueuePlay("heal1", AudioPosition.Right, false);
            switch (numpadKey) {
                case NumpadKey.TopKey: Debug.Log("Menu option"); break;
                case NumpadKey.N1Key: newAllyToasted = allies[0]; newAllyToasted.GetToast(); break;
                case NumpadKey.N2Key: newAllyToasted = allies[1]; newAllyToasted.GetToast(); break;
                case NumpadKey.N3Key: newAllyToasted = allies[2]; newAllyToasted.GetToast(); break;
                case NumpadKey.N4Key: lastAllyToasted.actions[0].GetToast(); break;
                case NumpadKey.N5Key: lastAllyToasted.actions[1].GetToast(); break;
                case NumpadKey.N6Key: lastAllyToasted.actions[2].GetToast(); break;
                case NumpadKey.N7Key: enemies[0].GetToast(); break;
                case NumpadKey.N8Key: enemies[1].GetToast(); break;
                case NumpadKey.N9Key: enemies[2].GetToast(); break;
            }
        }
        lastAllyToasted = newAllyToasted;
    }
}
