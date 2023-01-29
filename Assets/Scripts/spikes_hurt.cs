using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes_hurt : MonoBehaviour
{
    // Start is called before the first frame update
    public string unitName;
    public Unit playerUnit;
    private bool TrapHit;
    public AudioSource hit;
    public GameObject collided;
    GameObject soul;
    Coroutine spikes;
    void Start()
    {
        soul = GameObject.Find("Soul");
        // if (gameObject.tag == "Player") currentHP = GameObject.Find("GlobalObject").GetComponent<GlobalControl>().playerCurrentHP;
    }
    void Update()
    {
        
    }
    // Update is called once per frame
    
    void OnTriggerEnter2D(Collider2D other) { 
        print("test"); 
        if(other.CompareTag("Player")) {
            print("Another object has entered the trigger");
            collided = other.gameObject;
            TrapHit = true;
            spikes = StartCoroutine(CastDamage(0.05f));
        }
    } 
    void OnTriggerExit2D(Collider2D other) { 
        if(other.CompareTag("Player")) {
            StopCoroutine(spikes);
            TrapHit = false;
            collided = null;
        }
    } 
    IEnumerator CastDamage(float damage){
        while(TrapHit){
            if (playerUnit.currentHP <= 0.0f)
            {
                collided.GetComponent<PlayerMovement>().death = true;
                collided.GetComponent<Animator>().Play("death");
                yield return new WaitForSeconds(3.0f);

                soul.SetActive(true);
                soul.transform.position = collided.transform.position;
                GameObject.Find("CombatManager").GetComponent<CombatManager>().soulCurrency = GameObject.Find("GlobalObject").GetComponent<GlobalControl>().playerCurrency;
                GameObject.Find("GlobalObject").GetComponent<GlobalControl>().playerCurrency = 0;
                playerUnit.Reset(0);

                collided.transform.position = GameObject.Find("Player Start Pos").transform.position;
                collided.GetComponent<PlayerMovement>().death = false;
                collided.GetComponent<Animator>().Play("idle");
                TrapHit = false;
            }
            else
            {
                playerUnit.currentHP -= playerUnit.maxHP * 0.05f;
                hit.Play();
                yield return new WaitForSeconds(1.0f);
            }
        }
        
    }

}
