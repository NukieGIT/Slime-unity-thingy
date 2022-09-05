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

    public AbilityState State { get; private set; }


    private void Awake() {
        State = AbilityState.ready;
        foreach (Ability ability in abilitiesArr) {
            abilities[ability.AbilityName] = ability;
        }
    }

    private void Start()
    {
        cooldownUI.SetPlayerFollow(gameObject);
        cooldownUI.SetMaxCooldown(1f);
        cooldownUI.SetCooldown(1f);
        
    }

    private void Update() {
        HandleInput(abilities[currentAbility].InputType);
    }

    private bool InputTypeResult(AbilityInputType inputType)
    {
        switch (inputType)
        {
            case AbilityInputType.press:
                return false;
            case AbilityInputType.hold:
                return !Input.GetKey(key);
            case AbilityInputType.toggle:
                return Input.GetKeyDown(key);
        }

        return false;
    }

    private void HandleInput(AbilityInputType inputType)
    {
        switch (State)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    abilities[currentAbility].Activate(gameObject);
                    State = AbilityState.active;
                    activeTime = abilities[currentAbility].activeTime;
                    cooldownUI.SetMaxCooldown(abilities[currentAbility].activeTime);
                }
            break;
            case AbilityState.active:
                if (activeTime < 0 || InputTypeResult(inputType))
                {
                    abilities[currentAbility].Finshed(gameObject);
                    State = AbilityState.cooldown;
                    float cooldownMulti = 1f;
                    if (activeTime / abilities[currentAbility].activeTime > abilities[currentAbility].maxTimeBeforeReset)
                    {
                        cooldownMulti = 1 - (activeTime / abilities[currentAbility].activeTime);
                    }
                    cooldownTime = abilities[currentAbility].cooldownTime * cooldownMulti;
                    cooldownUI.SetMaxCooldown(abilities[currentAbility].cooldownTime);
                }
                else
                {
                    activeTime -= Time.deltaTime;
                }
                cooldownUI.SetCooldown(activeTime);
            break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    State = AbilityState.ready;
                }
                float cooldownReverse = abilities[currentAbility].cooldownTime - cooldownTime;
                cooldownUI.SetCooldown(cooldownReverse);
            break;
        }
    }

}
