using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementManager : MonoBehaviour
{
    public static MovementManager instance;

    [SerializeField]
    private Transform animalsContainer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    [SerializeField]
    private GameObject selectedObj;

    private void Update()
    {
        if (selectedObj)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int mouseDownPos = GridCellManager.instance.GetObjCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                MoveAnimal(mouseDownPos);
            }
        }
    }

    public void SelectingAnimal(GameObject animal)
    {
        this.selectedObj = animal;
    }

    public void MoveAnimal(Vector3Int targetCell)
    {
        if (selectedObj == null)
        {
            return;
        }

        Vector3Int currentCell = GridCellManager.instance.GetObjCell(selectedObj.transform.position);
        if (GridCellManager.instance.IsPlaceableArea(targetCell) && GridCellManager.instance.IsPlaceableArea(currentCell))
        {
            if (GridCellManager.instance.IsMoveableArea(targetCell))
            {
                Collider2D collider2D = selectedObj.GetComponent<Collider2D>();
                collider2D.enabled = false;
                bool isAllCantMove = false;
                GameManager.instance.DisableHand2();
                selectedObj.transform.DOMove(GridCellManager.instance.PositonToMove(targetCell), 0.5f).OnComplete(
                    () =>
                    {
                        collider2D.enabled = true;
                        foreach (Transform animal in animalsContainer)
                        {
                            animal.GetComponent<Animal>().WolfChecker();
                            if (animal.GetComponent<Animal>().IsMoveable())
                            {
                                isAllCantMove = true;
                            }
                        }
                        if (!isAllCantMove)
                        {
                            FoodManager.instance.CheckWin();
                            if(!GameManager.instance.IsGameLose() && !GameManager.instance.IsGameWin())
                            {
                                GameManager.instance.Lose();
                            }
                        }
                    });
                selectedObj = null;
                GridCellManager.instance.ClearHighlightedCells();
                if (!GameManager.instance.IsGameLose())
                {
                    FoodManager.instance.CheckWin();    
                }
            }
        }
    }

    public void MoveTo(Vector3Int targetCell)
    {
        selectedObj.transform.DOMove(GridCellManager.instance.PositonToMove(targetCell), 0.5f).SetUpdate(true);
        selectedObj = null;
        GameManager.instance.Lose();
    }
}
