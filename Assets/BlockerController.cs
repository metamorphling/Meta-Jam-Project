using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockerController : MonoBehaviour, IPointerClickHandler
{

    public Text tutorialTextBox;
    List<string> tutorialText;
    int textScroll = 0;

    void Start () {
        tutorialText = new List<string>(new string[] {
        "What the...?\nI'm a game sprite.\nIn a game.",
        "I can see a door,\nand a key but...\narrghggh... I cant move!",
        "And even if I could,\nthere’s no way I could\nmake that jump.\nWhat to do...?",
        "Hey, my inner\nmonologue is appearing\nas text on the screen...\nthis must be the\ntutorial!",
        "And if this is a\ntutorial, someone\nmust be playing\nthe game!",
        "Hello?\nIs anyone there?\nPress the keyboard if\nyou’re reading this!",
        "Hmmm... nothing...\nhow about the mouse?\nCan you move the\ncursor?",
        "Yes! Ok!\nwith your mouse and\nmy knowledge we can\nfinish this level!",
        "Maybe we should use\nthe cartridges and try\nreaching that door..."
    });

    }
	
    void SetText(int i)
    {
        tutorialTextBox.text = tutorialText[i];
    }

    void SetText(string str)
    {
        tutorialTextBox.text = str;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (EasterEgg.showEgg == false)
        {
            if (tutorialText.Count - 1 <= textScroll)
            {
                GetComponent<Image>().enabled = false;
                transform.GetChild(0).GetComponent<Image>().enabled = false;
                tutorialTextBox.enabled = false;
                return;
            }
            SetText(++textScroll);
        }
        else
        {
            SetText("Huh? Deja vu?");
            if (1 <= textScroll)
            {
                GetComponent<Image>().enabled = false;
                transform.GetChild(0).GetComponent<Image>().enabled = false;
                tutorialTextBox.enabled = false;
                return;
            }
            textScroll++;
        }
    }
}
