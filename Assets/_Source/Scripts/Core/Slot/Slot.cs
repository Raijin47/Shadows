using System.Collections;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private RectTransform[] _slotElement;

    private Coroutine _coroutine;
    private Vector2 _startPos;
    private Vector2 _target;
    private float _speed;
    public float Offset => Game.Locator.Spin.Offset;
    public float EndSpeed => Game.Locator.Spin.EndSpeed;
    public float MinSpeed => Game.Locator.Spin.MinSpeed;
    public float TimeSpin => Game.Locator.Spin.TimeSpin;
    public int Result { get; private set; }
    public bool IsActive { get; private set; }

    private void Start()
    {
        _startPos = _slotElement[^1].anchoredPosition;
        _target = _slotElement[0].anchoredPosition + new Vector2(0, -Offset);
    }

    public void Spin(float speed)
    {
        _speed = speed;
        Result = -1;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(SpinProcess());
    }

    private IEnumerator SpinProcess()
    {
        IsActive = true;

        float timeSpin = TimeSpin;

        while (timeSpin > 0)
        {
            for (int i = 0; i < _slotElement.Length; i++) MoveElement(i, _speed);
            timeSpin -= Time.deltaTime;
            yield return null;
        }

        while (_speed > EndSpeed)
        {
            for (int i = 0; i < _slotElement.Length; i++) MoveElement(i, _speed);
            _speed = Mathf.Lerp(_speed, MinSpeed, Time.deltaTime);
            yield return null;
        }

        while (Result == -1)
        {
            _speed = Mathf.Lerp(_speed, MinSpeed, Time.deltaTime);

            for (int i = 0; i < _slotElement.Length; i++)
            {
                MoveElement(i, _speed);
                if (Mathf.Abs(_slotElement[i].anchoredPosition.y) > 10f) continue;
                Result = i;
                break;
            }

            yield return null;
        }

        IsActive = false;
        Game.Locator.Spin.GetResult();
    }

    private void MoveElement(int index, float speed)
    {
        _slotElement[index].anchoredPosition = Vector2.MoveTowards(_slotElement[index].anchoredPosition, _target, speed * Time.deltaTime);
        if (_slotElement[index].anchoredPosition == _target) _slotElement[index].anchoredPosition = _startPos;
    }

    public void SetPosition(float offset)
    {
        float totalHeight = _slotElement.Length * offset;

        for (int i = 0; i < _slotElement.Length; i++)
        {
            float pos = -totalHeight / 2 + i * offset + offset / 2;
            if (Mathf.Abs(pos) < 0.1f) pos = 0;

            _slotElement[i].anchoredPosition = new Vector2(0, pos);
        }
    }
}