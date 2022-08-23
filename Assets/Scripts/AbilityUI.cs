using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
   private Slider _cooldownSlider;
   private Image _cooldownColor;
   private Color _orignalColor;
   private bool _colorChanged = false;

    private void Awake() {
        _cooldownSlider = GetComponent<Slider>();
        _cooldownColor = _cooldownSlider.GetComponentInChildren<Image>();
        _orignalColor = _cooldownColor.color;
    }

    public void SetMinCooldown(float minCooldown) {
        _cooldownSlider.minValue = minCooldown;
        SetCooldown(_cooldownSlider.maxValue);
    }

    public void SetCooldown(float cooldown) {
        _cooldownSlider.value = cooldown;
        if (cooldown == _cooldownSlider.maxValue) {
            _colorChanged = true;
            _cooldownColor.color = new Color(0, 1, 0, 1);
        } else if (_colorChanged) {
            _colorChanged = false;
            _cooldownColor.color = _orignalColor;
        }
    }

}
