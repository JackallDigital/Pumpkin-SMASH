using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCursor : MonoBehaviour
{
    public float followSpeed = 10.0f; // Adjust the speed as needed
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
        Cursor.visible = false; // Hide the system cursor
    }

    private void Update() {
        Cursor.visible = false;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - mainCamera.transform.position.z;

        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        targetPosition.y = 20;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
