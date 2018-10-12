using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction {
    public string name;
    public int staminaCost;
    public int range;
    public int effect;
    public EffectType effectType;
    public ModifierType modifier;
    public int modifierTurns;

    public CharacterAction(string name, int cost, int range, int effect, EffectType effectType) {
        this.name = name;
        this.staminaCost = cost;
        this.range = range;
        this.effect = effect;
        this.effectType = effectType;
    }

    public CharacterAction(string name, int cost, int range, int effect, ModifierType modifier, int turns) : this (name, cost, range, effect, EffectType.AddModifier) {
        this.modifier = modifier;
        this.modifierTurns = turns;
    }

    public void Apply(Character target) {
        // todo complete it
    }

}
