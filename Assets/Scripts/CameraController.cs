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
    private bool infiniteBounds = false;
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

        if (!infiniteBounds)
        {
            if (Mathf.Abs(screenCenterToWrld.x - playerPos.x) > bounds.x)
            {
                float camDir = Mathf.Sign(screenCenterToWrld.x - playerPos.x);
                mainCamera.transform.position = new Vector3(playerPos.x + bounds.x * camDir, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }

            if (Mathf.Abs(screenCenterToWrld.y - playerPos.y) > bounds.y)
            {
                float camDir = Mathf.Sign(screenCenterToWrld.y - playerPos.y);
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, playerPos.y + bounds.y * camDir, mainCamera.transform.position.z);
            }
        }

        Vector2 mousePosRatio = new Vector2(((mousePos.x / Screen.width) * 2) - 1, ((mousePos.y / Screen.height) * 2) - 1);
        mousePosRatio = new Vector2(Mathf.Clamp(mousePosRatio.x, -1, 1), Mathf.Clamp(mousePosRatio.y, -1, 1));

        Vector3 difference = mousePosToWrld - screenCenterToWrld;

        //Debug.Log($"{mousePosRatio} ------ {mousePosRatio * maxLookAhead}");
        Vector3 final = (Vector2)playerPos + (mousePosRatio * maxLookAhead);
        final.z = mainCamera.transform.position.z;


        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, final, Time.deltaTime * dampAmount);
    }
}
