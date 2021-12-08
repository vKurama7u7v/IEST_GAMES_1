using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{

    public GameObject[] levelPieces;
    public GameObject currentPiece, nextPiece;

    private void Start()
    {
        // Aleatoriamente decido que pieza del Arreglo debe de Spawnearse
        int i = Random.Range(0, levelPieces.Length);

        // Instanciar Primera Pieza
        nextPiece = Instantiate(levelPieces[0], this.transform.position, Quaternion.identity);
        SpawnNextPiece();
    }

    public void SpawnNextPiece()
    {
        currentPiece = nextPiece;
        currentPiece.GetComponent<Piece>().enabled = true;
        nextPiece.transform.position = new Vector3(4, 21, 0);

        StartCoroutine("PrepareNextPiece");
    }

    IEnumerator PrepareNextPiece()
    {
        yield return new WaitForSecondsRealtime(0.0f);

        // Aleatoriamente decido que pieza del Arreglo debe de Spawnearse
        int i = Random.Range(0, levelPieces.Length);

        // Instanciar Pieza Siguiente
        nextPiece = Instantiate(levelPieces[i], this.transform.position, Quaternion.identity);
        nextPiece.GetComponent<Piece>().enabled = false;
        nextPiece.transform.position = new Vector3(14f, 15.5f, 0);
    }
}