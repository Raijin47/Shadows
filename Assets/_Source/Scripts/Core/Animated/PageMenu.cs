using DG.Tweening;
using UnityEngine;

public class PageMenu : PanelBase
{
    [SerializeField] private Transform _menu;

    protected override void Hide()
    {
        _sequence.Append(_canvas.DOFade(0, _delay)).
            Join(_components[0].DOLocalMoveY(200, _delay)).
            Join(_components[1].DOLocalMoveX(-200, _delay)).
            Join(_components[2].DOLocalMoveX(200, _delay)).
            Join(_components[3].DOLocalMoveY(-200, _delay)).
            Join(_components[4].DOLocalMoveX(-200, _delay)).
            Join(_components[5].DOLocalMoveX(-200, _delay)).
            Join(_menu.DOLocalMoveX(-30, _delay));
    }

    protected override void Show()
    {
        _sequence.SetDelay(_delay).

            Append(_canvas.DOFade(1, _delay)).
            Join(_components[0].DOLocalMoveY(0, _delay)).
            Join(_components[1].DOLocalMoveX(0, _delay)).
            Join(_components[2].DOLocalMoveX(0, _delay)).
            Join(_components[3].DOLocalMoveY(0, _delay)).
            Join(_components[4].DOLocalMoveX(0, _delay)).
            Join(_components[5].DOLocalMoveX(0, _delay)).
            Join(_menu.DOLocalMoveX(0, _delay).From(30)).

            OnComplete(OnShowComplated);
    }

    protected override void Start()
    {
        _canvas.alpha = 1;
        IsActive = true;

        Game.Action.OnEnter += Exit;
        Game.Action.OnExit += Enter;
    }
}