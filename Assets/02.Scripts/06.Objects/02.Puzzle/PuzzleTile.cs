using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
    private ElectricalPuzzleController parentController;
    private int gridX, gridY;
    private int tileRotation = 0;

    [Header("Connection States")]
    public bool upConnected;
    public bool downConnected;
    public bool leftConnected;
    public bool rightConnected;

    public void Initialize(ElectricalPuzzleController controller, int x, int y)
    {
        this.parentController = controller;
        this.gridX = x;
        this.gridY = y;
    }

    void OnMouseDown()
    {
        RotatePuzzleTile();
        parentController.CheckPuzzleCompletion();
    }

    public void RotatePuzzleTile()
    {
        tileRotation = (tileRotation + 90) % 360;
        transform.localEulerAngles = new Vector3(0, 0, tileRotation);
        UpdateConnections();
    }

    private void UpdateConnections()
    {
        bool tempUp = upConnected;
        bool tempRight = rightConnected;
        bool tempDown = downConnected;
        bool tempLeft = leftConnected;

        switch (tileRotation)
        {
            case 0:
                upConnected = tempUp;
                rightConnected = tempRight;
                downConnected = tempDown;
                leftConnected = tempLeft;
                break;
            case 90:
                upConnected = tempLeft;
                rightConnected = tempUp;
                downConnected = tempRight;
                leftConnected = tempDown;
                break;
            case 180:
                upConnected = tempDown;
                rightConnected = tempLeft;
                downConnected = tempUp;
                leftConnected = tempRight;
                break;
            case 270:
                upConnected = tempRight;
                rightConnected = tempDown;
                downConnected = tempLeft;
                leftConnected = tempUp;
                break;
        }
    }

    public bool CheckAlignment() // 이름 변경
    {
        bool isAligned = true;

        if (parentController.tiles != null)
        {
            if (gridY > 0)
            {
                PuzzleTile aboveTile = parentController.tiles[gridX, gridY - 1];
                if (aboveTile != null && upConnected != aboveTile.downConnected)
                {
                    isAligned = false;
                }
            }

            if (gridY < parentController.gridSize - 1)
            {
                PuzzleTile belowTile = parentController.tiles[gridX, gridY + 1];
                if (belowTile != null && downConnected != belowTile.upConnected)
                {
                    isAligned = false;
                }
            }

            if (gridX > 0)
            {
                PuzzleTile leftTile = parentController.tiles[gridX - 1, gridY];
                if (leftTile != null && leftConnected != leftTile.rightConnected)
                {
                    isAligned = false;
                }
            }

            if (gridX < parentController.gridSize - 1)
            {
                PuzzleTile rightTile = parentController.tiles[gridX + 1, gridY];
                if (rightTile != null && rightConnected != rightTile.leftConnected)
                {
                    isAligned = false;
                }
            }
        }

        return isAligned;
    }
}