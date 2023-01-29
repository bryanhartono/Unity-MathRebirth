using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    GameObject glob;
    GlobalControl globalcontrol;
    // Start is called before the first frame update
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        //globalcontrol.LoadGame();
    }
    void Start()
    {
        Scene currScene = SceneManager.GetActiveScene();
        string sceneName = currScene.name;
        glob = GameObject.Find("GlobalObject");
        globalcontrol = glob.GetComponent<GlobalControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void closeWindow()
    {
        gameObject.SetActive(false);
        globalcontrol.inMap = false;
    }
    public void stage1()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("FirstStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage2()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("SecondStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage3()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("ThirdStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage4()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("FourthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage5()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("FifthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage6()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("SixthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage7()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("SeventhStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage8()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("EightStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage9()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("NinthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage10()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("TenthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage11()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("EleventhStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage12()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("TwelfthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage13()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("ThirteenthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage14()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("FourteenthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void stage15()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("FifteenthStage", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
    public void shop()
    {
        glob.GetComponent<GlobalControl>().inMap = false;
        globalcontrol.SaveGame();
        SceneManager.LoadScene("Shop", LoadSceneMode.Single);
        globalcontrol.LoadGame();
        globalcontrol.SaveGame();
        print("load");
    }
}
