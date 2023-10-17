using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    [SerializeField] int rows = 3;
    [SerializeField] int columns = 3;
    [SerializeField] Color[] colors;
    [SerializeField] float cellSize;
    [SerializeField] GameObject cell;
    [SerializeField] GameObject highlight;
    [SerializeField] Pattern[] patterns;
    [SerializeField] GameObject patternHolder;
    [SerializeField] UnityEvent OnPatternComplete;

    public bool paused;

    Dictionary<Vector2, Color> currentPatternColors = new Dictionary<Vector2, Color>();
    Color[,] gridColors;
    GameObject[,] gridGameobjects;
    Dictionary<Vector2, GameObject> cellToHighlight = new Dictionary<Vector2, GameObject>();
    List<GameObject> patternCells = new List<GameObject>();

    private void Awake()
    {
        paused = true;
        gridColors = new Color[rows, columns];
        gridGameobjects = new GameObject[rows, columns];
    }

    public void StartGame()
    {
        ClearPattern();
        GenerateRandomGrid();
        GenerateRandomPattern();
        DrawPattern();
        DeselectAllCells();
    }

    private void GenerateRandomGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int randomIndex = UnityEngine.Random.Range(0, colors.Length);
                Color color = colors[randomIndex];
                gridColors[row, col] = color;
            }
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject cellInstance = Instantiate(cell, transform, false);
                cellInstance.transform.position = CellRowColToCellCenter(row, col);
                cellInstance.GetComponent<SpriteRenderer>().color = gridColors[row, col];
                gridGameobjects[row, col] = cellInstance;
                cellInstance.transform.localScale = new Vector2(cellSize, cellSize);
            }
        }
    }

    private void GenerateRandomPattern()
    {
        Pattern pattern = patterns[UnityEngine.Random.Range(0, patterns.Length)];
        Vector2[] cellLocations = pattern.GetCellLocations();
        bool patternIsValid = false;
        int patternRow = 0;
        int patternCol = 0;
        while(!patternIsValid)
        {
            int randomRow = UnityEngine.Random.Range(0, rows);
            int randomCol = UnityEngine.Random.Range(0, columns);
            patternIsValid = true;
            for (int index = 0; index < cellLocations.Length; index++)
            {
                int gridCol = (int)cellLocations[index].x + randomCol;
                int gridRow = (int)cellLocations[index].y * -1 + randomRow;
                if(gridCol < 0 || gridCol >= columns || gridRow < 0 || gridRow >= rows)
                {
                    patternIsValid = false;
                }
            }
            patternRow = randomRow;
            patternCol = randomCol;
        }
        Debug.Log(patternRow + " " + patternCol);
        for (int index = 0; index < cellLocations.Length; index++)
        {
            int cellPosRow = (int)cellLocations[index].y * -1 + patternRow;
            int cellPosCol = (int)cellLocations[index].x + patternCol;
            Color color = gridColors[cellPosRow, cellPosCol];
            currentPatternColors.Add(cellLocations[index], color);
        }
    }

    private void DrawPattern()
    {
        foreach (KeyValuePair<Vector2, Color> entry in currentPatternColors)
        {
            GameObject patternCellInstance = Instantiate(cell, patternHolder.transform);
            patternCellInstance.transform.localPosition = new Vector2(entry.Key.x * cellSize, entry.Key.y * cellSize);
            patternCellInstance.transform.localScale = new Vector2(cellSize, cellSize);
            patternCellInstance.GetComponent<SpriteRenderer>().color = entry.Value;
            patternCells.Add(patternCellInstance);
        }
    }

    private void Update()
    {
        if(paused)
        {
            return;
        }
        bool clicked = Input.GetMouseButtonDown(0);
        if(clicked)
        {
            ProcessClick();
        }
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);
        if(spacePressed)
        {
            Debug.Log("SPACE");
            CheckSelectedCells();
        }
    }

    private void ProcessClick()
    {
        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2 topLeft = new Vector2(transform.position.x - columns * cellSize / 2, transform.position.y + rows * cellSize / 2);
        Vector2 rightBottom = new Vector2(topLeft.x + cellSize * columns, topLeft.y - cellSize * rows);
        if (worldPos.x >= topLeft.x && worldPos.x <= rightBottom.x)
        {
            if (worldPos.y <= topLeft.y && worldPos.y >= rightBottom.y)
            {
                int column = (int)((worldPos.x - topLeft.x) / cellSize);
                int row = (int)((topLeft.y - worldPos.y) / cellSize);
                if (cellToHighlight.ContainsKey(new Vector2(row, column)))
                {
                    DeselectCell(row, column);
                }
                else
                {
                    SelectCell(row, column);
                }
            }
            else
            {
                DeselectAllCells();
            }
        }
        else
        {
            DeselectAllCells();
        }
    }

    private void DeselectCell(int row, int column)
    {
        cellToHighlight.TryGetValue(new Vector2(row, column), out GameObject highlightInstance);
        Destroy(highlightInstance);
        cellToHighlight.Remove(new Vector2(row, column));
    }

    private Vector2 CellRowColToCellCenter(int row, int col)
    {
        float topLeftX = transform.position.x - ((columns * cellSize) / 2);
        float topLeftY = transform.position.y + ((rows * cellSize) / 2);
        return new Vector2(topLeftX + col * cellSize + cellSize / 2, topLeftY - row * cellSize - cellSize / 2);
    }

    private void SelectCell(int row, int col)
    {
        GameObject highlightInstance = Instantiate(highlight, CellRowColToCellCenter(row, col), Quaternion.identity);
        cellToHighlight.Add(new Vector2(row, col), highlightInstance);
    }

    private void DeselectAllCells()
    {
        foreach (KeyValuePair<Vector2, GameObject> entry in cellToHighlight)
        {
            Destroy(entry.Value);
        }
        cellToHighlight.Clear();
    }

    private void CheckSelectedCells()
    {
        if(cellToHighlight.Keys.Count != currentPatternColors.Keys.Count)
        {
            DeselectAllCells();
            return;
        }
        bool isCorrectPattern = true;
        foreach(KeyValuePair<Vector2, GameObject> selectEntry in cellToHighlight)
        {
            Vector2 center = selectEntry.Key;
            isCorrectPattern = true;
            foreach(KeyValuePair<Vector2, Color> patternEntry in currentPatternColors)
            {
                Vector2 currentCellPos = new Vector2(patternEntry.Key.y * -1 + center.x, patternEntry.Key.x + center.y);
                if(!cellToHighlight.ContainsKey(currentCellPos))
                {
                    isCorrectPattern = false;
                    break;
                }
                Color currentColor = gridColors[(int)currentCellPos.x, (int)currentCellPos.y];
                if(currentColor != patternEntry.Value)
                {
                    isCorrectPattern = false;
                    break;
                }
            }
            if(isCorrectPattern)
            {
                break;
            }
        }

        if (isCorrectPattern)
        {
            PatternCompleted();
        }
        else
        {
            DeselectAllCells();
        }
    }

    private void PatternCompleted()
    {
        DeselectAllCells();
        OnPatternComplete?.Invoke();
    }

    private void ClearPattern()
    {
        foreach (GameObject patternCell in patternCells)
        {
            Destroy(patternCell);
        }
        currentPatternColors.Clear();
    }

    public void SetEasy()
    {
        rows = 5;
        columns = 5;
    }

    public void SetMedium()
    {
        rows = 10;
        columns = 10;
    }

    public void SetHard()
    {
        rows = 15;
        columns = 15;
    }

    public void PauseGrid()
    {
        paused = true;
    }

}
