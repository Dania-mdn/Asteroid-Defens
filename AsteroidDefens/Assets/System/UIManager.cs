using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Slider Health;
    public int HealthCount = 5;
    public TextMeshProUGUI Step;
    public int StepCount = 5;

    public Match3Control Match3Control;

    private void OnEnable()
    {
        SystemEvent.Step += SetStep;
        SystemEvent.HitPlayer += SetHealth;
    }
    private void OnDisable()
    {
        SystemEvent.Step -= SetStep;
        SystemEvent.HitPlayer -= SetHealth;
    }
    private void Start()
    {
        Health.maxValue = HealthCount;
        Health.value = HealthCount;
        Step.text = StepCount.ToString();
    }
    private void SetStep()
    {
        StepCount--;
        Step.text = StepCount.ToString();

        if(StepCount == 0)
        {
            SystemEvent.DoEndStep();
            Match3Control.enabled = false;
        }
    }
    private void SetHealth()
    {
        HealthCount--;
        Health.value = HealthCount;

        if(HealthCount == 0)
        {
            SystemEvent.DoEndGame();
        }
    }
}
