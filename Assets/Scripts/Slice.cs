using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Slice : MonoBehaviour, ISlice
{
    [SerializeField]
    private float _animationDuration = 0.3f;
    [SerializeField]
    GameObject particles;
    public Position Position { get; private set; } = Position.First;

    public void Init(Position position)
    {
        Position = position;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 30 + 60 * (int)Position) * -1);
    }

    public void MoveSlice(Transform parent)
    {
        transform.SetParent(parent);
        transform.DOScale(Vector3.one, _animationDuration);
        Tween t = transform.DOMove(parent.position, _animationDuration);
        t.onComplete = () => EventsManager.Broadcast(EventsType.SliceFinishedMovement);
        t.Play();
    }

    public void ErrorAnimation()
    {
        Image image = GetComponent<Image>();
        Vector3 startingScale = transform.localScale;
        Color startingColor = image.color;

        image.DOColor(Color.red, _animationDuration / 2f);
        Tween t = transform.DOScale(startingScale * 1.2f, _animationDuration / 2f);
        t.onComplete = () =>
        {
            transform.DOScale(startingScale, _animationDuration / 2f);
            image.DOColor(startingColor, _animationDuration / 2f);
        };
        t.Play();
    }
    public void DestroySlice()
    {
        StartCoroutine(DestroySliceRoutine());
    }
    private IEnumerator DestroySliceRoutine()
    {
        particles.SetActive(true);
        Image image = GetComponent<Image>();
        Color startingColor = image.color, targetColor = new Color(image.color.r, image.color.g, image.color.b, 0);
        float timer = _animationDuration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            image.color = Color.Lerp(startingColor, targetColor, timer / _animationDuration);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }

}

public enum Position
{
    First, Second, Third, Fourth, Fifth, Sixth
}