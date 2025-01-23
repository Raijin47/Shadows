using System;
using UnityEngine;

[Serializable]
public class Locator
{
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private RotateViaDrag _rotateViaDrag;
    [SerializeField] private PageLevel _pageLevel;
    [SerializeField] private SpinDsaasController _spin;
    [SerializeField] private Timer _timer;
    [SerializeField] private PanelSlot _panelSlot;

    public LevelHandler LevelHandler => _levelHandler;
    public RotateViaDrag RotateViaDrag => _rotateViaDrag;
    public PageLevel PageLevel => _pageLevel;
    public SpinDsaasController Spin => _spin;
    public Timer Timer => _timer;
    public PanelSlot Panel => _panelSlot;
}