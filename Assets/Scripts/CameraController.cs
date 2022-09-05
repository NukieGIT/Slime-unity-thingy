using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour

{
    public static Camera mainCameraInstance;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Camera mainCamera;

    [Space]
    [Header("Settings")]
    [SerializeField]
    private float maxLookAhead = 4f;
    [SerializeField]
    private float dampAmount = 1f;
    [SerializeField]
    private Vector2 bounds = new Vector2(2f, 2f);


    private void Awake()
    {
        mainCameraInstance = mainCamera;
    }

    void LateUpdate()
    {
        Vector3 playerPos = player.position;

        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosToWrld = mainCamera.ScreenToWorldPoint(mousePos);
        mousePosToWrld.z = 0f;

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        Vector3 screenCenterToWrld = mainCamera.ScreenToWorldPoint(screenCenter);

        Vector3 difference = mousePosToWrld - screenCenterToWrld;
        float mag = difference.magnitude;
        Vector3 dir = difference.normalized;

        Vector3 final = playerPos + dir * Mathf.Min(mag, maxLookAhead);
        final.z = mainCamera.transform.position.z;


        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, final, Time.deltaTime * dampAmount);
    }
}
