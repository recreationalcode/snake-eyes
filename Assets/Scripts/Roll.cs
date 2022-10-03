using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Roll : MonoBehaviour
{
    public PlayerController player;
    public RollPreview preview; 
    public TextMeshProUGUI doublesText;
    public GameObject diePrefab;
    public Canvas canvas;
    public CanvasManager canvasManager;
    public Die[] dice;
    private List<Die> diceList;
    public bool canRoll = true;

    public AudioSource rollSound;
    public AudioSource freeRollSound;

    // Start is called before the first frame update
    void Start()
    {
        doublesText.gameObject.SetActive(false);
        diceList = new List<Die>(dice);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R) && canRoll)
        {
            canRoll = false;

            StartCoroutine(OnRoll());
        }

        if (!player.HasRolls()) {
            canRoll = false;
        }
    }

    public void CanRoll(bool canRoll)
    {
        this.canRoll = canRoll;
    }
    
    public bool GetCanRoll()
    {
        return canRoll;
    }

    public void AddDie()
    {
        // Instantiate Die prefab
        GameObject die = Instantiate(diePrefab, diePrefab.transform.position, diePrefab.transform.rotation);
        
        // Add die to array
        diceList.Add(die.GetComponent<Die>());

        // Make sure it shows up in the UI (take last and place it offset by 45?)
        die.transform.SetParent(diceList[0].transform.parent, false);
        RectTransform t = die.GetComponent<RectTransform>();
        t.anchoredPosition = diceList[0].gameObject.GetComponent<RectTransform>().anchoredPosition
            + new Vector2(45 * (diceList.Count - 1), 0);
    }

    private int _Roll()
    {
        return Random.Range(1, 7);
    }

    IEnumerator OnRoll()
    {

        player.UseRoll();

        rollSound.Play();

        foreach(Die die in diceList)
        {            
            die.Roll();
        }

        yield return new WaitForSeconds(0.5f);

        foreach(Die die in diceList)
        {
            int roll = _Roll();
            
            die.StopRoll();
            die.SetValue(roll);
        }

        rollSound.Stop();

        List<Die> distinctDice = diceList
            .GroupBy(p => p.GetValue())
            .Select(g => g.First())
            .ToList();

        if (distinctDice.Count != diceList.Count)
        {
            StartCoroutine(OnDoubles());
            player.AddRoll();
        }
            
        preview.Preview(diceList);
    }

    IEnumerator OnDoubles()
    {
        doublesText.gameObject.SetActive(true);

        freeRollSound.Play();

        yield return new WaitForSeconds(1f);

        doublesText.gameObject.SetActive(false);
    }


    public void Reset()
    {
        foreach(Die die in diceList)
        {
            die.SetValue(0);
        }

        canRoll = true;
    }
}
