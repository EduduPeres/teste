using System.Collections;
using UnityEngine;
namespace Kapote
{
    public class Game : MonoBehaviour
    {
        private Player[] players;
        private int currentTurn = -1;
        private int playersPassed = 0;
        private bool waitingForPassTurn;
        void Start()
        {
            players = FindObjectsOfType<Player>();
            ShuffleAndDistributeDominoes();
            StartRound();
        }

        void Update()
        {
            if (currentTurn == -1) return;
            int retVal = players[currentTurn].AlreadyPlayed();
            if(retVal > 0 && !waitingForPassTurn)
            {
                waitingForPassTurn = true;
                if (retVal == 2)
                {
                    playersPassed++;
                    StartCoroutine(NextTurn(2));
                }
                else
                {
                    playersPassed = 0;
                    StartCoroutine(NextTurn(0));
                }
            }
            
        }
        private void ShuffleAndDistributeDominoes()
        {
            int[] playersDominosCount = new int[4];
            int id;
            for (int i = 0; i < 7; i++)
            {
                for (int j = i; j < 7; j++)
                {
                    id = Random.Range(0, players.Length);
                    while (playersDominosCount[id] == 28 / players.Length) id = (id + 1) % players.Length;
                    playersDominosCount[id]++;
                    players[id].AddDomino(i, j);
                    if (i == 6 && j == 6) currentTurn = id; //Sena, o player "id" começa a jogar
                }
            }
        }
        private void StartRound()
        {
            //Assumindo que todas as pedras foram distribuídas, e não há "dorme", logo, algum jogador possui a Sena
            //Modificar se necessário
            Debug.Log(currentTurn + " " + players[currentTurn].name);
            for (int i = 0; i < players.Length; i++)
            {
                if (i == currentTurn)
                {
                    players[i].MyTurn();
                }
                else players[i].gameObject.SetActive(false);
            }
        }

        IEnumerator NextTurn(float time)
        {
            yield return new WaitForSeconds(time);
            currentTurn = (currentTurn + 1) % players.Length;
            players[currentTurn].MyTurn();
            waitingForPassTurn = false;
        }
    }
}