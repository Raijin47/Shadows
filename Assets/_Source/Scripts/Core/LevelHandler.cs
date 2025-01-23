using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private Transform _object;
    [SerializeField] private Transform _target;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private LevelData[] _levels;

    private int _currentLevel;
    public int Level => _currentLevel;
    private Sequence _sequence;

    private readonly float PermissibleError = 0.685f;
    private readonly float Duration = 1f;

    private void Start()
    {

        Game.Locator.RotateViaDrag.OnTouchUp += RotateViaDrag_OnTouchUp;
        Game.Locator.PageLevel.OnLevelSelected += PageLevel_OnLevelSelected;
        Game.Action.OnExit += ExitLevel;
        Game.Action.OnRestart += ResetLevel;
        Game.Action.OnNext += NextLevel;
        foreach (LevelData level in _levels)
            level.InitMaterial();
    }

    private void ExitLevel() => _levels[_currentLevel].ResetLevel();

    private void RotateViaDrag_OnTouchUp()
    {
        bool x = Mathf.Abs(_object.localRotation.y - _target.localRotation.y) <= PermissibleError;
        bool y = Mathf.Abs(_object.localRotation.x - _target.localRotation.x) <= PermissibleError;
        bool z = Mathf.Abs(_object.localRotation.z - _target.localRotation.z) <= PermissibleError;

        if (x && y && z)
        {
            Game.Action.SendPause(true);
            Game.Audio.PlayClip(0);

            _particle.Play();

            KillSequence();

            _sequence.Append(_object.DORotateQuaternion(_target.rotation, Duration)).
                Join(_levels[_currentLevel].Material.DOColor(Color.white, "_MainColor", Duration)).
                OnComplete(OnCompleteStage);
        }
    }

    private void PageLevel_OnLevelSelected(int level)
    {
        RandomRotate();

        _object.localScale = Vector3.one;
        _target.localScale = Vector3.one;
        _currentLevel = level;
        _levels[_currentLevel].StartLevel();
    }

    private void RandomRotate()
    {
        _target.rotation = Quaternion.Euler(new Vector3(
            Random.Range(-180, 180),
            Random.Range(-180, 180), 
            Random.Range(-180, 180)));
    }

    private void OnCompleteStage()
    {
        if (_levels[_currentLevel].IsLevelComplated())
        {
            Game.Data.Saves.IsLevelComplated[_currentLevel] = true;
            Game.Data.SaveProgress();

            Game.Action.SendWin();
        }
        else SwitchStage();
    }

    private void SwitchStage()
    {
        KillSequence();

        _sequence.
            Append(_object.DOScale(0, Duration).From(1)).
            Join(_target.DOScale(0, Duration).From(1).OnComplete(Game.Locator.Panel.Enter));
    }

    public void NextStage()
    {
        RandomRotate();
        _levels[_currentLevel].Next();

        KillSequence();

        _sequence.
            Append(_object.DOScale(1, Duration)).
            Join(_target.DOScale(1, Duration)).
            OnComplete(() => Game.Action.SendPause(false));
    }

    private void NextLevel()
    {
        RandomRotate();

        _levels[_currentLevel].DisableObject();
        _currentLevel++;
        _levels[_currentLevel].StartLevel();
    }

    private void ResetLevel()
    {
        RandomRotate();

        _levels[_currentLevel].DisableObject();
        _levels[_currentLevel].StartLevel();
    }

    private void KillSequence()
    {
        _sequence?.Kill();

        _sequence = DOTween.Sequence();
    }
}