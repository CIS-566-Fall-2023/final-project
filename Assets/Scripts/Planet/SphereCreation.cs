using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCreation : MonoBehaviour
{

    public GameObject tilePrefab;
    public GameObject tileParent;
    
    public sHexGrid finalGrid;
    public int size = 0;

    // Start is called before the first frame update
    void Start()
    {
        finalGrid = size_n_grid(size);
        CreateObjects();
    }

    sHexGrid size_n_grid(int size)
    {
        if(size == 0)
        {
            return CreateZeroSphere();
        }
        else
        {
            return CreateSubdividedSphere(size_n_grid(size - 1));
        }
    }

    void CreateObjects()
    {
        for(int i = 0; i < finalGrid.tiles.Count; i++)
        {
            var tile = Instantiate(tilePrefab);
            tile.transform.parent = tileParent.transform;
            tile.GetComponent<HexagonTile>().SetupTile(finalGrid.tiles[i].position, finalGrid.tiles[i].corners, finalGrid.tiles[i]);
            tile.name = i.ToString();
        }
    }

    sHexGrid CreateZeroSphere()
    {
        //starting points//
        sHexGrid grid = new sHexGrid();
        grid.SetupHexGrid(0);

        float x = -0.525731112119133606f;
        float z = -0.850650808352039932f;

        Vector3[] icosahedronTiles = //List of icosahedron tile placement// 
        {
            new Vector3(-x, 0, z), new Vector3(x, 0, z), new Vector3(-x, 0, -z), new Vector3(x, 0, -z),
            new Vector3(0, z, x), new Vector3(0, z, -x), new Vector3(0, -z, x), new Vector3(0, -z, -x),
            new Vector3(z, x, 0), new Vector3(-z, x, 0), new Vector3(z, -x, 0), new Vector3(-z, -x, 0)    
        };

        int[,] icosahedronTileNumbers = //List of icosahedron tile numbering//
        {
            {9, 4, 1, 6, 11}, {4, 8, 10, 6, 0}, {11, 7, 3, 5, 9}, {2, 7, 10, 8, 5},
            {9, 5, 8, 1, 0}, {2, 3, 8, 4, 9}, {0, 1, 10, 7, 11}, {11, 6, 10, 3, 2},
            {5, 3, 10, 1, 4}, {2, 5, 4, 0, 11}, {3, 7, 6, 1, 8}, {7, 2, 9, 0, 6}
        };

        Debug.Log("Step 5");
        foreach(sTile t in grid.tiles)
        {
            t.position = icosahedronTiles[t.id];
            for(int i = 0; i < 5; i++)
            {
                t.adjTiles[i] = grid.tiles[icosahedronTileNumbers[t.id, i]];
            }
        }

        for(int i = 0; i < 5; i++)
        {
            AddCorner(i, grid, 0, icosahedronTileNumbers[0, (i+4) % 5], icosahedronTileNumbers[0,i]);
        }

        for(int i = 0; i < 5; i++)
        {
            AddCorner(i + 5, grid, 3, icosahedronTileNumbers[3, (i+4) % 5], icosahedronTileNumbers[3,i]);
        }

        Debug.Log("Step 8");
        AddCorner(10, grid, 10, 1, 8);
        AddCorner(11, grid, 1, 10, 6);
        AddCorner(12, grid, 6, 10, 7);
        AddCorner(13, grid, 6, 7, 11);
        AddCorner(14, grid, 11, 7, 2);
        AddCorner(15, grid, 11, 2, 9);
        AddCorner(16, grid, 9, 2, 5);
        AddCorner(17, grid, 9, 5, 4);
        AddCorner(18, grid, 4, 5, 8);
        AddCorner(19, grid, 4, 8, 1);

        foreach(sCorner c in grid.corners)
        {
            for(int i = 0 ; i < 3; i++)
            {
                c.adjCorners[i] = c.adjTiles[i].corners[(sTileMaths.CalculatePosition(c.adjTiles[i],c) + 1) % 5];
            }
        }

        int newEdgeID = 0 ;
        foreach(sTile t in grid.tiles)
        {
            for(int i = 0; i < 5; i++)
            {
                if(t.edges[i] == null)
                {
                    AddEdge(newEdgeID, grid, t.id, icosahedronTileNumbers[t.id, i]);
                    newEdgeID++;
                }

            }
        }

        return grid;
    }

    sHexGrid CreateSubdividedSphere(sHexGrid previous)
    {
        sHexGrid nGrid = new sHexGrid();
        nGrid.SetupHexGrid(previous.gridSize + 1);

        int prev_tile_count = previous.tiles.Count;
        int prev_corner_count = previous.corners.Count;

        //old tiles
        for (int i = 0; i < prev_tile_count; i++)
        {
            nGrid.tiles[i].position = previous.tiles[i].position;
            for (int k = 0; k < nGrid.tiles[i].edgeCount; k++)
            {
                nGrid.tiles[i].adjTiles[k] = nGrid.tiles[previous.tiles[i].corners[k].id + prev_tile_count];
            }
        }
        //old corners become tiles
        for (int i = 0; i < prev_corner_count; i++)
        {
            nGrid.tiles[i + prev_tile_count].position = previous.corners[i].position;
            for (int k = 0; k < 3; k++)
            {
                nGrid.tiles[i + prev_tile_count].adjTiles[2 * k] = nGrid.tiles[previous.corners[i].adjCorners[k].id + prev_tile_count];
                nGrid.tiles[i + prev_tile_count].adjTiles[2 * k + 1] = nGrid.tiles[previous.corners[i].adjTiles[k].id];
            }
        }
        //new corners
        int next_corner_id = 0;
        foreach(sTile n in previous.tiles)
        {
            sTile t = nGrid.tiles[n.id];
            for(int k = 0; k < t.edgeCount;k++)
            {
                AddCorner(next_corner_id, nGrid, t.id, t.adjTiles[(k + t.edgeCount - 1) % t.edgeCount].id, t.adjTiles[k].id);
                next_corner_id++;
            }
        }

        //connect corners
        foreach(sCorner c in nGrid.corners)
        {
            for(int k = 0; k < 3;k++)
            {
                c.adjCorners[k] = c.adjTiles[k].corners[(sTileMaths.CalculatePosition(c.adjTiles[k], c) + 1) % (c.adjTiles[k].edgeCount)];
            }
        }

        //new edges
        int next_edge_id = 0;
        foreach(sTile t in nGrid.tiles)
        {
            for(int k = 0; k < t.edgeCount; k++)
            {
                if(t.edges[k] == null)
                {
                    AddEdge(next_edge_id, nGrid, t.id, t.adjTiles[k].id);
                    next_edge_id++;
                }
            }
        }

        return nGrid;
    }


    void AddCorner(int id, sHexGrid grid, int t1, int t2, int t3)
    {
        sCorner c = grid.corners[id];
        sTile[] t = {grid.tiles[t1], grid.tiles[t2], grid.tiles[t3]};
        Vector3 pos = t[0].position + t[1].position + t[2].position;
        c.position = sTileMaths.normal(pos);

        for(int i = 0 ; i < 3; i++)
        {
            t[i].corners[sTileMaths.CalculatePosition(t[i], t[(i+2) % 3 ])] = c;
            c.adjTiles[i] = t[i];
        }
    }

    void AddEdge(int id, sHexGrid grid, int t1, int t2)
    {
        sEdge e = grid.edges[id];
        sTile[] t = {grid.tiles[t1], grid.tiles[t2]};
        sCorner[] c = 
        {
            grid.corners[t[0].corners[sTileMaths.CalculatePosition(t[0], t[1])].id],grid.corners[t[0].corners[(sTileMaths.CalculatePosition(t[0], t[1])+1)%t[0].edgeCount].id]
        };

        for(int i = 0 ; i < 2; i++)
        {
            t[i].edges[sTileMaths.CalculatePosition(t[i], t[(i+1) % 2])] = e;
            e.adjTiles[i] = t[i];

            c[i].adjEdges[sTileMaths.CalculatePosition(c[i], c[(i + 1) % 2])] = e;
            e.adjCorners[i] = c[i];
        }
    }

}
