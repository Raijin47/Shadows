using DG.Tweening;
using UnityEngine;

public class PageGame : PanelBase
{
    [SerializeField] private Transform _game;

    protected override void Hide()
    {
        _sequence.Append(_canvas.DOFade(0, _delay).From(1)).
            Join(_game.DOLocalMoveX(-30, _delay).From(0));
    }

    protected override void Show()
    {
        _sequence.SetDelay(_delay).
            Append(_canvas.DOFade(1, _delay)).
            Join(_components[0].DOLocalMoveY(0, _delay).From(200).SetEase(Ease.InOutBack)).
            Join(_game.DOLocalMoveX(0, _delay).From(30)).
            OnComplete(OnShowComplated);
    }

    protected override void Start()
    {
        base.Start();

        Game.Action.OnEnter += Enter;
        Game.Action.OnWin += Exit;
        Game.Action.OnLose += Exit;
        Game.Action.OnNext += Enter;
        Game.Action.OnRestart += Enter;
    }

    protected override void OnShowComplated()
    {
        base.OnShowComplated();
        Game.Action.SendStart();
        Game.Action.SendPause(false);
    }
}