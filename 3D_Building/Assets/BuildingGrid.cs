using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;


    private void Awake()
    {
        mainCamera = Camera.main;
        grid = new Building[GridSize.x, GridSize.y];
        
    }

    public void StartPlacingBuilding(Building buildPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildPrefab);
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool avalibale = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.size.x) avalibale = false;
                if (y < 0 || y > GridSize.x - flyingBuilding.size.x) avalibale = false;

                if (avalibale && IsPlaceTaken(x, y)) avalibale = false;

                flyingBuilding.transform.position = new Vector3(x,0,y);
                flyingBuilding.SetTransparent(avalibale);

                if (avalibale && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuild(x,y);
                }
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.size.y; y++)
            {
                if(grid[placeX + x, placeY + y] != null) return true;
            }
        }
        return false;
    }

    private void PlaceFlyingBuild(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }


        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }

}
