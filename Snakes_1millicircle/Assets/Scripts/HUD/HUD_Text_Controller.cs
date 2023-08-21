using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD_Text_Controller : MonoBehaviour
{
    public enum TypeOfStat {health, mana, xp};
    public TypeOfStat typeOfStat;

    TextMeshProUGUI statText;

    HeroClass heroClassScript;
    private bool SetSkill = false;

    void Start()
    {
        if (!SetSkill) { return; }
        statText = GetComponent<TextMeshProUGUI>();

        heroClassScript = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<HeroClass>();
    }
    public void SetHero(HeroClass playerSl)
    {
        heroClassScript = playerSl;
        statText = GetComponent<TextMeshProUGUI>();
        SetSkill = true;
    }
    // Update is called once per frame
    void Update()
    {
        //heroClassScript.heroHealth = Mathf.Round(heroClassScript.heroHealth * 100f) / 100f;
        if(heroClassScript == null) { return; }
        //print(typeOfStat);
        switch (typeOfStat)
        {
            case TypeOfStat.health:
                statText.text = (heroClassScript.heroHealth + "/" + heroClassScript.heroMaxHealth);
                break;

            case TypeOfStat.mana:
                statText.text = (heroClassScript.heroMana + "/" + heroClassScript.heroMaxMana);
                break;

            case TypeOfStat.xp:
                statText.text = (heroClassScript.heroExp + "/" + heroClassScript.heroExpToNext);
                break;
        }
    }
}
