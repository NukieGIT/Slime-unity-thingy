using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    [SerializeField] private float positionSmooth = 10f;
    [SerializeField] private float appearSmooth = 0.5f;
    [SerializeField] private float hideSmooth = 0.1f;
    [SerializeField] private float timeBeforeHide = 5f;
    [SerializeField] private float maxDistanceFromPlayer = 2f;
    private Slider _cooldownSlider;
    private bool _abilityReady = false;
    private Image _cooldownColor;
    private Color _orignalColor;
    private GameObject _player;
    private Vector2 _valueRef;
    private float disappearTimer;
    private GameObject parent;
    private CanvasGroup parentGroup;

    private void Awake() {
        maxDistanceFromPlayer *= maxDistanceFromPlayer;
        parent = this.transform.parent.gameObject;
        parentGroup = parent.GetComponent<CanvasGroup>();
        _cooldownSlider = GetComponent<Slider>();
        _cooldownColor = _cooldownSlider.GetComponentInChildren<Image>();
        _orignalColor = _cooldownColor.color;
    }

    private void Update() {
        if (_abilityReady) {
            disappearTimer += Time.deltaTime;
        } else {
            disappearTimer = 0f;
        }
        if (disappearTimer > timeBeforeHide) {
            lerpHide();
        } else {
            lerpShow();
        }
    }

    public void SetPlayerFollow(GameObject player) {
        _player = player;
    }

    public void FollowPlayer() {
        if (_player == null) return;

        if ((parent.transform.position - _player.transform.position).sqrMagnitude > maxDistanceFromPlayer) {
            Vector3 offset = parent.transform.position - _player.transform.position;
            offset = offset.normalized * Mathf.Min(offset.sqrMagnitude, maxDistanceFromPlayer);
            parent.transform.position = _player.transform.position + offset;
        }

        parent.transform.position = Vector2.Lerp(parent.transform.position, _player.transform.position, Time.deltaTime * positionSmooth);


        // cool effect Don't remove!!!! VVVV
        // Vector3 endPosOffset = Vector2.ClampMagnitude(parent.transform.position - _player.transform.position, 2f);
        // parent.transform.position = Vector2.SmoothDamp(this.transform.parent.gameObject.transform.position, _player.transform.position + endPosOffset, ref _valueRef, positionSmooth);
    }

    public void SetMinCooldown(float minCooldown) {
        _cooldownSlider.minValue = minCooldown;
        SetCooldown(_cooldownSlider.maxValue);
    }

    private void lerpHide() {
        parentGroup.alpha = Mathf.Lerp(parentGroup.alpha, 0, Time.deltaTime * hideSmooth);
        // parentGroup.alpha = Mathf.SmoothDamp(parentGroup.alpha, 0, ref alphaRef, hideSmooth);
    }

    private void lerpShow() {
        parentGroup.alpha = Mathf.Lerp(parentGroup.alpha, 1, Time.deltaTime * appearSmooth);
    }

    public void SetCooldown(float cooldown) {
        _cooldownSlider.value = cooldown;
        if (cooldown == _cooldownSlider.maxValue) {
            _cooldownColor.color = new Color(0, 1, 0, 1);
            _abilityReady = true;
        } else {
            _cooldownColor.color = _orignalColor;
            _abilityReady = false;
        }
    }

}
