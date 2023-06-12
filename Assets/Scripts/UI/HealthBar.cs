using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;

    private Player_Health playerHealth;


    private void Awake()
    {
        healthBar = GetComponent<Image>(); 
        playerHealth = FindAnyObjectByType<Player_Health>();
    }

    private void Update()
    {
        healthBar.fillAmount = playerHealth.Health;
    }
}
