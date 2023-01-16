using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap map;

    [SerializeField] private Tilemap[] allTheMaps;

    [SerializeField] private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;
    
    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (TileData tileData in tileDatas)
        {
            foreach (TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

    }

    void Update()
    {
        
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        
        //TileBase clickedTile = map.GetTile(gridPosition);
        String something = "did not get anything";
        for (int i = 0; i < allTheMaps.Length; i++)
        {
            var hoveredTile = allTheMaps[i].GetTile(gridPosition);
            if(hoveredTile != null)
            {
                something = dataFromTiles[hoveredTile].GetTileInformation();
            }
        }

        //var something = dataFromTiles[clickedTile].GetTileInformation();

        //print(something);





    }
    
    
}
