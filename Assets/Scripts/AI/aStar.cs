using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class aStar : MonoBehaviour {


    public int width, height;
    public float size;

    [HideInInspector]
    private Grid<CustomGridObject> grid;

    public TextMesh[,] debugText;
    // this is cached because the calc is expensive
    private bool[,] cachedWalkable;
    private int textSize = 80;

    private void Start() 
    {
        Vector3 gridCenter = gameObject.transform.position;
        gridCenter.x -= (size*width)/2;
        gridCenter.z -= (size*height)/2;
        grid = new Grid<CustomGridObject>(width, height, size, gridCenter, (Grid<CustomGridObject> g, int x, int z) => new CustomGridObject(g, x, z));

        debugText = new TextMesh[width, height];
        cachedWalkable = new bool[width, height];

        setWalkable ();
    }

    //public Vector3[] getPath (Vector3 start, Vector3 end)
    public List<Vector3> getPath (Vector3 start, Vector3 end)
    {
        // get start grid location
        int start_x, start_z;
        if (grid == null) { return null; }
        
        grid.GetXZ(start, out start_x, out start_z);
        // debugText[start_x, start_z].color = Color.blue;
        // debugText[start_x, start_z].text = "*";

        // get end (dest) grid location
        int end_x, end_z;
        grid.GetXZ(end, out end_x, out end_z);
        // debugText[end_x, end_z].color = Color.white;
        // debugText[end_x, end_z].text = "*";

        // a* implementation
        List<Node> open_list = new List<Node>();
        List<Node> closed_list = new List<Node>();

        Node startNode = new Node(start_x, start_z, 0);
        Node endNode = new Node(end_x, end_z, 0);

        open_list.Add(startNode);

        //startNode.h = heuristic(startNode, endNode);

        while (open_list.Count > 0)
        {
            Node cur_node = null;
            int smallestF = 9999;
            foreach (Node n in open_list)
            {
                if (n.getF() < smallestF)
                {
                    smallestF = n.getF();
                    cur_node = n;
                }
            }
            open_list.Remove(cur_node);

            if (cur_node == null) { return null; }
            
            // if we are at the goal node break;
            if (cur_node.x == endNode.x && cur_node.z == endNode.z)
            {
                return reconstructPath (cur_node);
            }
            // successor array
            List<Node> successors = getSucessors (cur_node);
            foreach (Node n in successors)
            {
                // ensure the tile is walkable
                if (!cachedWalkable[n.x, n.z])
                {
                    // if start or end are not walkable continue anyways
                    if ((startNode.x != n.x || startNode.z != n.z) && 
                        (endNode.x != n.x || endNode.z != n.z))
                    {
                        continue;
                    }
                }

                n.h = heuristic(n, endNode);
                n.g = cur_node.g + n.h;
                n.parent = cur_node;

               

                // check if node is already in open list and has better score
                Node t = open_list.Find(s => s.x == n.x && s.z == n.z);
                if (t != null && t.getF() < n.getF())
                {
                    continue;
                }

                // check if node is already in closed list and has better score
                Node t2 = closed_list.Find(s => s.x == n.x && s.z == n.z);
                if (t2 != null && t2.getF() < n.getF())
                {
                    continue;
                }
                
                open_list.Add (n);
            }

            closed_list.Add(cur_node);
        }

        return null;
    }

    private int heuristic (Node start, Node end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.z - end.z);
    }

    private List<Node> getSucessors (Node n)
    {
        List<Node> successors = new List<Node> ();

        for (int x_off = -1; x_off <= 1; x_off++)
        {
            for (int z_off = -1; z_off <= 1; z_off++)
            {
                if (x_off == 0 && z_off == 0) { continue; }
                //if (Mathf.Abs(x_off) == 1 && Mathf.Abs(z_off) == 1) { continue; }

                // we must not go past final blocks
                if (x_off + n.x > width-1 || z_off + n.z > height-1) { continue; }
                // or past the start (before 0,0)
                if (x_off + n.x < 0 || z_off + n.z < 0) { continue; }

                successors.Add (new Node (n.x+x_off, n.z+z_off));
            }
        }

        return successors;
    }

    private List<Vector3> reconstructPath (Node cur_node)
    {
        List<Vector3> path = new List<Vector3> ();
        Node prev = cur_node.parent;
        while (prev != null)
        {
            path.Insert(0, cur_node.toVector());
            cur_node = cur_node.parent;
            prev = cur_node.parent;
        }

        return path;
    }

    public List<Vector3> gridListToWorldSpace (List<Vector3> node_list)
    {
        List<Vector3> worldSpacePath = new List<Vector3> ();
        foreach (Vector3 n in node_list)
        {
            Vector3 world = grid.GetWorldPosition ((int)n.x, (int)n.z);
            world.x += size/2;
            world.z += size/2;
            worldSpacePath.Add (world);
        }

        return worldSpacePath;
    }

    public void setWalkable ()
    {
        LayerMask mask = LayerMask.GetMask("Walls");
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 centerWorldPos = grid.GetWorldPosition(x,z) + new Vector3 (size, 0, size)*0.5f;
                centerWorldPos.y = transform.position.y;

                Vector3 halfSize = new Vector3 (size/2, size/2, size/2);
                Collider[] collisions = Physics.OverlapBox (centerWorldPos, halfSize, Quaternion.identity, mask.value);

                if (collisions.Length > 0)
                {
                    CustomGridObject gridObject = grid.GetGridObject(x, z);
                    gridObject.setWalkable (false);
                    //debugText[x,z] = UtilsClass.CreateWorldText ("*", null, centerWorldPos, textSize, Color.red, TextAnchor.MiddleCenter);
                    cachedWalkable[x,z] = false;
                } else {
                    //debugText[x,z] = UtilsClass.CreateWorldText ("*", null, centerWorldPos, textSize, Color.green, TextAnchor.MiddleCenter);
                    cachedWalkable[x,z] = true;
                }

            }
        }
    }

    public void refreshDebugUI ()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                TextMesh t = debugText[x,z];
                t.fontSize = textSize;
                if (cachedWalkable[x,z])
                {
                    //t.text = x+"-"+z+"\nTrue";
                    t.text = "*";
                    t.color = Color.green;
                } else {
                    //t.text = x+"-"+z+"\nFalse";
                    t.text = "*";
                    t.color = Color.red;
                }
            }
        }
    }
}



