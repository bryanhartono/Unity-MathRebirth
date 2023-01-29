using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    [SerializeField] Sprite[] characterSprites;
    [SerializeField] Unit playerUnit;
    [SerializeField] Image playerSprites;
    [SerializeField] TextMeshProUGUI Trait;
    [SerializeField] TextMeshProUGUI Level;

    [SerializeField] TextMeshProUGUI Str;
    [SerializeField] TextMeshProUGUI Agi;
    [SerializeField] TextMeshProUGUI Int;

    [SerializeField] TextMeshProUGUI Atk;
    [SerializeField] TextMeshProUGUI Def;
    [SerializeField] TextMeshProUGUI MaxHP;
    [SerializeField] TextMeshProUGUI Crit;
    [SerializeField] TextMeshProUGUI MathTime;
    [SerializeField] TextMeshProUGUI MathMult;

    [SerializeField] GameObject LevelUpPrompt;
    [SerializeField] TextMeshProUGUI LevelUpPromptText;
    public GameObject LevelUpResult;
    [SerializeField] TextMeshProUGUI LevelUpResultText;
    GlobalControl glob;
    // Start is called before the first frame update
    void Start()
    {
        glob = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Trait.text = playerUnit.trait;
        switch (playerUnit.trait)
        {
            case "Strong Body":
                playerSprites.sprite = characterSprites[0]; ;
                break;
            case "Agile Body":
                playerSprites.sprite = characterSprites[1]; ;
                break;
            case "Enhanced Mind":
                playerSprites.sprite = characterSprites[2]; ;
                break;
            case "Average Joe":
                playerSprites.sprite = characterSprites[3]; ;
                break;
            default:
                playerSprites.sprite = characterSprites[0]; ;
                break;

        }

        Level.text = playerUnit.unitLevel.ToString();

        Str.text = playerUnit.Str.ToString();
        Agi.text = playerUnit.Agi.ToString();
        Int.text = playerUnit.Int.ToString();

        Atk.text = playerUnit.Atk.ToString("0.0");
        Def.text = playerUnit.Def.ToString();
        MaxHP.text = playerUnit.currentHP.ToString("0.0") + "/" + playerUnit.maxHP.ToString("0.0");
        Crit.text = playerUnit.CRate.ToString("0.00") + "%" + "\n" + (playerUnit.CDmg * 100.0f).ToString("0.0") + "%";
        MathTime.text = (10.0f + playerUnit.ExtraMult).ToString("0.000");
        MathMult.text = playerUnit.ExtraTime.ToString("0.00");

    }

    public void closePage()
    {
        gameObject.SetActive(false);
        glob.inCharPage = false;
    }

    public void levelUp()
    {
        if (playerUnit.unitLevel + 1 > 100)
        {
            print("Max Level");
        }
        else playerUnit.unitLevel += 1;
    }

    public void levelUpPrompt()
    {
        float cost = 100 * playerUnit.unitLevel * (1 + playerUnit.unitLevel / 25.0f);
        int levelUpCost = (int)cost;
        if (glob.playerCurrency >= levelUpCost)
        {
            if(playerUnit.unitLevel >= 100)
            {
                LevelUpResultText.text = "Player Already Reached Max Level!";
                LevelUpResult.SetActive(true);
                StartCoroutine(Coroutine());
            }
            else
            {
                LevelUpResultText.text = "Player Level Up!";
                LevelUpResult.SetActive(true);
                glob.playerCurrency -= levelUpCost;
                StartCoroutine(Coroutine());
                levelUp();
            }
        }
        else
        {
            LevelUpResultText.text = "Not Enough IQ!";
            LevelUpResult.SetActive(true);
            StartCoroutine(Coroutine());
        }
        closeWindow();
    }

    public void openWindow()
    {
        float cost = 100 * playerUnit.unitLevel * (1 + playerUnit.unitLevel / 25.0f);
        int levelUpCost = (int)cost;
        LevelUpPrompt.SetActive(true);
        LevelUpPromptText.text = "Spend " + levelUpCost.ToString() + " IQ to Level Up?";
    }
    public void closeWindow()
    {
        LevelUpPrompt.SetActive(false);
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSecondsRealtime(3);
        LevelUpResult.SetActive(false);
    }
}
