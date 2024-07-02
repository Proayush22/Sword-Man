using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public Canvas healthcanvas;
    public Canvas enemycanvas;
    public Image healthbar;
    public Image enemyhealthbar;

    public TextMeshProUGUI playerhealthtext;
    public TextMeshProUGUI enemyhealthtext;

    public ParticleSystem playerhealparticle;

    public float playerhealth = 100f;
    public float enemyhealth = 100f;
    public bool isDead = false;
    public bool isEnemyDead = false;

    public float Timer;
    public float playerdamageTimer;
    public float playerhealthtimer;
    public float enemydamageTimer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerhealth <= 0)
        {
            isDead = true;
        }
        if (enemyhealth <= 0)
        {
            isEnemyDead = true;
        }

        playerhealthtext.text = playerhealth.ToString();
        enemyhealthtext.text = enemyhealth.ToString();

        Timer += Time.deltaTime;
        playerdamageTimer += Time.deltaTime;
        playerhealthtimer += Time.deltaTime;


        if (playerhealth <= 95)
        {
            if (playerhealthtimer >= 1f && playerdamageTimer >= 3f)
            {
                HealPlayer(5f);
                playerhealthtimer = 0;
            }
        }
    }
    public void TakeDamagePlayer(float damage)
    {
        if (playerdamageTimer >= 1f)
        {
            playerhealth -= damage;
            healthbar.fillAmount = playerhealth / 100f;
            playerdamageTimer = 0;
        }
    }
    public void HealPlayer(float heal)
    {
        playerhealparticle.Play();
        playerhealth += heal;
        playerhealth = Mathf.Clamp(playerhealth, 0f, 100f);
        healthbar.fillAmount = playerhealth / 100f;
    }
    public void TakeDamageEnemy(float damage)
    {
        if (playerdamageTimer >= 1f)
        {
            enemyhealth -= damage;
            enemyhealthbar.fillAmount = enemyhealth / 100f;
            enemydamageTimer = 0;
        }
    }
}
