using UnityEngine;

namespace Kapote
{
    public class Constants : MonoBehaviour
    {
        [SerializeField]private GameObject dominoModel;

        private Vector2 dominoSize;

        private void Awake()
        {
            dominoSize = dominoModel.GetComponent<Collider2D>().bounds.size;
        }

        public Vector2 GetDominoSize()
        {
            return dominoSize;
        }
        public void SetDominoSize(Vector2 size)
        {
            dominoSize = size;
        }

    }
}