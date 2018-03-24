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

    /* array number should match the name of the cartridge, 1-mario,2-zelda,3-sonic,4-kirby */
    public List<Sprite> floorBasic;
    public List<Sprite> wallBasic;
    public List<Sprite> springBasic;
    public List<Sprite> character;
    public List<Sprite> backgrounds;
    public List<Sprite> buttons;
    public List<GameObject> itemPrefabs;

    List<SpriteRenderer> floorSprites;
    List<SpriteRenderer> wallSprites;
    List<SpriteRenderer> springSprites;
    List<SpriteRenderer> characterSprites;
    List<Image> buttonSprites;
    List<GameObject> inventoryItems;


    void UpdateSprites(int updateTo)
    {
        updateTo--;
        foreach (SpriteRenderer go in floorSprites)
        {
            go.sprite = floorBasic[updateTo];
        }
        foreach (SpriteRenderer go in wallSprites)
        {
            go.sprite = wallBasic[updateTo];
        }
        foreach (SpriteRenderer go in springSprites)
        {
            go.sprite = springBasic[updateTo];
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
            //go.GetComponent<EventTrigger>().GetComponent
            go.GetComponent<Image>().sprite = itemPrefabs[buttonOffset].GetComponent<SpriteRenderer>().sprite;
            buttonOffset++;
        }
    }
    
    public void ChangeLevel(int world)
    {
        glitchAnimator.SetTrigger("playGlitch");
        UpdateSprites(world);
    } 

	void Start () {
        floorSprites = new List<SpriteRenderer>();
        foreach (SpriteRenderer go in Environment.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
        {
            floorSprites.Add(go);
        }
        wallSprites = new List<SpriteRenderer>();
        foreach (SpriteRenderer go in Environment.transform.GetChild(1).GetComponentsInChildren<SpriteRenderer>())
        {
            wallSprites.Add(go);
        }
        springSprites = new List<SpriteRenderer>();
        foreach (SpriteRenderer go in Environment.transform.GetChild(2).GetComponentsInChildren<SpriteRenderer>())
        {
            springSprites.Add(go);
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
