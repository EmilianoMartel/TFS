using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneryManager))]
public class SceneryManagerUI : MonoBehaviour
{
    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private float fillDuration = .25f;

    private SceneryManager _manager;

    private void OnEnable()
    {
        if (_manager)
        {
            _manager.onLoading += EnableLoadingScreen;
            _manager.onLoaded += DisableLoadingScreen;
            _manager.onLoadPercentage += UpdateLoadBarFill;
        }
    }

    private void OnDisable()
    {
        _manager.onLoading -= EnableLoadingScreen;
        _manager.onLoaded -= DisableLoadingScreen;
        _manager.onLoadPercentage -= UpdateLoadBarFill;
    }

    private void Awake()
    {
        _manager = GetComponent<SceneryManager>();
        _manager.onLoading += EnableLoadingScreen;
        _manager.onLoaded += DisableLoadingScreen;
        _manager.onLoadPercentage += UpdateLoadBarFill;
    }

    private void EnableLoadingScreen()
    {
        loadingScreen.enabled = true;
    }

    private void DisableLoadingScreen()
    {
        Invoke(nameof(TurnOffLoadingScreen), fillDuration);
    }

    private void TurnOffLoadingScreen()
    {
        loadingBarFill.fillAmount = 0f;
        loadingScreen.enabled = false;
    }

    private void UpdateLoadBarFill(float percentage)
    {
        if (percentage == 0)
        {
            loadingBarFill.fillAmount = 0f;
            return;
        }
            

        StartCoroutine(LerpFill(loadingBarFill.fillAmount, percentage));
    }

    private IEnumerator LerpFill(float from, float to)
    {
        var start = Time.time;
        var now = start;
        while (start + fillDuration > now)
        {
            loadingBarFill.fillAmount = Mathf.Lerp(from, to, (now - start) / fillDuration);
            yield return null;
            now = Time.time;
        }

        loadingBarFill.fillAmount = to;
    }
}
