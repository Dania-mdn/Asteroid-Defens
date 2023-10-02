using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Slider Health;
    public int HealthCount = 5;
    public TextMeshProUGUI Step;
    private int day = 1;
    public TextMeshProUGUI Day;
    public TextMeshProUGUI[] DayPanel;
    private int record;
    public TextMeshProUGUI[] recordPanel;
    public int DefoltStepCount = 5;
    public int StepCount;

    public Match3Control Match3Control;

    public GameObject GameOwer;

    public Slider BossHealth;

    private void OnEnable()
    {
        SystemEvent.Step += SetStep;
        SystemEvent.HitPlayer += SetHealth;
        SystemEvent.StartStep += StartStep;
        SystemEvent.FullStep += FullStep;
        SystemEvent.EndGame += SetGameOwer;
        SystemEvent.AddStep += AddStep;
        SystemEvent.SpawnBoss += SpawnBoss;
        SystemEvent.DestroyBoss += DestroyBoss;
        SystemEvent.HitBoss += HitBoss;
    }
    private void OnDisable()
    {
        SystemEvent.Step -= SetStep;
        SystemEvent.HitPlayer -= SetHealth;
        SystemEvent.StartStep -= StartStep;
        SystemEvent.FullStep -= FullStep;
        SystemEvent.EndGame -= SetGameOwer;
        SystemEvent.AddStep -= AddStep;
        SystemEvent.SpawnBoss -= SpawnBoss;
        SystemEvent.DestroyBoss -= DestroyBoss;
        SystemEvent.HitBoss -= HitBoss;
    }
    private void Start()
    {
        Health.maxValue = HealthCount;
        Health.value = HealthCount;
        StepCount = DefoltStepCount;
        Step.text = StepCount.ToString();
        Day.text = day.ToString(); 
        record = PlayerPrefs.GetInt("record");
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
        DayPanel[1].text = day.ToString();
        if (PlayerPrefs.HasKey("record"))
        {
            if (day > PlayerPrefs.GetInt("record"))
            {
                record = day;
                PlayerPrefs.SetInt("record", record);
            }
        }
        else
        {
            record = day;
            PlayerPrefs.SetInt("record", record);
        }
        recordPanel[1].text = record.ToString();
        GameOwer.SetActive(true);
    }
    private void StartStep()
    {
        day++;
        Day.text = day.ToString();
        StepCount = DefoltStepCount;
        Step.text = StepCount.ToString();
        Match3Control.enabled = true;
    }
    public void pausa(int timescale)
    {
        DayPanel[0].text = day.ToString();
        recordPanel[0].text = record.ToString();
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
    private void SpawnBoss(float hp)
    {
        BossHealth.gameObject.SetActive(true);
        BossHealth.maxValue = hp;
    }
    private void HitBoss(float hp)
    {
        BossHealth.value = hp;
    }
    private void DestroyBoss()
    {
        BossHealth.gameObject.SetActive(false);
    }
}
