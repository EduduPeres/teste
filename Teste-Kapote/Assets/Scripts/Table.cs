using System.Collections.Generic;
using UnityEngine;
namespace Kapote
{
    public class Table : MonoBehaviour
    {
        private Vector3 center;
        private List<Vector3> borderPosition;
        private List<int> borderValues;
        Vector2 dominoSize;
        // Start is called before the first frame update
        void Awake()
        {
            borderPosition = new List<Vector3>();
            borderValues = new List<int>();

            center = new Vector3(0, 0, 0);
            for (int i = 0; i < 4; i++)
            {
                borderValues.Add(-1);
            }
        }

        public List<Vector3> GetBorderPosition()
        {
            return borderPosition;
        }
        public Vector3 GetCenter()
        {
            return center;
        }
        public List<int> GetBorderValues()
        {
            return borderValues;
        }
        public void SetBorder(int i, float rotate, int val)
        {
            
            borderValues[i] = val;
            switch (i)
            {
                case 0:
                    borderPosition[0] -= new Vector3(dominoSize.x, 0, 0);
                    break;
                case 1:
                    borderPosition[1] += new Vector3(0, dominoSize.y, 0);
                    break;
                case 2:
                    borderPosition[2] += new Vector3(dominoSize.x, 0, 0);
                    break;
                default:
                    borderPosition[3] -= new Vector3(0, dominoSize.y, 0);
                    break;
            }
        }
        public void SetInitialBorder()
        {
            dominoSize = FindObjectOfType<Constants>().GetDominoSize();
            borderPosition.Add(center - new Vector3(dominoSize.x, 0, 0));   //0//          1
            borderPosition.Add(center + new Vector3(0, dominoSize.y, 0));   //1//   0    Center    2
            borderPosition.Add(center + new Vector3(dominoSize.x, 0, 0));   //2//          3
            borderPosition.Add(center - new Vector3(0, dominoSize.y, 0));   //3//
            for (int i = 0; i < 4; i++) borderValues[i] = 6;
        }
    }
}