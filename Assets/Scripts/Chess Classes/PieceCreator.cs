using System.Collections.Generic;
using Chess_Classes.Pieces;
using UnityEngine;
using Chess_Classes.Interfaces;
namespace Chess_Classes {
    public class PieceCreator : MonoBehaviour {
        [SerializeField] GameObject[] piecePrefabs;
        [SerializeField] Material whiteMaterial;
        [SerializeField] Material blackMaterial;
        Dictionary<string, GameObject> pieceNameDictionary = new Dictionary<string, GameObject>();

        void Awake() {
            foreach (GameObject prefab in piecePrefabs){
                pieceNameDictionary.Add(prefab.GetComponent<Piece>().GetType().ToString(), prefab);
            }
        }

        public GameObject CreatePiece(PieceType type) {
            GameObject instance = null;
            if (pieceNameDictionary.TryGetValue(type.ToString(), out instance))
            {
                return GameObject.Instantiate(instance);
            } else
            {
                Debug.Log("Piece Prefab not found");
                return null;
            }
        }

        public Material GetTeamMaterial(TeamColor teamColor) {
            return (teamColor == TeamColor.White)?whiteMaterial:blackMaterial;
        }
    }
}