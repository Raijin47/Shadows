using System;
using UnityEngine;

public class RotateViaDrag : MonoBehaviour
{
    public event Action OnTouchUp;

    [SerializeField] private Transform _main;
    [SerializeField] private Transform _anchor;
    private readonly float Speed = .3f;

    private bool _onPause;

    private void Start()
    {
        _onPause = true;
        Game.Action.OnPause += OnPause;
    }

    private void OnPause(bool pause) => _onPause = pause;


    void Update()
    {
        if (_onPause) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    transform.Rotate(Vector3.up, -touch.deltaPosition.x * Speed, Space.World);
                    transform.Rotate(Vector3.right, touch.deltaPosition.y * Speed, Space.World);
                    break;

                case TouchPhase.Ended:
                    Quaternion rotation = _anchor.rotation;
                    _main.rotation = Quaternion.Euler(Vector3.zero);
                    _anchor.rotation = rotation;
                    OnTouchUp?.Invoke();

                    break;
            }
        }
    }
}