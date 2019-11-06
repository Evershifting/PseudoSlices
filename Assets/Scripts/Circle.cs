using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
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

    public void PutSliceInCircle()
    {

    }
}
