using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameManager : MonoBehaviour
{
    public static List<Slice> CurrentSlice = new List<Slice>();


    [SerializeField]
    private Slice _slicePrefab;
    [SerializeField]
    private GameObject _startCircle;


    private List<Circle> _circles = new List<Circle>();

    [Header("Generation weights")]
    [SerializeField, Range(1, 100)]
    private List<float> weigths;


    private void Awake()
    {
        if (weigths == null || weigths.Count == 0)
        {
            weigths = new List<float>();
            weigths.Add(1);
        }
        if (weigths.Count > 6)
            weigths.RemoveRange(6, weigths.Count - 6);
        if (weigths.All(w => w == 0))
            weigths[0] = 1;
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

    private bool IsGameOver()
    {
        foreach (Circle circle in _circles)
        {
            if (circle.IsSliceFitting(CurrentSlice))
                return false;
        }
        return true;
    }

    public void Zuzu()
    {        
        for (int i = 0; i < 10; i++)
        {
            GenerateSlice();
        }
    }

    private void GenerateSlice()
    {
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
        }
    }
}