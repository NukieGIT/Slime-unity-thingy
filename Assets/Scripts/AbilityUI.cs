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
        // maxDistanceFromPlayer *= maxDistanceFromPlayer;
        parent = this.transform.parent.gameObject;
        parentGroup = parent.GetComponent<CanvasGroup>();
        _cooldownSlider = GetComponent<Slider>();
        _cooldownColor = _cooldownSlider.GetComponentInChildren<Image>();
        _orignalColor = _cooldownColor.color;
    }
    private void Update() {
        if (abilityHolder.state == AbilityState.ready) {
            disappearTimer += Time.deltaTime;
        } else {
            disappearTimer = 0f;
        }
        if (disappearTimer > timeBeforeHide) {
            LerpHide();
        } else {
            LerpShow();
        }

        if (abilityHolder.state == AbilityState.active)
        {
            //_cooldownColor.color = activeAbilityColor;
            _cooldownColor.color = Color.Lerp(_cooldownColor.color, activeAbilityColor, Time.deltaTime * colorChangeSmooth);
        }
        else if (abilityHolder.state == AbilityState.ready)
        {
            //_cooldownColor.color = new Color(0, 1, 0, 1);
            _cooldownColor.color = Color.Lerp(_cooldownColor.color, new Color(0, 1, 0, 1), Time.deltaTime * colorChangeSmooth);
        }
        else
        {
            //_cooldownColor.color = _orignalColor;
            _cooldownColor.color = Color.Lerp(_cooldownColor.color, _orignalColor, Time.deltaTime * colorChangeSmooth);
        }

    }

    public void SetPlayerFollow(GameObject player) {
        _player = player;
    }

    public void FollowPlayer() {
        if (_player == null) return;

        Vector3 offset = parent.transform.position - UItransform.position;
        if (offset.sqrMagnitude > maxDistanceFromPlayer * maxDistanceFromPlayer) {
            offset = offset.normalized * Mathf.Min(offset.magnitude, maxDistanceFromPlayer);
            parent.transform.position = UItransform.position + offset;
        }

        parent.transform.position = Vector2.Lerp(parent.transform.position, UItransform.position, Time.deltaTime * positionSmooth);


        // cool effect Don't remove!!!! VVVV
        // Vector3 endPosOffset = Vector2.ClampMagnitude(parent.transform.position - _player.transform.position, 2f);
        // parent.transform.position = Vector2.SmoothDamp(this.transform.parent.gameObject.transform.position, _player.transform.position + endPosOffset, ref _valueRef, positionSmooth);
    }

    public void SetMinCooldown(float minCooldown) {
        _cooldownSlider.minValue = minCooldown;
        //SetCooldown(_cooldownSlider.maxValue);
    }

    public void SetMaxCooldown(float maxCooldown) {
        _cooldownSlider.maxValue = maxCooldown;
        //SetCooldown(_cooldownSlider.minValue);
    }

    private void LerpHide() {
        parentGroup.alpha = Mathf.Lerp(parentGroup.alpha, 0, Time.deltaTime * hideSmooth);
        // parentGroup.alpha = Mathf.SmoothDamp(parentGroup.alpha, 0, ref alphaRef, hideSmooth);
    }

    private void LerpShow() {
        parentGroup.alpha = Mathf.Lerp(parentGroup.alpha, 1, Time.deltaTime * appearSmooth);
    }

    public void SetCooldown(float cooldown)
    {
        _cooldownSlider.value = cooldown;

    }

}
