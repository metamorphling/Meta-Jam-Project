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
    public List<Sprite> character;
    public List<Sprite> backgrounds;
    public List<Sprite> buttons;
    public List<GameObject> itemPrefabs;

    List<SpriteRenderer> environmentSprites;
    List<SpriteRenderer> characterSprites;
    List<Image> buttonSprites;
    List<GameObject> inventoryItems;


    void UpdateSprites(int updateTo)
    {
        updateTo--;
        foreach (SpriteRenderer go in environmentSprites)
        {
            go.sprite = floorBasic[updateTo];
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

        buttonOffset = updateTo * 2;
        foreach (GameObject go in inventoryItems)
        {
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
        environmentSprites = new List<SpriteRenderer>();
        foreach (SpriteRenderer go in Environment.GetComponentsInChildren<SpriteRenderer>())
        {
            environmentSprites.Add(go);
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
