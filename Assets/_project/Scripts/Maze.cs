using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MazeTile;

public class Maze : MonoBehaviour
{
    [SerializeField] List<TextAsset> _asciiMazes;
    [SerializeField] List<GameObject> _mapTilePrefabs;
    [SerializeField] Vector3Int _upperLeftCorner = new Vector3Int(-11, -5, 1);

    public int MazeWidth { get; private set; }
    public int MazeHeight { get; private set; }
    public Vector2Int StartPosition => new Vector2Int(0, 1);
    public Vector2Int EndPosition { get; private set; }
    public MazeTile TileAt(Vector2Int pos)
    {
        return _mazeTiles[pos.x][pos.y];
    }
    public MazeTile StartTile => TileAt(StartPosition);

    private List<List<MazeTile>> _mazeTiles;
    private static int _mazeIndex = 0;

    private void Awake()
    {
        LoadNextMaze();
        _mazeIndex++;
        if (_mazeIndex >= _asciiMazes.Count)
        {
            _mazeIndex = 0;
        }
    }

    private void LoadNextMaze()
    {
        StringReader reader = new StringReader(_asciiMazes[_mazeIndex].text);
        List<List<MazeTile.TileType>> tiles = new List<List<MazeTile.TileType>>();
        string row = reader.ReadLine();
        int y = 0;
        bool doubleSpace = false;
        while (row != null)
        {
            List<MazeTile.TileType> tileRow = new List<MazeTile.TileType>();
            for (int x = 0; x < row.Length; ++x) 
            {
                TileType tileType = GetTileType(x, y, row, ref doubleSpace);
                tileRow.Add(tileType);
                if (doubleSpace) ++x;
            }
            tiles.Add(tileRow);
            row = reader.ReadLine();
            ++y;
        }
        MazeHeight = tiles.Count;
        MazeWidth = tiles[0].Count;
        EndPosition = new Vector2Int(MazeWidth - 1, MazeHeight - 2);
        tiles[MazeHeight - 2][MazeWidth - 1] = MazeTile.TileType.Finish;
        _mazeTiles = new List<List<MazeTile>>();
        for (int x = 0; x < MazeWidth; ++x)
        {
            List<MazeTile> mazeRow = new List<MazeTile>();
            for (y = 0; y < MazeHeight; ++y)
            {
                MazeTile mazeTile = Instantiate(GetMapTilePrefab(tiles[y][x]), this.transform).GetComponent<MazeTile>();
                mazeTile.transform.position = new Vector3(_upperLeftCorner.x + x, _upperLeftCorner.y - y, 1);
                mazeRow.Add(mazeTile);
            }
            _mazeTiles.Add(mazeRow);
        }
    }

    private TileType GetTileType(int x, int y, string row, ref bool doubleSpace)
    {
        if (x == 0 && y == 1)
        {
            doubleSpace = false;
            return MazeTile.TileType.Start;
        }

        // First check for double space cells
        if (!doubleSpace && x < row.Length - 1)
        {
            string s = row.Substring(x, 2);
            if (s == "  ")
            {
                doubleSpace = true;
                return TileType.Floor;
            }
            else if (s == "--")
            {
                doubleSpace = true;
                return TileType.Wall;
            }
        }
        doubleSpace = false;
        return row[x] switch
        {
            ' ' => TileType.Floor,
            '+' => TileType.WallCorner,
            _ => TileType.Wall
        };
    }

    GameObject GetMapTilePrefab(MazeTile.TileType tileType)
    {
        return _mapTilePrefabs[(int)tileType];
    }

    public Stack<Vector3> BuildPathForSolution(List<Vector2Int> solution)
    {
        Stack<Vector3> path = new Stack<Vector3>();

        foreach(Vector2Int pos in solution)
        {
            path.Push(TileAt(pos).transform.position);
        }
        return path;
    }
}
