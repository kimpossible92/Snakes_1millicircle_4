using SVS;
using UnityEngine;


// Source https://github.com/SunnyValleyStudio/SimpleCityBuilder
public class GameManager2 : MonoBehaviour
{
	public CameraMovement cameraMovement;
	public RoadManager roadManager;
	public InputManager inputManager;
	public PlacementManager placementManager;
	public UIController uiController;
	public StructureManager structureManager;
	public static GameManager2 instance;
	
	public enum GameState
	{
		RoadBuilding,
		HouseBuilding,
		SingleStructure
	}

	private GameState gameState;
	private float lastUpdateTime;

	private void Awake()
	{
		instance = this;
	}
	
	public void UpdateGameState(GameState newState)
	{
		gameState = newState;
	}

	public GameState GetGameState()
	{
		return gameState;
	}

	public void ClearInputActions()
	{
		placementManager.StartPlacement();
	}

	private void Update()
	{
		cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
	}

}
