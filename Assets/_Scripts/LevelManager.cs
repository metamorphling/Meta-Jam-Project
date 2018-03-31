using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {

    public Animator glitchAnimator;
    public GameObject controller;
    public SpriteRenderer levelBackground;
    public SpriteRenderer player;
    public GameObject inventory;
    public GameObject Environment;
    public GameEditor gameEditor;

    /* array number should match the name of the cartridge, 1-mario,2-zelda,3-sonic,4-kirby */
    public List<Sprite> floorBasic;
    public List<Sprite> wallBasic;
    public List<Sprite> springBasic;
    public List<Sprite> character;
    public List<Sprite> backgrounds;
    public List<Sprite> buttons;
    public List<GameObject> itemPrefabs;
    public AudioClip glitchSound, defaultTheme;

    List<SpriteRenderer> floorSprites;
    List<SpriteRenderer> wallSprites;
    List<SpriteRenderer> springSprites;
    List<SpriteRenderer> characterSprites;
    List<Image> buttonSprites;
    List<GameObject> inventoryItems;
    AudioSource currentTheme;

    void UpdateSprites(int updateTo)
    {
        updateTo--;
        foreach (SpriteRenderer gof in floorSprites)
        {
            gof.sprite = floorBasic[updateTo];
        }
        foreach (SpriteRenderer gow in wallSprites)
        {
            gow.sprite = wallBasic[updateTo];
        }
        foreach (SpriteRenderer gos in springSprites)
        {
            gos.sprite = springBasic[updateTo];
        }
        player.sprite = character[updateTo];
        levelBackground.sprite = backgrounds[updateTo];

        int buttonOffset = updateTo * 4 + 1;
        int spriteCount = 0;
        foreach (Image go in buttonSprites)
        {
            if (spriteCount > 3)
                go.sprite = buttons[buttonOffset + 2];
            else
                go.sprite = buttons[buttonOffset];

            spriteCount++;
        }



        int[] sizes = { 3, 2, 1, 0};
        if (updateTo == 0)
        {
            buttonOffset = 0;
        } else if (updateTo == 1)
        {
            buttonOffset = 3;
        } else if (updateTo == 2)
        {
            buttonOffset = 5;
        } else
        {
            buttonOffset = 0;
        }

        int limit = buttonOffset + sizes[updateTo];
        foreach (GameObject go in inventoryItems)
        {
            if (buttonOffset >= limit)
            {
                go.GetComponent<ItemUse>().PrefabToSet = null;
                go.GetComponent<Image>().enabled = false;
                continue;
            }
            go.GetComponent<Image>().enabled = true;
            go.GetComponent<ItemUse>().PrefabToSet = itemPrefabs[buttonOffset];

            /* adding appropriate event trigger */
            EventTrigger evTrigger = go.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = evTrigger.triggers[0];
            entry.eventID = EventTriggerType.PointerDown;
            GameObject toSet = itemPrefabs[buttonOffset];
            entry.callback.AddListener((data) => { gameEditor.ChangeCurrentTile((GameObject)toSet); });
            go.GetComponent<Image>().sprite = itemPrefabs[buttonOffset].GetComponent<SpriteRenderer>().sprite;

            buttonOffset++;
        }
    }

    IEnumerator playTheme(AudioClip theme)
    {
        yield return new WaitForSeconds(1f);
        currentTheme.clip = theme;
        currentTheme.Play();
    }

    public void PlayGlitchSound()
    {
        currentTheme.clip = glitchSound;
        currentTheme.Play();
    }
    
    public void ChangeLevel(int world, AudioClip theme)
    {
        currentTheme.Stop();
        PlayGlitchSound();
        glitchAnimator.SetTrigger("playGlitch");
        StartCoroutine("playTheme", theme);
        UpdateSprites(world);
    } 

	void Start () {
        if (gameEditor == null)
            gameEditor = GameObject.Find("Editor").GetComponent<GameEditor>();
        currentTheme = gameObject.AddComponent<AudioSource>();
        currentTheme.clip = defaultTheme;
        currentTheme.Play();

        Transform tr1, tr2, tr3;
        floorSprites = new List<SpriteRenderer>();
        int childCount = Environment.transform.childCount;
        if (childCount >= 1)
        {
            tr1 = Environment.transform.GetChild(0);
            foreach (SpriteRenderer go in tr1.GetComponentsInChildren<SpriteRenderer>())
            {
                floorSprites.Add(go);
            }
        }
        wallSprites = new List<SpriteRenderer>();
        if (childCount >= 2)
        {
            tr2 = Environment.transform.GetChild(1);
            foreach (SpriteRenderer go in tr2.GetComponentsInChildren<SpriteRenderer>())
            {
                wallSprites.Add(go);
            }
        }
        springSprites = new List<SpriteRenderer>();
        if (childCount >= 3)
        {
            tr3 = Environment.transform.GetChild(2);
            foreach (SpriteRenderer go in tr3.GetComponentsInChildren<SpriteRenderer>())
            {
                springSprites.Add(go);
            }
        }
        buttonSprites = new List<Image>();
        foreach (Image go in controller.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Image>())
        {
            buttonSprites.Add(go);
        }

        inventoryItems = new List<GameObject>();
        foreach (ItemUse go in inventory.transform.GetChild(1).GetComponentsInChildren<ItemUse>())
        {
            inventoryItems.Add(go.gameObject);
        }
    }
}
