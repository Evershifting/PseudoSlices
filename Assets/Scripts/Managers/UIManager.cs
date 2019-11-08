
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceUICounter;

    public delegate void OnScore(int value);
    public static event OnScore OnScoreEvent;

    private void OnEnable()
    {
        EventsManager.AddListener<float>(EventsType.CircleDestroyed, UpdateScore);
    }
    private void OnApplicationQuit()
    {
        OnDisable();
    }
    private void OnDisable()
    {
        PlayerPrefs.SetInt("Score", int.Parse(_resourceUICounter.text));
        EventsManager.RemoveListener<float>(EventsType.CircleDestroyed, UpdateScore);
    }

    private void Start()
    {
        _resourceUICounter.text = "0";
        if (!PlayerPrefs.HasKey("Score"))
            PlayerPrefs.SetInt("Score", 0);
        UpdateScore(PlayerPrefs.GetInt("Score"));
    }


    private void UpdateScore(float value)
    {
        if (_resourceUICounter)
            _resourceUICounter.text = (float.Parse(_resourceUICounter.text) + value).ToString();
    }
}
