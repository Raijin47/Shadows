using DG.Tweening;
using UnityEngine;

public class PanelSlot : PanelBase
{
    [SerializeField] private RectTransform _upper;
    [SerializeField] private RectTransform _lower;

    protected override void Hide()
    {
        _sequence.SetDelay(_delay).
            Append(_canvas.DOFade(0, _delay).From(1)).
            Join(_upper.DOScaleY(0, _delay).From(1)).
            Join(_lower.DOScaleY(0, _delay).From(-1)).
            OnComplete(Game.Locator.LevelHandler.NextStage);
    }

    protected override void Show()
    {
        Game.Locator.Spin.ResetPosition();

        _sequence.
            Append(_canvas.DOFade(1, _delay).From(0)).
            Join(_upper.DOScaleY(1, _delay).From(0).SetEase(Ease.OutBack)).
            Join(_lower.DOScaleY(-1, _delay).From(0).SetEase(Ease.OutBack)).
            OnComplete(OnShowComplated).AppendCallback(Game.Locator.Spin.Spin);
    }
}