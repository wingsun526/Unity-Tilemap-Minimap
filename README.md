# What it does?

A script that takes in your Tilemap level and creates a minimap with it. The minimap auto tracks the "player" and follows it around.

# How does it work?

## Scroll View - Unity Component

Usually, a minimap indicates where the current screen lies within the scope of the whole map, a scroll view being about to scroll through its content is a perfect candidate. 

## Grid Layout Group

Considering the nature of Tilemap being a grid base component, I decided to use the grid layout group component in the content to represent the tiles in the tilemap.
```c#
for (int i = 0; i < tileMapsToBeTranslateInOrder.Length; i++)
{
    if (tileMapsToBeTranslateInOrder[i].GetTile(cellPosition) != null)
    {
        Sprite cellSprite = tileMapsToBeTranslateInOrder[i].GetSprite(cellPosition);
        var gridCell = Instantiate(miniMapGridPrefab, theMinimapGridLayout.transform, false);
        gridCell.GetComponent<Image>().sprite = cellSprite;
        break;
    }
}
```
## Tracking
The Tracking red dot is drawn each frame to show the player location. It is an overlay on top of the minimap content. By doing so, I can avoid drawing the same minimap each frame, which reduces the impact on performance. 






# My Notes



### Task
*  Control cell spawning in the Grid (content).
*  Get and fix the height/width of the Grid. (level may have different size)
   * opening the debug mode allows you to see the grid size, also "compress tile map bound" can be found by clicking the three dot button  
*
* looping through the tile map
   * get x bound and y bound with tilemap.cellbounds() and compresscellbound()
   * two for loops for each coordinate, one inner loop for each TileMap from top to bottom (sorting order),
     if there is a TileBase present, stop the inner loop
   * get the sprite and set it to the corresponding grid.
* Done!

    
### Additional function
* No Scroll Bar, instead scroll with buttons.
   * new input system settings.
   * clamped scroll view in editor.
   * (Jan 2nd) Done! Minimap script now depends on PlayerMovement input.
* View Full Minimap.
   * ScrollView Size, and Content Size are different, beware
   * (Jan 2nd) Done!
* toggle full map
   * destroy all previous child
   * (Jan 6th) Done!
   * (Jan 8th) Centered when in Full Map Scale.
* show player location
   * (using gridlayoutgroup might be a stupid way to make a minimap, instead should just use another camera?)
   * (Jan 8th) Done! Performance drops significantly, maybe another overlay to show player position.
* another overlay to show player position
   * cellbound return bottom-left corner coordinates, the UI Dot should anchor to bottom-left.
   * (Jan 9th) Done! By doing so, minimap doesn't need to be refresh frequently.
* automatically center on player
   * Transform dot world position to rect local position, and move according to offset,
       * Heavy stuttering problem.
       * Smoother with smaller scroll speed factor
   * (Jan 12th) Done! Transform Dot position to rectTransform local position.
* 
