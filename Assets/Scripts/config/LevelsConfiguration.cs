using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelsConfiguration {

    public enum Difficulty { Easy, Medium, Hard };

    public static Room[] GetRooms(int level) {
        Room[] rooms = new Room[3];
        rooms[0] = null;
        rooms[1] = null;
        rooms[2] = null;
        int i = Random.Range(3, 6);
        int ii = (Random.Range(0, 2) * 2) - 1; // +1 or -1
        int p = Random.Range(1, 101);
        switch (level) {
            case 1:
            case 12:
                rooms[1] = new Room(true, GetEnemies(level));
                break;
            case 2:
            case 3:
                rooms[i % 3] = new Room(true, GetEnemies(level));
                break;
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                rooms[i % 3] = new Room(p < 40, GetEnemies(level, Difficulty.Easy)); i += ii;
                rooms[i % 3] = new Room(p >= 40, GetEnemies(level, Difficulty.Medium));
                break;
            case 11:
                rooms[i % 3] = new Room(p < 23, GetEnemies(level, Difficulty.Easy)); i += ii;
                rooms[i % 3] = new Room(p >= 23 && p < 57, GetEnemies(level, Difficulty.Medium)); i += ii;
                rooms[i % 3] = new Room(p >= 57, GetEnemies(level, Difficulty.Hard));
                break;
        }
        return rooms;
    }


    public static Character[] GetEnemies(int level, Difficulty difficulty = Difficulty.Easy) {
        Character[] enemies;
        enemies = new Character[3];
        enemies[0] = null;
        enemies[1] = null;
        enemies[2] = null;
        string enemySet = level.ToString();
        switch (difficulty) {
            case Difficulty.Easy: enemySet += "E"; break;
            case Difficulty.Medium: enemySet += "M"; break;
            case Difficulty.Hard: enemySet += "H"; break;
        }
        int i = Random.Range(3, 6);
        int ii = (Random.Range(0, 2) * 2) - 1; // +1 or -1
        switch (enemySet) {
            case "1E":
                enemies[1] = GetEnemy("Slime1");
                break;
            case "2E":
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "3E":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "4E":
                enemies[i % 3] = GetEnemy("Bee1");
                break;
            case "4M":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "5E":
                enemies[i % 3] = GetEnemy("Bee1");
                break;
            case "5M":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Bee1");
                break;
            case "6E":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "6M":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "7E":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Wolf1");
                break;
            case "7M":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Bee1");
                break;
            case "8E":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Bee1");
                break;
            case "8M":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Wolf1");
                break;
            case "9E":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Wolf1");
                break;
            case "9M":
                enemies[i % 3] = GetEnemy("Bear1");
                break;
            case "10E":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Bear1");
                break;
            case "10M":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Bear1");
                break;
            case "11E":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Wolf1"); i += ii;
                enemies[i % 3] = GetEnemy("Wolf1");
                break;
            case "11M":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Wolf1"); i += ii;
                enemies[i % 3] = GetEnemy("Wolf1");
                break;
            case "11H":
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Bee1"); i += ii;
                enemies[i % 3] = GetEnemy("Bear1");
                break;
            case "12E":
                enemies[1] = GetEnemy("Orc");
                break;

        }
        return enemies;
    }

    private static Character GetEnemy(string code) {
        Character enemy = null;
        CharacterAction[] actions = null;
        switch (code) {
            case "Slime1":
                enemy = new Character("Slime", 20, 5, 10, 0, 10);
                enemy.IncreaseStatsRandomly(5, 5, 0, 0, 10);
                actions = new CharacterAction[2];
                actions[0] = new CharacterAction("Squish", 0, 1, 5, EffectType.MeleeAttack);
                actions[1] = new CharacterAction("Spittle", 0, 1, 10, EffectType.MagicAttack);
                enemy.SetActions(actions);
                break;
            case "Bee1":
                enemy = new Character("Killer-bee", 20, 10, 5, 0, 10);
                enemy.IncreaseStatsRandomly(15, 0, 5, 0, 10);
                actions = new CharacterAction[2];
                actions[0] = new CharacterAction("Sting", 0, 1, 10, EffectType.MeleeAttack);
                actions[1] = new CharacterAction("Magic-pollen", 0, 1, 5, EffectType.MagicAttack);
                enemy.SetActions(actions);
                break;
            case "Wolf1":
                enemy = new Character("Wolf", 25, 10, 0, 0, 15);
                enemy.IncreaseStatsRandomly(15, 5, 0, 0, 10);
                enemy.SetAction(new CharacterAction("Bites", 0, 1, 10, EffectType.MeleeAttack));
                break;
            case "Bear1":
                enemy = new Character("Bear", 40, 10, 0, 0, 5);
                enemy.IncreaseStatsRandomly(15, 5, 0, 0, 10);
                enemy.SetAction(new CharacterAction("Claw", 0, 1, 10, EffectType.MeleeAttack));
                break;
            case "Orc":
                enemy = new Character("Orc", 80, 25, 15, 0, 20);
                actions[0] = new CharacterAction("Claw", 0, 1, 25, EffectType.MeleeAttack);
                actions[1] = new CharacterAction("Magic-roar", 0, 1, 15, EffectType.MagicAttack);
                enemy.SetActions(actions);
                break;
        }
        return enemy;
    }


    public static Character[] UpdateAllies(Character[] allies, int level) {
        switch (level) {
            case 1:
                allies[0] = new Character("Scotty", 70, 10, 8, 15, 20, 80, 8);
                CharacterAction[] scottyActions = new CharacterAction[3];
                scottyActions[0] = new CharacterAction("Knights-Knife", 10, 1, 15, EffectType.MeleeAttack);
                scottyActions[1] = new CharacterAction("Shining-Slashes", 8, 2, 10, EffectType.MeleeAttack);
                scottyActions[2] = new CharacterAction("Super-Sword", 20, 1, 20, EffectType.MeleeAttack);
                allies[0].SetActions(scottyActions);

                allies[1] = new Character("Dog", 20, 8, 10, 0, 40, 40, 5);
                CharacterAction[] dogActions = new CharacterAction[3];
                dogActions[0] = new CharacterAction("Bites", 8, 2, 10, EffectType.MeleeAttack);
                dogActions[1] = new CharacterAction("Magic-bark", 10, 1, 15, EffectType.MagicAttack);
                dogActions[2] = null;
                allies[1].SetActions(dogActions);
                break;
        }
        return allies;
    }

}
