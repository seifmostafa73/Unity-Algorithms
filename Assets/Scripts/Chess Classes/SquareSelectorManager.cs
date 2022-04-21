using System.Collections.Generic;
using UnityEngine;

namespace Chess_Classes {
    public class SquareSelectorManager : MonoBehaviour {
        [SerializeField]
        Color freeSquareColor = new Color();
        [SerializeField]
        Color opponentSquareColor = new Color();
        [SerializeField]
        GameObject squareHighlighterPrefab = null;
        List<GameObject> highlightedSquares = new List<GameObject>();

        public void ShowSelection (Dictionary<Vector3,bool> squareData)
        {
            ClearSelection();
            foreach (var square in squareData)
            {
                GameObject newHighlighter = GameObject.Instantiate(squareHighlighterPrefab,square.Key,Quaternion.identity);
                newHighlighter.GetComponent<MeshRenderer>().material.color = (square.Value == true)? freeSquareColor: opponentSquareColor;
                highlightedSquares.Add(newHighlighter);
            }
        }
        public void ClearSelection()
        {
            if(highlightedSquares.Count == 0) return;
            Debug.Log(highlightedSquares.Count == 0);
            foreach (var square in highlightedSquares)
            {
                GameObject.Destroy(square);
            }
            highlightedSquares.Clear();
        }

    }
}
