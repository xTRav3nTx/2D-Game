using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;

    [SerializeField]
    private Player_Health playerHealth;
    [SerializeField]
    private Enemy_Health enemyhealth;


    private void Awake()
    {
        healthBar = GetComponent<Image>(); 
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            healthBar.fillAmount = playerHealth.Health;
        }
        if(enemyhealth != null)
        {
            healthBar.fillAmount = enemyhealth.Health;
        }
        
    }
}
