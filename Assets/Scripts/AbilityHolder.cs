using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private Ability[] abilitiesArr;
    [SerializeField] private AbilityUI cooldownUI;

    private Dictionary<string, Ability> abilities =  new Dictionary<string, Ability>();
    
    private float cooldownTime;
    private float activeTime;

    private bool setCooldownTime = false;

    enum AbilityState{
        ready,
        active,
        cooldown
    }
    
    AbilityState state = AbilityState.ready;

    private void Awake() {
        foreach (Ability ability in abilitiesArr) {
            abilities[ability.name] = ability;
        }
    }

    private void Update(){
        if (!setCooldownTime) cooldownUI.SetMinCooldown(-abilities["Dash"].cooldownTime);
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key)) {
                    abilities["Dash"].Activate(gameObject);
                    state = AbilityState.active;
                    activeTime = abilities["Dash"].activeTime;
                }
            break;
            case AbilityState.active:
                if (activeTime > 0) {
                    activeTime -= Time.deltaTime;
                } else {
                    abilities["Dash"].Finshed(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = abilities["Dash"].cooldownTime;
                }
            break;
            case AbilityState.cooldown:
                if (cooldownTime > 0) {
                    cooldownTime -= Time.deltaTime;
                } else {
                    state = AbilityState.ready;
                }
                cooldownUI.SetCooldown(-cooldownTime);
            break;
        }
    }
}
