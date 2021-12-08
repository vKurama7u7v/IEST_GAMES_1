using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    float lastFall = 0.0f;
    float dificultad = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        dificultad = 1.0f;

        if (!IsValidPiecePosition())
        {
            Debug.Log("Game Over");
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // MOVER IZQUIERDA
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePieceHorizontally(-1);
        }

        // MOVER DERECHA
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePieceHorizontally(1);
        }

        // ROTAR LA PIEZA
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.Rotate(0, 0, -90);

            if (IsValidPiecePosition())
            {
                UpdateGrid();
            }
            else
            {
                this.transform.Rotate(0, 0, 90);
            }
        }

        // MOVER ABAJO
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Time.time-lastFall) > dificultad)
        {
            

            this.transform.position += new Vector3(0, -1, 0);
            if (IsValidPiecePosition())
            {
                UpdateGrid();
            }
            else
            {
                this.transform.position += new Vector3(0, 1, 0);

                // Como la pieza no puede bajar mas, a lo mejor ha llegado el momento de eliminar filas
                GridHelper.DeleteAllFullRows();

                // Spawnear la Siguiente Pieza
                FindObjectOfType<PieceSpawner>().SpawnNextPiece();

                // Deshabilitamos el Script para que esta pieza no a moverse
                this.enabled = false;

            }

            lastFall = Time.time;
        }
    }


    // Movimiento en Horizontal
    void MovePieceHorizontally(int direction)
    {
        // Muevo la pieza a los laterales
        this.transform.position += new Vector3(direction, 0, 0);

        //Comprobaremos si la nueva posicion es valida
        if (IsValidPiecePosition())
        {
            // Persisto la informacion del movimiendo del tablero del helper
            UpdateGrid();
        }
        else
        {
            // Revierto el Movimiento
            this.transform.position += new Vector3(-direction, 0, 0);
        }
    }


    // Comprueba si la posicion de la pieza es valida o no
    bool IsValidPiecePosition()
    {
        foreach (Transform block in this.transform)
        {
            // Posicion de cada uno de los hijos de la pieza
            Vector2 pos = GridHelper.RoundVector(block.position);

            // Si la posicion esta fuera de los limites, entonces no es una posicion valida.
            if (!GridHelper.IsInsideBorders(pos))
            {
                return false;
            }

            // Si ya hay otro bloque en esa posicion, entonces no es una posicion valida
            Transform possibleObject = GridHelper.grid[(int)pos.x, (int)pos.y];
            if (possibleObject != null && possibleObject.parent != this.transform)
            {
                return false;
            }
        }

        return true;
    }

    void UpdateGrid()
    {
        for(int y = 0; y < GridHelper.height; y++)
        {
            for(int x = 0; x < GridHelper.width; x++)
            {
                if(GridHelper.grid[x,y] != null)
                {

                    // El padre del bloque es la pieza del propio script
                    if(GridHelper.grid[x,y].parent == this.transform)
                    {
                        GridHelper.grid[x, y] = null;
                    }
                }
            }
        }


        foreach(Transform block in this.transform)
        {
            // Redondear Posiciones
            Vector2 pos = GridHelper.RoundVector(block.position);

            // Guardo hijo en dicha posicion
            GridHelper.grid[(int)pos.x, (int)pos.y] = block;
        }
    }
}
