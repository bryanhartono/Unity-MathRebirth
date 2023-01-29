using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage12PlatformSwapper : MonoBehaviour
{
    public GameObject platform1;
    public GameObject platform2;

    public bool disableSelfOption;
    // Start is called before the first frame update


    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (platform1.activeSelf)
        {
            platform1.SetActive(false);
            platform2.SetActive(true);
        }
        else
        {
            platform1.SetActive(true);
            platform2.SetActive(false);
        }
    }
}
