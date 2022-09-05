using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    [SerializeField] private AbilityHolder abilityHolder;
    [SerializeField] private Transform UItransform;
    [SerializeField] private float positionSmooth = 10f;
    [SerializeField] private float appearSmooth = 0.5f;
    [SerializeField] private float hideSmooth = 0.1f;
    [SerializeField] private float timeBeforeHide = 5f;
    [SerializeField] private float maxDistanceFromPlayer = 2f;
    [SerializeField] private Color activeAbilityColor;
    [SerializeField] private float colorChangeSmooth = 10f;

    private Slider _cooldownSlider;
    private Image _cooldownColor;
    private Color _orignalColor;
    private GameObject _player;
    private float disappearTimer;
    private GameObject parent;
    private CanvasGroup parentGroup;

    public bool FastMoveLerp { get; set; }

    private void Awake() {
        parent = this.transform.parent.gameObject;
        parentGroup = parent.GetComponent<CanvasGroup>();
        _cooldownSlider = GetComponent<Slider>();
        _cooldownColor = _cooldownSlider.GetComponentInChildren<Image>();
        _orignalColor = _cooldownColor.color;
    }
    private void Update() {

        FollowPlayer();

        if (abilityHolder.State == AbilityState.ready) {
            disappearTimer += Time.deltaTime;
        } else {
            disappearTimer = 0f;
        }
        if (disappearTimer > timeBeforeHide) {
            LerpHide();
        } else {
            LerpShow();
        }

        if (abilityHolder.State == AbilityState.active)
        {
            _cooldownColor.color = Color.Lerp(_cooldownColor.color, activeAbilityColor, Time.deltaTime * colorChangeSmooth);
        }
        else if (abilityHolder.State == AbilityState.ready)
        {
            _cooldownColor.color = Color.Lerp(_cooldownColor.color, new Color(0, 1, 0, 1), Time.deltaTime * colorChangeSmooth);
        }
        else
        {
            _cooldownColor.color = Color.Lerp(_cooldownColor.color, _orignalColor, Time.deltaTime * colorChangeSmooth);
        }

    }

    public void SetPlayerFollow(GameObject player) {
        _player = player;
    }

    public void FollowPlayer() {
        if (_player == null) return;

        Vector3 offset = parent.transform.position - UItransform.position;
        if (offset.sqrMagnitude > maxDistanceFromPlayer * maxDistanceFromPlayer)
        {
            offset = offset.normalized * Mathf.Min(offset.magnitude, maxDistanceFromPlayer);
            parent.transform.position = UItransform.position + offset;
        }

        parent.transform.position = Vector2.Lerp(parent.transform.position, UItransform.position, Time.deltaTime * positionSmooth);
    }

    private void LerpHide() {
        parentGroup.alpha = Mathf.Lerp(parentGroup.alpha, 0, Time.deltaTime * hideSmooth);
    }

    private void LerpShow() {
        parentGroup.alpha = Mathf.Lerp(parentGroup.alpha, 1, Time.deltaTime * appearSmooth);
    }

    public void SetMinCooldown(float minCooldown) {
        _cooldownSlider.minValue = minCooldown;
    }

    public void SetMaxCooldown(float maxCooldown) {
        _cooldownSlider.maxValue = maxCooldown;
    }

    public void SetCooldown(float cooldown)
    {
        _cooldownSlider.value = cooldown;

    }

}
