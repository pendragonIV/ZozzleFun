using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    private Transform foodContainer;

    public static FoodManager instance;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void CheckWin()
    {
        if (foodContainer.childCount == 0)
        {
            GameManager.instance.Win();
        }
    }
}
