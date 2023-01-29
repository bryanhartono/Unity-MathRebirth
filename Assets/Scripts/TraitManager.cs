using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraitManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI traitName;
    [SerializeField] TextMeshProUGUI traitInfo;
    //[SerializeField] Animator anim;
    [SerializeField] Image sprites;
    [SerializeField] Sprite[] img;
    public string[] Tname;
    public int idx = 0;
    float sec = 0.0f;
    string globStr;
    string[] Tinfo = new string[4];
    
    // Start is called before the first frame update
    void Start()
    {
        Tinfo[0] = @"Having a hobby of going to the gym during his leisure time. He has a very healthy body and trained muscle. Someone saw him lifting a car by himself, but that itself is just a rumour.

Trait focuses on Strength, +5 Str per Level & +2 for other Stats.";
        Tinfo[1] = @"He was once the ace of his university track and field team. He has a very good agility and quick. It was rumoured that he's as fast as Usain Bolt.

Trait focuses on Agility, +5 Agi per Level & +2 for other Stats.";
        Tinfo[2] = @"Reading is the same as breathing for him. He has been reading lots of books since young age, that being said, he knows a lot of knowledge thus having a sharper mind than the average human. Some suspect he might be the reincarnation of Albert Einstein.

Trait focuses on Intelligent, +5 Int per Level & +2 for other Stats.";
        Tinfo[3] = @"Not much can be said about this guy. He is just your ordinary Joe you can find anywhere. Not sure how did he get into this mess.

Trait is equally distributed, +3 for all Stats per Level.";

        traitName.text = Tname[0];
        traitInfo.text = Tinfo[0];
        sprites.sprite = img[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void previousTrait()
    {

        if (idx - 1 < 0) idx = Tname.Length - 1;
        else idx -= 1;
        traitName.text = Tname[idx];
        traitInfo.text = Tinfo[idx];
        sprites.sprite = img[idx];
        //anim.SetInteger("index", idx);
    }
    public void nextTrait()
    {
        if (idx + 1 > Tname.Length - 1) idx = 0;
        else idx += 1;
        traitName.text = Tname[idx];
        traitInfo.text = Tinfo[idx];
        sprites.sprite = img[idx];
        //anim.SetInteger("index", idx);
    }

    public void startGame(string scene)
    {
        GameObject glob = GameObject.Find("GlobalObject");
        glob.GetComponent<GlobalControl>().TraitSet(Tname[idx]);
        glob.GetComponent<GlobalControl>().LoadGame();
        sec = 1.0f;
        globStr = scene;
        Invoke("changeScene", sec);
    }

    void changeScene()
    {
        SceneManager.LoadScene(globStr, LoadSceneMode.Single);
    }
}
