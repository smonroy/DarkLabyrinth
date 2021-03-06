﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {
    private AudioManager audioManager;
    public Room[] rooms;
    public Character[] allies;
    public int level;


    public Path(int level, Character[] allies, AudioManager audioManager) {
        this.audioManager = audioManager;
        this.level = level;
        this.allies = LevelsConfiguration.UpdateAllies(allies, this.level);
        this.rooms = LevelsConfiguration.GetRooms(level);
    }

    public Character[] GetEnemies(int room) {
        return rooms[room].GetEnemies();
    }

    public bool isEmpty(NumpadKey numpadKey) {
        switch (numpadKey) {
            case NumpadKey.N1Key: return allies[0] == null;
            case NumpadKey.N2Key: return allies[1] == null;
            case NumpadKey.N3Key: return allies[2] == null;
            case NumpadKey.N7Key: return rooms[0] == null;
            case NumpadKey.N8Key: return rooms[1] == null;
            case NumpadKey.N9Key: return rooms[2] == null;
        }
        return false;
    }

    public void GetToast(NumpadKey numpadKey) {
        if (isEmpty(numpadKey)) {
            audioManager.Play("is-empty");
        }
        else {
            switch (numpadKey) {
                case NumpadKey.TopKey: audioManager.Play("menu-option"); break;
                case NumpadKey.N1Key: allies[0].GetToast(audioManager, AudioPosition.Left); break;
                case NumpadKey.N2Key: allies[1].GetToast(audioManager, AudioPosition.Center); break;
                case NumpadKey.N3Key: allies[2].GetToast(audioManager, AudioPosition.Right); break;
                case NumpadKey.N5Key: LevelDescription(); break;
                case NumpadKey.N7Key: rooms[0].GetToast(audioManager, AudioPosition.Left); break;
                case NumpadKey.N8Key: rooms[1].GetToast(audioManager, AudioPosition.Center); break;
                case NumpadKey.N9Key: rooms[2].GetToast(audioManager, AudioPosition.Right); break;
            }
        }
    }

    public Room GetRoom(Queue<NumpadKey> sequence) {
        switch (sequence.ToArray()[0]) {    // take the first key of the sequence
            case NumpadKey.N7Key: return rooms[0];
            case NumpadKey.N8Key: return rooms[1];
            case NumpadKey.N9Key: return rooms[2];
        }
        return null;
    }

    public void LevelDescription() {
        int roomNum = 0;
        int roomExplored = 0;
        foreach (Room room in rooms) {
            if(room != null) {
                roomNum++;
                if(room.isExplored()) {
                    roomExplored++;
                }
            }
        }
        // audioManager.Play("level " + level + ", there-are " + roomNum + " possible-paths, " + roomExplored + " of-them-have-been-explored" ,"", true);
        audioManager.Play("level " + level + ", there-are " + roomNum + " possible-paths", "", true);
    }

}
