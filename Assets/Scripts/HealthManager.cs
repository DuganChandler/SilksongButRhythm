using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    
    private int currentHealth;

    public static event Action OnAllHealthLoss;

    private void Awake()
    {
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

        if (currentHealth <= 0)
        {
            OnAllHealthLoss?.Invoke();
        }
    }
}
