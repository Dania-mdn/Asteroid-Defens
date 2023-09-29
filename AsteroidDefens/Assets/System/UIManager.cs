using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Slider Health;
    public int HealthCount = 5;
    public TextMeshProUGUI Step;
    public int DefoltStepCount = 5;
    public int StepCount;

    public Match3Control Match3Control;

    public GameObject GameOwer;

    private void OnEnable()
    {
        SystemEvent.Step += SetStep;
        SystemEvent.HitPlayer += SetHealth;
        SystemEvent.StartStep += StartStep;
        SystemEvent.FullStep += FullStep;
        SystemEvent.EndGame += SetGameOwer;
        SystemEvent.AddStep += AddStep;
    }
    private void OnDisable()
    {
        SystemEvent.Step -= SetStep;
        SystemEvent.HitPlayer -= SetHealth;
        SystemEvent.StartStep -= StartStep;
        SystemEvent.FullStep -= FullStep;
        SystemEvent.EndGame -= SetGameOwer;
        SystemEvent.AddStep -= AddStep;
    }
    private void Start()
    {
        Health.maxValue = HealthCount;
        Health.value = HealthCount;
        StepCount = DefoltStepCount;
        Step.text = StepCount.ToString();
    }
    private void SetStep()
    {
        StepCount--;
        Step.text = StepCount.ToString();
    }
    private void FullStep()
    {
        if (StepCount == 0)
        {
            SystemEvent.DoEndStep(true);
            Match3Control.enabled = false;
        }
    }
    private void SetHealth(GameObject Asteroid)
    {
        if(HealthCount > 0)
        {
            HealthCount--;
            Health.value = HealthCount;
        }

        if(HealthCount == 0)
        {
            SystemEvent.DoEndGame();
        }
    }
    private void SetGameOwer()
    {
        GameOwer.SetActive(true);
    }
    private void StartStep()
    {
        StepCount = DefoltStepCount;
        Step.text = StepCount.ToString();
        Match3Control.enabled = true;
    }
    public void pausa(int timescale)
    {
        Time.timeScale = timescale;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void AddStep(int step)
    {
        StepCount = StepCount + step;
        Step.text = StepCount.ToString();
    }
}
