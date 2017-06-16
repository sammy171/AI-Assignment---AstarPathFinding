using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Grid))]
public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;


    private Grid grid;
    // Use this for initialization
    void Awake()
    {
        grid = GetComponent<Grid>();
            
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.GetClosestNode(startPos);
        Node targetNode = grid.GetClosestNode(targetPos);
        // OPEN list
        List<Node> openList = new List<Node>();
        // CLOSED list
        List<Node> closedList = new List<Node>();
        // Add the start node to OPEN
        openList.Add(startNode);
        // Loop - If the open list is empty, the target was NOT found (worst case)
        while (openList.Count > 0) 
        {
            // Set a currentNode to first one in open list
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                // Check if fCost is lower than currentNode fCost
                if (openList[i].fCost < currentNode.fCost ||
                    // If the fCost iw the same as currentNode fCost
                    openList[i].fCost == currentNode.fCost &&
                    // If the hCost is less than the currentNode hCost
                    openList[i].hCost < currentNode.hCost)
                {
                    // let the currentNode to this node
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Check if current is the target node
            if (currentNode == targetNode) 
            {
                // Found our path
                RetracePath(startNode, targetNode);
                return;
            }

            // Loop through each of the currentNode
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                // Check if neighbour is NOT walkable OR if neighbour is CLOSED
                if (neighbour.walkable || closedList.Contains(neighbour))
                {
                    // Skip to next neighbour
                    continue;

                }
                // Create a new cost for the node
                int newHCost = GetDistance(currentNode, neighbour);
                // Calculate new fCost
                int newCost = currentNode.gCost = newHCost;
                // Check if neighbour is shorter OR neighbour is not OPEN
                if (newCost < neighbour.gCost || !openList.Contains(neighbour))
                    {
                    // Setting is fCost for neighbour
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    // Check if neighbour is NOT in the open list
                    if(openList.Contains(neighbour))
                    {
                        // Add neighbour to open list
                        openList.Add(neighbour);
                    }



                }



            }


        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        // Path list
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // Trace backwards beacuse of parent
        while (currentNode != startNode)
        {
            // Add each of the current nodes
            path.Add(currentNode);
            // Traverse
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
        
     }

    int GetDistance(Node nodeA, Node nodeB)
    {
        // Get DistanceX & DistanceZ
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);
        // Check if DistX is greater than DistZ
        if (distX > distZ)
        {
            // Generate heuristic
            return 14 * distZ + 10 * (distX - distZ);
        }
        // Generate heuristic
        return 14 * distX + 10 * (distZ - distX);
    }

}
