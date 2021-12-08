using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper : MonoBehaviour
{
    public static int width = 10, height = 20+5;
    public static Transform[,] grid = new Transform[width, height];
    public static int score_game = (int) 0;

    public static Vector2 RoundVector(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool IsInsideBorders(Vector2 pos)
    {
        if (pos.x >= 0 && pos.y>=0 && pos.x < width)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public static void DeleteRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void DecreaseRow(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if(grid[x,y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                //Repintamos el bloque, una posicion mas abajo en la pantalla.
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }


    public static void DecreaseRowsAbove(int y)
    {
        for(int i = y; i < height; i++)
        {
            DecreaseRow(i);
        }
    }


    // Si la fila esta llena
    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if(grid[x,y] == null)
            {
                return false;
            }
        }
        
        return true;
    }

    public static int SumaScore()
    {
        return score_game;
    }

    // Reset Game
    public static void DeleteAllFullRows()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                y--;
                score_game += 100;
            }
        }

        CleanPieces();
    }

    private static void CleanPieces()
    {
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            if (piece.transform.childCount == 0)
            {
                Destroy(piece);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
