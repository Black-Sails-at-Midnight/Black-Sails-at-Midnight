using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [System.Serializable]
    public class TimeSpan {
        [Min(0)]
        public int Minutes = 10;

        [Min(0)]
        public int Seconds = 0;

        public float TotalSeconds 
        {
            get {
                return (Minutes * 60) + Seconds;
            }
        }
    }

    [SerializeField]
    public TimeSpan dayNightCycleTime;

    private Animator sunAnimator;

    void Start()
    {
        sunAnimator = GetComponentInChildren<Animator>();

        float playbackRate = (1f / dayNightCycleTime.TotalSeconds);
        sunAnimator.speed = (playbackRate);
    }
}
