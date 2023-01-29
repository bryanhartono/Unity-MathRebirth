using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float runSpeed = 0.1f;
    public float jumpPower = 400f;
    float horizontalMove = 0f;
    private bool fallingKey;
    public ParticleSystem runpart;

    private bool trigger = false;
    //public GameObject CombatScene;
    public GameObject[] skeletons;
    public GameObject[] shades;
    [SerializeField] CinemachineVirtualCamera walk_cam;
    [SerializeField] CinemachineVirtualCamera combat_cam;
    [SerializeField] GameObject characterPage;
    [SerializeField] GameObject skillPage;
    public Animator cm_cam1;
    public GameObject canvas_scroll;
    public GameObject enemy;

    public float rayLength = 0.55f;
    public float rayPositionOffset = 0.4f;

    Vector3 RayPosCenter;
    Vector3 RayPosLeft;
    Vector3 RayPosRight;

    RaycastHit2D[] HitsCenter;
    RaycastHit2D[] HitsLeft;
    RaycastHit2D[] HitsRight;

    RaycastHit2D[][] AllRayHits = new RaycastHit2D[3][];
    bool grounded;
    public bool death = false;
    bool isFalling => gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0 && !grounded;
    bool isRising => !isFalling && !grounded;
    bool flipped = false;

    GameObject glob;
    GlobalControl globalcontrol;
    public List<GameObject> unityGameObjects = new List<GameObject>();
    public List<string> StringTagEnemy = new List<string>() {"Shade","Skeleton"};
    [SerializeField] AudioClip clips;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        skeletons = GameObject.FindGameObjectsWithTag("Skeleton");
        shades = GameObject.FindGameObjectsWithTag("Shade");
        glob = GameObject.Find("GlobalObject");
        globalcontrol = glob.GetComponent<GlobalControl>();
        CameraSwitch.register(walk_cam);
        CameraSwitch.register(combat_cam);
    }
    private void LateUpdate()
    {
        enemy.GetComponent<SpriteRenderer>().flipX = flipped;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCharPage && Input.GetKeyDown(KeyCode.C))
        {
            characterPage.SetActive(false);
            characterPage.GetComponent<StatsManager>().closeWindow();
            characterPage.GetComponent<StatsManager>().LevelUpResult.SetActive(false);
            GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCharPage = false;
        }

        else if (Input.GetKeyDown(KeyCode.C) && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCombat && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inMap && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inInventory && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inShop && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inSkillPage && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inOptions)
        {
            characterPage.SetActive(true);
            GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCharPage = true;
        }

        if (GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inSkillPage && Input.GetKeyDown(KeyCode.K))
        {
            skillPage.SetActive(false);
            skillPage.GetComponent<SkillManager>().closeWindow();
            GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inSkillPage = false;
        }

        else if (Input.GetKeyDown(KeyCode.K) && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCombat && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inMap && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inInventory && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inShop && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCharPage && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inOptions)
        {
            skillPage.SetActive(true);
            GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inSkillPage = true;
        }



        if (glob.GetComponent<GlobalControl>().inCombat || glob.GetComponent<GlobalControl>().inMap)
        {  
           if(glob.GetComponent<GlobalControl>().inCharPage){
                characterPage.GetComponent<StatsManager>().closePage();
                glob.GetComponent<GlobalControl>().inCharPage = false;
           }
           else if (glob.GetComponent<GlobalControl>().inSkillPage)
            {
                skillPage.SetActive(false);
                skillPage.GetComponent<SkillManager>().closeWindow();
                glob.GetComponent<GlobalControl>().inSkillPage = false;
            }
        }

        RayPosCenter = transform.position + new Vector3(0, -0.5f, 0);
        RayPosLeft = transform.position + new Vector3(-rayPositionOffset, -0.5f, 0);
        RayPosRight = transform.position + new Vector3(rayPositionOffset, -0.5f, 0);

        HitsCenter = Physics2D.RaycastAll(RayPosCenter, Vector2.down, rayLength);
        HitsLeft = Physics2D.RaycastAll(RayPosLeft, Vector2.down, rayLength);
        HitsRight = Physics2D.RaycastAll(RayPosRight, Vector2.down, rayLength);

        AllRayHits[0] = HitsCenter;
        AllRayHits[1] = HitsLeft;
        AllRayHits[2] = HitsRight;

        Debug.DrawRay(RayPosCenter, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(RayPosLeft, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(RayPosRight, Vector2.down * rayLength, Color.red);

        grounded = grounding(AllRayHits);

        // print(trigger);
        if (!GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCombat && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inMap && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inInventory && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inShop && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inCharPage && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inSkillPage && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inOptions && !death)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (Input.GetKeyDown("w") && grounded)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpPower), ForceMode2D.Force);
            }

            if(isRising)
            {
                gameObject.GetComponent<Animator>().Play("Jump");
            
            }
            else if(isFalling)
            {
                if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    gameObject.GetComponent<Animator>().Play("JumpFall");
                }
                else if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("JumpFall") && !fallingKey)
                {
                    gameObject.GetComponent<Animator>().Play("Fall");
                    fallingKey = true;
                }
                else if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Fall") && horizontalMove > 0 && grounded)
                {
                    gameObject.GetComponent<Animator>().SetFloat("velocity", Mathf.Abs(horizontalMove));
                }
                else if(!fallingKey)
                {
                    gameObject.GetComponent<Animator>().Play("Fall");
                    fallingKey = true;
                }
            }

            if(horizontalMove == 0 && grounded && !death)
            {
                gameObject.GetComponent<Animator>().Play("idle");
                gameObject.GetComponent<Animator>().SetBool("isMoving", false);
                fallingKey = false;
            }
            else if(horizontalMove != 0 && grounded && !death)
            {
                // gameObject.GetComponent<Animator>().SetFloat("velocity", Mathf.Abs(horizontalMove));
                gameObject.GetComponent<Animator>().Play("Run");
                runpart.Play();
                gameObject.GetComponent<Animator>().SetBool("isMoving", true);
            }
            
        }
        else horizontalMove = 0.0f;

        if (trigger)
        {
            CameraSwitch.register(walk_cam);
            CameraSwitch.register(combat_cam);
            CameraSwitch.swithcam(combat_cam);
            trigger = false;
        }
    }

    bool grounding(RaycastHit2D[][] AllRayHits)
    {
        foreach (RaycastHit2D[] HitList in AllRayHits)
        {
            foreach (RaycastHit2D hit in HitList)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag != "Player" && hit.collider.tag != "Confiner" && hit.collider.tag != "Shop" && hit.collider.tag != "Checkpoints" && hit.collider.tag != "Finish")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void FixedUpdate()
    {
        if (horizontalMove < 0)
        {
            Quaternion rot = gameObject.transform.rotation;
            rot = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
            gameObject.transform.rotation = rot;
        }
        else if (horizontalMove > 0)
        {
            Quaternion rot = gameObject.transform.rotation;
            rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            gameObject.transform.rotation = rot;
        }

        Vector2 pos = gameObject.transform.position;
        pos += new Vector2(horizontalMove, 0.0f);
        gameObject.transform.position = pos;
    }
    public GameObject collidedd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            Scene currScene = SceneManager.GetActiveScene();
            string sceneName = currScene.name;
            if(sceneName == "FirstStage")
            {
                globalcontrol.StageFinish(0);
            }
            if(sceneName == "SecondStage")
            {
                globalcontrol.StageFinish(1);
            }
            if(sceneName == "ThirdStage")
            {
                globalcontrol.StageFinish(2);
            }
            if(sceneName == "FourthStage")
            {
                globalcontrol.StageFinish(3);
            }
            if(sceneName == "FifthStage")
            {
                globalcontrol.StageFinish(4);
            }
            if(sceneName == "SixthStage")
            {
                globalcontrol.StageFinish(5);
            }
            if(sceneName == "SeventhStage")
            {
                globalcontrol.StageFinish(6);
            }
            if(sceneName == "EighthStage")
            {
                globalcontrol.StageFinish(7);
            }
            if(sceneName == "NinthStage")
            {
                globalcontrol.StageFinish(8);
            }
            if(sceneName == "TenthStage")
            {
                globalcontrol.StageFinish(9);
            }
            if(sceneName == "EleventhStage")
            {
                globalcontrol.StageFinish(10);
            }
            if(sceneName == "TwelfthStage")
            {
                globalcontrol.StageFinish(11);
            }
            if(sceneName == "ThirteenthStage")
            {
                globalcontrol.StageFinish(12);
            }
            if(sceneName == "FourteenthStage")
            {
                globalcontrol.StageFinish(13);
            }
            if(sceneName == "FifteenthStage")
            {
                globalcontrol.StageFinish(14);
            }
        }
        else if (collision.tag != "Stage" && collision.tag != "Untagged" && collision.tag != "Confiner" && collision.tag != "Player" && collision.tag != "Shop" && collision.tag != "spikes" && collision.tag != "Checkpoints" && collision.tag != "Finish")
        {
            collision.GetComponent<Animator>().Play("idle");
            unityGameObjects.Add(collision.gameObject);
            enemy.tag = collision.tag;
            if (collision.CompareTag("Bat") || collision.CompareTag("Shade") || collision.CompareTag("TrashCave") || collision.CompareTag("TrashForest") || collision.CompareTag("SlimeForest") || collision.CompareTag("Boss1") || collision.CompareTag("Boss2"))
            {
                flipped = true;
            }
            else
            {
                flipped = false;
            }

            enemy.GetComponent<Animator>().runtimeAnimatorController = collision.GetComponent<EnemyBehaviour>().m_anim;
            enemy.GetComponent<Unit>().unitLevel = collision.GetComponent<EnemyBehaviour>().level;
            enemy.GetComponent<Unit>().prevLevel = collision.GetComponent<EnemyBehaviour>().level - 1;

            if (collision.CompareTag("Boss1"))
            {
                enemy.GetComponent<Unit>().unitName = "Prae' Gerrim, The Bringer of Death";
            }
            else if (collision.CompareTag("Boss2"))
            {
                enemy.GetComponent<Unit>().unitName = "Beelzebub, The Insatiable Demon Slime";
            }
            else if (collision.CompareTag("Boss3"))
            {
                enemy.GetComponent<Unit>().unitName = "Tsarleiche, The Lich of the Greater Hell";
            }
            else
            {
                enemy.GetComponent<Unit>().unitName = collision.tag;
            }
            collidedd = collision.gameObject;
            gameObject.GetComponent<Animator>().SetBool("inCombat", true);
            GameObject.Find("CombatManager").GetComponent<CombatManager>().StartCombat();
            StartCoroutine(Coroutine());
            print("Enemy Found");
        }

        if (collision.gameObject.name == "Soul")
        {
            print("IQ collected");
            gameObject.GetComponent<AudioSource>().PlayOneShot(clips);
            GameObject glob = GameObject.Find("GlobalObject");
            glob.GetComponent<GlobalControl>().playerCurrency += GameObject.Find("CombatManager").GetComponent<CombatManager>().soulCurrency;
            GameObject.Find("CombatManager").GetComponent<CombatManager>().soulCurrency = 0;
            GameObject.Find("CombatManager").GetComponent<CombatManager>().dead = 0;
            collision.gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.CompareTag("Skeleton"))
        //{
        //    trigger = false;
        //    cm_cam1.SetBool("enter", false);
        //}
    }

    private void OnEnable()
    {
        CameraSwitch.register(walk_cam);
        CameraSwitch.register(combat_cam);
    }
    private void OnDisable()
    {
        CameraSwitch.unregister(walk_cam);
        CameraSwitch.unregister(combat_cam);
    }

    public void playerDeath()
    {

    }

    IEnumerator Coroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        cm_cam1.SetBool("enter", true);
        GameObject glob = GameObject.Find("GlobalObject");
        glob.GetComponent<GlobalControl>().inCombat = true;
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSecondsRealtime(1.6f);
        cm_cam1.SetBool("enter", false);
        trigger = true;
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    IEnumerator DoSomething()
    {
        yield return new WaitForSecondsRealtime(1.6f);
    }
}
