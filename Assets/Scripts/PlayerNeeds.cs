using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerNeeds : MonoBehaviour {
    float health; //When any need is 0 it starts reducing health instead

    float hunger;
    float warmth;

    //Amount of need that degenerates each second
    public float hungerDecay = 0.1f;
    public float warmthDecay = 0.1f;

    //Modifiers for needs to allow for clothing/shelter
    public float warmthDecayMod = 1;
    public float hungerDecayMod = 1;

    public Gradient iconColors;
    public RawImage healthIcon;
    public RawImage warmthIcon;
    public RawImage hungerIcon;

    bool isHealthDecaying;

    FPPlayer player;

    public float Health
    {
        get { return health; }
    }

    public float Hunger
    {
        get { return hunger; }
    }

    public float Warmth
    {
        get { return warmth; }
    }

    public float changeHealth(float amount)
    {
        health += amount;
        if (health > 100) health = 100;
        else if (health <= 0) player.gameOver = true;
        return health;
    }

    public float changeHunger(float amount)
    {
        hunger += amount;
        if (hunger < 0)
        {
            StartCoroutine(BlinkHunger());
        }
        if (hunger > 100) hunger = 100;
        return hunger;
    }

    public float changeWarmth(float amount)
    {
        warmth += amount;
        if (warmth < 0)
        {
            StartCoroutine(BlinkWarmth());
        }
        if (warmth > 100) warmth = 100;
        return warmth;
    }

    void Start()
    {
        health = 100.0f;
        hunger = 100.0f;
        warmth = 100.0f;

        player = GetComponent<FPPlayer>();
    }

    IEnumerator BlinkHealth()
    {
        int numBlinks = 5;
        for (int i = 0; i < numBlinks; i++)
        {
            healthIcon.enabled = false;
            yield return new WaitForSeconds(0.15f);
            healthIcon.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator BlinkWarmth()
    {
        int numBlinks = 8;
        for (int i = 0; i < numBlinks; i++)
        {
            warmthIcon.enabled = false;
            yield return new WaitForSeconds(0.15f);
            warmthIcon.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator BlinkHunger()
    {
        int numBlinks = 8;
        for (int i = 0; i < numBlinks; i++)
        {
            hungerIcon.enabled = false;
            yield return new WaitForSeconds(0.15f);
            hungerIcon.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
    }

    void Update()
    {
        if (hunger > 0)
        {
            changeHunger(-hungerDecay * hungerDecayMod * Time.deltaTime);
        }
        else if (hunger > 100)
        {
            hunger = 100;
        }
        else
        {
            changeHealth(-hungerDecay * hungerDecayMod * Time.deltaTime);
        }

        if (warmth > 0)
        {
           changeWarmth(-warmthDecay * warmthDecayMod * Time.deltaTime);
        }
        else if (warmth > 100)
        {
            hunger = 100;
        }
        else
        {
            changeHealth(-warmthDecay * warmthDecayMod * Time.deltaTime);
        }

        healthIcon.color = iconColors.Evaluate(health / 100);
        warmthIcon.color = iconColors.Evaluate(warmth / 100);
        hungerIcon.color = iconColors.Evaluate(hunger / 100);
    }
}
