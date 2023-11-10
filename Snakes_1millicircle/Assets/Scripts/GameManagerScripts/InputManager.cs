using UnityEngine;
using UnityEngine.EventSystems;

// Source https://github.com/SunnyValleyStudio/SimpleCityBuilder

public class InputManager : MonoBehaviour
{
    public LayerMask groundMask;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RoadManager roadManager;
    [SerializeField] private StructureManager structureManager;
    [SerializeField] private GameManager2 gameManager;
    [SerializeField] private BuildingPlacer buildingPlacer;
    [SerializeField] private PlacementManager placementManager;

    private Vector2 cameraMovementVector;
    private Vector3Int previusPos;

    public Vector2 CameraMovementVector
    {
        get { return cameraMovementVector; }
    }

    private void Update()
    {
        CheckClickDownEvent();
        CheckClickUpEvent();
        CheckClickHoldEvent();
        CheckArrowInput();
    }

    public Vector3Int? RaycastGround()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
            return positionInt;
        }

        return null;
    }

    private void CheckArrowInput()
    {
        cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void CheckClickHoldEvent()
    {
        if (!Input.GetMouseButton(0)) return;
        var position = RaycastGround();
        if (position == null) return;
        if (position == previusPos) return;
        previusPos = (Vector3Int) position;
        if (GameManager2.instance.GetGameState() == GameManager2.GameState.RoadBuilding &&
            EventSystem.current.IsPointerOverGameObject() == false)
        {
            gameManager.ClearInputActions();
            var pos = (Vector3Int) position;
            roadManager.PlaceRoad(pos);
        }

        if (GameManager2.instance.GetGameState() == GameManager2.GameState.HouseBuilding &&
            EventSystem.current.IsPointerOverGameObject() == false)
        {
            gameManager.ClearInputActions();
            var pos = (Vector3Int) position;
            buildingPlacer.DraggingHouses(pos);
        }

        if (GameManager2.instance.GetGameState() == GameManager2.GameState.SingleStructure &&
            EventSystem.current.IsPointerOverGameObject() == false)
        {
            gameManager.ClearInputActions();
            var pos = (Vector3Int) position;
            buildingPlacer.DraggingSingleBuilding(pos);
        }
    }

    private void CheckClickUpEvent()
    {
        if (!Input.GetMouseButtonUp(0)) return;
        if (GameManager2.instance.GetGameState() == GameManager2.GameState.RoadBuilding)
        {
            gameManager.ClearInputActions();
            var position = RaycastGround();
            if (position != null)
            {
                var pos = (Vector3Int) position;
                roadManager.PlaceRoad(pos);
            }

            roadManager.FinishPlacingRoad();
        }

        if (GameManager2.instance.GetGameState() != GameManager2.GameState.RoadBuilding)
        {
            gameManager.ClearInputActions();
            BuildingPlacer.inst.FinishBuildingHouse();
        }
    }

    private void CheckClickDownEvent()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (GameManager2.instance.GetGameState() != GameManager2.GameState.RoadBuilding)
        {
            var position = RaycastGround();
            if (position != null)
            {
                BuildingPlacer.inst.BeginNewBuildingPlacement(City.inst.buildings[City.inst.indexOfSelectedBuilding],
                    (Vector3Int) position);
            
            }
        }
    }
}