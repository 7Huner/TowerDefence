using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;
    
    Queue<Node> frontier = new Queue<Node>(); // creates a queue

    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>(); // creates a dictionary for all reached nodes

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down }; // creates a directions array

    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>(); // creates a dictionary for the grid

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
	{
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes(); // reset all nodes
        BreadthFirstSearch(coordinates); // looks for the shortest path to destination
        return BuildPath(); // builds path to destination
    }

    void ExploreNeighbors() // loop through directions array to check if next tiles are walkable
    {
        List<Node> neighbors = new List<Node>();

        foreach(Vector2Int direction in directions) // loop through directions to get neighbors
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;

            if(grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        foreach(Node neighbor in neighbors) // loop through neighbors to check if is walkable
        {
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor); // add coordinate to queue
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear(); // clear previous frontier queue
        reached.Clear(); // clear previous reached dictionary

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]); // add coordinates to queue
        reached.Add(coordinates, grid[coordinates]); // add coordinates to dictionary

        while(frontier.Count > 0 && isRunning) 
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() // build the shortest path to destination then reverse it to get path
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates) // checks if the tower will block the path to destination
	{
		if (grid.ContainsKey(coordinates))
		{
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newPath.Count <= 1)
			{
                GetNewPath();
                return true;
			}
		}

        return false;
	}

    public void NotifyReceivers()
	{
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
	}
}
