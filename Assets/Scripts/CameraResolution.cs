using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private float _defaltWidth;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;

        _defaltWidth = _camera.orthographicSize * _camera.aspect;
    }

    private void Update()
    {
        _camera.orthographicSize = _defaltWidth / _camera.aspect;
    }
}
