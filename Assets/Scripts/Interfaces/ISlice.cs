using UnityEngine;

public interface ISlice
{
    Position Position { get; }

    void DestroySlice();
    void ErrorAnimation();
    void Init(Position position);
    void MoveSlice(Transform parent);
}