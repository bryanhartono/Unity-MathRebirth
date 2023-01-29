using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Calculator : MonoBehaviour
{
    int final;
    public CombatManager combatManagerScript;
    public BattleHUD playerHUD;

    public GameObject Question, Result;
    public TextMeshProUGUI PrimaryDigit, SecondaryDigit, TertiaryDigit, SignDigit;
    public GameObject TMP_InputField_Answer;
    public GameObject Correct, Wrong, TimesUp;
    public GameObject hourclock;
    public Unit player;
    [SerializeField] TextMeshProUGUI countdownText;
    public float currentTime = 0f, startTime = 10f, maxTime = 0.0f;
    int temp;
    public bool onTime = true, answer_correct = false, keepTimer=true;
    public Animator hourclockAnimator;
    float lotteryTime = 3f;
    bool runLottery = false;
    public int mode;
    public string actions = "Attack";
    Vector2 startingPos;
    Vector2 rotationPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos.x = hourclock.transform.position.x;
        startingPos.y = hourclock.transform.position.y;
        rotationPos.x = hourclock.transform.rotation.x;
        rotationPos.y = hourclock.transform.rotation.y;
    }

    void OnEnable()
    {
        print("enabled");
        lotteryTime = 2f;
        runLottery = true;
        keepTimer = true;
        // CalculatorFn("addition");
        countdownText.color = Color.white;
        maxTime = startTime + player.ExtraTime;
        currentTime = maxTime;
        Question.SetActive(true);
        TMP_InputField_Answer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && combatManagerScript.state == BattleState.PLAYERTURN && TMP_InputField_Answer.GetComponent<TMP_InputField>().text!="" && TMP_InputField_Answer.GetComponent<TMP_InputField>().text!="-")
        {
            string answer = TMP_InputField_Answer.GetComponent<TMP_InputField>().text;
            Debug.Log("User Answer: " + answer);
            combatManagerScript.answered = true;
            keepTimer = false;
            if(mode == 1){
                CheckAnswerFn(answer,mode,int.Parse(PrimaryDigit.text),int.Parse(SecondaryDigit.text),SignDigit.text);
            }
            else if(mode == 2){
                CheckAnswerFn(answer,mode,int.Parse(PrimaryDigit.text),int.Parse(TertiaryDigit.text),SignDigit.text);
            }
            else if(mode == 3){
                CheckAnswerFn(answer,mode,int.Parse(SecondaryDigit.text),int.Parse(TertiaryDigit.text),SignDigit.text);
            } 
        }

        // timer
        if(keepTimer && combatManagerScript.state == BattleState.PLAYERTURN)
        {   
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0.0"); 
        }
        
        if(currentTime <= 3)
        {   
            countdownText.color = Color.red;
            hourclockAnimator.Play("hourclock_rotate");
        }

        if (currentTime > 0)
        {
            onTime = true;
        }
        else if (currentTime <= 0)
        {   
            currentTime = 0;
            combatManagerScript.answered = true;
            keepTimer = false;
            onTime = false;
            answer_correct = false;
            Question.SetActive(false);
            TimesUp.SetActive(true);
            Result.SetActive(true);
            TMP_InputField_Answer.GetComponent<TMP_InputField>().text = "";
            combatManagerScript.goDown = false;
            hourclock.transform.rotation = Quaternion.Euler(rotationPos.x,rotationPos.y,0f);
        }
        if (runLottery && lotteryTime > 0.2)
        {
            lotteryTime -= 1 * Time.deltaTime;
            setSign(SignDigit);
            print("mode: " + mode);
            if(mode == 1){
                setDigit(TertiaryDigit);
                if(SignDigit.text == "/")
                {
                    setDigit(SecondaryDigit);
                    int assignedDigit = int.Parse(SecondaryDigit.text);
                    PrimaryDigit.text = (UnityEngine.Random.Range(1, 10) * assignedDigit).ToString();
                }
                else {
                    setDigit(PrimaryDigit);
                    setDigit(SecondaryDigit);
                }
            }
            else if(mode == 2){
                setDigit(SecondaryDigit);
                if(SignDigit.text == "*")
                {
                    setDigit(PrimaryDigit);
                    int assignedDigit = int.Parse(PrimaryDigit.text);
                    TertiaryDigit.text = (UnityEngine.Random.Range(1, 10) * assignedDigit).ToString();
                }
                else if (SignDigit.text == "/"){
                    setDigit(TertiaryDigit);
                    int assignedDigit = int.Parse(TertiaryDigit.text);
                    PrimaryDigit.text = (UnityEngine.Random.Range(1, 10) * assignedDigit).ToString();
                }
                else if (SignDigit.text == "%"){
                    setDigit(PrimaryDigit);
                    int assignedDigit = int.Parse(PrimaryDigit.text);
                    if(assignedDigit == 1)
                    {
                    TertiaryDigit.text = "0";
                    }
                    else if(assignedDigit % 2 == 0)
                    {
                    TertiaryDigit.text = UnityEngine.Random.Range(0, assignedDigit/2).ToString();                        
                    }
                    else if (assignedDigit % 2 != 0)
                    {
                    TertiaryDigit.text = UnityEngine.Random.Range(0, assignedDigit/2+1).ToString();
                    }
                }
                else {
                    setDigit(PrimaryDigit); 
                    setDigit(TertiaryDigit);
                }
            }
            else if(mode == 3){
                setDigit(PrimaryDigit);
                if(SignDigit.text == "*")
                {
                    setDigit(SecondaryDigit);
                    int assignedDigit = int.Parse(SecondaryDigit.text);
                    TertiaryDigit.text = (UnityEngine.Random.Range(1, 10) * assignedDigit).ToString();
                }
                else if (SignDigit.text == "/"){
                    setDigit(TertiaryDigit);
                    int assignedDigit = int.Parse(TertiaryDigit.text);
                    SecondaryDigit.text = (UnityEngine.Random.Range(1, 10)).ToString();
                }
                else if (SignDigit.text == "%"){
                    setDigit(SecondaryDigit);
                    int assignedDigit = int.Parse(SecondaryDigit.text);
                    if(assignedDigit == 1)
                    {
                    TertiaryDigit.text = "0";
                    }
                    else if(assignedDigit % 2 == 0)
                    {
                    TertiaryDigit.text = UnityEngine.Random.Range(0, assignedDigit/2).ToString();                        
                    }
                    else if (assignedDigit % 2 != 0)
                    {
                    TertiaryDigit.text = UnityEngine.Random.Range(0, assignedDigit/2+1).ToString();
                    }
                }
                else {
                    setDigit(SecondaryDigit);
                    setDigit(TertiaryDigit);
                }
            }                
        }
        else if (runLottery)
        {   
            runLottery = false;
            if(mode == 1)
            {
                TertiaryDigit.text = "?";
            }
            else if(mode == 2)
            {
                SecondaryDigit.text = "?";
            }
            else if(mode == 3)
            {
                PrimaryDigit.text = "?";
            }
            TMP_InputField_Answer.SetActive(true);
            TMP_InputField_Answer.GetComponent<TMP_InputField>().Select();
            TMP_InputField_Answer.GetComponent<TMP_InputField>().ActivateInputField();  
        }

    }

    public void CalculatorFn(string operation, int primary, int secondary, int mode)
    {
        print("primary" + primary);
        print("secondary" + secondary);

        if (operation == "+")
        {
            final = primary + secondary;
        }
        if (operation == "-")
        {
            final = primary - secondary;
        }
        if (operation == "*")
        {
            final = primary * secondary;
        }
        if (operation == "/")
        {
            final = primary / secondary;
        }
        if (operation == "%")
        {
            final = primary % secondary;
        }
    }
    public void CheckAnswerFn(string answer,int mode,int a, int b,string operation)
    {   
        int user_answer = Convert.ToInt32(answer);
        if(mode == 1){
            if (operation == "+")
            {
                if(a+b == user_answer)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "-")
            {
                if(a-b == user_answer)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "*")
            {
                if(a*b == user_answer)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "/")
            {
                if(a/b == user_answer)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "%")
            {
                if(a%b == user_answer)
                {
                    answer_correct = true;
                }
            }
        }
        else if(mode == 2)
        {
            if (operation == "+")
            {
                if(a+user_answer == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "-")
            {
                if(a-user_answer == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "*")
            {
                if(a*user_answer == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "/")
            {
                if(user_answer != 0 && a / user_answer == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "%")
            {
                if(a%user_answer == b)
                {
                    answer_correct = true;
                }
            }
        }
        else if(mode == 3)
        {
            if (operation == "+")
            {
                if(user_answer+a == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "-")
            {
                if(user_answer-a == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "*")
            {
                if(user_answer*a == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "/")
            {
                if(user_answer/a == b)
                {
                    answer_correct = true;
                }
            }
            else if (operation == "%")
            {
                if(user_answer%a == b)
                {
                    answer_correct = true;
                }
            }
        }
        // Debug.Log("user_answer: " + answer.GetType() + user_answer + answer + " " + "final: " + final);
        
        if (answer_correct)
        {   
            answer_correct = true;
            Question.SetActive(false);
            Result.SetActive(true);
            Correct.SetActive(true);
            TimesUp.SetActive(false);
            Debug.Log("Correct");
        }
        else if (!answer_correct)
        {
            Question.SetActive(false);
            Result.SetActive(true);
            Wrong.SetActive(true);
            TimesUp.SetActive(false);
            Debug.Log("wrong");
        }
        TMP_InputField_Answer.GetComponent<TMP_InputField>().text = "";
        combatManagerScript.goDown = false;
        hourclock.transform.rotation = Quaternion.Euler(rotationPos.x,rotationPos.y,0f);
    }

    public void setDigit(TextMeshProUGUI digitUI)
    {
        digitUI.text = UnityEngine.Random.Range(1, 10).ToString();
    }

    public void setSign(TextMeshProUGUI signUI)
    {
        GlobalControl globc = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
        string[] signs = { "+", "-", "*", "/", "%" };
        string[] low_signs = { "+", "-" };
        string[] high_signs = { "*", "/", "%" };
        string[] splitted;
        //print(actions + " AAAAAAAAAAAAAAAAA");
        if (actions == "Attack")
        {
            signUI.text = signs[UnityEngine.Random.Range(0, signs.Length)];
        }
        else if (actions == "Skill1")
        {
            splitted = globc.skill1.Split("_");
            if (splitted[1] == "Skill1" || splitted[1] == "Skill2")
            {
                signUI.text = low_signs[UnityEngine.Random.Range(0, low_signs.Length)];
            }
            else if (splitted[1] == "Skill3" || splitted[1] == "Skill4")
            {
                signUI.text = high_signs[UnityEngine.Random.Range(0, high_signs.Length)];
            }
        }
        else if (actions == "Skill2")
        {
            splitted = globc.skill2.Split("_");
            if (splitted[1] == "Skill" || splitted[1] == "Skill1" || splitted[1] == "Skill2")
            {
                signUI.text = low_signs[UnityEngine.Random.Range(0, low_signs.Length)];
            }
            else if (splitted[1] == "Skill3" || splitted[1] == "Skill4")
            {
                signUI.text = high_signs[UnityEngine.Random.Range(0, high_signs.Length)];
            }
        }

    }

}
