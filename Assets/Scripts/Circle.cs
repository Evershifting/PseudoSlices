using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Circle : MonoBehaviour, ICircle
{
    [SerializeField]
    private float _scoreValue = 25f;
    private int[] _slices = new int[6];

    private void OnEnable()
    {
        EventsManager.AddListener(EventsType.SliceFinishedMovement, CheckForFullCircle);
    }
    private void OnApplicationQuit()
    {
        OnDisable();
    }
    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsType.SliceFinishedMovement, CheckForFullCircle);
    }
    private void Awake()
    {
        EventsManager.Broadcast(EventsType.CircleSpawned, this);
    }

    public bool IsSliceFitting(List<Slice> bigSlice)
    {
        foreach (Slice slice in bigSlice)
        {
            if (slice.Position >= 0 && (int)slice.Position < _slices.Length)
            {
                if (_slices[(int)slice.Position] != 0)
                {
                    return false;
                }
            }
            else
            {
                Debug.Log("Wrong position value");
                return false;
            }
        }
        return true;
    }
    public void DestroySlices()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        if (_slices.Any(s => s > 0))
            EventsManager.Broadcast(EventsType.CircleDestroyed, _scoreValue);
        _slices = new int[6];
    }

    //called from a button on this prefab
    public void PutSliceInCircle()
    {
        if (IsSliceFitting(GameManager.CurrentSlice))
        {
            foreach (Slice slice in GameManager.CurrentSlice)
            {
                _slices[(int)slice.Position] = 1;
                slice.MoveSlice(transform);
            }
        }
        else
        {
            EventsManager.Broadcast(EventsType.SlicePutInCircle, false, this);
        }
    }

    private void CheckForFullCircle()
    {
        if (_slices.All(s => s >= 1))
        {
            EventsManager.Broadcast(EventsType.CircleIsFull, this);
        }
        EventsManager.Broadcast(EventsType.SlicePutInCircle, true, this);
    }

    public void ShowErrorAnimation()
    {
        Debug.Log("Error Animation");
    }
}
