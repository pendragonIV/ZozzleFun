using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Carrot,
    Meat,
    Honey,
    Leaf,
}

public class Food : MonoBehaviour
{
    [SerializeField]
    private FoodType foodType;

    private void Start()
    {
        SetDefaultCell();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Animal"))
        {
            Animal animal = collision.GetComponentInParent<Animal>();
            if (animal.GetAnimalFood() == foodType)
            {
                Destroy(this.gameObject);
            }
        }
    }

    #region Cell
    private void SetDefaultCell()
    {
        Vector3Int currentPos = GridCellManager.instance.GetObjCell(transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(currentPos);
    }
    #endregion

    #region GetSet
    public FoodType GetFoodType()
    {
        return foodType;
    }
    #endregion
}
