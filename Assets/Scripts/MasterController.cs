using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterController : MonoBehaviour
{
    public List<GameObject> playerUnits; // Your guys
    public GameObject singleUnit; // The skin of your units

    public List<GameObject> cpuUnits; // Computer's units
    public GameObject cpuSingleUnit;

    public Transform spawnPosition;

    public Text unitCounter;
    int totalUnits = 10; // Total units that the player "owns"
    int placedUnits = 0; // Total

    public Transform cpuSpawnPosition;

    public Text cpuCounter;
    int cpuPlacedUnits = 0;
    int cpuTotalUnits = 10;

    public bool started {get; set;}
    public bool matchFinished {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        playerUnits = new List<GameObject>();
        cpuUnits = new List<GameObject>();
        started = false;
        matchFinished = false;
        updateUnitCount();
        setupCPU();
        updateCPUCount();
    }

    public void sendOut(){
        if (!started){
            foreach ( GameObject unit in playerUnits){
                unit.GetComponent<GuyProperties>().moveGuy(true);
            }

            // Release CPU units as well
            foreach ( GameObject unit in cpuUnits) {
                unit.GetComponent<GuyProperties>().moveGuy(false);
            }
        }
        started = true;
    }

    public void increaseUnits(bool isPlayer){

        // For Players
        // Instantiate unit if we don't exceed the totalUnits we have.
        if (isPlayer){
            if (placedUnits < totalUnits){
                GameObject createdUnit = Instantiate(singleUnit, spawnPosition, true);
                // Add the unit to the list
                playerUnits.Add(createdUnit);
                placedUnits++;
            }
        }
        else {
            GameObject createdUnit = Instantiate(cpuSingleUnit, cpuSpawnPosition, true);
            cpuUnits.Add(createdUnit);
        }
        // For CPUs

    }

    public void decreaseUnits(){
        if (placedUnits > 0){
            playerUnits[playerUnits.Count-1].SetActive(false);
            playerUnits.RemoveAt(playerUnits.Count-1);
            placedUnits--;
        }
    }

    // Called when power of a colliding unit is less than the enemy
    public void destroyUnit(GameObject unit){
        Debug.Log(unit.tag);
        if (unit.tag == "Player"){
            unit.SetActive(false);
            playerUnits.Remove(unit);
            placedUnits--;
            totalUnits--;
            updateUnitCount();
            if (placedUnits <= 0){
                fightOver(true);// You lose
            }
        }
        else {
            unit.SetActive(false);
            cpuUnits.Remove(unit);
            cpuPlacedUnits--;
            cpuTotalUnits--;
            updateCPUCount();
            if (cpuPlacedUnits <= 0){
                fightOver(false);// They lose
            }
        }

    }

    public void switchDirection(string unitTag){
        Debug.Log(unitTag);
        if (unitTag == "Enemy"){
            foreach (GameObject unit in cpuUnits){
                unit.GetComponent<GuyProperties>().walkSpeed *= -1;
                StartCoroutine(unit.GetComponent<GuyProperties>().startMovement());
            }
        }
        else if (unitTag == "Player"){
            foreach (GameObject unit in playerUnits){
                unit.GetComponent<GuyProperties>().walkSpeed *= -1;
                StartCoroutine(unit.GetComponent<GuyProperties>().startMovement());
            }
        }
    }

    public void setupCPU(){
        cpuPlacedUnits = Random.Range(1, cpuTotalUnits);
        for (int i = 0; i < cpuPlacedUnits; i++){
            increaseUnits(false);
        }
    }

    public void fightOver(bool playerWon){
        matchFinished = true;
        if (playerWon){
            // Display player wins
            // Give territory to player
        }
        else {
            // Display CPU wins
            // Give territory to CPU
        }
    }

    public void updateUnitCount(){
        unitCounter.text = placedUnits.ToString() + " / " + totalUnits.ToString();
    }

    public void updateCPUCount(){
        cpuCounter.text = cpuPlacedUnits.ToString() + " / " + cpuTotalUnits.ToString();
    }
}
