using System.Collections.Generic;

public interface ICircle
{
    void DestroySlices();
    bool IsSliceFitting(List<Slice> bigSlice);
    void PutSliceInCircle();
    void ShowErrorAnimation();
}