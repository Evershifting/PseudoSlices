using DG.Tweening;
using UnityEngine;

public class Slice : MonoBehaviour
{
    public Position Position { get; private set; } = Position.First;

    public void Init(Position position)
    {
        Position = position;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 30 + 60 * (int)Position) * -1);
    }

    public void MoveSlice(Transform parent)
    {
        transform.SetParent(parent);
        transform.DOScale(Vector3.one, 0.3f);
        Tween t = transform.DOMove(parent.position, 0.3f);
        t.onComplete = () => EventsManager.Broadcast(EventsType.SliceFinishedMovement);
        t.Play();
    }
}

public enum Position
{
    First, Second, Third, Fourth, Fifth, Sixth
}