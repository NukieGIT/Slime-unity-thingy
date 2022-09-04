using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    void Update()
    {
        Vector3 playerPos = player.position;

        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosToWrld = mainCamera.ScreenToWorldPoint(mousePos);

        Vector3 difference = mousePosToWrld - playerPos;
        Vector3 dir = difference.normalized;

        Vector3 final = playerPos + dir * maxLookAhead;
        final.z = mainCamera.transform.position.z;


        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, final, Time.deltaTime * dampAmount);
    }
}
