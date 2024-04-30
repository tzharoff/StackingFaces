using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersManager : MonoBehaviour
{
    public static Action<int> SetNextNumberSprite;


    [Header("References")]
    [SerializeField] private Numbers _numPrefab;
    [SerializeField] private Transform _numParent;
    [SerializeField] private float _spawnPosY;
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Settings")]
    [SerializeField] private float delayBetweenSpawns;
    [SerializeField] private float spawnDelay;

    [Header("Number Range")]
    [SerializeField] private int minNumber;
    [SerializeField] private int maxNumber;

    private new Camera camera;
    private float _spawnPosX;

    private Vector2 lastTouchedPosition = new Vector2(0, 0);

    private Numbers currentNumber;
    private bool canControl;
    private bool isControlling;
    private int numNextIndex;

    #region listen to events
    private void Awake()
    {
        TouchManager.TouchStart += TouchStartedCallback;
        TouchManager.TouchStop += TouchStoppedCallback;
        TouchManager.TouchPosition += TouchPositionCallback;
        NumbersMergManager.onMergeProcessed += MergeProcessedCallback;
    }


    private void OnDestroy()
    {
        TouchManager.TouchStart -= TouchStartedCallback;
        TouchManager.TouchStop -= TouchStoppedCallback;
        TouchManager.TouchPosition -= TouchPositionCallback;
        NumbersMergManager.onMergeProcessed -= MergeProcessedCallback;
    }
    #endregion

    private void Start()
    {
        numNextIndex = GetNextNumberIndex();
        Init();
    }

    private void Init()
    {
        HideLine();
        camera = Camera.main;
        canControl = true;
        isControlling = false;
    }

    #region line renderer
    //show line render
    private void ShowLine()
    {
        _lineRenderer.gameObject.SetActive(true);
        SetLinePosition();
    }
    //give the line render two position points
    private void SetLinePosition()
    {
        _lineRenderer.SetPosition(0, SpawnPosition);
        _lineRenderer.SetPosition(1, SpawnPosition + Vector2.down * 15);
    }
    //hide line render
    private void HideLine()
    {
        _lineRenderer.gameObject.SetActive(false);
    }
    #endregion

    #region touch input
    //touch down
    private void TouchStartedCallback()
    {
        if (!canControl)
            return;

        isControlling = true;

        ShowLine();
        SpawnFace();
    }

    //touch moving
    private void TouchPositionCallback(Vector2 touchPos)
    {
        if (currentNumber == null)
            return;

        if (!isControlling)
            return;

        lastTouchedPosition = touchPos;
        currentNumber.MoveTo(SpawnPosition);
        SetLinePosition();
    }

    //touch up
    private void TouchStoppedCallback()
    {
        currentNumber?.SetBodyToDynamic();

        HideLine();
        canControl = false;
        isControlling = false;

        StartControlTimer();
    }
    #endregion


    private void MergeProcessedCallback(int numValue, Vector2 spawnPos)
    {
        SpawnNumber(numValue, spawnPos);
    }

    private void StartControlTimer()
    {
        Invoke(nameof(StopControlTimer), spawnDelay);
    }

    private void StopControlTimer()
    {
        canControl = true;
    }

    private IEnumerator controlTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        canControl = true;
    }

    private void SpawnNumber(int numValue, Vector2 spawnPos)
    {
        Numbers newNumber = Instantiate(_numPrefab, spawnPos, Quaternion.identity, _numParent);
        newNumber.SetBodyToDynamic();
        newNumber.NumValue = numValue;

        //currentFace.transform.position = spawnPos;
        //currentFace.SetBodyToDynamic();
    }

    private void SpawnNumber(int numValue)
    {
        currentNumber = Instantiate(_numPrefab, SpawnPosition, Quaternion.identity, _numParent);
        currentNumber.NumValue = numValue;
    }

    private void SpawnFace()
    {
        SpawnNumber(numNextIndex);
        GetNextNumberIndex();
    }

    private Numbers GetFace
    {
        get
        {
            return _numPrefab;
        }
    }

    private int GetNextNumberIndex()
    {
        numNextIndex = UnityEngine.Random.Range(minNumber, maxNumber);
        SetNextNumberSprite?.Invoke(numNextIndex);
        return numNextIndex;
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
            return _spawnPosY;
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
