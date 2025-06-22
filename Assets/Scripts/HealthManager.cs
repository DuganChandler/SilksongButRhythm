using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    [SerializeField] private Slider slider;
    
    private int currentHealth;

    public static event Action OnAllHealthLoss;

    private void Awake()
    {
        slider.maxValue = startingHealth;
        currentHealth = startingHealth;
    }

    private void OnEnable()
    {
        NoteNode.HitFood += LoseHealth;
    }

    private void OnDisable()
    {
        NoteNode.HitFood -= LoseHealth;
    }

    private void LoseHealth()
    {
        currentHealth--;
        slider.value++;

        if (currentHealth <= 0)
        {
            OnAllHealthLoss?.Invoke();
        }
    }
}
