using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kapote
{
    public class Player : MonoBehaviour
    {
        private List<Domino> dominos;
        [SerializeField] GameObject dominoModel;
        int dominosCanMove = 0;
        Domino remove;
        private bool canPlay;
        private void Awake()
        {
            dominos = new List<Domino>();
        }

        private void Update()
        {
            if (!canPlay) return;
            foreach(Domino domino in dominos)
            {
                if (domino.IsOnTable())
                {
                    remove = domino;
                    canPlay = false;
                    break;
                }
            }
            if (!canPlay)
            {
                dominos.Remove(remove);
                gameObject.SetActive(false);
            }
        }

        public void AddDomino(int domV1, int domV2)//Domino Values
        {
            GameObject dom = Instantiate(dominoModel);
            dom.GetComponent<SpriteRenderer>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/DominoTiles/Dom-" + domV1 + "-" + domV2 + ".png", typeof(Sprite));

            dom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            Vector3 domPos = new Vector3(0, -4.29f, 0f);
            if (dominos.Count == 0) domPos.x = -2.81f;
            else domPos.x = dominos[dominos.Count - 1].transform.position.x + 0.7f;
            dom.transform.position = domPos;

            dom.name = "Dom-" + domV1 + "-" + domV2;
            dom.transform.SetParent(transform);

            Domino domino = dom.GetComponent<Domino>();
            domino.SetVals(domV1, domV2);
            dominos.Add(domino);
        }

        public void MyTurn()
        {
            dominosCanMove = 0;
            canPlay = true;
            gameObject.SetActive(true);
            foreach (Domino domino in dominos)
            {
                if (domino.VerifyIfCanMove())
                {
                    domino.SetMove(true);
                    dominosCanMove++;
                }
                else
                {
                    domino.SetMove(false);
                }
            }
            if (dominosCanMove == 0)
            {
                canPlay = false;
                gameObject.SetActive(false);
            }
        }
        public int AlreadyPlayed()
        {
            if (!canPlay)
            {
                if (dominosCanMove == 0)
                {
                    return 2; // passou a vez
                
                }
                    return 1; //jogou uma peça
            }
            return 0; //esperando jogar peça
        }
    }
}