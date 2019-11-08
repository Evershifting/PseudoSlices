using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Circle : MonoBehaviour, ICircle
{
    [SerializeField]
    private float _scoreValue = 25f;
    private List<ISlice> _slices = new List<ISlice>();

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

    public bool IsSliceFitting(List<ISlice> bigSlice)
    {
        if (_slices.Count > 0)
            foreach (ISlice slice in bigSlice)
            {
                if (_slices.Any(sl => sl.Position == slice.Position))
                {
                    return false;
                }
            }
        return true;
    }

    public void ClearSlices()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _slices.Clear();
    }
    public void DestroySlices()
    {
        if (_slices.Count > 0)
        {
            foreach (ISlice child in _slices)
            {
                child.DestroySlice();
            }
            EventsManager.Broadcast(EventsType.CircleDestroyed, _scoreValue);
            _slices.Clear();
        }
    }

    //called from a button on this prefab
    public void PutSliceInCircle()
    {
        if (IsSliceFitting(GameManager.CurrentSlice))
        {
            foreach (ISlice slice in GameManager.CurrentSlice)
            {
                _slices.Add(slice);
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
        if (_slices.Count >= 6)
        {
            EventsManager.Broadcast(EventsType.CircleIsFull, this);
        }
        EventsManager.Broadcast(EventsType.SlicePutInCircle, true, this);
    }

    public void ShowErrorAnimation()
    {
        foreach (ISlice slice in _slices)
        {
            slice.ErrorAnimation();
        }
        foreach (ISlice slice in GameManager.CurrentSlice)
        {
            slice.ErrorAnimation();
        }
    }
}
