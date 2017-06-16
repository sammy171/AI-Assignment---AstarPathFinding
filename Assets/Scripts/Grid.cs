using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    // Public:
    public List<Node> path;
    public LayerMask unwalkableMask;
    public float gridWorldSizeX, gridWorldSizeZ;
    public float nodeRadius;

    // Private:
    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeZ;
    
    // Use this for initialization
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSizeX / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSizeZ / nodeDiameter);
        CreateGrid();
    }
    void CreateGrid()
    {
        // Create our grid 
        grid = new Node[gridSizeX, gridSizeZ];
        // Create world origin point
        Vector3 origin = transform.position - Vector3.right * gridWorldSizeX / 2 - 
                                              Vector3.forward * gridWorldSizeZ / 2;
        // Loop through and generate nodes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                // Positions node based on grid location to the world
                Vector3 position = origin + Vector3.right * (x * nodeDiameter + nodeRadius)
                                          + Vector3.forward * (z * nodeDiameter + nodeRadius);
                // Checks if node is within an obstacle and sets it's walkable
                bool walkable = !(Physics.CheckSphere(position, nodeRadius, unwalkableMask));
                // Create the new node
                grid[x, z] = new Node(walkable, position, x, z);
            }
        }      
    }
    void OnDrawGizmos()
    {
        // Draw wired cube to illustrate where grid will be generated
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSizeX, 1, gridWorldSizeZ));
        // If grid is valid
        if(grid != null)
        {
            // Loop through all nodes in grid
            foreach (Node node in grid)
            {
                // Terniary operator which means (is value true) ? return 1 : return 0 
                Gizmos.color = node.walkable ? Color.white : Color.red;
                if(path != null && path.Contains(node))
                {
                    Gizmos.color = Color.black;
                }


                // Draw a sphere for the node
                Gizmos.DrawCube(node.position, Vector3.one * nodeRadius);
            }
        }

    }

    public Node GetClosestNode(Vector3 worldPosition)
    {
        // Convert world position to grid percent
        float percentX = (worldPosition.x = gridWorldSizeX / 2) / gridWorldSizeX;
        float percentZ = (worldPosition.z = gridWorldSizeZ / 2) / gridWorldSizeZ;
        // Clamp percentage infor 0% to 100%
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        // Trauncate the result to a coordinate
        int x = Mathf.RoundToInt((gridWorldSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridWorldSizeZ - 1) * percentZ);
        // Return that mode
        return grid[x, z];
    }


    public List<Node> GetNeighbours(Node node)
    {
        // Create a list of neightbours
        List<Node> neighbours = new List<Node>();

        // Loop through axis
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                // If x is 0 and z is 0 that means it's the current node
                if (x == 0 && z == 0)
                    continue;

                // Obtain coordinates of next neighbour
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;




                if(checkX >= 0 && checkX < gridSizeX &&
                    checkZ >= 0 && checkZ < gridSizeZ)

                {
                    neighbours.Add(grid[checkX, checkZ]);
                }
            }
        }
        return neighbours;
    }
}
