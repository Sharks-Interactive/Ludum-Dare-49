using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chrio.World;
using Chrio;
using UnityEngine.UI;
using TMPro;

public class infoPopupController : SharksBehaviour
{
    // Start is called before the first frame update
    public GameObject text;
    public float time;
    public bool enableSomething;
    public bool useAnimation;
    public GameObject thingToEnable;
    public Animator animator;
    public string boolToEnable;
    public float timeForAnimation;
    private bool run;
    public bool disable;
    public Image img;
    public TextMeshProUGUI textt;
    private string oldText = "Game started. Have fun!";

    // Update is called once per frame
    public void Update()
    {
        if (textt.text != oldText)
        {
            oldText = textt.text;
            if (!run)
            {
                if (disable)
                {
                    img.enabled = true;
                }
                StartCoroutine("Wait");
                run = true;
                if (useAnimation)
                {
                    animator.SetBool(boolToEnable, true);
                }
            }
        }
    }
    //Wait time before disabling the text
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        if (enableSomething)
        {
            thingToEnable.SetActive(true);
        }
        if (useAnimation)
        {
            animator.SetBool(boolToEnable, false);
            yield return new WaitForSeconds(timeForAnimation);
        }
        if (disable)
        {
            img.enabled = false;
        }
        textt.text = "";
        oldText = textt.text;
        run = false;
    }

    public void Hide()
    {
        StartCoroutine("Close");
    }

    IEnumerator Close()
    {
        animator.SetBool(boolToEnable, false);
        yield return new WaitForSeconds(0.75f);
        run = false;
        img.enabled = false;
    }

    public static void ShowMessage(string text)
    {
        GameObject.Find("InfoPopup").GetComponent<Image>().enabled = true;
        GameObject.Find("Info Text Popup").GetComponent<TextMeshProUGUI>().text = text;
        
    }
}