using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelsConfiguration {

    public enum Difficulty { Easy, Medium, Hard };

    public static Character[] GetEnemies(int room, Difficulty difficulty = Difficulty.Easy) {
        Character[] enemies;
        switch (room) {
            case 1:
                enemies = new Character[3];
                enemies[0] = new Character("rat", 10, 5, 0, 2, 5);
                enemies[1] = null;
                enemies[2] = new Character("bear", 20, 15, 0, 2, 5);
                return enemies;
        }
        return null;
    }

    public static Character GetNewAlly(int room)
    {
        switch (room)
        {
            case 1:
                Character scotty = new Character("Scotty", 70, 10, 2, 15, 20, 80, 10);
                CharacterAction[] scottyActions = new CharacterAction[3];
                scottyActions[0] = new CharacterAction("Knight’s Knife", 10, 1, 15, EffectType.MeleeAttack);
                scottyActions[1] = new CharacterAction("Shining Slashes", 10, 2, 10, EffectType.MeleeAttack);
                scottyActions[2] = new CharacterAction("Super Sword", 20, 1, 20, EffectType.MeleeAttack);
                scotty.SetActions(scottyActions);
                return scotty;
        }
        return null;
    }

}
