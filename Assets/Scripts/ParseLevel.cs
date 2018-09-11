using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParseLevel : MonoBehaviour {

    public TextAsset level;
    static int length = 29 * 2, height = 31;

    public Tilemap tm;
    public Tile floor, wall, corner, edge, end, endleft, endright, door;



	// Use this for initialization
	void Start () {
        LoadLevel();
	}

    //To make it uniform we need an extra blank line at the end, or tbh anything wokrs as long as its another character thats in place of the "\n".

    private void LoadLevel()
    {
        for(int y = 0;y <= height; y++)
        {
            for(int x = 0; x < length - 2; x+=2)
            {
                int rotation = 0;
                switch (level.text[y * length + x])
                {
                    case 'E':
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), edge);
                        if (level.text[y * length + x + 1] == '1')
                        {
                            rotation = 270;
                        }
                        else if (level.text[y * length + x + 1] == '2')
                        {
                            rotation = 180;
                        }
                        else if (level.text[y * length + x + 1] == '3')
                        {
                            rotation = 90;
                        }

                        if (rotation != 0)
                        { 
                            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotation), Vector3.one);
                            tm.SetTransformMatrix(new Vector3Int(x / 2, (height - y), 0), matrix);
                        }
                        break;

                    case 'C':
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), corner);
                        if (level.text[y * length + x + 1] == '1')
                        {
                            rotation = 270;
                        }
                        else if (level.text[y * length + x + 1] == '2')
                        {
                            rotation = 180;
                        }
                        else if (level.text[y * length + x + 1] == '3')
                        {
                            rotation = 90;
                        }

                        if (rotation != 0)
                        {
                            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotation), Vector3.one);
                            tm.SetTransformMatrix(new Vector3Int(x / 2, (height - y), 0), matrix);
                        }
                        break;

                    case 'R':
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), endright);
                        if (level.text[y * length + x + 1] == '1')
                        {
                            rotation = 270;
                        }
                        else if (level.text[y * length + x + 1] == '2')
                        {
                            rotation = 180;
                        }
                        else if (level.text[y * length + x + 1] == '3')
                        {
                            rotation = 90;
                        }

                        if (rotation != 0)
                        {
                            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotation), Vector3.one);
                            tm.SetTransformMatrix(new Vector3Int(x / 2, (height - y), 0), matrix);
                        }
                        break;

                    case 'L':
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), endleft);
                        if (level.text[y * length + x + 1] == '1')
                        {
                            rotation = 270;
                        }
                        else if (level.text[y * length + x + 1] == '2')
                        {
                            rotation = 180;
                        }
                        else if (level.text[y * length + x + 1] == '3')
                        {
                            rotation = 90;
                        }

                        if (rotation != 0)
                        {
                            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotation), Vector3.one);
                            tm.SetTransformMatrix(new Vector3Int(x / 2, (height - y), 0), matrix);
                        }
                        break;

                    case 'D':
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), door);
                        break;

                    case 'W':
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), wall);
                        break;
                        
                    default:
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), floor);
                        break;

                }
            }
        }
    }

	
}
