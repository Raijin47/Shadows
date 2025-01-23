using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private GameObject[] _shadows;
    [SerializeField] private Material[] _materials;
    [SerializeField] private int[] _stages;

    private int _current;

    public Material Material => _materials[_current];

    private const string Name = "_MainColor";
    private readonly Color DisableColor = new(0.1f, 0.1f, 0.1f);

    public void InitMaterial()
    {
        for (int i = 0; i < _materials.Length; i++)
            _materials[i].SetColor(Name,
                Game.Data.Saves.IsStageComplated[_stages[i]]
                ? Color.white : DisableColor);
    }

    public void StartLevel()
    {
        _current = 0;
        Show();
    }

    private void Show()
    {
        _objects[_current].SetActive(true);
        _shadows[_current].SetActive(true);
        _materials[_current].SetColor(Name, DisableColor);
    }

    public void Next()
    {
        ResetLevel();

        _current++;
        Show();
    }

    public void ResetLevel()
    {
        DisableObject();
        InitMaterial();
    }

    public void DisableObject()
    {
        _objects[_current].SetActive(false);
        _shadows[_current].SetActive(false);
    }

    public bool IsLevelComplated()
    {
        Game.Data.Saves.IsStageComplated[_stages[_current]] = true;
        Game.Data.SaveProgress();

        if (_current + 1 != _stages.Length) return false;
        return true;
    }
}