public class CustomGridObject 
{
    private Grid<CustomGridObject> grid;
    private int x;
    private int z;
    private bool walkable;

    public CustomGridObject(Grid<CustomGridObject> grid, int x, int z) 
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        walkable = true;
    }

    public void setWalkable(bool b)
    {
        walkable = b;
    }
}




public class HeatMapGridObject {

    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue) {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized() {
        return (float)value / MAX;
    }

    public override string ToString() {
        return value.ToString();
    }

}

public class StringGridObject {
    
    private Grid<StringGridObject> grid;
    private int x;
    private int y;

    private string letters;
    private string numbers;
    
    public StringGridObject(Grid<StringGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
        numbers = "";
    }

    public void AddLetter(string letter) {
        letters += letter;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void AddNumber(string number) {
        numbers += number;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return letters + "\n" + numbers;
    }

}

public class Node 
{
    public int x, z, g, h;
    public Node parent;
    private int f = 0;
    public Node(int x, int z, int h=0, int g=0)
    {
        this.x = x;
        this.z = z;
        this.h = h;
        this.g = g;
        this.f = g + h;
        this.parent = null;
    }

    public string toString()
    {
        string str = this.x + "-" + this.z;
        str += "\tf: " + this.f + " ";
        str += "\tg: " + this.g + " ";
        str += "\th: " + this.h + " ";

        return str;
    }

    public Vector3 toVector()
    {
        return new Vector3 (this.x, 0, this.z);
    }

    public int getF() 
    { 
        this.f = this.g + this.h;
        return this.f; 
    }

}
