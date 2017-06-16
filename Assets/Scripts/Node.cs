using UnityEngine;
using System.Collections;


public class Node : MonoBehaviour
{
    public bool     walkable;
    public Vector3  position;

    public Node parent;



    public int gridX;
    public int gridZ;

    public int gCost;
    public int hCost;
    public int fCost
    {
       get
        {
            return gCost + hCost;
        } 
    
    }




    public Node(bool walkable, Vector3 position, int gridX, int gridZ)
    {
        this.walkable = walkable;
        this.position = position;
        this.gridX = gridX;
        this.gridZ = gridZ;
    }
       
}
