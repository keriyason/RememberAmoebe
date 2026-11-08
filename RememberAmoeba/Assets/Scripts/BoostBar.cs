using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoostBar : MonoBehaviour

{
    public static BoostBar Instance;

    [SerializeField] private Slider slider; //the slider that shows the boost
    [SerializeField] private float maxBoost = 100f; //how much the bar holds
    [SerializeField] private float startingBoost = 0f; //how full the bar is at the start
    [SerializeField] private float pelletAmount = 20f; //boost gained per pellet
    [SerializeField] private float burstDuration = 1.5f; //seconds a FULL bar lasts while holding
    [SerializeField] private bool requireFullBar = false; //if true, you can only start boosting at full

    private float current; //how much boost you have right now

    public bool IsBursting { get; private set; } //true while boosting

    private void Awake()
    {
        Instance = this;
        current = Mathf.Clamp(startingBoost, 0f, maxBoost);

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = maxBoost;
            slider.interactable = false; //so clicking the bar does nothing
        }

        UpdateBar();
    }

    private void Update()
    {
        bool holding = Input.GetKey(KeyCode.LeftShift);

        if (IsBursting)
        {
            if (!holding)
            {
                IsBursting = false; //let go, stop boosting and keep what's left
                return;
            }

            current -= (maxBoost / burstDuration) * Time.deltaTime; //drains while held

            if (current <= 0f)
            {
                current = 0f;
                IsBursting = false; //ran out, pellets work again
            }

            UpdateBar();
        }
        else if (holding && Input.GetKeyDown(KeyCode.LeftShift)) //start on a fresh press
        {
            bool canStart = requireFullBar ? current >= maxBoost : current > 0f;
            if (canStart) IsBursting = true;
        }
    }

    public void AddBoost()
    {
        if (IsBursting) return; //pellets don't fill the bar during a boost

        current = Mathf.Min(current + pelletAmount, maxBoost); //fills the bar, never past the max
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (slider != null) slider.value = current; //shows the bar on screen
    }
}