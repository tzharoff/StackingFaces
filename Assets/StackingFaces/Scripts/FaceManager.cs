using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _facePrefab;
    [SerializeField] private Transform _spawnPosY;
    [SerializeField] private LineRenderer _lineRenderer;

    private Camera camera;
    private float _spawnPosX;

    private Vector2 lastTouchedPosition = new Vector2(0, 0);

    private void Awake()
    {
        camera = Camera.main;
        TouchManager.TouchStart += TouchStartedCallback;
        TouchManager.TouchStop += TouchStoppedCallback;
        TouchManager.TouchPosition += TouchPositionCallback;
        HideLine();
    }


    private void OnDestroy()
    {
        TouchManager.TouchStart -= TouchStartedCallback;
        TouchManager.TouchStop -= TouchStoppedCallback;
        TouchManager.TouchPosition -= TouchPositionCallback;
    }

    private void ShowLine()
    {
        _lineRenderer.gameObject.SetActive(true);
        SetLinePosition();
    }

    private void SetLinePosition()
    {
        _lineRenderer.SetPosition(0, SpawnPosition);
        _lineRenderer.SetPosition(1, SpawnPosition + Vector2.down * 15);
    }

    private void HideLine()
    {
        _lineRenderer.gameObject.SetActive(false);
    }

    private void TouchStartedCallback()
    {
        ShowLine();
    }

    private void TouchPositionCallback(Vector2 touchPos)
    {
        lastTouchedPosition = touchPos;
        SetLinePosition();
    }

    private void TouchStoppedCallback()
    {
        Instantiate(_facePrefab, SpawnPosition, Quaternion.identity);
        HideLine();
    }

    public Vector2 SpawnPosition
    {
        get
        {
            return new Vector2(SpawnPosX, SpawnPosY);
        }
    }


    public float SpawnPosY
    {
        get
        {
            return _spawnPosY.position.y;
        }
    }


    public float SpawnPosX
    {
        get
        {
            return GetTappedPosition.x;
        }
        private set
        {
            _spawnPosX = value;
        }
    }

    public Vector2 DropPosition
    {
        get
        {
            return new Vector2(SpawnPosX, SpawnPosY);
        }
    }

    public Vector2 GetTappedPosition
    {
        get
        {
            return camera.ScreenToWorldPoint(lastTouchedPosition);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-10, SpawnPosY, 0), new Vector3(10, SpawnPosY, 0));
    }
#endif
}
