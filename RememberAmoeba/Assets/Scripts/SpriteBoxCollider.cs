using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteBoxCollider : MonoBehaviour
{
    [SerializeField] private int cellSize = 8; //size of each box in pixels
    [SerializeField] private float solidAmount = 0.5f; //how much of a cell must be filled to count as floor
    [SerializeField] private float thickness = 0.2f; //how thick the collider is

    private void Awake()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite; //calls to sprite rendere
        Texture2D texture = sprite.texture; //texture

        if (!texture.isReadable)
        {
            Debug.LogError("Turn on Read/Write in the sprite's import settings");
            return;
        }

        Rect rect = sprite.rect; //where the sprite sits on its texture
        Vector2 pivot = sprite.pivot; //sprite center point in pixels
        float ppu = sprite.pixelsPerUnit; //pixels per unit

        int cols = Mathf.CeilToInt(rect.width / cellSize); //width
        int rows = Mathf.CeilToInt(rect.height / cellSize); //hieght
         
        for (int row = 0; row < rows; row++)
        {
            int runStart = -1; //where a line of solid cells began

            for (int col = 0; col <= cols; col++)
            {
                bool solid = col < cols && IsSolid(texture, rect, col, row); //is this cell floor

                if (solid && runStart < 0) runStart = col; //start a new line of boxes

                if (!solid && runStart >= 0) //line ended, make one box for it
                {
                    AddBox(rect, pivot, ppu, runStart, col, row);
                    runStart = -1;
                }
            }
        }
    }

    private bool IsSolid(Texture2D texture, Rect rect, int col, int row)
    {
        int x = (int)rect.x + col * cellSize;
        int y = (int)rect.y + row * cellSize;
        int width = Mathf.Min(cellSize, (int)(rect.xMax - x));
        int height = Mathf.Min(cellSize, (int)(rect.yMax - y));

        Color[] pixels = texture.GetPixels(x, y, width, height);
        int filled = 0;

        foreach (Color pixel in pixels)
            if (pixel.a > 0.1f) filled++; //counts pixels that aren't see-through

        return filled >= pixels.Length * solidAmount;
    }

    private void AddBox(Rect rect, Vector2 pivot, float ppu, int colStart, int colEnd, int row)
    {
        float xMin = (colStart * cellSize - pivot.x) / ppu;
        float xMax = (Mathf.Min(colEnd * cellSize, rect.width) - pivot.x) / ppu;
        float yMin = (row * cellSize - pivot.y) / ppu;
        float yMax = (Mathf.Min((row + 1) * cellSize, rect.height) - pivot.y) / ppu;

        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3((xMin + xMax) / 2f, (yMin + yMax) / 2f, 0f);
        box.size = new Vector3(xMax - xMin, yMax - yMin, thickness);
    }
}