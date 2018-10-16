using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelsConfiguration {

    public enum Difficulty { Easy, Medium, Hard };

    public static Room[] GetRooms(int level) {
        Room[] rooms = new Room[3];
        switch (level) {
            case 1:
                rooms[0] = null;
                rooms[1] = new Room(true, GetEnemies(1));
                rooms[2] = null;
                break;
            case 2:
                rooms[0] = null;
                rooms[1] = new Room(true, GetEnemies(2));
                rooms[2] = null;
                break;
        }
        return rooms;
    }


    public static Character[] GetEnemies(int level, Difficulty difficulty = Difficulty.Easy) {
        Character[] enemies;
        switch (level) {
            case 1:
                enemies = new Character[3];
                enemies[0] = new Character("rat", 10, 5, 0, 2, 5);
                enemies[0].SetAction(new CharacterAction("bite", 0, 1, 2, EffectType.MeleeAttack));
                enemies[1] = null;
                enemies[2] = new Character("bear", 20, 15, 0, 2, 5);
                enemies[2].SetAction(new CharacterAction("claw", 0, 1, 5, EffectType.MeleeAttack));
                return enemies;
            case 2:
                enemies = new Character[3];
                enemies[0] = new Character("wild dog", 22, 5, 0, 2, 5);
                enemies[0].SetAction(new CharacterAction("barth", 0, 1, 2, EffectType.MeleeAttack));
                enemies[1] = null;
                enemies[2] = new Character("bat", 10, 15, 0, 2, 5);
                enemies[2].SetAction(new CharacterAction("crazy eyes", 0, 1, 8, EffectType.MeleeAttack));
                return enemies;
        }
        return null;
    }

    public static Character GetNewAlly(int level)
    {
        switch (level)
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
