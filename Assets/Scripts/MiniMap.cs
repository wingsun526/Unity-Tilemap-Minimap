using System;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup theMinimapGridLayout;

    [SerializeField] private ScrollRect theScrollRect;

    [SerializeField] private GameObject miniMapGridPrefab;

    [SerializeField] private Tilemap boundaryTileMap;

    [SerializeField] private Tilemap[] tileMapsToBeTranslateInOrder;

    [SerializeField] private RectTransform emptyParentContent;

    [SerializeField] private RectTransform playerDotOverlay;

    

    [SerializeField] private GameObject playerDot;

    [Header("Settings")]
    [SerializeField] private int cellSize = 30;

    [SerializeField] private Vector2 miniMapSmallSize = new Vector2(320, 180);
    [SerializeField] private Vector2 miniMapSmallSizePosition = new Vector2(750, 300);

    [SerializeField] [Range(0f, 0.5f)] private float deadZoneWidthPercentage;
    [SerializeField] [Range(0f, 0.5f)] private float deadZoneHeightPercentage;
    [SerializeField] private float autoScrollSpeed = 1f;
    
    
    
    private Vector2 scrollViewPlayerInput;
    private bool FullMapIsActive = false;
    private Vector3 playerPosition;
    private RectTransform theScrollRectRectTransform;
    private bool contentJustRefreshed = false;
    private float contentLastRefreshed;

    private void Awake()
    {
        theScrollRectRectTransform = theScrollRect.GetComponent<RectTransform>();
    }

    void Start()
    {
        RedrawMiniMap();
    }

    private void Update()
    {
        UpdateContentRefreshState(0.2f);
        ChangePlayerDotPosition();
        ScrollTheView();
        
    }
    

    public void SendPlayerPosition(Vector3 value)
    {
        playerPosition = value;
    }
    public void ChangeScrollViewInput(Vector2 value) //PlayerMovement
    {
        scrollViewPlayerInput = value;
    }
    
    public void ToggleFullMap()
    {
        FullMapIsActive = !FullMapIsActive;
        RedrawMiniMap();
    }

    private void UpdateContentRefreshState(float duration)
    {
        if (Time.time - contentLastRefreshed >= duration)
        {
            contentJustRefreshed = false;
        }
    }

    private void ContentRefreshed()
    {
        contentJustRefreshed = true;
        contentLastRefreshed = Time.time;
    }
    private void RedrawMiniMap()
    {
        ChangeScrollViewSize();
        DrawMiniMapContent();
    }

    private void ChangeScrollViewSize()
    {
        boundaryTileMap.CompressBounds();
        var bounds = boundaryTileMap.cellBounds;
        var size = bounds.size;
        if (FullMapIsActive)
        {
            cellSize = 60;
            theScrollRectRectTransform.anchoredPosition = new Vector2(0 ,0);
            theScrollRectRectTransform.sizeDelta = new Vector2(cellSize * size.x, cellSize * size.y);
        }
        else
        {
            cellSize = 30;
            theScrollRectRectTransform.anchoredPosition = miniMapSmallSizePosition;
            theScrollRectRectTransform.sizeDelta = miniMapSmallSize;
        }

        
    }
    private void ScrollTheView()
    {
        //Manual Scrolling
        if(scrollViewPlayerInput != Vector2.zero)
        {
            //Controlled by Player
            theScrollRect.normalizedPosition += scrollViewPlayerInput * Time.deltaTime;
        }
        //Auto scroll to playerDot when out of range
        else
        {
            var dotPosInRect = theScrollRectRectTransform.InverseTransformPoint(playerDot.transform.position);
            var xOffSet = dotPosInRect.x;
            var yOffSet = dotPosInRect.y;
            var xDeadZone = deadZoneWidthPercentage * miniMapSmallSize.x;
            var yDeadZone = deadZoneHeightPercentage * miniMapSmallSize.y;
            var scrollSpeed = contentJustRefreshed ? 10 : autoScrollSpeed;//if content just refreshed, scrollView will instant home in on the player dot.
            if (xOffSet > xDeadZone || xOffSet < -xDeadZone) //Math.Clamp will always return 1 or -1 this way
            {
                theScrollRect.horizontalNormalizedPosition += Math.Clamp(xOffSet, -1, 1) * scrollSpeed * Time.deltaTime;
            }
            if (yOffSet > yDeadZone || yOffSet < -yDeadZone)
            {
                theScrollRect.verticalNormalizedPosition += Math.Clamp(yOffSet, -1, 1) * scrollSpeed * Time.deltaTime;
            }
        }
        
        
        
        
        
    }
    
    private void ChangePlayerDotPosition() // Calculate player position relative to the bottom-left corner of boundary map, and show it on the minimap.
    {
        BoundsInt bounds = boundaryTileMap.cellBounds;
        //Debug.Log(bounds.position);
        var xPercentage = (playerPosition.x - bounds.position.x) / bounds.size.x;
        var yPercentage = (playerPosition.y - bounds.position.y) / bounds.size.y;
        var overLaySize = playerDotOverlay.sizeDelta;
        playerDot.GetComponent<RectTransform>().anchoredPosition = new Vector2(overLaySize.x * xPercentage, overLaySize.y * yPercentage);
    }
    private void DrawMiniMapContent()
    {
        // Destroy all child
        foreach (Transform child in theMinimapGridLayout.transform)
        {
            Destroy(child.gameObject);
        }
        boundaryTileMap.CompressBounds();
        BoundsInt bounds = boundaryTileMap.cellBounds;
        var size = bounds.size;
        theMinimapGridLayout.cellSize = new Vector2(cellSize, cellSize);
        
        
        //change gridContent size
        theMinimapGridLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize * size.x, cellSize * size.y);
        //change parentContent size
        emptyParentContent.GetComponent<RectTransform>().sizeDelta =
            theMinimapGridLayout.GetComponent<RectTransform>().sizeDelta;
        //change playerDotOverlay size
        playerDotOverlay.sizeDelta = theMinimapGridLayout.GetComponent<RectTransform>().sizeDelta;
        
        
        
        for (int y = bounds.max.y - 1; y >= bounds.min.y; y--)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var cellPosition = new Vector3Int(x, y, 0);
                //Debug.Log(Vector3Int.Equals(playerPos, cellPosition));
                
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
                
                
                
            }
        }
        //Content Refreshed
        ContentRefreshed();
    }
}
