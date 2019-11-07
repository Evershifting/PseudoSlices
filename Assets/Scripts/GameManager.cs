using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameManager : MonoBehaviour
{
    public static List<Slice> CurrentSlice = new List<Slice>();

    #region Inspector
    [Header("Prefabs")]
    [SerializeField]
    private Slice _slicePrefab;
    [SerializeField]
    private GameObject _circlePrefab;

    [Header("ObjectsHolders")]
    [SerializeField]
    private GameObject _startCircle;
    [SerializeField]
    private List<GameObject> _circleSpawnPositions = new List<GameObject>();

    [Header("Generation weights")]
    [SerializeField, Range(1, 100)]
    private List<float> weigths;
    #endregion

    private List<Circle> _circles = new List<Circle>();


    private void OnEnable()
    {
        EventsManager.AddListener<bool>(EventsType.SlicePutInCircle, OnSlicePutInCircle);
        EventsManager.AddListener<Circle>(EventsType.CircleIsFull, OnCircleIsFull);
    }
    private void OnApplicationQuit()
    {
        OnDisable();
    }
    private void OnDisable()
    {
        EventsManager.RemoveListener<bool>(EventsType.SlicePutInCircle, OnSlicePutInCircle);
        EventsManager.RemoveListener<Circle>(EventsType.CircleIsFull, OnCircleIsFull);
    }

    private void Awake()
    {
        if (weigths == null || weigths.Count == 0)
        {
            weigths = new List<float> { 1 };
        }
        if (weigths.Count > 6)
            weigths.RemoveRange(6, weigths.Count - 6);
        if (weigths.All(w => w == 0))
            weigths[0] = 1;
    }

    private void Start()
    {
        GenerateCircles();
        GenerateSlice();
    }

    private void GenerateCircles()
    {
        foreach (GameObject parent in _circleSpawnPositions)
        {
            Instantiate(_circlePrefab, parent.transform);
        }
    }

    private void GenerateSlice()
    {
        CurrentSlice.Clear();
        foreach (Transform child in _startCircle.transform)
        {
            Destroy(child.gameObject);
        }

        float usefulWeight = 0f;

        float randomResult = Random.Range(0, weigths.Sum(w => w));
        int amountOfSlicesToSpawn = 0;

        for (int i = 0; i < weigths.Count; i++)
        {
            if (randomResult <= weigths[i] + usefulWeight)
            {
                amountOfSlicesToSpawn = i + 1;
                break;
            }
            usefulWeight += weigths[i];
        }

        if (amountOfSlicesToSpawn == 0)
        {
            Debug.Log("Generator generated nothing");
            amountOfSlicesToSpawn = 1;
        }

        int startingPosition = Random.Range(0, 6);
        for (int i = 0; i < amountOfSlicesToSpawn; i++)
        {
            Slice newSlice = Instantiate(_slicePrefab, _startCircle.transform);
            newSlice.Init(startingPosition + i > 5 ? (Position)(startingPosition + i - 6) : (Position)(startingPosition + i));
            CurrentSlice.Add(newSlice);
        }
        if (IsGameOver())
            Debug.Log("GameOver");
    }

    private bool IsGameOver()
    {
        foreach (Circle circle in _circles)
        {
            if (circle.IsSliceFitting(CurrentSlice))
                return false;
        }
        return true;
    }


    private void OnSlicePutInCircle(bool value)
    {
        if (value)
            GenerateSlice();
    }
    private void OnCircleIsFull(Circle circle)
    {
        List<Circle> circlesToDestroy = GetNeighbourCircles(circle);

        foreach (Circle circleToDestroy in circlesToDestroy)
        {
            circleToDestroy.DestroySlices();
        }
    }
    private List<Circle> GetNeighbourCircles(Circle circle)
    {
        int index = _circles.IndexOf(circle);
        int prevIndex = index - 1 >= 0 ? index - 1 : _circles.Count - 1;
        int nextIndex = index + 1 <= _circles.Count - 1 ? index + 1 : 0;

        return new List<Circle>() { _circles[prevIndex], _circles[index], _circles[nextIndex] };
    }

    internal void AddCircle(Circle circle)
    {
        if (!_circles.Contains(circle))
            _circles.Add(circle);
        else
        {
            Debug.Log($"Duplicated circle: {circle.name}");
        }
    }

    public void ZuZu()
    {
        PlayerPrefs.SetInt("Score", 0);
    }
}