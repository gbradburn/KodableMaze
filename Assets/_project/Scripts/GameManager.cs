using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Maze _maze;
    [SerializeField] Player _player;
    [SerializeField] Button _solveButton;
    [SerializeField] GameObject _solvedPanel;
    [SerializeField] GameObject _errorPanel;

    private void OnEnable()
    {
        _player.gameObject.SetActive(false);
        _player.transform.position = Vector3.zero;
        _solvedPanel.SetActive(false);
        _errorPanel.SetActive(false);
    }
    public void SolveMaze()
    {
        _solveButton.enabled = false;
        MazeSolver solver = new MazeSolver(_maze);
        List<Vector2Int> solution = solver.SolveMaze();
        if (solution == null)
        {
            DisplayErrorPanel();
            return;
        }
        AnimatePlayer(solution);
    }

    private void AnimatePlayer(List<Vector2Int> solution)
    {
        _player.transform.position = _maze.StartTile.transform.position;
        _player.Init(_maze.BuildPathForSolution(solution));
        _player.EndOfMazeReached += Player_EndOfMazeReached;
        _player.gameObject.SetActive(true);
    }

    private void DisplayErrorPanel()
    {
        _errorPanel.SetActive(true);
    }

    private void Player_EndOfMazeReached()
    {
        _player.EndOfMazeReached -= Player_EndOfMazeReached;
        _solvedPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainScene");
    }
}
