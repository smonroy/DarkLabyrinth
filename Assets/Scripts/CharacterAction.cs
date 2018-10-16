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

    public bool Use(Character target) {
        if (!owner.moved) {
            if (owner.GetStamina() >= staminaCost) {
                int healthLost = target.ReduceHealth(effect);
                int staminaLost = owner.ReduceStamina(staminaCost);
                owner.moved = owner.isAlly;
                Debug.Log(owner.name + " attacks " + target.name + " with " + name + ", " + target.name + " lost " + healthLost + " health points" + 
                          (owner.isAlly ? ", " + owner.name + " lost " + staminaLost + " stamina points" : ""));
                if (target.IsDead()) {
                    Debug.Log(target.name + " is dead");
                }
                return true;
            }
            else {
                Debug.Log(owner.name + " can not attack because doesn't have enough stamina, current stamina: " + owner.GetStamina() + ", required stamina: " + staminaCost);
            }
        }
        else {
            Debug.Log(owner.name + " already has moved this turn.");
        }
        return false;
    }

    public void GetToast() {
        Debug.Log(name + " effect: " + effect + ", stamina cost: " + staminaCost);
    }

}
