using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
    public enum Side { Left, Center, Right, Random };
    public readonly string name;
    public CharacterAction[] actions;
    public bool isAlly;
    public bool moved;

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

    public Character(string name, int health, int attack, int magic, int defense, int speed) {
        this.name = name;
        this.maxHealth = health;
        this.currentHealth = health;
        this.attack = attack;
        this.magic = magic;
        this.defense = defense;
        this.speed = speed;
        this.isAlly = false;
        this.maxStamina = -1;
        this.currentStamina = -1;
        this.staminaRecovery = 0;
        this.modifiers = new List<Modifier>();
        this.moved = false;
    }

    public Character(string name, int health, int attack, int magic, int defense, int speed, int stamina, int staminaRecovery) 
        : this (name, health, attack, magic, defense, speed) {
        this.isAlly = true;
        this.maxStamina = stamina;
        this.currentStamina = stamina;
        this.staminaRecovery = staminaRecovery;
    }

    public void IncreaseStatsRandomly(int health, int attack, int magic, int defense, int speed) {
        this.maxHealth += Random.Range(0, health + 1);
        this.currentHealth += Random.Range(0, health + 1);
        this.attack += Random.Range(0, attack + 1);
        this.magic += Random.Range(0, magic + 1);
        this.defense += Random.Range(0, defense + 1);
        this.speed += Random.Range(0, speed + 1);
    }

    public void GetToast(string previous = "") {
        if(IsDead()) {
            Debug.Log(previous + name + " is dead");
        } else {
            Debug.Log(previous + name + ", health: " + currentHealth + (currentStamina < 0 ? "" : ", stamina: " + currentStamina));
        }
    }

    public void SetActions(CharacterAction[] characterActions) {
        this.actions = characterActions;
        foreach(CharacterAction action in characterActions) {
            if(action != null) {
                action.owner = this;
            }
        }
    }

    public void SetAction(CharacterAction characterAction)
    {
        this.actions = new CharacterAction[1];
        actions[0] = characterAction;
        actions[0].owner = this;
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

    public int GetStamina() {
        return maxStamina == -1 ? 9999 : currentStamina;
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

    public int ReduceStamina(int expendStamina) {
        int previousStamina = currentStamina;
        if(maxStamina != -1) {
            currentStamina = Mathf.Max(0, currentStamina -= expendStamina);
        }
        return currentStamina - previousStamina;
    }

    public void RecoverStamina() {
        if (maxStamina != -1) {
            currentStamina = Mathf.Min(maxStamina, currentStamina += GetStaminaRecovery());
        }
    }

    public void ResetStamina() {
        currentStamina = maxStamina;
    }

    public int ReduceHealth(int healthLess) {
        int previousHealth = currentHealth;
        currentHealth = Mathf.Max(0, this.currentHealth -= healthLess);
        return currentHealth - previousHealth;
    }

    public void RecoverHealth(int healthRecovered = 0) {
        if(healthRecovered == 0) {
            healthRecovered = Random.Range(0, 3);
        }
        currentHealth = Mathf.Min(maxHealth, currentHealth += healthRecovered);
    }

    public void ResetHealth() {
        currentHealth = maxHealth;
    }

    public bool CanMove() {
        return GetModifierValue(ModifierType.LoseTurns) == 0;
    }

    public bool IsDead() {
        return currentHealth <= 0;
    }

    public static Character GetRandomCharacter(Character[] group)
    {
        int i = Random.Range(0, group.Length) + group.Length;
        int incr = (Random.Range(0, 2) * 2) - 1; // +1 or -1
        int limit = 3;
        Character selected = group[i % group.Length];
        while ((selected == null || selected.IsDead()) && limit > 0)
        {
            limit--;
            i += incr;
            selected = group[i % group.Length];
        }
        return selected;
    }

}
