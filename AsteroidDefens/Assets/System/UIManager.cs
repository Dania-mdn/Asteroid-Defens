using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public int StepCount = 5;
    private void OnEnable()
    {
        SystemEvent.Step += SetStep;
    }
    private void OnDisable()
    {
        SystemEvent.Step -= SetStep;
    }
    private void SetStep()
    {
        StepCount--;
    }
}
