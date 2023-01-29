using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchStage : MonoBehaviour
{
    // Start is called before the first frame update
    private bool istrigger = false;
    public GameObject map;
    GameObject glob;
    GlobalControl globalcontrol;
    public GameObject[] buttons;
    void Start()
    {
        glob = GameObject.Find("GlobalObject");
        globalcontrol = glob.GetComponent<GlobalControl>();
        glob.GetComponent<GlobalControl>().inMap = false;

    }

    // Update is called once per frame
    void Update()
    {
        Scene currScene = SceneManager.GetActiveScene();
        string sceneName = currScene.name;
        if (istrigger)
        {

            istrigger = false;
            map.SetActive(true);
            for (int x = 0; x < 15; x++)
                if (globalcontrol.stageList[x] == 1 && x != 14)
                {
                    string buttonName = "StageSelectionCanvas/map/stage" + (x + 2);
                    print(buttonName);
                    GameObject.Find(buttonName).SetActive(true);
                }


        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.CompareTag("Player"))
        {
            Scene currScene = SceneManager.GetActiveScene();
            if (currScene.name == "FifteenthStage" && gameObject.CompareTag("Finish"))
            {
                glob.GetComponent<GlobalControl>().SaveGame();
                SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
            }
            else
            {
                glob.GetComponent<GlobalControl>().inMap = true;
                glob.GetComponent<GlobalControl>().SaveGame();
                istrigger = true;
            }
            print("col_true");
        }

    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            istrigger = false;
        }

    }

}
