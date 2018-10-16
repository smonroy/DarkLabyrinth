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
                rooms[0] = new Room(false, GetEnemies(1));
                rooms[1] = new Room(true, GetEnemies(1));
                rooms[2] = new Room(false, GetEnemies(1));
                break;
            case 2:
                rooms[i % 3] = new Room(true, GetEnemies(2));
                break;
            case 3:
                rooms[i % 3] = new Room(p < 40, GetEnemies(3, Difficulty.Easy)); i += ii;
                rooms[i % 3] = new Room(p >= 40, GetEnemies(3, Difficulty.Medium));
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
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "2E":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "3E":
                enemies[i % 3] = GetEnemy("Slime1");
                break;
            case "3M":
                enemies[i % 3] = GetEnemy("Slime1"); i += ii;
                enemies[i % 3] = GetEnemy("Slime1");
                break;
        }
        return enemies;
    }

    private static Character GetEnemy(string code) {
        Character enemy = null;
        switch (code) {
            case "Slime1":
                enemy = new Character("Slime", 20, 5, 10, 5, 10);
                enemy.IncreaseStatsRandomly(5, 5, 0, 5, 10);
                enemy.SetAction(new CharacterAction("Squish", 0, 1, 2, EffectType.MeleeAttack));
                break;
        }
        return enemy;
    }


    public static Character[] UpdateAllies(Character[] allies, int level) {
        switch (level) {
            case 1:
                allies[0] = new Character("Scotty", 70, 10, 2, 15, 20, 80, 8);
                CharacterAction[] scottyActions = new CharacterAction[3];
                scottyActions[0] = new CharacterAction("Knights-Knife", 10, 1, 15, EffectType.MeleeAttack);
                scottyActions[1] = new CharacterAction("Shining-Slashes", 8, 2, 10, EffectType.MeleeAttack);
                scottyActions[2] = new CharacterAction("Super-Sword", 20, 1, 20, EffectType.MeleeAttack);
                allies[0].SetActions(scottyActions);

                allies[1] = new Character("Dog", 20, 20, 2, 15, 50, 40, 4);
                CharacterAction[] dogActions = new CharacterAction[3];
                dogActions[0] = new CharacterAction("Claw", 10, 1, 15, EffectType.MeleeAttack);
                dogActions[1] = new CharacterAction("Bites", 8, 2, 10, EffectType.MeleeAttack);
                dogActions[2] = null;
                allies[1].SetActions(dogActions);
                break;
        }
        return allies;
    }

}
