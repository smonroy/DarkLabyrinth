using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
    public readonly string name;

    private int maxHealth;
    private int maxStamina;
    private int currentHealth;
    private int attack;
    private int magic;
    private int defense;
    private int speed;
    private int currentStamina;
    private int staminaRecovery;
    private List<Modifier> modifiers;

    public Character(string name, int health, int attack, int magic, int defense, int speed, int stamina, int staminaRecovery) {
        this.name = name;
        this.maxHealth = health;
        this.currentHealth = health;
        this.attack = attack;
        this.magic = magic;
        this.defense = defense;
        this.speed = speed;
        this.maxStamina = stamina;
        this.currentStamina = stamina;
        this.staminaRecovery = staminaRecovery;
        this.modifiers = new List<Modifier>();
    }


    public void AddModifier(ModifierType type, int value, int duration) {
        Modifier modif = modifiers.Find((Modifier obj) => obj.type == type);
        if (modif != null)
        {
            modif.value = value;
            modif.duration += duration;
        }
        else
        {
            modifiers.Add(new Modifier(type, value, duration));
        }
    }

    public void ClearModifiers(){
        modifiers.Clear();
    }

    public void RemoveModifier(ModifierType type) {
        if (modifiers.Exists((Modifier obj) => obj.type == type))
        {
            modifiers.Remove(modifiers.Find((Modifier obj) => obj.type == type));
        }
    }

    private int GetModifierValue(ModifierType type) {
        if(modifiers.Exists((Modifier obj) => obj.type == type)) {
            return modifiers.Find((Modifier obj) => obj.type == type).value;
        } else {
            return 0;
        }
    }

    private void ConsumeModifiersTurn() {
        List<Modifier> expired = new List<Modifier>();
        foreach (Modifier mod in modifiers) {
            mod.ExpendOneTurn();
            if(mod.IsExpired()) {
                expired.Add(mod);
            }
        }

        foreach(Modifier mod in expired) {
            modifiers.Remove(mod);
        }
    }

    public int GetHealth() {
        return this.currentHealth;
    }

    public int GetAttack() {
        return Mathf.Max(0, attack
                         + GetModifierValue(ModifierType.AttackUp)
                         - GetModifierValue(ModifierType.AttackDown));
    }

    public int GetMagic()
    {
        return Mathf.Max(0, magic 
                         + GetModifierValue(ModifierType.MagicUp)
                         - GetModifierValue(ModifierType.MagicDown));

    }

    public int GetDefense()
    {
        return Mathf.Max(0, defense
                         + GetModifierValue(ModifierType.DefenceUp)
                         - GetModifierValue(ModifierType.DefenceDown));

    }

    public int GetSpeed() {
        return Mathf.Max(0, this.speed
                         + GetModifierValue(ModifierType.SpeedUp)
                         - GetModifierValue(ModifierType.SpeedDown));
    }

    public int GetStaminaRecovery()
    {
        return Mathf.Max(0, this.staminaRecovery
                         + GetModifierValue(ModifierType.StaminaRegenUp)
                         - GetModifierValue(ModifierType.StaminaRegenDwon));
    }

    public void ReduceStamina(int expendStamina) {
        currentStamina = Mathf.Max(0, currentStamina -= expendStamina);
    }

    public void RecoverStamina() {
        currentStamina = Mathf.Min(maxStamina, currentStamina += GetStaminaRecovery());
    }

    public void ResetStamina() {
        currentStamina = maxStamina;
    }

    public void ReduceHealth(int healthLess) {
        currentHealth = Mathf.Max(0, this.currentHealth -= healthLess);
    }

    public void RecoverHealth(int healthRecovered) {
        currentHealth = Mathf.Min(maxHealth, currentHealth += healthRecovered);
    }

    public void ResetHealth() {
        currentHealth = maxHealth;
    }

    public bool CanMove() {
        return GetModifierValue(ModifierType.LoseTurns) == 0;
    }

    public bool IsDead() {
        return currentHealth == 0;
    }

}
