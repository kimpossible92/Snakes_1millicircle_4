using System;
using System.Collections.Generic;
using System.Linq;
using PathologicalGames;
using UnityEngine;

// Source https://github.com/SunnyValleyStudio/SimpleCityBuilder

public class PlacementManager : MonoBehaviour
{
	public int width, height;
	public Grid placementGrid;
	public InputManager inputManager;
	private float placementIndicatorUpdateRate = 0.05f;
	private float lastUpdateTime;
	public GameObject placementIndicator;
	private bool currentlyPlacing;

	public Dictionary<Vector3Int, StructureModel> temporaryRoadobjects = new Dictionary<Vector3Int, StructureModel>();
	public Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>();

	public static PlacementManager inst;


	private void Awake()
	{
		inst = this;
	}

	private void Start()
	{
		placementGrid = new Grid(width, height);
	}
	private void Update()
	{
		if (Time.time - lastUpdateTime > placementIndicatorUpdateRate && currentlyPlacing)
		{
			lastUpdateTime = Time.time;

			var position = inputManager.RaycastGround();
			if (position != null)
			{
				placementIndicator.transform.position = (Vector3)position;
			}
		}

	}

	public void StartPlacement()
	{
		currentlyPlacing = true;
		placementIndicator.SetActive(true);
	}

	public void EndPlacement()
	{
		currentlyPlacing = false;
		placementIndicator.SetActive(false);
	}

	internal CellType[] GetNeighbourTypesFor(Vector3Int position)
	{
		return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
	}

	internal bool CheckIfPositionInBound(Vector3Int position)
	{
		if (position.x >= 0 && position.x < width && position.z >= 0 && position.z < height)
		{
			return true;
		}
		return false;
	}

	internal void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type)
	{
		placementGrid[position.x, position.z] = type;
		StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
		structureDictionary.Add(position, structure);
		DestroyNatureAt(position);
	}

	private void DestroyNatureAt(Vector3Int position)
	{
		RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
		foreach (var item in hits)
		{
			Destroy(item.collider.gameObject);
		}
	}

	internal bool CheckIfPositionIsFree(Vector3Int position)
	{
		return CheckIfPositionIsOfType(position, CellType.Empty);
	}

	private bool CheckIfPositionIsOfType(Vector3Int position, CellType type)
	{
		return placementGrid[position.x, position.z] == type;
	}

	internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
	{
		placementGrid[position.x, position.z] = type;
		StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
		temporaryRoadobjects.Add(position, structure);
//		print("postavlja se privremena struktura " + position + " " + structurePrefab.name + " tip celije je " + type);
	}

	internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type)
	{
		var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
		List<Vector3Int> neighbours = new List<Vector3Int>();
		foreach (var point in neighbourVertices)
		{
			neighbours.Add(new Vector3Int(point.X, 0, point.Y));
		}
		return neighbours;
	}

	private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
	{
		var structure = PoolManager2.Pools["Buildings"].Spawn(structurePrefab);
		structure.position = position;
		var structureModel = structure.gameObject.GetComponent<StructureModel>();
		return structureModel;
	}

	internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)
	{
		var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z));
		List<Vector3Int> path = new List<Vector3Int>();
		foreach (Point point in resultPath)
		{
			path.Add(new Vector3Int(point.X, 0, point.Y));
		}
		return path;
	}

	internal void RemoveAllTemporaryStructures()
	{
		foreach (var structure in temporaryRoadobjects.Values)
		{
			var position = Vector3Int.RoundToInt(structure.transform.position);
			placementGrid[position.x, position.z] = CellType.Empty;
			PoolManager2.Pools["Buildings"].Despawn(structure.transform);
		}
		temporaryRoadobjects.Clear();
	}

	internal void AddtemporaryStructuresToStructureDictionary()
	{
		foreach (var structure in temporaryRoadobjects)
		{
			structureDictionary.Add(structure.Key, structure.Value);
			DestroyNatureAt(structure.Key);
		}
		temporaryRoadobjects.Clear();
	}

	public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
	{
		if (temporaryRoadobjects.ContainsKey(position))
			temporaryRoadobjects[position].SwapModel(newModel, rotation);
		else if (structureDictionary.ContainsKey(position))
			structureDictionary[position].SwapModel(newModel, rotation);
	}
}
