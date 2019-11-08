using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameManager : MonoBehaviour
{
    public static List<ISlice> CurrentSlice = new List<ISlice>();

    #region Inspector
    [Header("Prefabs")]
    [SerializeField]
    private GameObject _slicePrefab;
    [SerializeField]
    private GameObject _circlePrefab;

    [Header("ObjectsHolders")]
    [SerializeField]
    private GameObject _startCircle;
    [SerializeField]
    private List<GameObject> _circleSpawnPositions = new List<GameObject>();

    [Header("Generation weights")]
    [SerializeField, Range(0, 100)]
    private List<float> weigths;
    #endregion

    private List<ICircle> _circles = new List<ICircle>();


    private void OnEnable()
    {
        EventsManager.AddListener<ICircle>(EventsType.CircleSpawned, OnCircleSpawned);
        EventsManager.AddListener<bool, ICircle>(EventsType.SlicePutInCircle, OnSlicePutInCircle);
        EventsManager.AddListener<ICircle>(EventsType.CircleIsFull, OnCircleIsFull);
        EventsManager.AddListener(EventsType.RestartGame, Restart);
    }

    private void OnApplicationQuit()
    {
        OnDisable();
    }
    private void OnDisable()
    {
        EventsManager.RemoveListener<ICircle>(EventsType.CircleSpawned, OnCircleSpawned);
        EventsManager.RemoveListener<bool, ICircle>(EventsType.SlicePutInCircle, OnSlicePutInCircle);
        EventsManager.RemoveListener<ICircle>(EventsType.CircleIsFull, OnCircleIsFull);
        EventsManager.RemoveListener(EventsType.RestartGame, Restart);
    }

    private void Awake()
    {
        CheckGenerationWeights();
    }

    private void CheckGenerationWeights()
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
    }

    private void GenerateCircles()
    {
        _circles.Clear();
        foreach (GameObject parent in _circleSpawnPositions)
        {
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }
            Instantiate(_circlePrefab, parent.transform);
        }
        GenerateSlice();
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
            GameObject newSlice = Instantiate(_slicePrefab, _startCircle.transform);
            ISlice sliceComponent = newSlice.GetComponent<ISlice>();
            sliceComponent.Init(startingPosition + i > 5 ? (Position)(startingPosition + i - 6) : (Position)(startingPosition + i));
            CurrentSlice.Add(sliceComponent);
        }
        if (IsGameOver())
        {
            EventsManager.Broadcast(EventsType.GameOver);
        }
    }

    private void Restart()
    {
        foreach (Circle circle in _circles)
        {
            circle.ClearSlices();
        }
        GenerateSlice();
    }
    private bool IsGameOver()
    {
        foreach (ICircle ICircle in _circles)
        {
            if (ICircle.IsSliceFitting(CurrentSlice))
                return false;
        }
        return true;
    }
    private List<ICircle> GetNeighbourCircles(ICircle Circle)
    {
        int index = _circles.IndexOf(Circle);
        int prevIndex = index - 1 >= 0 ? index - 1 : _circles.Count - 1;
        int nextIndex = index + 1 <= _circles.Count - 1 ? index + 1 : 0;

        return new List<ICircle>() { _circles[prevIndex], _circles[index], _circles[nextIndex] };
    }



    private void OnSlicePutInCircle(bool value, ICircle circle)
    {
        if (value)
            GenerateSlice();
        else
        {
            circle.ShowErrorAnimation();
        }
    }
    private void OnCircleIsFull(ICircle Circle)
    {
        List<ICircle> circlesToDestroy = GetNeighbourCircles(Circle);

        foreach (ICircle circleToDestroy in circlesToDestroy)
        {
            circleToDestroy.DestroySlices();
        }
    }
    private void OnCircleSpawned(ICircle Circle)
    {
        if (!_circles.Contains(Circle))
            _circles.Add(Circle);
        else
        {
            Debug.LogError($"Duplicated Circle!");
        }
    }
}