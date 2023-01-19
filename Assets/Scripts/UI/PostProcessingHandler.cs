using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessingHandler : MonoBehaviour
{
    private Volume _volume;
    private VolumeProfile _volumeProfile;
    private Vignette _vignette;
    private ColorAdjustments _colorAdjustments;

    void Start()
    {
        _volume = GetComponent<Volume>();
        _volumeProfile = _volume.sharedProfile;

        GameEventSystem.Instance.OnPlayerHealthy += disableHearbeatEffect;
        GameEventSystem.Instance.OnPlayerHurt += enableHearbeatEffect;

        _volumeProfile.TryGet<Vignette>(out _vignette);
        _volumeProfile.TryGet<ColorAdjustments>(out _colorAdjustments);


        //heartbeat effect
        _vignette.smoothness.value = 0.3f;

        _vignette.active = false;
        _colorAdjustments.active = false;
    }

    private void Update()
    {
        //heartbeat effect
        if (_vignette.active)
        {
            _vignette.intensity.value = Mathf.PingPong(Time.time, 0.3f) + 0.3f;
        }
    }
    void enableHearbeatEffect()
    {
        _vignette.active = true;
        _colorAdjustments.active = true;
    }

    void disableHearbeatEffect()
    {
        _vignette.active = false;
        _colorAdjustments.active = false;
    }

}
