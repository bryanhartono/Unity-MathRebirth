using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject EquipSkillPrompt;
    public GameObject[] SkillButtons;
    public GameObject[] JoeSkillButtons;
    Dictionary<string, float[]> skillDict;

    GameObject glob;
    [SerializeField] Unit playerUnit;
    string skill1;
    string skill2;
    string Trait;

    public Sprite[] strImage;
    public Sprite[] agiImage;
    public Sprite[] intImage;
    public Sprite[] avgImage;

    string[] strSkill = { "Default_Skill1", "Str_Skill1", "Str_Skill2", "Str_Skill3", "Str_Skill4", "Agi_Skill1", "Int_Skill1" };
    string[] agiSkill = { "Default_Skill1", "Agi_Skill1", "Agi_Skill2", "Agi_Skill3", "Agi_Skill4", "Str_Skill1", "Int_Skill1" };
    string[] intSkill = { "Default_Skill1", "Int_Skill1", "Int_Skill2", "Int_Skill3", "Int_Skill4", "Agi_Skill1", "Str_Skill1" };
    string[] avgSkill = { "Default_Skill1", "Str_Skill1", "Str_Skill2", "Agi_Skill1", "Agi_Skill2", "Int_Skill1", "Int_Skill2" };

    string[] skillSet;

    int openedSkillIndex;
    Dictionary<string, string[]> skillText = new Dictionary<string, string[]>();
    void Start()
    {
        glob = GameObject.Find("GlobalObject");
        skillDict = glob.GetComponent<GlobalControl>().skillDict;
        skill1 = glob.GetComponent<GlobalControl>().skill1;
        skill2 = glob.GetComponent<GlobalControl>().skill2;
        Trait = glob.GetComponent<GlobalControl>().playerTrait;
        //skill1 = "Default_Skill1";
        //skill2 = "Agi_Skill1";
        //Trait = "Agile Body";

        skillText.Add("Default_Skill1", new string[] { "Numbers Tomb", "Summon the help of calculator that rushes at nearby enemies and attacks them rapidly." });

        skillText.Add("Str_Skill1", new string[] { "Earth Spike", "Delivers an area of effect earth attack." });
        skillText.Add("Str_Skill2", new string[] { "Fireball", "Unleashes a ball of fire towards target." });
        skillText.Add("Str_Skill3", new string[] { "Earth Crusher", " An overcharged version of Earth Spike." });
        skillText.Add("Str_Skill4", new string[] { "Fire Explode", "Release an area of effect explosion, striking whatever is in range." });

        skillText.Add("Agi_Skill1", new string[] { "Water Ball", " A water attack that shoots water orb at your target." });
        skillText.Add("Agi_Skill2", new string[] { "Dark Bolt", "Evocation spell that caused dark arcs of lightning to emanate from the caster's fingertips." });
        skillText.Add("Agi_Skill3", new string[] { "Shadow Ghost", "Creates an area of darkness on the enemy, reduce enemy defense for the next turn." });
        skillText.Add("Agi_Skill4", new string[] { "Spark Lighting", "Symbolic representation of lightning when accompanied by a loud thunderclap." });

        skillText.Add("Int_Skill1", new string[] { "Pulse", " Continuously emits a single, uninterrupted light amplification by stimulated emission of radiation." });
        skillText.Add("Int_Skill2", new string[] { "Crossed Pulse", "Emits light through a process of optical amplification based on the stimulated emission of electromagnetic radiation." });
        skillText.Add("Int_Skill3", new string[] { "Holy Heal", "Action of replenishing player to full health and increase defense for the next 2 turns." });
        skillText.Add("Int_Skill4", new string[] { "Water Blast", "Water Blast grants you command over the power of water. This can be used to weaken and crush your foes with all the power of a tidal wave." });


        switch (Trait)
        {
            case "Strong Body":
                skillSet = strSkill;
                break;
            case "Agile Body":
                skillSet = agiSkill;
                break;
            case "Enhanced Mind":
                skillSet = intSkill;
                break;
            case "Average Joe":
                skillSet = avgSkill;
                break;
            default:
                skillSet = strSkill;
                break;
        }
        if (Trait == "Average Joe")
        {
            for (int i = 0; i < SkillButtons.Length; i++)
            {
                SkillButtons[i].SetActive(false);
                JoeSkillButtons[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < SkillButtons.Length; i++)
            {
                SkillButtons[i].SetActive(true);
                JoeSkillButtons[i].SetActive(false);
            }
        }
        for (int i = 0; i < skillSet.Length; i++)
        {
            if (Trait == "Average Joe")
            {
                JoeSkillButtons[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = avgImage[i];
            }
            else
            {
                switch (Trait)
                {
                    case "Strong Body":
                        SkillButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = strImage[i];
                        break;
                    case "Agile Body":
                        SkillButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = agiImage[i];
                        break;
                    case "Enhanced Mind":
                        SkillButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = intImage[i];
                        break;
                    case "Average Joe":
                        SkillButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = avgImage[i];
                        break;
                    default:
                        SkillButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = strImage[i];
                        break;
                }
            }

        }
        for (int i = 0; i < skillSet.Length; i++)
        {
            if (skillSet[i] == skill1 || skillSet[i] == skill2)
            {
                if (Trait == "Average Joe")
                {
                    JoeSkillButtons[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

                }
                else
                {
                    SkillButtons[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                }
            }
        }

    }
    public void openWindow(int index)
    {
        EquipSkillPrompt.SetActive(true);
        openedSkillIndex = index;
        EquipSkillPrompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = skillText[skillSet[index]][0];
        EquipSkillPrompt.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = skillText[skillSet[index]][1];
        if (skillSet[index] == skill1 || skillSet[index] == skill2)
        {

            print("equipped");
            if (Trait == "Average Joe")
            {
                if (index == 0 || index == 1)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = " ";
                }
                else if (index == 3 || index == 5)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level 10 ";
                }
                else if (index == 2 || index == 4 || index == 6)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level 25";
                }
            }
            else
            {
                if (index == 0 || index == 1)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = " ";
                }
                else if (index == 5 || index == 6)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level 10 ";
                }
                else if (index == 2)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level 25";
                }
                else if (index == 3)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level 50";
                }
                else if (index == 4)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level 75";
                }
            }
            EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(false);
            EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            print("not equipped");
            if (Trait == "Average Joe")
            {
                print("average joe");
                if (index == 0 || index == 1)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = " ";

                    EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                    EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                }
                else if (index == 3 || index == 5)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level: 10";
                    if (playerUnit.unitLevel >= 10)
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(false);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
                else if (index == 2 || index == 4 || index == 6)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level: 25";
                    if (playerUnit.unitLevel >= 25)
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(false);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (index == 0 || index == 1)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = " ";
                    EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                    EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                }
                else if (index == 5 || index == 6)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level: 10";
                    if (playerUnit.unitLevel >= 10)
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(false);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
                else if (index == 2)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level: 25";
                    if (playerUnit.unitLevel >= 25)
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(false);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
                else if (index == 3)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level: 50";
                    if (playerUnit.unitLevel >= 50)
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(false);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
                else if (index == 4)
                {
                    EquipSkillPrompt.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Minimum Level: 75";
                    if (playerUnit.unitLevel >= 75)
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(true);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        EquipSkillPrompt.transform.GetChild(3).gameObject.SetActive(false);
                        EquipSkillPrompt.transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void closeWindow()
    {
        EquipSkillPrompt.SetActive(false);
    }

    public void equipSlot1()
    {
        if (Trait == "Average Joe")
        {
            var oldIndex = Array.FindIndex(skillSet, row => row == skill1);
            JoeSkillButtons[oldIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            JoeSkillButtons[openedSkillIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            skill1 = skillSet[openedSkillIndex];
            glob.GetComponent<GlobalControl>().skill1 = skill1;
            EquipSkillPrompt.SetActive(false);
        }
        else
        {
            var oldIndex = Array.FindIndex(skillSet, row => row == skill1);
            SkillButtons[oldIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            SkillButtons[openedSkillIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            skill1 = skillSet[openedSkillIndex];
            glob.GetComponent<GlobalControl>().skill1 = skill1;
            EquipSkillPrompt.SetActive(false);
        }

    }

    public void equipSlot2()
    {
        if (Trait == "Average Joe")
        {
            var oldIndex = Array.FindIndex(skillSet, row => row == skill2);
            JoeSkillButtons[oldIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            JoeSkillButtons[openedSkillIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            skill2 = skillSet[openedSkillIndex];
            glob.GetComponent<GlobalControl>().skill2 = skill2;
            EquipSkillPrompt.SetActive(false);
        }
        else
        {
            var oldIndex = Array.FindIndex(skillSet, row => row == skill2);
            SkillButtons[oldIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            SkillButtons[openedSkillIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            skill2 = skillSet[openedSkillIndex];
            glob.GetComponent<GlobalControl>().skill2 = skill2;
            EquipSkillPrompt.SetActive(false);
        }


    }
}
