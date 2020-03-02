using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    void Start()
    {

    }
    public static List<GameObject> FindPath(GameObject targetLocation, GameObject startingPosition)
    {
        List<GameObject> openlist = new List<GameObject>();
        List<GameObject> closedlist = new List<GameObject>();
        List<GameObject> path = new List<GameObject>();
        startingPosition.GetComponent<PointInfo>().G = 0;
        openlist.Add(startingPosition);
        GameObject[,] array;
        int size = GameObject.FindObjectOfType<BoardDictionary>().size;
        //Debug.Log(size);
        array = new GameObject[size, size];
        int unicode;

        for (int i = 0; i < size; i += 1)
        {
            unicode = 65 + i;
            for (int j = 1; j < size + 1; j += 1)
            {
                char character = (char)unicode;
                string text = character.ToString();
                var temp = Object.FindObjectOfType<BoardDictionary>().Board[text + System.Convert.ToString(j)];
                
                array[i, j - 1] = temp;
                array[i, j - 1].GetComponent<PointInfo>().setDist(targetLocation);
                array[i, j - 1].GetComponent<PointInfo>().X = i;
                array[i, j - 1].GetComponent<PointInfo>().Y = j - 1;
                array[i, j - 1].GetComponent<PointInfo>().G = int.MaxValue;
                array[i, j - 1].GetComponent<PointInfo>().F = int.MaxValue;
                //bug.Log(array[i, j - 1].name);
            }
        }
        
        GameObject currentbest = startingPosition;
        while (currentbest.name != targetLocation.name)
        {
            List<GameObject> neighbours = new List<GameObject>();
            neighbours = GetNeighbours(array, currentbest.GetComponent<PointInfo>().X, currentbest.GetComponent<PointInfo>().Y, size);
            for (int i = 0; i < neighbours.Count; i += 1)
            {
                string neighbourname = neighbours[i].name;
                if (!IsChecked(openlist, closedlist, neighbourname))
                {
                    openlist.Add(neighbours[i]);
                }
            }
            GameObject tempc = currentbest;
            openlist.Remove(currentbest);
            closedlist.Add(tempc);
            EvaluateRoute(openlist, tempc);
            currentbest= openlist[SelectBest(openlist)];
            //Debug.Log("Current best:"+ currentbest.name);
        }
        path=RecreatePath(targetLocation.name,startingPosition.name);
        return path;
        //geting neighbours
        
    }
    static List<GameObject> RecreatePath(string last,string first)
    {
       
        List<GameObject> path = new List<GameObject>();
        string curtile = last;
        while (curtile!=first)
        {
            GameObject ctile = Object.FindObjectOfType<BoardDictionary>().Board[curtile];
            path.Add(ctile);
            Debug.Log("Current best:" + curtile);

            curtile = ctile.GetComponent<PointInfo>().Camefrom.name;
        }
        path.Add(Object.FindObjectOfType<BoardDictionary>().Board[first]);
        return path;

    }
    static void EvaluateRoute(List<GameObject> olist,GameObject curb)
    {
        int g =curb.GetComponent<PointInfo>().G;
        for (int i=0;i< olist.Count;i+=1)
        {
            if (olist[i].GetComponent<PointInfo>().G > g+10)
            {
                olist[i].GetComponent<PointInfo>().Camefrom = curb;
                olist[i].GetComponent<PointInfo>().G = g + 10;
                olist[i].GetComponent<PointInfo>().F = olist[i].GetComponent<PointInfo>().G + olist[i].GetComponent<PointInfo>().H;
                Debug.Log(olist[i].GetComponent<PointInfo>().G);
            }
        }
    }
    
    static int SelectBest(List<GameObject> olist)
    {
        int curbest=int.MaxValue;
        float fscore=int.MaxValue;
        for(int i = 0; i < olist.Count; i += 1)
        {
            if (olist[i].GetComponent<PointInfo>().F<fscore)
            {
                curbest = i;
                fscore = olist[i].GetComponent<PointInfo>().F;
            }
        }
        return curbest;
    }
    

    // Update is called once per frame
    public static List<GameObject> GetNeighbours(GameObject[,] array, int x, int y, int asize)
    {
        List<GameObject> neighbours = new List<GameObject>();
        if (x > 0 && array[x - 1, y].GetComponent<PointInfo>().Walkable)
        {
            neighbours.Add(array[x - 1, y]);
        }
        if (x < asize- 1 && array[x + 1, y].GetComponent<PointInfo>().Walkable)
        {
            neighbours.Add(array[x + 1, y]);
        }
        if (y > 0 && array[x, y - 1].GetComponent<PointInfo>().Walkable)
        {
            neighbours.Add(array[x, y - 1]);
        }
        if (y < asize- 1 && array[x, y + 1].GetComponent<PointInfo>().Walkable)
        {
            neighbours.Add(array[x, y + 1]);
        }
        return neighbours;
    }
    static bool IsChecked(List<GameObject> olist, List<GameObject> clist,string name)
    {
        for (int i=0;i<olist.Count;i+=1)
        {
            if (olist[i].name == name)
            {
                return true;
            }
            
        }
        for (int i = 0; i < clist.Count; i += 1)
        {

            if (clist[i].name == name)
            {
                return true;
            }
        }
        return false;
    }
}
