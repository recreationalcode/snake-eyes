using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Grid grid;
    public Vector3Int cellPosition;
    public float moveSpeed = 5f;
    public RollPreview rollPreview;
    public CanvasManager canvasManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = grid.GetCellCenterWorld(cellPosition);        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movePosition = grid.GetCellCenterWorld(cellPosition);

        transform.position = Vector3.MoveTowards(transform.position, movePosition, moveSpeed * Time.deltaTime);        
    }

    private Vector3Int[] directions = new[] { Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down };

    public void Move()
    {
        StartCoroutine(_Move());
    }

    IEnumerator _Move()
    {
        yield return new WaitForSeconds(0.25f);

        int roll = Random.Range(1, 5);

        Vector3Int newCellPosition = Vector3Int.zero;

        while (newCellPosition == Vector3Int.zero)
        {
            Vector3Int direction = directions[Random.Range(0, directions.Length)];

            newCellPosition = rollPreview.GetRollCellPosition(cellPosition, roll, direction);
        }

        cellPosition = newCellPosition;
    }

    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(col.gameObject);

            canvasManager.GameOver();
        }
    }
}
