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

    public bool Use(Character target, AudioManager audioManager, Character defender = null) {
        if (owner.IsDead()) {
            audioManager.Play("Imposible-attack-because " + owner.name + " is-dead");
            return false;
        }
        if (target.IsDead()) {
            audioManager.Play("Imposible-attack-because " + target.name + " is-dead");
            return false;
        }
        if (owner.moved){
            audioManager.Play("Imposible-attack-because " + owner.name + " already-has-moved-this-turn");
            return false;
        }
        if (owner.GetStamina() < staminaCost) {
            audioManager.Play("Imposible-attack-because " + owner.name + " does-not-have-enough-stamina, stamina " + owner.GetStamina() + ", required-stamina " + staminaCost);
            return false;
        }

        // move is counted as done from this point
        owner.moved = owner.isAlly;

        int probability = Mathf.Clamp(50 + owner.GetSpeed() - target.GetSpeed(), 40, 90);
        int staminaLost = owner.ReduceStamina(staminaCost);
        if (Random.Range(1, 101) > probability) {
            audioManager.Play(owner.name + " failed-to-attack " + target.name);
            return true;
        }

        int damage = 0;
        if (effectType == EffectType.MeleeAttack) {
            damage = Mathf.Max(this.effect, owner.GetAttack());
        }
        if (effectType == EffectType.MagicAttack) {
            damage = Mathf.Max(this.effect, owner.GetMagic());
        }

        int defenderHealthLost = 0;
        if (defender != null) {
            defenderHealthLost = defender.ReduceHealth(damage / 2);
            damage -= defenderHealthLost;
        }
        int healthLost = target.ReduceHealth(damage);

        //audioManager.Play(owner.name + " attacks " + target.name + " with " + name + ", " + target.name + " lost " + healthLost + " health points, current-health " + target.GetHealth() +
                  //(owner.isAlly ? ", " + owner.name + " lost " + staminaLost + " stamina-points" : ""));
        audioManager.Play(owner.name + " attacks " + target.name + " with " + name + ", " + target.name + " current-health " + target.GetHealth());

        if (target.IsDead()) {
            audioManager.Play(target.name + " is-dead");
        }

        if (defender != null) {
            audioManager.Play(defender.name + " defends " + target.name + " and-absorbs-part-of-the-damage " + defenderHealthLost);
            if (defender.IsDead()) {
                audioManager.Play(defender.name + " is-dead");
            }
            defender.defending = false;
        }

        return true;
    }

    public void GetToast(AudioManager audioManager) {
        audioManager.Play("_" + name + " effect " + effect + ", stamina-cost " + staminaCost, "", true);
    }

}
