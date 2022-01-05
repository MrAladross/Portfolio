using System.Collections.Generic;
using UnityEngine;

public class GridCalculator : MonoBehaviour
{
    [SerializeField] private int rowWidth;
    [SerializeField] private int numberOfRows;
    private List<int> positions;
    private List<GridElement> _elements;
    private List<bool> isChecked;

[SerializeField]    private int _indexToCheck = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearChecks();
            CheckNeighbors(_indexToCheck);
            //handle explosion here
        }
            
    }

    void ClearChecks()
    {
        for (int i = 0; i < isChecked.Count; ++i)
            isChecked[i] = false;
    }

    void CheckNeighbors(int indexToCheck)
    {
        Debug.Log("Checking space " + indexToCheck);
        List<int> neighbors = GetNeighbors(indexToCheck);
        for (int i = 0; i < neighbors.Count; ++i)
        {
            if (!isChecked[neighbors[i]])
                if (_elements[neighbors[i]].GetColor() == _elements[indexToCheck].GetColor()) //same color
                {
                    Debug.Log(neighbors[i] + " is a match.");
                    isChecked[neighbors[i]] = true;
                    CheckNeighbors(neighbors[i]);
                }
        }
    }
    void GenerateGrid()
    {
        positions = new List<int>();
        _elements = new List<GridElement>();
        isChecked = new List<bool>();
        
        for (int i = 0; i < numberOfRows; ++i)
        {
            for (int j = 0; j < rowWidth; ++j)
            {
                int index = i * rowWidth + j;
                index = index % 2;
                
                
                //positions defines neighbors and shouldn't change
                //_elements represents balls and should change if a ball pops or moves
                //isChecked matches size of positions
                positions.Add(index);
                _elements.Add(new GridElement(index%rowWidth));
                isChecked.Add(false);

            }
        }

        Debug.Log("There are " + rowWidth * numberOfRows + " rows. The list has a count of " + positions.Count);
        Debug.Log("There are " + (positions[positions.Count - 1] +1) + " elements.");
        
        
       
    }

    List<int> GetNeighbors(int index)
    {
        List<int> neighbors = new List<int>();


        if ((index / rowWidth) % 2 == 0)
        {
            if (index - rowWidth - 1 > 0)
            {
                if (index % rowWidth > 0)
                    neighbors.Add(index - rowWidth); //upleft

                neighbors.Add(index - rowWidth); //upright
            }

//left
            if (index % rowWidth > 0)
                neighbors.Add(index - 1);
            //right
            if (index % rowWidth < rowWidth - 1)
                neighbors.Add(index + 1);
            //downleft
            if (index + rowWidth - 1 < rowWidth * numberOfRows && index % rowWidth > 0)
                neighbors.Add(index + rowWidth - 1);
            //downright
            if (index + rowWidth < rowWidth * numberOfRows)
                neighbors.Add(index + rowWidth);
        }
        else
        {
            if (index - rowWidth > 0)
            {
                //upleft
                neighbors.Add(index - rowWidth);
                //upright
                if ((index % rowWidth) < rowWidth - 1)
                    neighbors.Add(index - rowWidth + 1);
            }

            //left
            if (index % rowWidth > 0)
                neighbors.Add(index - 1);
            //right
            if (index % rowWidth < rowWidth - 1)
                neighbors.Add(index + 1);
            //downleft
            if (index + rowWidth < rowWidth * numberOfRows)
                neighbors.Add(index + rowWidth);
            //downright
            if (index + rowWidth + 1 < rowWidth * numberOfRows && (index + rowWidth + 1) % rowWidth > 0)
                neighbors.Add(index + rowWidth + 1);
        }

        return neighbors;

    }

    private void Awake()
    {
        GenerateGrid();
    }
}

public class GridElement
{
    private int color;

    public int GetColor()
    {
        return color;
    }
    public GridElement(int index)
    {
        color = index;
    }
}
