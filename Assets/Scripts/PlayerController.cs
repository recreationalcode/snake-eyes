using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Grid grid;
    public Vector3Int cellPosition;
    public float moveSpeed = 5f;
    public int numKeys = 0;
    public TextMeshProUGUI keysText;
    public int numRolls = 50;
    public TextMeshProUGUI rollsText;
    public CanvasManager canvasManager;
    public Enemy[] enemies;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = grid.GetCellCenterWorld(cellPosition);
        SetKeyText();
        SetRollsText();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movePosition = grid.GetCellCenterWorld(cellPosition);

        Vector3 oldPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, movePosition, moveSpeed * Time.deltaTime);

        float playerSpeed = ((transform.position - oldPosition) / Time.deltaTime).magnitude;

        animator.SetFloat("playerSpeed", playerSpeed);

        if (!HasRolls() && playerSpeed == 0f)
        {
            canvasManager.GameOver();
        }
    }

    public Vector3Int GetCellPosition()
    {
        return cellPosition;
    }

    public void Move(Vector3Int newCellPosition)
    {
        Vector3 newMovePosition = grid.GetCellCenterWorld(newCellPosition);
            
        cellPosition = newCellPosition;

        foreach(Enemy enemy in enemies)
        {
            enemy.Move();
        }
    }

    public void AddKey()
    {
        numKeys++;
        SetKeyText();
    }

    public void UseKey()
    {
        numKeys--;
        SetKeyText();
    }

    private void SetKeyText()
    {
        keysText.SetText("Keys: " + numKeys);
    }

    public bool HasKeys()
    {
        return numKeys > 0;
    }

    public void AddRoll()
    {
        numRolls++;
        SetRollsText();
    }

    public void UseRoll()
    {
        numRolls--;
        SetRollsText();
    }

    public bool HasRolls()
    {
        return numRolls > 0;
    }

    private void SetRollsText()
    {
        rollsText.SetText("Rolls: " + numRolls);
    }
}
