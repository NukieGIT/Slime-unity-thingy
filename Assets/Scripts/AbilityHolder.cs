using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState {
    ready,
    active,
    cooldown
}

public class AbilityHolder : MonoBehaviour
{

    [SerializeField] private KeyCode key;
    [SerializeField] private Ability[] abilitiesArr;
    [SerializeField] private AbilityUI cooldownUI;
    [SerializeField] private Abilities currentAbility = Abilities.Dash;

    private Dictionary<Abilities, Ability> abilities =  new Dictionary<Abilities, Ability>();
    
    private float cooldownTime;
    private float activeTime;

    private float cooldownMultiplier = 1f;

    private bool cooldownsSet = false;


    public AbilityState state { get; private set; }


    private void Awake() {
        state = AbilityState.ready;
        foreach (Ability ability in abilitiesArr) {
            abilities[ability.AbilityName] = ability;
        }
        cooldownUI.SetPlayerFollow(gameObject);
    }

    private void Update() {
        if (!cooldownsSet) { 
            cooldownUI.SetMinCooldown(-abilities[currentAbility].cooldownTime);
            cooldownUI.SetCooldown(abilities[currentAbility].cooldownTime);
        }
        cooldownUI.FollowPlayer();


        if (abilities[currentAbility].InputType == AbilityInputType.press) {
            switch (state)
            {
                case AbilityState.ready:
                    if (Input.GetKeyDown(key)) {
                        abilities[currentAbility].Activate(gameObject);
                        state = AbilityState.active;
                        activeTime = abilities[currentAbility].activeTime;
                    }
                break;
                case AbilityState.active:
                    if (activeTime > 0) {
                        activeTime -= Time.deltaTime;
                    } else {
                        abilities[currentAbility].Finshed(gameObject);
                        state = AbilityState.cooldown;
                        cooldownTime = abilities[currentAbility].cooldownTime;
                    }
                    cooldownUI.SetMinCooldown(0f);
                    cooldownUI.SetMaxCooldown(abilities[currentAbility].activeTime);
                    cooldownUI.SetCooldown(activeTime);
                break;
                case AbilityState.cooldown:
                    if (cooldownTime > 0) {
                        cooldownTime -= Time.deltaTime;
                    } else {
                        state = AbilityState.ready;
                    }
                    cooldownUI.SetMaxCooldown(0f);
                    cooldownUI.SetCooldown(-cooldownTime);
                break;
            }
        } else if (abilities[currentAbility].InputType == AbilityInputType.hold) {
            switch (state)
            {
                case AbilityState.ready:
                    if (Input.GetKeyDown(key)) {
                        abilities[currentAbility].Activate(gameObject);
                        state = AbilityState.active;
                        activeTime = abilities[currentAbility].activeTime;
                    }
                    break;
                case AbilityState.active:
                    if (activeTime < 0 || !Input.GetKey(key)) {
                        abilities[currentAbility].Finshed(gameObject);
                        state = AbilityState.cooldown;
                        if (activeTime / abilities[currentAbility].activeTime > abilities[currentAbility].maxTimeBeforeReset) cooldownMultiplier = 1 - (activeTime / abilities[currentAbility].activeTime);
                        cooldownTime = abilities[currentAbility].cooldownTime * cooldownMultiplier;
                        cooldownMultiplier = 1f;
                    }
                    else {
                        activeTime -= Time.deltaTime;
                    }
                    cooldownUI.SetMinCooldown(0f);
                    cooldownUI.SetMaxCooldown(abilities[currentAbility].activeTime);
                    cooldownUI.SetCooldown(activeTime);
                break;
                case AbilityState.cooldown:
                    if (cooldownTime > 0) {
                        cooldownTime -= Time.deltaTime;
                    } else {
                        state = AbilityState.ready;
                    }
                    cooldownUI.SetMaxCooldown(0f);
                    cooldownUI.SetCooldown(-cooldownTime);
                break;
            }
        } else if (abilities[currentAbility].InputType == AbilityInputType.toggle) {
            switch (state)
            {
                case AbilityState.ready:
                    if (Input.GetKeyDown(key)) {
                        abilities[currentAbility].Activate(gameObject);
                        state = AbilityState.active;
                        activeTime = abilities[currentAbility].activeTime;
                    }
                break;
                case AbilityState.active:
                    if (activeTime < 0 || Input.GetKeyDown(key)) {
                        abilities[currentAbility].Finshed(gameObject);
                        state = AbilityState.cooldown;
                        cooldownTime = abilities[currentAbility].cooldownTime;
                    } else {
                        activeTime -= Time.deltaTime;
                    }
                    cooldownUI.SetMinCooldown(0f);
                    cooldownUI.SetMaxCooldown(abilities[currentAbility].activeTime);
                    cooldownUI.SetCooldown(activeTime);
                break;
                case AbilityState.cooldown:
                    if (cooldownTime > 0) {
                        cooldownTime -= Time.deltaTime;
                    } else {
                        state = AbilityState.ready;
                    }
                    cooldownUI.SetMaxCooldown(0f);
                    cooldownUI.SetCooldown(-cooldownTime);
                break;
            }           
        }
    }
}
