using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float maxLookAhead = 5f;
    [SerializeField]
    private float dampAmount = 0.4f;
    [SerializeField]
    private Camera mainCamera;


    private Vector3 cameraPos;
    //private Vector3 velocity = Vector3.zero;

    void Update()
    {
        //cameraPos = new Vector3(player.position.x, player.position.y, -10f);
        //transform.position = Vector3.SmoothDamp(gameObject.transform.position, cameraPos, ref velocity, dampTime);

        Vector3 playerPos = player.position;

        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosToWrld = mainCamera.ScreenToWorldPoint(mousePos);

        Vector3 difference = mousePosToWrld - playerPos;
        Vector3 dir = difference.normalized;

        Vector3 final = playerPos + dir * maxLookAhead;
        final.z = mainCamera.transform.position.z;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, final, Time.deltaTime * 10f);
    }
}
