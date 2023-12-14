using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalType
{
    Rabbit,
    Wolf,
    Bear,
    Deer,
}

public class Animal : MonoBehaviour
{
    [SerializeField]
    private AnimalType animalType;

    private void Start()
    {
        SetDefaultCell();
    }

    private void OnMouseDown()
    {
        MovementManager.instance.SelectingAnimal(this.gameObject);
        GameManager.instance.DisableHand();
        CheckFood();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Animal"))
        {
            if (animalType != AnimalType.Wolf)
            {
                return;
            }
            Destroy(collision.gameObject);
            //GameManager.instance.Lose();
        }
    }

    #region Cell
    private void SetDefaultCell()
    {
        Vector3Int currentPos = GridCellManager.instance.GetObjCell(transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(currentPos);
    }
    #endregion

    #region Checking

    public void WolfChecker()
    {
        if(animalType != AnimalType.Wolf)
        {
            return;
        }
        Vector3Int playerPosition = GridCellManager.instance.GetObjCell(this.transform.position);

        Vector3Int[] dir = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
                , Vector3Int.right + Vector3Int.up, Vector3Int.right + Vector3Int.down, Vector3Int.left + Vector3Int.up,Vector3Int.left + Vector3Int.down };

        foreach (Vector3Int direction in dir)
        {
            if (CheckAnimalsCell(playerPosition, direction))
            {
                GameManager.instance.SetLose();
                break;
            }
        }
    }

    private bool CheckAnimalsCell(Vector3Int startPosition, Vector3Int direction)
    {
        Vector3Int next = startPosition + direction;
        while (GridCellManager.instance.IsPlaceableArea(next))
        {
            if (IsFood(next)){
                return false;
            }
            if (IsAnimal(next))
            {
                MovementManager.instance.SelectingAnimal(this.gameObject);
                MovementManager.instance.MoveTo(next);
                return true;
            }
            next += direction;
        }
        return false;
    }

    public bool IsMoveable()
    {
        GridCellManager.instance.ClearHighlightedCells();

        Vector3Int playerPosition = GridCellManager.instance.GetObjCell(this.transform.position);
        CheckCells(playerPosition, Vector3Int.up);
        CheckCells(playerPosition, Vector3Int.down);
        CheckCells(playerPosition, Vector3Int.left);
        CheckCells(playerPosition, Vector3Int.right);
        CheckCells(playerPosition, Vector3Int.right + Vector3Int.up);
        CheckCells(playerPosition, Vector3Int.right + Vector3Int.down);
        CheckCells(playerPosition, Vector3Int.left + Vector3Int.up);
        CheckCells(playerPosition, Vector3Int.left + Vector3Int.down);

        if (GridCellManager.instance.GetHighlightedCells().Count > 0)
        {
            return true;
        }

        return false;
    }

    private void CheckFood()
    {
        GridCellManager.instance.ClearHighlightedCells();

        Vector3Int playerPosition = GridCellManager.instance.GetObjCell(this.transform.position);
        CheckCells(playerPosition, Vector3Int.up);
        CheckCells(playerPosition, Vector3Int.down);
        CheckCells(playerPosition, Vector3Int.left);
        CheckCells(playerPosition, Vector3Int.right);
        CheckCells(playerPosition, Vector3Int.right + Vector3Int.up);
        CheckCells(playerPosition, Vector3Int.right + Vector3Int.down);
        CheckCells(playerPosition, Vector3Int.left + Vector3Int.up);
        CheckCells(playerPosition, Vector3Int.left + Vector3Int.down);
    }

    private void CheckCells(Vector3Int startPosition, Vector3Int direction)
    {
        Vector3Int next = startPosition + direction;
        bool isDetectFood = false;
        while (GridCellManager.instance.IsPlaceableArea(next))
        {
            if (IsAnimal(next))
            {
                break;
            }

            if (IsEtableFood(next))
            {
                isDetectFood = true;
            }
            else if (isDetectFood && !IsFood(next))
            {
                GridCellManager.instance.HighlightCell(next);
            }
            next += direction;
        }
    }

    private bool IsEtableFood(Vector3Int positionToCheck)
    {
        Collider2D isHaveNext = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(positionToCheck), LayerMask.GetMask("Food"));
        if (isHaveNext)
        {
            if (isHaveNext.GetComponent<Food>().GetFoodType() == GetAnimalFood())
            {
                return true;
            }
        } 
        return false;
    }

    private bool IsFood(Vector3Int positionToCheck)
    {
        Collider2D isHaveNext = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(positionToCheck), LayerMask.GetMask("Food"));
        return isHaveNext;
    }

    private bool IsAnimal(Vector3Int positionToCheck)
    {
        Collider2D isHaveNext = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(positionToCheck), LayerMask.GetMask("Animal"));
        return isHaveNext;
    }

    public FoodType GetAnimalFood()
    {
        switch(animalType)
        {
            case AnimalType.Rabbit:
                return FoodType.Carrot;
            case AnimalType.Wolf:
                return FoodType.Meat;
            case AnimalType.Bear:
                return FoodType.Honey;
            case AnimalType.Deer:
                return FoodType.Leaf;
        }
        return FoodType.Carrot;
    }
    #endregion
}
