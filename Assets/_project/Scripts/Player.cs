using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action EndOfMazeReached;

    [SerializeField] float _speed = 5f;

    Stack<Vector3> _pathThroughMaze;
    bool _reachedEnd;
    Vector3 _destination;

    public void Init(Stack<Vector3> path)
    {
        _pathThroughMaze = path;
        _reachedEnd = false;
        _destination = path.Pop();
    }

    private void Update()
    {
        if (_reachedEnd) return;
        float distance = Vector3.Distance(transform.position, _destination);
        if (Mathf.Approximately(distance, 0f))
        {
            if (!_pathThroughMaze.TryPop(out _destination)) ReachedEnd();
            return;
        }
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _destination, step);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReachedEnd();
    }

    private void ReachedEnd()
    {
        _reachedEnd = true;
        EndOfMazeReached?.Invoke();
    }
}
