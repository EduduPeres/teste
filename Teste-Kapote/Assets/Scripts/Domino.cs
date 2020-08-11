using System.Collections.Generic;
using UnityEngine;
namespace Kapote
{
    public class Domino : MonoBehaviour
    {
        private Collider2D col;
        private readonly string childName = "borda";
        GameObject selected;
        private int val1, val2;

        private Table table;
        private List<Vector3> currentBorderPosition;
        private List<int>   idCanMoveTo; //indices das bordas que eu posso "conectar" com esse peça
        private bool moveAllowed = false;
        private bool canMove = false;
        private bool isOnTable = false;
        void Awake()
        {
            col = GetComponent<Collider2D>();
            table = FindObjectOfType<Table>();
            selected = gameObject.transform.Find(childName).gameObject;
        }

        void Update()
        {
            if (canMove) Move();
            else selected.SetActive(false);
        }
        public bool IsOnTable()
        {
            return isOnTable;
        }
        public void SetVals(int v1, int v2)
        {
            val1 = v1;
            val2 = v2;
        }
        public void SetMove(bool can)
        {
            canMove = can;
            selected.SetActive(true);
        }
        public bool VerifyIfCanMove()
        {
            //posso me mover para alguma das bordas?
            moveAllowed = false;
            int emptyBorder = 0;
            idCanMoveTo = new List<int>();
            currentBorderPosition = table.GetBorderPosition();
            List<int> borderValues = table.GetBorderValues();
            for(int i = 0; i < 4; i++)
            {
                if (borderValues[i] == -1) emptyBorder++;
                if(borderValues[i] == val1 || borderValues[i] == val2)
                {
                    idCanMoveTo.Add(i);
                }
            }
            if (emptyBorder == 4 && val1 == 6 && val2 == 6) return true;
            return idCanMoveTo.Count > 0;
        }
        private void Move()
        {
            if(val1 == 6 && val2 == 6)
            {
                canMove = moveAllowed = false;
                isOnTable = true;
                transform.position = table.GetCenter();
                transform.parent = table.transform;
                table.SetInitialBorder();
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (touch.phase == TouchPhase.Began)
                {
                    Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                    if(col == touchedCollider)  moveAllowed = true;
                }
                if(moveAllowed)
                {
                    float menDist = 1000, distanceToId;
                    int idMoveTo = -1;
                    for (int i = 0; i < idCanMoveTo.Count; i++)
                    {
                        distanceToId = (currentBorderPosition[idCanMoveTo[i]] - transform.position).magnitude;
                        if (distanceToId < menDist)
                        {
                            menDist = distanceToId;
                            idMoveTo = idCanMoveTo[i];
                        }
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        transform.position = new Vector2(touchPosition.x, touchPosition.y);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        canMove = moveAllowed = false;
                        isOnTable = true;
                        transform.position = currentBorderPosition[idMoveTo];
                        transform.parent = table.transform;

                        table.SetBorder(idMoveTo, 0f, val1 == table.GetBorderValues()[idMoveTo] ? val2 : val1);
                    }
                }
            }
        }
    }
}