using System.Collections.Generic;

public interface ICircle
{
    void DestroySlices();
    bool IsSliceFitting(List<ISlice> bigSlice);
    void PutSliceInCircle();
    void ShowErrorAnimation();
}