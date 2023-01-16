# Unity-Tilemap-Minimap

 // how to draw a minimap on the scene
        // Adding another Camera directly on the level, this may be the better solution, but what if I want a minimalistic style for the mini map?
        // Sprite
        // TileMap
        // UI Element
            // UI has the property of overlaying on the scene, which doesn't need me to set up another camera
            
    // UI minimap
    // One thing that I can think of is using scroll view, which has a property of grid system, which I 
        // may use as a tool to read the tilemap information and draw it out in the grid.
    
        // Minimap size = level size for now (solved)
    // A Scroll View would be a perfect tool for this assignment, a view port shows the intended section, and the content containing the whole mini map.

    // Task
    /*  Control cell spawning in the Grid (content).
     *  Get and fix the height/width of the Grid. (level may have different size)
        * opening the debug mode allows you to see the grid size, also "compress tile map bound" can be found by clicking the three dot button  
     *
     * looping through the tile map
        * get x bound and y bound with tilemap.cellbounds() and compresscellbound()
        * two for loops for each coordinate, one inner loop for each TileMap from top to bottom (sorting order),
          if there is a TileBase present, stop the inner loop
            
        * get the sprite and set it to the corresponding grid.
        
     * Done!
     */
    
    // Additional function
    /* No Scroll Bar, instead scroll with buttons.
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
     */
    
    // Improvements
    /* Irregular shaped level/map
     * 
     */
