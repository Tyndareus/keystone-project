using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float xMinBorder;
    [SerializeField] private float xMaxBorder;
    [SerializeField] private float yMinBorder;
    [SerializeField] private float yMaxBorder;
    
    private void LateUpdate()
    {
        float xPos = playerTransform.position.x;
        xPos = Mathf.Clamp(xPos, xMinBorder, xMaxBorder);

        float yPos = playerTransform.position.y;
        yPos = Mathf.Clamp(yPos, yMinBorder, yMaxBorder);
        
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
