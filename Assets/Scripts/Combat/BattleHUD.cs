using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{   
    public CombatManager combatManagerScript;
    public Calculator calculatorScript;
    
    public GameObject player;
    Unit playerUnit;
    public GameObject enemy;
    Unit enemyUnit;
    public Slider enemyhealth_slider;
    public Image health;
    public TextMeshProUGUI health_text;
    public TextMeshProUGUI stamina_text;
    public TextMeshProUGUI battle_text;
    public Material graphmat_green;
    public Material graphmat_black;


    GameObject glob;
    GlobalControl globc;
    float curr_health;
    float max_health;
    float curr_stamina;
    float max_stamina;
    float removed;
    public void SetMaxHealth(float health)
    {
        enemyhealth_slider.maxValue = health;
        enemyhealth_slider.value = health;
    }
    public void SetHealth(float health)
    {
        enemyhealth_slider.value = health;
    }
    void Start()
    {
        glob = GameObject.Find("GlobalObject");
        globc = glob.GetComponent<GlobalControl>();

        playerUnit = player.GetComponent<Unit>();
        enemyUnit = enemy.GetComponent<Unit>();
        curr_health = playerUnit.currentHP;
        max_health = playerUnit.maxHP;
        curr_stamina = playerUnit.currentStamina;
        max_stamina = playerUnit.maxStamina;
        health.fillAmount = curr_health/max_health;
        health_text.text = (curr_health/max_health).ToString("P").Replace(" ",string.Empty);
        stamina_text.text = curr_stamina.ToString();

        removed = max_stamina - curr_stamina;
        graphmat_green.SetFloat("_segmentCount", max_stamina);
        graphmat_green.SetFloat("_RemovedSegment", removed);
        graphmat_black.SetFloat("_segmentCount", max_stamina);
    }
    public void SetHUD(Unit unit)
    {   
        curr_health = unit.currentHP;
        max_health = unit.maxHP;
        health.fillAmount = curr_health/max_health;
        health_text.text = (curr_health/max_health).ToString("P").Replace(" ",string.Empty);

        curr_stamina = unit.currentStamina;
        max_stamina = unit.maxStamina;
        removed = max_stamina - curr_stamina;
        graphmat_green.SetFloat("_segmentCount", max_stamina);
        graphmat_green.SetFloat("_RemovedSegment", removed);
        graphmat_black.SetFloat("_segmentCount", max_stamina);
        stamina_text.text = curr_stamina.ToString();  
    }
    public void onPressAttack()
    {   
        curr_stamina -=1;
        playerUnit.currentStamina -= 1;
        removed = max_stamina - curr_stamina;
        graphmat_green.SetFloat("_RemovedSegment", removed);
        stamina_text.text = curr_stamina.ToString();
        combatManagerScript.Attack.OnPointerExit(null);
    }
    public void onPressDefend()
    {   
        curr_stamina -=1;
        playerUnit.currentStamina -= 1;
        removed = max_stamina - curr_stamina;
        graphmat_green.SetFloat("_RemovedSegment", removed);
        stamina_text.text = curr_stamina.ToString();     
    }
    public void onPressSkill1()
    {
        curr_stamina -= globc.skillDict[globc.skill1][4];
        playerUnit.currentStamina -= (int)globc.skillDict[globc.skill1][4];
        removed = max_stamina - curr_stamina;
        graphmat_green.SetFloat("_RemovedSegment", removed);
        stamina_text.text = curr_stamina.ToString();
    }
    public void onPressSkill2()
    {
        curr_stamina -= globc.skillDict[globc.skill2][4];
        playerUnit.currentStamina -= (int)globc.skillDict[globc.skill2][4];
        removed = max_stamina - curr_stamina;
        graphmat_green.SetFloat("_RemovedSegment", removed);
        stamina_text.text = curr_stamina.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
