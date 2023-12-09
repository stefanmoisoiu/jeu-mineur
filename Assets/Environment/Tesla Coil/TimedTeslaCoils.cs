using UnityEngine;

public class TimedTeslaCoils : TeslaCoils
{
    [SerializeField] private float activeLength = 1;
    [SerializeField] private float deactivatedLength = 2;
    private float _time;
    private bool _active;

    private void Update()
    {
        _time += Time.deltaTime;
        if (_active && _time >= activeLength)
        {
            _time = 0;
            _active = false;
            Deactivate();
        }
        else if (!_active && _time >= deactivatedLength)
        {
            _time = 0;
            _active = true;
            Activate();
        }
    }
}