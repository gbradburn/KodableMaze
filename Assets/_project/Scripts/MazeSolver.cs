using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeSolver 
{
    Maze _maze;
    List<Vector2Int> _traversedTiles;
    List<Vector2Int> _correctPath;

    public MazeSolver(Maze maze)
    {
        _maze = maze;
    }

    public List<Vector2Int> SolveMaze()
    {
        _traversedTiles = new List<Vector2Int>();
        _correctPath = new List<Vector2Int>();

        if (SolveRecursively(_maze.StartPosition))
        {
            return _correctPath;
        }

        return null;
    }

    private bool SolveRecursively(Vector2Int position)
    {
        if (position == _maze.EndPosition)
        {
            return true;
        }
        MazeTile tile = _maze.TileAt(position);
        if (tile.IsWall || BeenHere(position))
        {
            return false;
        }

        // Mark that we've been here
        _traversedTiles.Add(position);

        if ((position.x > 0) && SolveRecursively(new Vector2Int(position.x - 1, position.y)))
        { 
            _correctPath.Add(position);
            return true;
        }
        if (position.x < _maze.MazeWidth - 1)
        {
            if (SolveRecursively(new Vector2Int(position.x + 1, position.y)))
            {
                _correctPath.Add(position);
                return true;
            }
        }
        if ((position.y > 0) && SolveRecursively(new Vector2Int(position.x, position.y - 1)))
        {
                _correctPath.Add(position);
                return true;
        }
        if (position.y < _maze.MazeHeight - 1)
        {
            if (SolveRecursively(new Vector2Int(position.x, position.y + 1)))
            {
                _correctPath.Add(position);
                return true;
            }
        }
        return false;
    }

    private bool BeenHere(Vector2Int position)
    {
        return _traversedTiles.Any(p => p.x == position.x && p.y == position.y);
    }
}
