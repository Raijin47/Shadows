using DG.Tweening;
using UnityEngine;

public class SpinDsaasController : MonoBehaviour
{
    [SerializeField] private Slot[] _slots;
    [SerializeField] private RectTransform _textTransform;
    private Sequence _sequence;

    [Space(10)]
    [SerializeField] private float _offsetVertical;
    [SerializeField] private float _offsetHorizontal;

    [Space(10)]
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _endSpeed;
    [SerializeField] private float _minSpeed;

    [SerializeField] private float _timeSpin;

    public float EndSpeed => _endSpeed;
    public float MinSpeed => _minSpeed;
    public float TimeSpin => _timeSpin;
    public float Offset => _offsetVertical;

    public void ResetPosition()
    {
        _textTransform.localScale = Vector3.zero;
        foreach (Slot slot in _slots) slot.SetPosition(_offsetVertical);
    }

    public void Spin()
    {
        Game.Audio.PlayClip(1);

        float speed = _startSpeed * (Random.value + 1);
        float increaseSpeed = 1.5f;

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].Spin(speed);
            speed *= increaseSpeed;
        }
    }

    public void GetResult()
    {
        bool isActive = true;

        foreach (Slot slot in _slots)
            if (slot.IsActive) isActive = false;

        if (!isActive) return;

        int countWin = 0;

        for (int i = 0; i < _slots.Length; i++)
            for (int a = i + 1; a < _slots.Length; a++)
                if (_slots[i].Result == _slots[a].Result)          
                    countWin++;



        if (countWin >= 1)
        {
            Game.Locator.Timer.AddTime(countWin * 5);
            AddBonusTime();
        }
        else Game.Locator.Panel.Exit();
    }

    private void AddBonusTime()
    {
        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        _sequence.
            Append(_textTransform.DOScale(1, 1f).SetEase(Ease.OutBack)).
            SetDelay(1f).
            OnComplete(Game.Locator.Panel.Exit);

    }

    private void OnValidate()
    {
        _slots = gameObject.GetComponentsInChildren<Slot>();

        float totalHeight = _slots.Length * _offsetHorizontal;

        for (int i = 0; i < _slots.Length; i++)
        {
            float pos = -totalHeight / 2 + i * _offsetHorizontal + _offsetHorizontal / 2;
            if (Mathf.Abs(pos) < 0.1f) pos = 0;

            RectTransform transform = _slots[i].GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(pos, 0);
        }

        foreach (Slot slot in _slots) slot.SetPosition(_offsetVertical);
    }
}