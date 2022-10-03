using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RollPreview : MonoBehaviour
{
    public PlayerController player;
    public Grid grid;
    public LayerMask stopMovement;
    public Tilemap rollPreviewTilemap;
    public Tilemap moveHighlightTilemap;

    public Roll roll;
    private Vector3Int currentMoveHighlightPosition = Vector3Int.zero;
    private HashSet<Vector3Int> options = new HashSet<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        moveHighlightTilemap.ClearAllTiles();

        if(rollPreviewTilemap.HasTile(rollPreviewTilemap.WorldToCell(mousePosition)))
        {
            Vector3Int tilePosition = rollPreviewTilemap.WorldToCell(mousePosition);

            if (!moveHighlightTilemap.HasTile(tilePosition))
            {
                currentMoveHighlightPosition = tilePosition;
                moveHighlightTilemap.SetTile(tilePosition, rollPreviewTilemap.GetTile(rollPreviewTilemap.WorldToCell(mousePosition)));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3Int tilePosition = rollPreviewTilemap.WorldToCell(mousePosition);

            if (moveHighlightTilemap.HasTile(tilePosition))
            {
                moveHighlightTilemap.SetTile(currentMoveHighlightPosition, null);
                currentMoveHighlightPosition = Vector3Int.zero;
                rollPreviewTilemap.ClearAllTiles();

                roll.Reset();

                player.Move(player.grid.WorldToCell(mousePosition));
            }
        }
    }

    private Vector3Int[] directions = new[] { Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down };

    public void Preview(List<Die> dice)
    {
        foreach(Die die in dice)
        {
            Preview(die);
        }
    }

    private void Preview(Die die)
    {
        int roll = die.GetValue();

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3Int newCellPosition = GetRollCellPosition(player.GetCellPosition(), roll, directions[i], player.HasKeys());
            Vector3 newMovePosition = grid.GetCellCenterWorld(newCellPosition);

            if (newCellPosition != Vector3Int.zero)
            {
                Vector3Int tilePosition = rollPreviewTilemap.WorldToCell(newMovePosition);
                options.Add(tilePosition);

                rollPreviewTilemap.SetTile(tilePosition, die.GetValueTile());
            }
        }
    }

    public Vector3Int GetRollCellPosition(Vector3Int currentCellPosition, int r, Vector3Int direction, bool hasKeys = false)
    {
        Vector3Int movement = r * direction;
        Vector3Int newCellPosition = currentCellPosition + movement;
        Vector3 newMovePosition = grid.GetCellCenterWorld(newCellPosition);
        Vector3 currentMovePosition = grid.GetCellCenterWorld(currentCellPosition);
        Vector3 collisionBoxCenter = Vector3.Lerp(currentMovePosition, newMovePosition, 0.5f);

        while(r > 0)
        {
            RaycastHit2D hit = Physics2D.BoxCast(currentMovePosition, Vector2.one / 2, 0f, (Vector2) ((Vector3) direction), r, stopMovement);

            if (hit.collider == null) break;

            if (hit.transform.name.Contains("Lock"))
            {
                if (hasKeys && Vector3.Distance(hit.transform.position, currentMovePosition) == r)
                {
                    break;
                }
            }

            r--;
            movement = r * direction;
            newCellPosition = currentCellPosition + movement;
            newMovePosition = grid.GetCellCenterWorld(newCellPosition);
            currentMovePosition = grid.GetCellCenterWorld(currentCellPosition);
            collisionBoxCenter = currentMovePosition + ((newMovePosition - currentMovePosition) / 2);
        }

        return r > 0 ? newCellPosition : Vector3Int.zero;
    }

    // void DebugDrawBox( Vector2 point, Vector2 size, float angle, Color color, float duration) {

    //     var orientation = Quaternion.Euler(0, 0, angle);

    //     // Basis vectors, half the size in each direction from the center.
    //     Vector2 right = orientation * Vector2.right * size.x/2f;
    //     Vector2 up = orientation * Vector2.up * size.y/2f;

    //     // Four box corners.
    //     var topLeft = point + up - right;
    //     var topRight = point + up + right;
    //     var bottomRight = point - up + right;
    //     var bottomLeft = point - up - right;

    //     // Now we've reduced the problem to drawing lines.
    //     Debug.DrawLine(topLeft, topRight, color, duration);
    //     Debug.DrawLine(topRight, bottomRight, color, duration);
    //     Debug.DrawLine(bottomRight, bottomLeft, color, duration);
    //     Debug.DrawLine(bottomLeft, topLeft, color, duration);
    // }
}
