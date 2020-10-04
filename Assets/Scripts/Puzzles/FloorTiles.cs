using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTiles : MonoBehaviour
{
    [SerializeField]
    private List<TileRow> tileRows;

	// Start is called before the first frame update
	void Start()
    {
        bool first = true;
        int lastLastTileNum = 0;
        int lastTileNum = 0;
        int i = 0;

        foreach (TileRow row in tileRows)
		{
            if (first)
			{
                int tileNum = Random.Range(0, 4);
                row.tiles[tileNum].GetComponent<FloorTile>().safe = true;

                lastLastTileNum = lastTileNum;
                lastTileNum = tileNum;

                first = false;
			}
            else if (i == tileRows.Count - 1)
			{
                row.tiles[lastTileNum].GetComponent<FloorTile>().safe = true;
            }
            else
			{
                int tileNum = Mathf.Clamp(lastTileNum + Random.Range(-1, 2), 0, 3);

                if (tileNum == lastLastTileNum)
                {
                    tileNum = lastTileNum;
                }

                row.tiles[tileNum].GetComponent<FloorTile>().safe = true;

                if (lastTileNum != tileNum)
                {
                    row.tiles[lastTileNum].GetComponent<FloorTile>().safe = true;
                }

                lastLastTileNum = lastTileNum;
                lastTileNum = tileNum;
            }

            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}


[System.Serializable]
public class TileRow
{
    public List<GameObject> tiles;
}