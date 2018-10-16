using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
    private Character[] enemies;
    private bool open;
    private bool explored;

    public Room (bool open, Character[] enemies) {
        this.enemies = enemies;
        this.open = open;
        this.explored = false;
    }

    public void Explore() {
        this.explored = true;
    }

    public bool isOpen() {
        return this.open;
    }

    public bool isExplored() {
        return this.explored;
    }

    public Character[] GetEnemies() {
        return this.enemies;
    }

    public void GetToast() {
        if(explored) {
            Debug.Log("This path have beed already explored");
        } else {
            Character.GetRandomCharacter(enemies).GetToast("You peek at this path: ");
        }
    }


}
