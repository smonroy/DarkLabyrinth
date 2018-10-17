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
    public Character owner;

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

    public bool Use(Character target, AudioManager audioManager) {
        if (target.IsDead()) {
            audioManager.Play("Imposible-attack-because " + target.name + " is-dead");
            return false;
        }
        if (owner.moved){
            audioManager.Play("Imposible-attack-because " + owner.name + " already-has-moved-this-turn");
            return false;
        }
        if (owner.GetStamina() < staminaCost) {
            audioManager.Play("Imposible-attack-because " + owner.name + " does-not-have-enough-stamina, current-stamina " + owner.GetStamina() + ", required-stamina " + staminaCost);
            return false;
        }

        int healthLost = target.ReduceHealth(effect);
        int staminaLost = owner.ReduceStamina(staminaCost);
        owner.moved = owner.isAlly;
        audioManager.Play(owner.name + " attacks " + target.name + " with " + name + ", " + target.name + " lost " + healthLost + " health points, current-health " + target.GetHealth() + 
                  (owner.isAlly ? ", " + owner.name + " lost " + staminaLost + " stamina-points" : ""));
        if (target.IsDead()) {
            audioManager.Play(target.name + " is-dead");
        }

        return true;
    }

    public void GetToast(AudioManager audioManager) {
        audioManager.Play(name + " effect " + effect + ", stamina-cost " + staminaCost);
    }

}
