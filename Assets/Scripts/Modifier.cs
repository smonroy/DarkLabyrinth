using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier {
    public ModifierType type;
    public int value;
    public int duration; // turns remainders

    public Modifier(ModifierType type, int value, int duration) {
        this.type = type;
        this.value = value;
        this.duration = duration;
    }

    public void ExpendOneTurn() {
        if(duration > 1) {
            duration--;
        }
    }

    public bool IsExpired() {
        return duration <= 0;
    }

}
