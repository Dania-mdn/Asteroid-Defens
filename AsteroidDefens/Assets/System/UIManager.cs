using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Slider Health;
    public int HealthCount = 5;
    public TextMeshProUGUI Step;
    public TextMeshProUGUI StepAdd;
    private int day = 1;
    public TextMeshProUGUI Day;
    public TextMeshProUGUI DayAdd;
    public TextMeshProUGUI[] DayPanel;
    private int record;
    public TextMeshProUGUI[] recordPanel;
    public int DefoltStepCount = 5;
    public int StepCount;

    public Match3Control Match3Control;

    public GameObject GameOwer;
    public GameObject WatchAdd;

    public Slider BossHealth;
    public Animation AnimationHit;
    public Animation AnimationStep;
    public Animation AnimationDay;
    public Toggle TogglAudio;
    public AudioSource StepAudio;
    public AudioSource DayAudio;
    private bool isMuteAudio;

    public GameObject tich;

    private void OnEnable()
    {
        SystemEvent.Step += SetStep;
        SystemEvent.HitPlayer += SetHealth;
        SystemEvent.AddHealth += AddHealth;
        SystemEvent.StartStep += StartStep;
        SystemEvent.FullStep += FullStep;
        SystemEvent.EndGame += SetGameOwer;
        SystemEvent.AddStep += AddStep;
        SystemEvent.SpawnBoss += SpawnBoss;
        SystemEvent.DestroyBoss += DestroyBoss;
        SystemEvent.HitBoss += HitBoss;
        SystemEvent.MuteAudio += AudioMute;
        SystemEvent.PlayAudio += AudioPlay;
    }
    private void OnDisable()
    {
        SystemEvent.Step -= SetStep;
        SystemEvent.HitPlayer -= SetHealth;
        SystemEvent.AddHealth -= AddHealth;
        SystemEvent.StartStep -= StartStep;
        SystemEvent.FullStep -= FullStep;
        SystemEvent.EndGame -= SetGameOwer;
        SystemEvent.AddStep -= AddStep;
        SystemEvent.SpawnBoss -= SpawnBoss;
        SystemEvent.DestroyBoss -= DestroyBoss;
        SystemEvent.HitBoss -= HitBoss;
        SystemEvent.MuteAudio -= AudioMute;
        SystemEvent.PlayAudio -= AudioPlay;
    }
    private void Start()
    {
        Health.maxValue = HealthCount;
        Health.value = HealthCount;
        StepCount = DefoltStepCount;
        Step.text = StepCount.ToString();
        Day.text = day.ToString(); 
        record = PlayerPrefs.GetInt("record");
        if (PlayerPrefs.HasKey("MuteAudio"))
            TogglAudio.isOn = true;

        PlayerPrefs.DeleteKey("Reward");

        if(PlayerPrefs.HasKey("Tich") == false)
            tich.SetActive(true);
        else
            tich.SetActive(false);
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
    private void AddHealth()
    {
        HealthCount = 5;
        Health.value = HealthCount;
        AnimationHit.Play();
        PlayerPrefs.SetInt("Reward", 1);
    }
    private void SetHealth(GameObject Asteroid)
    {
        if(HealthCount > 0)
        {
            HealthCount--;
            Health.value = HealthCount;
            AnimationHit.Play();
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
        if (PlayerPrefs.HasKey("Reward") == false)
        {
            WatchAdd.SetActive(true);
        }
        else
        {
            WatchAdd.SetActive(false);
            PlayerPrefs.DeleteKey("Reward");
        }
    }
    private void StartStep()
    {
        DayAudio.Play();
        day++; 
        AnimationDay.Play();
        DayAdd.text = "+1".ToString();
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
        PlayerPrefs.SetInt("Tich", 1);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void AddStep(int step)
    {
        StepAudio.Play();
        AnimationStep.Play();
        StepCount = StepCount + step;
        Step.text = StepCount.ToString();
        StepAdd.text = "+" + step.ToString();
    }
    private void SpawnBoss(float hp)
    {
        BossHealth.gameObject.SetActive(true);
        BossHealth.maxValue = hp;
        BossHealth.value = hp;
    }
    private void HitBoss(float hp)
    {
        BossHealth.value = hp;
    }
    private void DestroyBoss()
    {
        BossHealth.gameObject.SetActive(false);
    }
    public void Audio()
    {
        if (isMuteAudio == false)
        {
            isMuteAudio = true;
            SystemEvent.DoMuteAudio();
            PlayerPrefs.SetInt("MuteAudio", 1);
        }
        else
        {
            isMuteAudio = false;
            SystemEvent.DoPlayAudio();
            PlayerPrefs.DeleteKey("MuteAudio");
        }
    }
    public void AudioMute()
    {
        StepAudio.mute = true;
        DayAudio.mute = true;
    }
    public void AudioPlay()
    {
        StepAudio.mute = false;
        DayAudio.mute = false;
    }
}
