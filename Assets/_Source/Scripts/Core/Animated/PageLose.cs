using DG.Tweening;
using UnityEngine;

public class PageLose : PanelBase
{
    [SerializeField] private ParticleSystem _particle;

    protected override void Hide()
    {
        _particle.Stop();

        _sequence.Append(_canvas.DOFade(0, _delay).From(1));
    }

    protected override void Show()
    {
        _particle.Play();

        _sequence.SetDelay(_delay).
            Append(_canvas.DOFade(1, _delay)).
            Join(_components[0].DOLocalMoveY(0, _delay).From(200).SetEase(Ease.OutBack)).
            Join(_components[3].DOScale(1, _delay).From(0).SetEase(Ease.OutBack)).
            Join(_components[1].DOScale(1, _delay).From(0)).
            Join(_components[2].DOScale(1, _delay).From(0)).
            OnComplete(OnShowComplated);
    }

    protected override void Start()
    {
        base.Start();

        Game.Action.OnLose += Enter;
        Game.Action.OnRestart += Exit;
    }
}