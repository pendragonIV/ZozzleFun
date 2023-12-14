using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private Tilemap hightlightedMap;
    [SerializeField]
    private TileBase mark;

    [SerializeField]
    private List<Vector3Int> locations = new List<Vector3Int>();
    [SerializeField]
    private List<Vector3Int> hightlightedLocations = new List<Vector3Int>();

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GetMapLocation();
    }

    #region GetMapLocation
    private void GetMapLocation()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);

                if (tileMap.HasTile(localLocation))
                {
                    locations.Add(localLocation);
                }
            }
        }
    }

    #endregion

    #region Highlighted

    public void HighlightCell(Vector3Int cellPosition)
    {
        hightlightedMap.SetTile(cellPosition, mark);
        hightlightedLocations.Add(cellPosition);
    }

    public void HighlightCells(Vector3Int[] cellPosition)
    {
        foreach (Vector3Int location in cellPosition)
        {
            hightlightedMap.SetTile(location, mark);
            hightlightedLocations.Add(location);
        }
    }

    public void ClearHighlightedCells()
    {
        foreach (Vector3Int location in hightlightedLocations)
        {
            hightlightedMap.SetTile(location, null);
        }
        hightlightedLocations.Clear();
    }

    #endregion

    #region Getters

    public List<Vector3Int> GetHighlightedCells()
    {
        return hightlightedLocations;
    }

    public bool IsMoveableArea(Vector3Int cellPos)
    {
        if (hightlightedMap.GetTile(cellPos) == null)
        {
            return false;
        }
        return true;
    }

    public bool IsPlaceableArea(Vector3Int cellPos)
    {
        if (tileMap.GetTile(cellPos) == null)
        {
            return false;
        }
        return true;
    }

    public List<Vector3Int> GetCellsPosition()
    {
        return locations;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }

    #endregion

    #region Setters
    public void SetMap(Grid map)
    {
        //this.tileMap = map.transform.GetChild(0).GetComponent<Tilemap>();
        //this.hightlightedMap = map.transform.GetChild(2).GetComponent<Tilemap>();
    }

    #endregion
}
