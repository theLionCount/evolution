using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Organism : MonoBehaviour
{
    public static int maxSize = 40;

    string[,] dna = new string[maxSize, maxSize];

    [Multiline]
    public string readableDna;

    public List<GameObject> possibleCells;
    Dictionary<string, GameObject> cellList = new Dictionary<string, GameObject>();

    public float deathWhenStarvingSince = 500;
    public float deathByOldAgeAt = 5000;
    public float thriveWhenEnergyIsMoreThan = 70;
    public float maxEnergy = 100;
    public float maxHunger = 30;
    public float reproductionChanceWhenFlourishing = 0.1f;
    public int reproductionTick = 50;

    public float energy = 20;
    public float starvingSince = 0;

    public float energyConsumptionRate = 1;

    public TMP_Text energyText, hungerText;

    // Start is called before the first frame update
    void Start()
    {
        //Resolving dna if needed
        if (readableDna != "")
        {
            var lines = readableDna.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] >= 'A' && lines[i][j] <= 'Z') dna[i, j] = lines[i][j].ToString();
                }
            }
        }

        //Just putting the cells in an easily readable form
        foreach (var item in possibleCells)
        {
            cellList.Add(item.GetComponent<CellBase>().cellType, item);
        }

        //Making dna readable for debug
        readableDna = "";
        for (int i = 0; i < maxSize; i++)
        {
            string line = "";
            for (int j = 0; j < maxSize; j++)
            {
                line += dna[i, j];
            }
            readableDna += line + "\n";
        }

        //birth
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                if (dna[i, j] != "" && dna[i, j] != null)
                {
                    var cell = Instantiate(cellList[dna[i, j]], transform);
                    cell.transform.localPosition = new Vector3(j - maxSize / 2, -i + maxSize / 2, 0);
                    cell.GetComponent<CellBase>().host = this;
                }
            }
        }
    }

    int reproductionCD = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        burnEnergy();
        starve();
        thrive();

        if (energy < -maxHunger) energy = -maxHunger;
        if (energy > maxEnergy) energy = maxEnergy;
        setUI();
    }

    public void burnEnergy()
    {
        energy-=energyConsumptionRate;
    }

    public void starve()
    {
        if (energy <= 0) starvingSince++;
        else starvingSince = 0;
        if (starvingSince >= deathWhenStarvingSince) dieByStarvation();
    }

    public void dieByStarvation()
    {
        Destroy(gameObject);
    }

    public void feed(float amount)
    {
        energy += amount;
    }

    public void thrive()
    {
        if (energy > thriveWhenEnergyIsMoreThan)
        {
            reproductionCD++;
            if (reproductionCD >= reproductionTick)
            {
                reproductionCD = 0;
                if (Random.Range(0f, 1f) < reproductionChanceWhenFlourishing) reproduce();
            }
        }
    }

    public float mutationChance;
    public float growChance;
    public float changeChance;

    public float spawnDistance = 6;
    public float spawnDistanceVarianceLow = 0.8f;
    public float spawnDistanceVarianceHigh = 1.2f;
    public int numberOfOffsprings = 2;

    public GameObject offspringBase;

    public void reproduce()
    {
        for (int i = 0; i < numberOfOffsprings; i++)
        {
            Vector2 pos = transform.position + Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.left * spawnDistance * Random.Range(spawnDistanceVarianceLow, spawnDistanceVarianceHigh);
            var organism = Instantiate(offspringBase, pos, Quaternion.identity);
            organism.GetComponent<Organism>().offspringBase = offspringBase;
            organism.GetComponent<Organism>().readableDna = "";
            organism.GetComponent<Organism>().dna = mutate();
        }
        Destroy(gameObject);
    }

    public string[,] mutate()
    {
        return dna;
    }

    public void setUI()
    {
        energyText.text = "Energy: " + (energy > 0 ? energy.ToString() : "0");
        hungerText.text = "Hunger: " + (energy < 0 ? (-energy).ToString() : "0");
    }
}


