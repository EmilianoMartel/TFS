using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsGunController : MonoBehaviour
{
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _reloadSound;
    [Header("EventChannels")]
    [SerializeField] private EmptyAction _shootMoment;
    [SerializeField] private EmptyAction _reloadEvent;
    [Header("Particles")]
    [SerializeField] private ParticleSystem _spark;
    [SerializeField] private ParticleSystem _flash;

    private void OnEnable()
    {
        if(_shootMoment)
            _shootMoment.Sucription(HandleStartExplotion);
        _spark.gameObject.SetActive(true);
        _flash.gameObject.SetActive(true);
        _reloadEvent.Sucription(HandleReload);
    }

    private void OnDisable()
    {
        if(_shootMoment)
            _shootMoment.Unsuscribe(HandleStartExplotion);
        _spark.gameObject.SetActive(false);
        _flash.gameObject.SetActive(false);
        _reloadEvent.Unsuscribe(HandleReload);
    }

    private void Awake()
    {
        if (!_spark)
        {
            Debug.LogError($"{name}: Spark is null.\nCheck and assigned one.\nDisabled component.");
            enabled = false;
            return;
        }
        if (!_flash)
        {
            Debug.LogError($"{name}: Flash is null.\nCheck and assigned one.\nDisabled component.");
            enabled = false;
            return;
        }
    }

    private void HandleStartExplotion()
    {
        _spark.Play();
        _flash.Play();
        _shootSound.Play();
    }

    private void HandleReload()
    {
        _reloadSound.Play();
    }
}
