using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction {
    public string name;
    public int staminaCost;
    public int range;
    public EffectType effectType;
    public ModifierType modifier;
    public int modifierTurns;

    public CharacterAction(string name, int cost, int range, EffectType effect) {
        this.name = name;
        this.staminaCost = cost;
        this.range = range;
        this.effectType = effect;
    }

    public CharacterAction(string name, int cost, int range, ModifierType modifier, int turns) : this (name, cost, range, EffectType.AddModifier) {
        this.modifier = modifier;
        this.modifierTurns = turns;
    }

    public void Apply(Character target) {
        // todo complete it
    }

}
