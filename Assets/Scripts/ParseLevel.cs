using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParseLevel : MonoBehaviour {

    public TextAsset level;
    static int length = 29 * 2, height = 31;

    public Tilemap tm;
    public Tile floor, wall, corner, edge, end, endleft, endright, door;
	public GameObject pellet,heckman, ghostA, ghostB, ghostC, ghostD;
    public Score score;

	public int[,] levelArray = new int[28,32];

	// Use this for initialization
	void Start () {
        LoadLevel();
        score = (GameObject.Find("UI")).GetComponent<Score>();
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
						levelArray[x / 2, (height - y)] = 0;
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
						levelArray[x / 2, (height - y)] = 0;
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
						levelArray[x / 2, (height - y)] = 0;
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
						levelArray[x / 2, (height - y)] = 0;
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
						levelArray[x / 2, (height - y)] = 2;
						break;

                    case 'W':
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), wall);
						levelArray[x / 2, (height - y)] = 0;
						break;

					case '.':
						GameObject.Instantiate(pellet, new Vector3(x / 2 + .5f, (height - y) + .5f, 0), new Quaternion());
						levelArray[x / 2, (height - y)] = 1;
						break;

                    case 'P':
                        GameObject p = GameObject.Instantiate(heckman, new Vector3(x / 2 + .5f, (height - y) + .5f, 0), new Quaternion());
                        p.GetComponent<PacMaster>().pl = this;
                        p.GetComponent<PacMaster>().scoring = score;
                        levelArray[x / 2, (height - y)] = 1;
                        break;

                    case 'G':
                        GameObject g;
                        if (level.text[y * length + x + 1] == 'A')
                        {
                            g =  GameObject.Instantiate(ghostA, new Vector3(x / 2 + .5f, (height - y) + .5f, 0), new Quaternion());
                        }
                        else if (level.text[y * length + x + 1] == 'B')
                        {
                            g = GameObject.Instantiate(ghostB, new Vector3(x / 2 + .5f, (height - y) + .5f, 0), new Quaternion());
                        }
                        else if (level.text[y * length + x + 1] == 'C')
                        {
                            g = GameObject.Instantiate(ghostC, new Vector3(x / 2 + .5f, (height - y) + .5f, 0), new Quaternion());
                        }
                        else
                        {
                            g = GameObject.Instantiate(ghostD, new Vector3(x / 2 + .5f, (height - y) + .5f, 0), new Quaternion());
                        }

                        g.GetComponent<Ghost>().pl = this;

                        levelArray[x / 2, (height - y)] = 1;
                        break;

                    default:
                        tm.SetTile(new Vector3Int(x / 2, (height - y), 0), floor);
						levelArray[x / 2, (height - y)] = 1;
						break;

                }
            }
        }
    }
}
