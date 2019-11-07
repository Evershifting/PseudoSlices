using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Circle : MonoBehaviour
{
    [SerializeField]
    private float _scoreValue = 25f;
    private int[] _slices = new int[6];

    private void Awake()
    {
        FindObjectOfType<GameManager>().AddCircle(this);
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
                slice.transform.SetParent(transform);
                slice.transform.localPosition = Vector3.zero;
                slice.transform.localScale = Vector3.one;
            }
            CheckForFullCircle();
            EventsManager.Broadcast(EventsType.SlicePutInCircle, true);
        }
        else
        {
            EventsManager.Broadcast(EventsType.SlicePutInCircle, false);
        }
    }

    private void CheckForFullCircle()
    {
        if (_slices.All(s => s >= 1))
        {
            EventsManager.Broadcast(EventsType.CircleIsFull, this);
        }
    }
}
