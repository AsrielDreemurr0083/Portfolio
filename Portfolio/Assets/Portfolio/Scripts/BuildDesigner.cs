using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDesigner : MonoBehaviour
{
    [Serializable]
    public struct StructureType
    {
        public GameObject prefab;
        public int count;
        public Vector3Int size; // x, y(높이), z 축의 크기
        public Vector3Int exclusionZone;
    }

    public StructureType[] structures;
    public Vector3Int gridSize = new Vector3Int(20, 1, 20); // x, y, z 축의 그리드 크기
    public float cellSize = 1f;
    public Vector3 spawnAreaCenter;

    private bool[,,] occupiedCells;

    void Start()
    {
        occupiedCells = new bool[gridSize.x, gridSize.y, gridSize.z];
        foreach (var structure in structures)
        {
            if (structure.size.x <= 0 || structure.size.y <= 0 || structure.size.z <= 0)
            {
                Debug.LogError($"Invalid size for structure {structure.prefab.name}: {structure.size}");
            }
        }
        SpawnStructures();
    }

    void SpawnStructures()
    {
        foreach (var structure in structures)
        {
            for (int i = 0; i < structure.count; i++)
            {
                TryPlaceStructure(structure);
            }
        }
    }

    bool TryPlaceStructure(StructureType structure)
    {
        for (int attempts = 0; attempts < 100; attempts++)
        {
            Vector3Int randomCell = GetRandomCell();
            Debug.Log($"Attempting to place {structure.prefab.name} at {randomCell}");
            if (CanPlaceStructure(randomCell, structure.size, structure.exclusionZone))
            {
                PlaceStructure(randomCell, structure);
                Debug.Log($"Successfully placed {structure.prefab.name} at {randomCell}");
                return true;
            }
        }
        Debug.LogWarning($"Failed to place structure: {structure.prefab.name}");
        return false;
    }

    Vector3Int GetRandomCell()
    {
        return new Vector3Int(
            UnityEngine.Random.Range(0, gridSize.x),
            0, // 항상 바닥에 배치
            UnityEngine.Random.Range(0, gridSize.z)
        );
    }

    bool CanPlaceStructure(Vector3Int cell, Vector3Int size, Vector3Int exclusionZone)
    {
        Vector3Int totalSize = size + exclusionZone * 2;
        for (int x = -exclusionZone.x; x < totalSize.x - exclusionZone.x; x++)
        {
            for (int y = -exclusionZone.y; y < totalSize.y - exclusionZone.y; y++)
            {
                for (int z = -exclusionZone.z; z < totalSize.z - exclusionZone.z; z++)
                {
                    int checkX = cell.x + x;
                    int checkY = cell.y + y;
                    int checkZ = cell.z + z;

                    if (checkX < 0 || checkX >= gridSize.x ||
                        checkY < 0 || checkY >= gridSize.y ||
                        checkZ < 0 || checkZ >= gridSize.z)
                    {
                        return false;
                    }

                    if (occupiedCells[checkX, checkY, checkZ])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    void PlaceStructure(Vector3Int cell, StructureType structure)
    {
        Vector3 position = CellToWorldPosition(cell);
        GameObject instance = Instantiate(structure.prefab, position, Quaternion.identity);

        Vector3Int totalSize = structure.size + structure.exclusionZone * 2;
        for (int x = -structure.exclusionZone.x; x < totalSize.x - structure.exclusionZone.x; x++)
        {
            for (int y = -structure.exclusionZone.y; y < totalSize.y - structure.exclusionZone.y; y++)
            {
                for (int z = -structure.exclusionZone.z; z < totalSize.z - structure.exclusionZone.z; z++)
                {
                    int markX = cell.x + x;
                    int markY = cell.y + y;
                    int markZ = cell.z + z;

                    if (markX >= 0 && markX < gridSize.x &&
                        markY >= 0 && markY < gridSize.y &&
                        markZ >= 0 && markZ < gridSize.z)
                    {
                        occupiedCells[markX, markY, markZ] = true;
                    }
                }
            }
        }
    }

    Vector3 CellToWorldPosition(Vector3Int cell)
    {
        return spawnAreaCenter + new Vector3(
            (cell.x - gridSize.x / 2f + 0.5f) * cellSize,
            cell.y * cellSize,
            (cell.z - gridSize.z / 2f + 0.5f) * cellSize
        );
    }

    void OnDrawGizmosSelected()
    {
        if (occupiedCells == null) return;

        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(gridSize.x * cellSize, gridSize.y * cellSize, gridSize.z * cellSize);
        Gizmos.DrawWireCube(spawnAreaCenter, size);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    if (occupiedCells[x, y, z])
                    {
                        Gizmos.color = Color.red;
                        Vector3 pos = CellToWorldPosition(new Vector3Int(x, y, z));
                        Gizmos.DrawCube(pos, Vector3.one * cellSize * 0.9f);
                    }
                }
            }
        }
    }

}