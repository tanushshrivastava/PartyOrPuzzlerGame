using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Transform transform1;
    [SerializeField] Transform transform2;
    [SerializeField] Camera cam;
    [SerializeField] float maxViewportDist = 0.8f;
    [SerializeField] float minViewportDist = 0.7f;
    [SerializeField] float minZoomLevel = 5;
    [SerializeField] float zoomSpeed = 2f;
    CinemachineVirtualCamera vcam;
    float minViewportDistX;
    float minViewportDistY;
    float maxViewportDistX;
    float maxViewportDistY;
    float ZoomLevel;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        ZoomLevel = cam.orthographicSize;
        minViewportDistY = minViewportDist * 0.5f;
        maxViewportDistY = maxViewportDist * 0.5f;
        minViewportDistX = minViewportDist;
        maxViewportDistX = maxViewportDist;
    }

    private void Update()
    {
        float distX = CalcTransformViewportXDistance();
        float distY = CalcTransformViewportYDistance();
        if(distX >= maxViewportDistX || distY >= maxViewportDistY)
        {
            ZoomLevel += Time.deltaTime * zoomSpeed;
        }
        else if((distX <= minViewportDistX || distY <= minViewportDistY) && ZoomLevel >= minZoomLevel)
        {
            ZoomLevel -= Time.deltaTime * zoomSpeed;
        }
        vcam.m_Lens.OrthographicSize = ZoomLevel;
    }

    private float CalcTransformViewportYDistance()
    {
        Vector2 point1 = cam.WorldToViewportPoint(transform1.position);
        Vector2 point2 = cam.WorldToViewportPoint(transform2.position);
        float dist = point1.y - point2.y;
        return Math.Abs(dist);
    }

    private float CalcTransformViewportXDistance()
    {
        Vector2 point1 = cam.WorldToViewportPoint(transform1.position);
        Vector2 point2 = cam.WorldToViewportPoint(transform2.position);
        float dist = point1.x - point2.x;
        return Math.Abs(dist);
    }
}
