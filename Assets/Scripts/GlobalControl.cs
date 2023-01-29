using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public bool inCombat = false;
    public bool inMap = false;
    public bool inInventory = false;
    public bool inShop = false;
    public bool inCharPage = false;
    public bool inSkillPage = false;
    public bool inOptions = false;
    public string skill1 = "Default_Skill1";
    public string skill2 = null;
    public string playerTrait = "Strong Body";
    string curScene;
    public float playerCurrentHP = 0;
    public int playerCurrency = 0;
    int playerLevel = 1;
    public float musicVol = 0.5f;
    public float soundVol = 0.5f;

    [SerializeField] bool trigger = false;
    [SerializeField] bool save = false;
    [SerializeField] InventorySO InventSO = null;

    static Dictionary<string, ItemSO> allItemCodes = new Dictionary<string, ItemSO>();

    string[] playerInventory;
    Dictionary<int, InventoryItem> InventoryDict;
    public int[] stageList = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public Dictionary<string, float[]> skillDict = new Dictionary<string, float[]>();
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        ItemSO[] allItems = Resources.LoadAll<ItemSO>("");

        foreach (ItemSO i in allItems)
        {
            allItemCodes[i.name] = i;
        }

        float[] mult;
        //                    Hit , Low , High, Bool, Stamina, Type (1 Offensive, 2 Buff, 3 Debuff)
        mult = new float[6] { 1.0f, 1.0f, 1.2f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Default_Skill1", mult);


        mult = new float[6] { 1.0f, 1.1f, 1.3f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Str_Skill1", mult);
        mult = new float[6] { 1.0f, 1.3f, 1.5f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Str_Skill2", mult);
        mult = new float[6] { 1.0f, 1.4f, 1.8f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Str_Skill3", mult);
        mult = new float[6] { 1.0f, 1.8f, 2.4f, 1.0f, 3.0f, 1.0f };
        skillDict.Add("Str_Skill4", mult);

        mult = new float[6] { 1.0f, 1.2f, 1.5f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Agi_Skill1", mult);
        mult = new float[6] { 1.0f, 1.6f, 1.9f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Agi_Skill2", mult);
        mult = new float[6] { 0.0f, 0.0f, 0.0f, 1.0f, 2.0f, 3.0f };
        skillDict.Add("Agi_Skill3", mult);
        mult = new float[6] { 1.0f, 2.0f, 2.5f, 1.0f, 3.0f, 1.0f };
        skillDict.Add("Agi_Skill4", mult);

        mult = new float[6] { 1.0f, 1.2f, 1.8f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Int_Skill1", mult);
        mult = new float[6] { 1.0f, 1.7f, 2.2f, 1.0f, 2.0f, 1.0f };
        skillDict.Add("Int_Skill2", mult);
        mult = new float[6] { 0.0f, 0.0f, 0.0f, 1.0f, 2.0f, 2.0f };
        skillDict.Add("Int_Skill3", mult);
        mult = new float[6] { 1.0f, 2.1f, 2.7f, 1.0f, 3.0f, 1.0f };
        skillDict.Add("Int_Skill4", mult);

        PlayerData data = LoadGame();
    }

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            Invoke("printOut", 0.0f);
        }

        if (save)
        {
            save = false;
            SaveGame();
        }

        foreach (AudioSource audios in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (audios.tag == "BGM") audios.volume = musicVol * 0.5f;
        }
        foreach (GameObject sliders in GameObject.FindGameObjectsWithTag("MusicSlider"))
        {
            sliders.GetComponent<Slider>().value = musicVol;
            sliders.GetComponent<Slider>().onValueChanged.AddListener((value) => { musicVol = value; });
        }

        foreach (AudioSource audios in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (audios.tag != "BGM") audios.volume = soundVol * 0.8f;
        }
        foreach (GameObject sliders in GameObject.FindGameObjectsWithTag("SFXSlider"))
        {
            sliders.GetComponent<Slider>().value = soundVol;
            sliders.GetComponent<Slider>().onValueChanged.AddListener((value) => { soundVol = value; });
        }
        //SetVolume(musicVol);
        //SetSFX(soundVol);
    }

    public void SetVolume(float vol)
    {
        musicVol = vol;
        foreach (AudioSource audios in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (audios.tag == "BGM") audios.volume = musicVol * 0.6f;
        }

        foreach (GameObject sliders in GameObject.FindGameObjectsWithTag("MusicSlider"))
        {
            sliders.GetComponent<Slider>().value = musicVol;
        }
    }

    public void SetSFX(float vol)
    {
        soundVol = vol;
        foreach (AudioSource audios in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (audios.tag != "BGM") audios.volume = soundVol;
        }

        foreach (GameObject sliders in GameObject.FindGameObjectsWithTag("SFXSlider"))
        {
            sliders.GetComponent<Slider>().value = soundVol;
        }
    }

    public void TraitSet(string trait)
    {
        playerTrait = trait;
    }

    public string TraitGet()
    {
        return playerTrait;
    }

    public void LevelSet(int lvl)
    {
        playerLevel = lvl;
    }

    public int LevelGet()
    {
        return playerLevel;
    }

    public string SceneGet()
    {
        return curScene;
    }

    public string[] InventoryGet()
    {
        return playerInventory;
    }

    public Dictionary<int, InventoryItem> InventoryDictGet()
    {
        return InventoryDict;
    }

    public int CurrencyGet()
    {
        return playerCurrency;
    }

    void printOut()
    {
        print(playerTrait);
        print(playerLevel);
        print(curScene);
        print(playerCurrency);
    }

    public void SaveGame()
    {
        print("Saving game files...");
        print(Application.dataPath);
        Scene scene = SceneManager.GetActiveScene();
        curScene = scene.name;


        InventoryDict = InventSO.GetCurrentInventoryState();
        playerInventory = new string[InventoryDict.Count];
        int i = 0;
        foreach (KeyValuePair<int, InventoryItem> kvp in InventoryDict)
        {
            ItemSO item = kvp.Value.item;
            int count = kvp.Value.quantity;

            playerInventory[i] = item + "_" + count;
            i++;
        }

        playerLevel = GameObject.Find("Combat Overlay/Combat_UI/Player").GetComponent<Unit>().unitLevel;
        playerCurrentHP = GameObject.Find("Combat Overlay/Combat_UI/Player").GetComponent<Unit>().currentHP;
        SaveSystem.SaveGame(this);
    }

    public PlayerData LoadGame()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            print("Loading saved files...");
            playerTrait = data.trait;
            playerLevel = data.level;
            curScene = data.scene;
            playerCurrentHP = data.curHP;
            playerInventory = data.inventory;
            playerCurrency = data.currency;
            stageList = data.stageList;
            skill1 = data.skill1;
            skill2 = data.skill2;
            musicVol = data.BGM;
            soundVol = data.SFX;
            InventoryDict = new Dictionary<int, InventoryItem>();

            int i = 0;
            foreach (string items in playerInventory)
            {
                string key = items.Split("_")[0];
                key = key.Split(" ")[0];
                ItemSO item = allItemCodes[key];
                int count = int.Parse(items.Split("_")[1]);
                InventoryItem inventoryItem = new InventoryItem
                {
                    item = item,
                    quantity = count
                };

                InventoryDict[i] = inventoryItem;
                i++;
            }
        }
        else
        {
            print("No saved files! New game...");

            if (playerTrait == "Strong Body")
            {
                skill2 = "Str_Skill1";
            }
            else if (playerTrait == "Agile Body")
            {
                skill2 = "Agi_Skill1";
            }
            else if (playerTrait == "Enhanced Mind")
            {
                skill2 = "Int_Skill1";
            }
            else if (playerTrait == "Average Joe")
            {
                skill2 = "Str_Skill1";
            }
        }

        return data;
    }

    public void StageFinish(int idx)
    {
        stageList[idx] = 1;
    }
}
