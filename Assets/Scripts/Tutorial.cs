using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public GameObject tutorial_background;
    public GameObject move_tutorial;
    public GameObject inventory_tutorial;
    public GameObject skill_tutorial;
    public GameObject map_tutorial;
    public GameObject status_tutorial;
    private int g = 1;
    // Start is called before the first frame update
    void Start()
    {
        tutorial_background.SetActive(true);
        move_tutorial.SetActive(true);
        inventory_tutorial.SetActive(false);
        skill_tutorial.SetActive(false);
        map_tutorial.SetActive(false);
        status_tutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown("d") || Input.GetKeyDown("a") ||Input.GetKeyDown("w") )&& g == 1 )
        {
            tutorial_background.SetActive(true);
            move_tutorial.SetActive(false);
            inventory_tutorial.SetActive(false);
            skill_tutorial.SetActive(true);
            map_tutorial.SetActive(false);
            status_tutorial.SetActive(false);

            g = 0;
        }
        else if(Input.GetKeyDown("k") && g == 0)
        {
            tutorial_background.SetActive(false);
            move_tutorial.SetActive(false);
            inventory_tutorial.SetActive(false);
            skill_tutorial.SetActive(false);
            map_tutorial.SetActive(false);
            g = 2;
        }
        else if(Input.GetKeyDown("k") && g == 2)
        {
            tutorial_background.SetActive(true);
            move_tutorial.SetActive(false);
            inventory_tutorial.SetActive(true);
            skill_tutorial.SetActive(false);
            map_tutorial.SetActive(false);
            status_tutorial.SetActive(false);
            g = 3;
        }
        else if(Input.GetKeyDown("i") && g == 3)
        {
            tutorial_background.SetActive(false);
            move_tutorial.SetActive(false);
            inventory_tutorial.SetActive(false);
            skill_tutorial.SetActive(false);
            map_tutorial.SetActive(false);
            status_tutorial.SetActive(false);
            g = 4;
        }
        else if(Input.GetKeyDown("i") && g == 4)
        {
            tutorial_background.SetActive(true);
            move_tutorial.SetActive(false);
            inventory_tutorial.SetActive(false);
            skill_tutorial.SetActive(false);
            map_tutorial.SetActive(false);
            status_tutorial.SetActive(true);
            g = 5;
            //StartCoroutine(map());
        }
        else if(Input.GetKeyDown("c") && g == 5)
        {
            tutorial_background.SetActive(false);
            move_tutorial.SetActive(false);
            inventory_tutorial.SetActive(false);
            skill_tutorial.SetActive(false);
            map_tutorial.SetActive(false);
            status_tutorial.SetActive(false);
            g = 6;
        }
        else if(Input.GetKeyDown("c") && g == 6)
        {
            tutorial_background.SetActive(true);
            move_tutorial.SetActive(false);
            inventory_tutorial.SetActive(false);
            skill_tutorial.SetActive(false);
            map_tutorial.SetActive(true);
            status_tutorial.SetActive(false);
            g = 7;
        }
        // if (Input.GetKeyDown("i") )
        // {
        //     tutorial_background.SetActive(false);
        //     move_tutorial.SetActive(false);
        //     inventory_tutorial.SetActive(false);
        //     skill_tutorial.SetActive(false);
        //     map_tutorial.SetActive(false);

        // }
    }

    IEnumerator map(){
        yield return new WaitForSeconds(3.0f);
        tutorial_background.SetActive(false);
        move_tutorial.SetActive(false);
        inventory_tutorial.SetActive(false);
        skill_tutorial.SetActive(false);
        map_tutorial.SetActive(false);
    }
        
    
}
