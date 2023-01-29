using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Cinemachine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class CombatManager : MonoBehaviour
{
    public Calculator calculatorScript;
    public BattleHUD playerHUD;
    public PlayerMovement playerMov;
    public CinemachineVirtualCamera CMVir;

    public GameObject combatUI;
    public GameObject moves;

    public Button Attack;
    public Button Defend;
    public Button Special1;
    public Button Special2;

    public GameObject questions;
    public GameObject player;
    public GameObject enemy;
    public GameObject soul;

    public bool answered = false;
    private float movementSpeed = 400f;
    public bool goDown = false;

    private Vector3 curr_position;

    public Animator playerAnimator;
    public Animator skeletonAnimator;
    public Animator CalculatorAnimator;

    Unit playerUnit;
    Unit enemyUnit;
    public BattleState state;
    public int dead = 0;
    public int soulCurrency = 0;
    public int roundCounter = 0;
    bool isDead, isDefend = false;
    float actionMultiplier = 1.0f;
    float actionHit = 1.0f;
    string actionName = "Attack";
    int buffTurns = 0;
    int debuffTurns = 0;
    int flag = 0;

    GameObject glob;
    GlobalControl globc;

    Dictionary<EnemyBehaviour, float> speedHolder;
    Dictionary<PlatformMovement, float> speedPlatformHolder;
    Dictionary<string, int> currencyMult = new Dictionary<string, int>();
    Dictionary<string, float[]> skillDict = new Dictionary<string, float[]>();
    Dictionary<string, string[]> skillList = new Dictionary<string, string[]>();

    void Start()
    {
        glob = GameObject.Find("GlobalObject");
        globc = glob.GetComponent<GlobalControl>();

        skillDict = globc.skillDict;

        string[] animations;
        //                           player  , calculator
        animations = new string[2] { "AttackSpike", "NumbersTomb" };
        skillList.Add("Default_Skill1", animations);

        animations = new string[2] { "AttackSpike", "EarthSpike" };
        skillList.Add("Str_Skill1", animations);
        animations = new string[2] { "AttackSpike", "Fireball" };
        skillList.Add("Str_Skill2", animations);
        animations = new string[2] { "AttackSpike", "EarthCrusher" };
        skillList.Add("Str_Skill3", animations);
        animations = new string[2] { "AttackSpike", "FireExplode" };
        skillList.Add("Str_Skill4", animations);

        animations = new string[2] { "AttackSpike", "Waterball" };
        skillList.Add("Agi_Skill1", animations);
        animations = new string[2] { "AttackSpike", "DarkBolt" };
        skillList.Add("Agi_Skill2", animations);
        animations = new string[2] { "AttackSpike", "ShadowGhost" };
        skillList.Add("Agi_Skill3", animations);
        animations = new string[2] { "AttackSpike", "SparkLightning" };
        skillList.Add("Agi_Skill4", animations);

        animations = new string[2] { "AttackSpike", "Pulse" };
        skillList.Add("Int_Skill1", animations);
        animations = new string[2] { "AttackSpike", "CrossedPulse" };
        skillList.Add("Int_Skill2", animations);
        animations = new string[2] { "AttackSpike", "HolyHeal" };
        skillList.Add("Int_Skill3", animations);
        animations = new string[2] { "AttackSpike", "WaterBlast" };
        skillList.Add("Int_Skill4", animations);

        // Area 1
        currencyMult.Add("Skeleton", 8);
        currencyMult.Add("Bat", 8);
        currencyMult.Add("Zombie", 10);
        currencyMult.Add("Shade", 12);
        currencyMult.Add("TrashCave", 14);
        currencyMult.Add("Boss1", 90);

        // Area 2
        currencyMult.Add("SlimeForest", 8);
        currencyMult.Add("Mushroom", 8);
        currencyMult.Add("Tooth", 10);
        currencyMult.Add("Goblin", 12);
        currencyMult.Add("TrashForest", 14);
        currencyMult.Add("Boss2", 120);

        // Area 3
        currencyMult.Add("Eyeball", 8);
        currencyMult.Add("FlyEye", 8);
        currencyMult.Add("Fireworm", 10);
        currencyMult.Add("Imp", 12);
        currencyMult.Add("Demon", 14);
        currencyMult.Add("Boss3", 150);

    }
    public void StartCombat()
    {
        roundCounter = 0;

        playerUnit = player.GetComponent<Unit>();
        enemyUnit = enemy.GetComponent<Unit>();
        enemyUnit.ReStat();
        enemyUnit.currentHP = enemyUnit.maxHP;
        calculatorScript.enabled = false;
        playerHUD.SetHUD(playerUnit);
        playerHUD.SetMaxHealth(enemyUnit.maxHP);
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        curr_position = combatUI.transform.position;
        playerHUD.SetMaxHealth(enemyUnit.maxHP);

    }
    IEnumerator SetupBattle()
    {
        playerUnit = player.GetComponent<Unit>();
        enemyUnit = enemy.GetComponent<Unit>();
        speedHolder = new Dictionary<EnemyBehaviour, float>();
        foreach (EnemyBehaviour enemies in GameObject.FindObjectsOfType<EnemyBehaviour>())
        {
            speedHolder.Add(enemies, enemies.speed);
            enemies.speed = 0;
            enemies.startIdle();
        }

        speedPlatformHolder = new Dictionary<PlatformMovement, float>();
        foreach (PlatformMovement platform in GameObject.FindObjectsOfType<PlatformMovement>())
        {
            speedPlatformHolder.Add(platform, platform.speed);
            platform.speed = 0;
        }
        print("Battle Starts");
        yield return new WaitForSeconds(0.5f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    void PlayerTurn()
    {
        playerHUD.battle_text.color = Color.white;
        roundCounter += 1;
        print("Player Turn");
        playerHUD.battle_text.text = "Player Turn";
        // moves.SetActive(true);
        Attack.OnPointerExit(null);
        Defend.OnPointerExit(null);
        Special1.OnPointerExit(null);
        Special2.OnPointerExit(null);
        Attack.interactable = true;
        Defend.interactable = true;
        if (playerUnit.currentStamina >= (int)skillDict[globc.skill1][4])
        {
            Special1.interactable = true;
        }
        else Special1.interactable = false;

        if (playerUnit.currentStamina >= (int)skillDict[globc.skill2][4])
        {
            Special2.interactable = true;
        }
        else Special2.interactable = false;

        if (buffTurns == 0) playerUnit.ReStat();
        if (debuffTurns == 0) enemyUnit.ReStat();

        buffTurns -= 1;
        debuffTurns -= 1;
    }
    // Update is called once per frame
    void Update()
    {

        Vector3 aPos = combatUI.transform.position;
        if (aPos.y > -300 && goDown)
        {
            aPos.x = aPos.z = 0;
            aPos.y = -movementSpeed * Time.deltaTime;
            // print(Time.deltaTime);
            // print(aPos);
            combatUI.transform.position += aPos;
        }
        if (!goDown && state == BattleState.PLAYERTURN)
        {
            combatUI.transform.position = curr_position;
        }
    }

    public void onPressed(string action)
    {
        print("pressed");
        goDown = true;
        calculatorScript.enabled = true;
        questions.SetActive(true);
    }

    public void onAttackButton()
    {
        actionName = "Attack";
        calculatorScript.actions = actionName;
        //print(actionName);
        actionHit = 1.0f;
        actionMultiplier = Random.Range(0.8f, 1.0f);
        calculatorScript.mode = UnityEngine.Random.Range(1, 4);

        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }

    public void onDefendButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        // moves.SetActive(false);
        Attack.OnPointerExit(null);
        Defend.OnPointerExit(null);
        Special1.OnPointerExit(null);
        Special2.OnPointerExit(null);
        Attack.interactable = false;
        Defend.interactable = false;
        Special1.interactable = false;
        Special2.interactable = false;

        StartCoroutine(PlayerDefend());
    }

    public void onSkill1Button()
    {
        actionName = "Skill1";
        calculatorScript.actions = actionName;
        //print(actionName);
        actionHit = skillDict[globc.skill1][0];
        actionMultiplier = Random.Range(skillDict[globc.skill1][1], skillDict[globc.skill1][2]);
        calculatorScript.mode = UnityEngine.Random.Range(1, 4);

        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }

    public void onSkill2Button()
    {
        actionName = "Skill2";
        calculatorScript.actions = actionName;
        //print(actionName);
        actionHit = skillDict[globc.skill2][0];
        actionMultiplier = Random.Range(skillDict[globc.skill2][1], skillDict[globc.skill2][2]);
        calculatorScript.mode = UnityEngine.Random.Range(1, 4);

        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }
    IEnumerator PlayerDefend()
    {
        isDefend = true;
        CalculatorAnimator.Play("Shield");
        yield return new WaitForSeconds(1.0f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator PlayerAttack()
    {
        yield return new WaitUntil(() => answered == true);

        float mathMult;
        float critRoller = Random.Range(0.01f, 100.0f);
        bool isCrit = critRoller <= playerUnit.CRate;
        float CDmg = playerUnit.CDmg;

        Attack.OnPointerExit(null);
        Defend.OnPointerExit(null);
        Special1.OnPointerExit(null);
        Special2.OnPointerExit(null);
        Attack.interactable = false;
        Defend.interactable = false;
        Special1.interactable = false;
        Special2.interactable = false;
        flag = 0;
        enemy.GetComponent<SpriteRenderer>().sortingOrder = 0;
        player.GetComponent<SpriteRenderer>().sortingOrder = 1;

        if (isCrit && (skillDict[globc.skill1][5] == 1.0f && actionName == "Skill1" || skillDict[globc.skill2][5] == 1.0f && actionName == "Skill2" || actionName == "Attack"))
        {
            bool redCrit = playerUnit.CRate > 100.0f;
            float redCritRoller = Random.Range(0.01f, 100.0f);

            if (redCrit)
            {
                redCrit = redCritRoller <= (playerUnit.CRate - 100.0f);
            }

            if (redCrit)
            {
                print("Red Critical Damage! Rate Rolled: " + redCritRoller);
                CDmg = playerUnit.CDmg * 1.5f;
                flag = 2;
            }
            else if (!redCrit)
            {
                print("Critical Damage! Rate Rolled: " + critRoller);
                CDmg = playerUnit.CDmg;
                flag = 1;
            }
        }
        else CDmg = 1.0f;
        

        //moves.SetActive(false);
        if (actionName == "Attack")
        {
            playerAnimator.Play("attack");
            CalculatorAnimator.Play("CalculatorThrow");
            skeletonAnimator.Play("hurt");
            //skeletonAnimator.Play("idle");
            CalculatorAnimator.Play("CalculatorIdle");
        }
        else if (actionName == "Skill1")
        {
            playerAnimator.Play(skillList[globc.skill1][0]);
            CalculatorAnimator.Play(skillList[globc.skill1][1]);
            if(skillDict[globc.skill1][5] == 1.0f)
            {
                skeletonAnimator.Play("hurt");
                //skeletonAnimator.Play("idle");
            }
            else if (skillDict[globc.skill1][5] == 2.0f)
            {
                playerUnit.currentHP = playerUnit.maxHP;
                if (calculatorScript.answer_correct && calculatorScript.onTime)
                {
                    playerUnit.Def *= 1.0f + 0.7f * (0.1f + calculatorScript.currentTime / calculatorScript.maxTime);
                }
                else playerUnit.Def *= 1.0f + 0.7f * 0.2f;
                playerHUD.battle_text.text = playerUnit.tag + " got buffed! Fully healed and defense increased to " + playerUnit.Def.ToString("0");
                playerHUD.SetHUD(playerUnit);
                buffTurns = 1;
            }
            else if (skillDict[globc.skill1][5] == 3.0f)
            {
                if (calculatorScript.answer_correct && calculatorScript.onTime)
                {
                    enemyUnit.Def *= 1.0f - 0.8f * (0.1f + calculatorScript.currentTime / calculatorScript.maxTime);
                    enemyUnit.Atk *= 1.0f - 0.2f * (0.1f + calculatorScript.currentTime / calculatorScript.maxTime);
                }
                else
                {
                    enemyUnit.Def *= 1.0f - 0.8f * 0.2f;
                    enemyUnit.Atk *= 1.0f - 0.2f * 0.2f;
                }
                playerHUD.battle_text.text = enemyUnit.unitName + " got debuffed! Defense decreased to " + enemyUnit.Def.ToString("0") + " and Attack decreased to " + enemyUnit.Atk.ToString("0");
                debuffTurns = 1;
            }

            if (skillDict[globc.skill1][3] > 0)
            {
                CalculatorAnimator.Play("CalculatorIdle");
            }
        }
        else if (actionName == "Skill2")
        {
            playerAnimator.Play(skillList[globc.skill2][0]);
            CalculatorAnimator.Play(skillList[globc.skill2][1]);
            if (skillDict[globc.skill2][5] == 1.0f)
            {
                skeletonAnimator.Play("hurt");
            }
            else if (skillDict[globc.skill2][5] == 2.0f)
            {
                playerUnit.currentHP = playerUnit.maxHP;
                if (calculatorScript.answer_correct && calculatorScript.onTime)
                {
                    playerUnit.Def *= 1.0f + 0.7f * (0.1f + calculatorScript.currentTime / calculatorScript.maxTime);
                }
                else playerUnit.Def *= 1.0f + 0.7f * 0.2f;
                playerHUD.battle_text.text = playerUnit.tag + " got buffed! Fully healed and defense increased to " + playerUnit.Def.ToString("0");
                playerHUD.SetHUD(playerUnit);
                buffTurns = 1;
            }
            else if (skillDict[globc.skill2][5] == 3.0f)
            {
                if (calculatorScript.answer_correct && calculatorScript.onTime)
                {
                    enemyUnit.Def *= 1.0f - 0.8f * (0.1f + calculatorScript.currentTime / calculatorScript.maxTime);
                    enemyUnit.Atk *= 1.0f - 0.2f * (0.1f + calculatorScript.currentTime / calculatorScript.maxTime);
                }
                else
                {
                    enemyUnit.Def *= 1.0f - 0.8f * 0.2f;
                    enemyUnit.Atk *= 1.0f - 0.2f * 0.2f;
                }
                playerHUD.battle_text.text = enemyUnit.unitName + " got debuffed! Defense decreased to " + enemyUnit.Def.ToString("0") + " and Attack decreased to " + enemyUnit.Atk.ToString("0");
                debuffTurns = 1;
            }
            if (skillDict[globc.skill2][3] > 0)
            {
                CalculatorAnimator.Play("CalculatorIdle");
            }
        }

        yield return new WaitForSeconds(2.0f);
        if (calculatorScript.answer_correct == true && calculatorScript.onTime == true)
        {
            mathMult = 1.0f + ((1.0f + playerUnit.ExtraMult) * (0.12f + calculatorScript.currentTime / calculatorScript.maxTime));

            if (skillDict[globc.skill1][5] == 1.0f && actionName == "Skill1" || skillDict[globc.skill2][5] == 1.0f && actionName == "Skill2" || actionName == "Attack")
            {
                isDead = enemyUnit.TakeDamage(playerUnit.Atk * mathMult * actionMultiplier * actionHit * CDmg, flag);
            }
            else isDead = false;
                
            print("Attack is succesful");
        }
        else if (calculatorScript.answer_correct == false || calculatorScript.onTime == false)
        {
            mathMult = 0.5f;
            if (skillDict[globc.skill1][5] == 1.0f && actionName == "Skill1" || skillDict[globc.skill2][5] == 1.0f && actionName == "Skill2" || actionName == "Attack")
            {
                isDead = enemyUnit.TakeDamage(playerUnit.Atk * mathMult * actionMultiplier * actionHit * CDmg, flag);
            }
            else isDead = false;

            print("Attack not successful");
            goDown = false;
        }

        calculatorScript.answer_correct = false;
        answered = false;

        playerHUD.SetHealth(enemyUnit.currentHP);
        playerHUD.SetHUD(playerUnit);

        if (isDead)
        {
            //end battle
            yield return new WaitForSeconds(1.0f);
            playerHUD.battle_text.color = Color.white;
            playerHUD.battle_text.text = "You Won the Battle!";
            skeletonAnimator.Play("death");
            if (skeletonAnimator.tag == "Boss1" || skeletonAnimator.tag == "Boss2" || skeletonAnimator.tag == "Boss3")
            {
                yield return new WaitForSeconds(1.0f);

                AudioClip winning = (AudioClip) Resources.Load("win");

                enemy.GetComponent<AudioSource>().PlayOneShot(winning);
                yield return new WaitForSeconds(6.0f);
            }
            else yield return new WaitForSeconds(2.8f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //enemy turn
            yield return new WaitForSeconds(2.0f);
            calculatorScript.enabled = false;
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator EnemyTurn()
    {
        playerHUD.battle_text.color = Color.white;
        playerHUD.battle_text.text = enemyUnit.unitName + " Turn";
        print(enemyUnit.tag + " attacks!");
        calculatorScript.Question.SetActive(false);
        calculatorScript.Result.SetActive(false);
        calculatorScript.Wrong.SetActive(false);
        calculatorScript.Correct.SetActive(false);
        calculatorScript.TimesUp.SetActive(false);
        flag = 0;
        yield return new WaitForSeconds(2.0f);

        float EnemyMod = Random.Range(0.8f, 1.1f);
        float critRoller = Random.Range(0.01f, 100.0f);
        bool isCrit = critRoller <= enemyUnit.CRate;
        float CDmg = enemyUnit.CDmg;

        playerHUD.battle_text.text = enemyUnit.unitName + " attacks!";
        enemy.GetComponent<SpriteRenderer>().sortingOrder = 1;
        player.GetComponent<SpriteRenderer>().sortingOrder = 0;
        if (isCrit)
        {
            bool redCrit = enemyUnit.CRate > 100.0f;
            float redCritRoller = Random.Range(0.01f, 100.0f);

            if (redCrit)
            {
                redCrit = redCritRoller <= (enemyUnit.CRate - 100.0f);
            }

            if (redCrit)
            {
                print("Red Critical Damage! Rate Rolled: " + redCritRoller);
                CDmg = enemyUnit.CDmg * 1.5f;
                flag = 2;
            }
            else if (!redCrit)
            {
                print("Critical Damage! Rate Rolled: " + critRoller);
                CDmg = enemyUnit.CDmg;
                flag = 1;
            }
        }
        else CDmg = 1.0f;

        if (enemyUnit.CompareTag("Boss1") || enemyUnit.CompareTag("Boss2") || enemyUnit.CompareTag("Boss3"))
        {
            int bossAttackMode = 0;
            if(roundCounter % 3 == 0)
            {
                bossAttackMode = 1;
            }

            if (bossAttackMode == 0)
            {
                EnemyMod = Random.Range(0.8f, 1.1f);
                skeletonAnimator.Play("attack");
            }
            else if(bossAttackMode == 1)
            {
                EnemyMod = Random.Range(1.5f, 2.5f);
                skeletonAnimator.Play("special");
            }
        }
        else
        {
            skeletonAnimator.Play("attack");
        }
        
        playerAnimator.Play("hurt");

        yield return new WaitForSeconds(2.0f);

        if (isDefend)
        {
            isDead = playerUnit.TakeDamage(enemyUnit.Atk * EnemyMod * CDmg * 0.5f, flag);
        }
        else if (!isDefend)
        {
            isDead = playerUnit.TakeDamage(enemyUnit.Atk * EnemyMod * CDmg, flag);
        }

        playerHUD.SetHUD(playerUnit);
        isDefend = false;

        if (isDead)
        {
            yield return new WaitForSeconds(1.0f);
            playerHUD.battle_text.color = Color.white;
            print("player died");
            playerHUD.battle_text.text = "You Were Defeated!";
            playerAnimator.Play("death");
            yield return new WaitForSeconds(3.5f);
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            goDown = false;
            calculatorScript.enabled = false;
            answered = false;
            if (playerUnit.currentStamina > 0)
            {
                yield return new WaitForSeconds(2.0f);
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
                playerHUD.battle_text.color = Color.white;
                print("player died");
                playerHUD.battle_text.text = "You Were Defeated!";
                playerAnimator.Play("death");
                yield return new WaitForSeconds(3.5f);
                state = BattleState.LOST;
                EndBattle();
            }
        }
    }
    void EndBattle()
    {
        CameraSwitch.register(CMVir);

        PlayerMovement.instance.gameObject.GetComponent<Animator>().SetBool("inCombat", false);
        glob.GetComponent<GlobalControl>().inCombat = false;
        moves.SetActive(true);
        // Attack.enabled = true;
        // Defend.enabled = true;
        // Special1.enabled = true;
        // Special2.enabled = true;

        if (state == BattleState.WON)
        {
            print("You won the battle!");
            playerHUD.battle_text.text = "You Won the Battle!";
            calculatorScript.Question.SetActive(false);
            calculatorScript.Result.SetActive(false);
            calculatorScript.Wrong.SetActive(false);
            calculatorScript.Correct.SetActive(false);
            calculatorScript.TimesUp.SetActive(false);
            calculatorScript.enabled = false;

            foreach (Collider2D col in playerMov.collidedd.GetComponents<Collider2D>())
                {
                    col.enabled = false;
                }
            playerMov.collidedd.GetComponent<SpriteRenderer>().enabled = false;
            playerMov.collidedd.GetComponent<EnemyBehaviour>().infoText.SetActive(false);

            foreach (EnemyBehaviour enemies in GameObject.FindObjectsOfType<EnemyBehaviour>())
            {
                enemies.speed = speedHolder[enemies];
                if (speedHolder[enemies] == 0)
                {
                    enemies.startIdle();
                }
                else
                {
                    enemies.startWalk(speedHolder[enemies]);
                }

            }

            foreach (PlatformMovement platform in GameObject.FindObjectsOfType<PlatformMovement>())
            {
                platform.speed = speedPlatformHolder[platform];
            }

            StartCoroutine(Coroutine());

            glob.GetComponent<GlobalControl>().playerCurrency += enemyUnit.unitLevel * currencyMult[enemyUnit.tag] * Random.Range(7, 12);

            CameraSwitch.swithcam(CMVir);
            //print(CameraSwitch.isActiveCam(CMVir));
            playerUnit.ReStat();
            player.GetComponent<Unit>().Reset(1);
        }
        else if (state == BattleState.LOST)
        {
            print("You were defeated");
            playerHUD.battle_text.text = "You Were Defeated!";

            calculatorScript.Question.SetActive(false);
            calculatorScript.Result.SetActive(false);
            calculatorScript.Wrong.SetActive(false);
            calculatorScript.Correct.SetActive(false);
            calculatorScript.TimesUp.SetActive(false);
            calculatorScript.enabled = false;
            foreach (GameObject enemy in playerMov.unityGameObjects)
            {
                foreach (Collider2D col in enemy.GetComponents<Collider2D>())
                {
                    col.enabled = true;
                }
                enemy.GetComponent<SpriteRenderer>().enabled = true;
                enemy.GetComponent<EnemyBehaviour>().infoText.SetActive(true);
            }

            soul.SetActive(true);
            soul.transform.position = playerMov.transform.position;
            soulCurrency = glob.GetComponent<GlobalControl>().playerCurrency;
            dead += 1;

            playerMov.transform.position = GameObject.Find("Player Start Pos").transform.position;
            foreach (EnemyBehaviour enemies in GameObject.FindObjectsOfType<EnemyBehaviour>())
            {
                enemies.speed = speedHolder[enemies];
                if (speedHolder[enemies] == 0)
                {
                    enemies.startIdle();
                }
                else
                {
                    enemies.startWalk(speedHolder[enemies]);
                }

            }

            foreach (PlatformMovement platform in GameObject.FindObjectsOfType<PlatformMovement>())
            {
                platform.speed = speedPlatformHolder[platform];
            }
            StartCoroutine(Coroutine());

            glob.GetComponent<GlobalControl>().playerCurrency = 0;

            CameraSwitch.swithcam(CMVir);
            playerUnit.ReStat();
            player.GetComponent<Unit>().Reset(0);
            enemy.GetComponent<Unit>().Reset(0);
        }

        foreach (EnemyBehaviour enemies in GameObject.FindObjectsOfType<EnemyBehaviour>())
        {
            enemies.speed = speedHolder[enemies];
            if (speedHolder[enemies] == 0)
            {
                enemies.startIdle();
            }
            else
            {
                enemies.startWalk(speedHolder[enemies]);
            }
            
        }

        foreach (PlatformMovement platform in GameObject.FindObjectsOfType<PlatformMovement>())
        {
            platform.speed = speedPlatformHolder[platform];
        }

        playerMov.collidedd.GetComponent<Animator>().Play("walk");
        playerHUD.SetHUD(playerUnit);
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSecondsRealtime(2.0f);
    }
}
