using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManager : MonoBehaviour
{
    public static Action<Sprite> SetNextFaceSprite;


    [Header("References")]
    [SerializeField] private Face[] _facePrefabs;
    [SerializeField] private Face[] _facePlayable;
    [SerializeField] private Transform facesParent;
    [SerializeField] private Transform _spawnPosY;
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Settings")]
    [SerializeField] private float delayBetweenSpawns;
    [SerializeField] private float spawnDelay; 

    private new Camera camera;
    private float _spawnPosX;

    private Vector2 lastTouchedPosition = new Vector2(0, 0);

    private Face currentFace;
    private bool canControl;
    private bool isControlling;
    private int faceNextIndex;

    #region listen to events
    private void Awake()
    {
        TouchManager.TouchStart += TouchStartedCallback;
        TouchManager.TouchStop += TouchStoppedCallback;
        TouchManager.TouchPosition += TouchPositionCallback;
        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }


    private void OnDestroy()
    {
        TouchManager.TouchStart -= TouchStartedCallback;
        TouchManager.TouchStop -= TouchStoppedCallback;
        TouchManager.TouchPosition -= TouchPositionCallback;
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }
    #endregion

    private void Start()
    {
        faceNextIndex = GetNextFaceIndex();
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
        if(currentFace == null)
            return;

        if (!isControlling)
        {
            //TouchStoppedCallback();
            return;
        }

        lastTouchedPosition = touchPos;
        currentFace.MoveTo(SpawnPosition);
        SetLinePosition();
    }

    //touch up
    private void TouchStoppedCallback()
    {
        if (currentFace == null)
            return;

        HideLine();
        currentFace.SetBodyToDynamic();
        canControl = false;
        StartControlTimer();
        isControlling = false;
    }
    #endregion


    private void MergeProcessedCallback(FaceType faceType, Vector2 spawnPos){
        for(int i = 0; i < _facePrefabs.Length - 1; i++){
            if(_facePrefabs[i].MyFaceType == faceType){
                SpawnFace(_facePrefabs[i]);
                currentFace.transform.position = spawnPos;
                currentFace.SetBodyToDynamic();
                break;
            }
        }
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


    private void SpawnFace(Face face){
        currentFace = Instantiate(face, SpawnPosition, Quaternion.identity, facesParent);
    }

    private void SpawnFace()
    {
        SpawnFace(_facePlayable[faceNextIndex]);
        GetNextFaceIndex();
        //currentFace = Instantiate(GetFace, SpawnPosition, Quaternion.identity);
    }

    private Face GetFace{
        get{
            return _facePrefabs[faceNextIndex];
        }
    }

    private int GetNextFaceIndex(){
        faceNextIndex = UnityEngine.Random.Range(0, _facePlayable.Length - 1);
        SetNextFaceSprite?.Invoke(_facePlayable[faceNextIndex].FaceSprite);
        return faceNextIndex;
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
