using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {
    int scene = 0;
    public Animator text1;
    public Animator text2;
    public Animator joe;
    public Animator bg;
    public List<Animator> anims;

    void Start () {
		
	}
	
    IEnumerator nextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Update () {
        if (Input.GetButtonDown("Fire1") && scene < 18)
        {
            scene++;
            if (scene == 1)
            {
                text1.SetTrigger("play");
            }
            else if (scene == 2)
            {
                text2.SetTrigger("play");
            }
            else if (scene == 3)
            {
                joe.SetTrigger("play");
            }
            else if (scene == 4)
            {
                text2.GetComponent<Text>().text = "";
                text1.SetTrigger("play");
                text1.GetComponent<Text>().text = "HE'S ALSO\nOUR HERO";
            }
            else if (scene == 5)
            {
                text1.GetComponent<Text>().text = "";
                joe.SetTrigger("fade");
                bg.SetTrigger("play");
            }
            else if (scene >= 6)
            {
                Debug.Log(scene);
                if (scene == 10)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        anims[i].GetComponent<Image>().color = new Color(0xFF, 0xFF, 0xFF, 0x00);
                        if (anims[i].transform.childCount != 0)
                        {
                            Transform chld = anims[i].transform.GetChild(0);
                            Text txt = chld.GetComponent<Text>();
                            if (txt != null)
                                txt.enabled = false;
                        }
                    }
                }
                else if (scene == 13)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        anims[i + 3].GetComponent<Image>().color = new Color(0xFF, 0xFF, 0xFF, 0x00);
                        if (anims[i + 3].transform.childCount != 0)
                        {
                            Transform chld = anims[i + 3].transform.GetChild(0);
                            Text txt = chld.GetComponent<Text>();
                            if (txt != null)
                                txt.enabled = false;
                        }
                    }
                }
                anims[scene - 6].GetComponent<Image>().color = new Color(0xFF, 0xFF, 0xFF, 0xFF);
                anims[scene - 6].SetTrigger("play");
            }
        }
        if (scene == 17)
        {
            StartCoroutine("nextLevel");
        }
    }
}
