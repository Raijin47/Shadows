using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class PageLevel : MonoBehaviour
{
    public event Action<int> OnLevelSelected;

    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private ButtonBase _buttonNext;
    [SerializeField] private ButtonBase _buttonPreview;
    [SerializeField] private ButtonBase _buttonSelected;

    [Space(10)]
    [SerializeField] private Transform[] _levels;
    [SerializeField] private string[] _names;

    private readonly float Duration = 1f;
    private Sequence _sequence;
    private int _level;

    public int Level
    {
        get => _level;
        set
        {
            int preview = _level;
            _level = value;
            UpdateUI();

            _sequence?.Kill(true);
            _sequence = DOTween.Sequence();

            _sequence.
                Append(_levels[preview].DOLocalMoveX(value > preview ? 50 : -50, Duration)).
                Join(_levels[value].DOLocalMoveX(0, Duration).From(value > preview ? -50 : 50)).
                Join(_textName.transform.DOScale(1, Duration).From(0)).
                Join(_levels[preview].DOScale(0, Duration)).
                Join(_levels[value].DOScale(1,Duration).From(0));
        }
    }

    private void UpdateUI()
    {
        _textName.text = _names[_level];
        _buttonPreview.Interactable = _level != 0;
        _buttonNext.Interactable = Game.Data.Saves.IsLevelComplated[_level] & _level < _levels.Length - 1;
    }

    private void Start()
    {
        _buttonSelected.OnClick.AddListener(StartGame);
        _buttonNext.OnClick.AddListener(() => Level++);
        _buttonPreview.OnClick.AddListener(() => Level--);
        Game.Action.OnExit += UpdateUI;

        UpdateUI();
    }

    private void StartGame()
    {
        OnLevelSelected?.Invoke(_level);
        Game.Action.SendEnter();
    }
}