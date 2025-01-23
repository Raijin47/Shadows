using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _addTimerText;

    private TextMeshProUGUI _timerText;

    private Coroutine _coroutine;
    private float _currentTime;
    private bool _isActive;

#if UNITY_EDITOR
    private readonly float RequireTime = 5f;
#else
    private readonly float RequireTime = 90f;
#endif

    private Sequence _sequence;

    private float CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = Mathf.Clamp(value, 0, RequireTime);
            _timerText.text = TextUtility.FormatSeconds(_currentTime);
        }
    }

    private void Awake() => _timerText = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        Game.Action.OnStart += StartTimer;
        Game.Action.OnPause += OnPause;
        Game.Action.OnNext += ResetTimer;
        Game.Action.OnExit += ResetTimer;
        Game.Action.OnRestart += ResetTimer;
        
        CurrentTime = RequireTime;
    }

    private void StartTimer()
    {
        _isActive = true;

        Release();
        _coroutine = StartCoroutine(TimerCoroutine());
    }

    private void OnPause(bool pause)
    {
        if (!_isActive) return;

        Release();
        if (!pause) _coroutine = StartCoroutine(TimerCoroutine());
    }

    private void ResetTimer()
    {
        CurrentTime = RequireTime;
        _isActive = false;
    }

    private IEnumerator TimerCoroutine()
    {
        while(CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
            yield return null;
        }

        Game.Action.SendLose();
        Game.Action.SendPause(true);
    }

    public void AddTime(float value)
    {
        CurrentTime += value;
        _addTimerText.text = $"+{Mathf.RoundToInt(value)}";

        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        _sequence.
            Append(_addTimerText.transform.DOScaleY(1f, 3f).From(0).SetEase(Ease.InOutBack)).
            Append(_addTimerText.transform.DOScaleY(0, 2f).From(0));
    }

    private void Release()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}