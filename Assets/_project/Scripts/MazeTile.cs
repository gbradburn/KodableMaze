using UnityEngine;

public class MazeTile : MonoBehaviour
{
    public enum TileType : int
    {
        Start = 0,
        Wall = 1,
        WallCorner = 2,
        Floor = 3,
        Finish = 4
    }

    [SerializeField] TileType _tileType = TileType.Floor;

    public bool IsWall
    {
        get
        {
            if (_tileType == TileType.Wall || _tileType == TileType.WallCorner)
            {
                return true;
            }
            return false;
        }
    }
    public bool FinishLine => _tileType == TileType.Finish;


}
