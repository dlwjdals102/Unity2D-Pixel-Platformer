using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayer;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }

    // 배경 떨림을 방지하기 위한 FixedUpdate
    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;

        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        foreach (var layer in backgroundLayer)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void InitializeLayers()
    {
        foreach (var layer in backgroundLayer)
            layer.CalculateImageWidth();
    }
}

[Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float imageWidthOffset = 10; // 카메라가 가장자리에 도착했을 때 약간의 차이가 있음, offset 위치 설정으로 해결

    private float imageFullWidth;
    private float imageHalfWidth;

    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distanceToMove)
    {
        background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset;
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;

        if (imageLeftEdge > cameraRightEdge)
            background.position += Vector3.right * -imageFullWidth;
        else if (imageRightEdge < cameraLeftEdge)
            background.position += Vector3.right * imageFullWidth;
    }
}