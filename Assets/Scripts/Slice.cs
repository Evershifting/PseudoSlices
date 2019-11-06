using UnityEngine;

public class Slice : MonoBehaviour
{
    public Position Position { get; private set; } = Position.First;

    public void Init(Position position)
    {
        Position = position;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 30 + 60 * (int)Position) * -1);
    }
}

public enum Position
{
    First, Second, Third, Fourth, Fifth, Sixth
}