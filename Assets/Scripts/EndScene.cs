using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public float fadeSpeed;
    public string MainMenu;
    private bool shouldFadeToBlack;
    private bool shouldFadeFromBlack;
    public GameObject fadeScreen;

    void Start()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = false;
        StartCoroutine(eFadeIn());
    }

    void Update()
    {
        if(shouldFadeToBlack)
        {
            fadeScreen.GetComponent<Image>().color = new Color(fadeScreen.GetComponent<Image>().color.r, fadeScreen.GetComponent<Image>().color.g, fadeScreen.GetComponent<Image>().color.b, Mathf.MoveTowards(fadeScreen.GetComponent<Image>().color.a, 1.0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.GetComponent<Image>().color.a == 1.0f)
            {
                shouldFadeToBlack = false;
            }
        }

        if(shouldFadeFromBlack)
        {
            fadeScreen.GetComponent<Image>().color = new Color(fadeScreen.GetComponent<Image>().color.r, fadeScreen.GetComponent<Image>().color.g, fadeScreen.GetComponent<Image>().color.b, Mathf.MoveTowards(fadeScreen.GetComponent<Image>().color.a, 0.0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.GetComponent<Image>().color.a == 0.0f)
            {
                shouldFadeFromBlack = false;
                fadeScreen.SetActive(false);
            }
        }
    }

    public void ReturnToMainMenu()
    {
        fadeScreen.SetActive(true);
        StartCoroutine(eFadeOut());
    }

    void Fadeout()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }

    private IEnumerator eFadeOut()
    {
        yield return new WaitForSeconds(1.0f / fadeSpeed);
        Fadeout();

        yield return new WaitForSeconds((1.0f / fadeSpeed));
        SceneManager.LoadScene(MainMenu);
    }

    private IEnumerator eFadeIn()
    {
        yield return new WaitForSeconds(1.0f / fadeSpeed);
        FadeFromBlack();
        
    }
}
