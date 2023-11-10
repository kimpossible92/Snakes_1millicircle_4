using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
	public int money;
	public int curPopulation;
	public int curJobs;
	public int curFood;

	private int turnsElapsed;
	public DateTime currentDate = new DateTime(1815, 3, 16);
	public int maxPopulation;
	public int maxJobs;
	public int incomePerJob;

	public Boolean debugMode;

	public TextMeshProUGUI statsText;

	public List<BuildingPreset> buildings = new List<BuildingPreset>();

	public static City inst;
	public int indexOfSelectedBuilding;

	void Awake()
	{
		inst = this;

		TimeManager.OnGameTick += EndTurn;
	}

	public void OnPlaceBuilding(BuildingPreset building)
	{
		maxPopulation += building.population;
		maxJobs += building.jobs;
		buildings.Add(building);
	}

	public void EndTurn()
	{
		turnsElapsed++;
		CalculateMoney();
		CalculatePopulation();
		CalculateJobs();
		CalculateFood();
		currentDate = currentDate.AddDays(1);
		string formattedDate = CalculateDate();
		statsText.text = string.Format("ƒ {0}     Pop {1}     {2}     Turn{3}", new object[4] { money, curPopulation, formattedDate, turnsElapsed });
	}

	string CalculateDate()
	{
		return currentDate.ToString("dd MMMM yyyy");
	}

	void CalculateMoney()
	{
		money += curJobs * incomePerJob;

		foreach (BuildingPreset building in buildings)
			money -= building.costPerTurn;
	}

	void CalculatePopulation()
	{
		maxPopulation = 0;

		foreach (BuildingPreset building in buildings)
			maxPopulation += building.population;

		if (curFood >= curPopulation && curPopulation < maxPopulation)
		{
			curFood -= curPopulation / 4;
			curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation);
		}
		else if (curFood < curPopulation)
		{
			curPopulation = curFood;
		}
	}

	void CalculateJobs()
	{
		curJobs = 0;
		maxJobs = 0;

		foreach (BuildingPreset building in buildings)
			maxJobs += building.jobs;

		curJobs = Mathf.Min(curPopulation, maxJobs);
	}

	void CalculateFood()
	{
		curFood = 0;

		foreach (BuildingPreset building in buildings)
			curFood += building.food;
	}
}