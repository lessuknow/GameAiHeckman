using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If you hold left as you respawn you go through wall slol
public class PacMaster : MonoBehaviour
{
	public GameObject pacSide;
	public GameObject pacTop;
	public ParseLevel pl;
	private bool levelSet = false;
    public Score scoring;
	private int[,] levelRef;
	private Vector3 goal;
	private Vector3 invGoal;
    private Vector3 initgoal, initinvgoal;
    private int initdir;
	public int dir = 1;
	private float t = 0;
	public float speed = .12f;
    public int dots_eaten;
    public bool is_super = false;
    private Vector3 initPos;
    private int point_combo = 0;

	void Start ()
	{
		pacSide.transform.localPosition = new Vector3(0, 0, -5);
        initPos = transform.localPosition;
  
	}

	void Update()
	{
		bool nearGoal = false;

		if (!levelSet)
		{
			levelSet = true;
			levelRef = pl.levelArray;
			invGoal = transform.position;
			goal = new Vector3(transform.position.x + 1, transform.position.y, 0); initgoal = goal;
            initinvgoal = invGoal;
            initdir = dir;
        }

		if (Mathf.Abs(goal.x - transform.position.x) < .1f && Mathf.Abs(goal.y - transform.position.y) < .1f)
			nearGoal = true;

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if (dir == 2)
			{
				dir = 0;
				Vector3 temp = goal;
				goal = invGoal;
				invGoal = temp;
				pacTop.transform.localPosition = new Vector3(0, 0, -5);
				pacSide.transform.localPosition = new Vector3(0, 0, 5);
				pacTop.transform.localScale = new Vector3(1, 1, 1);
			}
			else if (!((int)(goal.y - .5f) + 1 > 31) && ((goal.y - .5f) % 1f == 0 && (goal.x - .5f) % 1f == 0 &&
				levelRef[(int)(goal.x - .5f), (int)(goal.y - .5f) + 1] == 1) && nearGoal)
			{
				if (dir != 0)
					transform.position = goal;
				dir = 0;
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if (dir == 3)
			{
				dir = 1;
				Vector3 temp = goal;
				goal = invGoal;
				invGoal = temp;
				pacSide.transform.localPosition = new Vector3(0, 0, -5);
				pacTop.transform.localPosition = new Vector3(0, 0, 5);
				pacSide.transform.localScale = new Vector3(1, 1, 1);
			}
			else if (!((int)(goal.x - .5f) + 1 > 27) && ((goal.y - .5f) % 1f == 0 && (goal.x - .5f) % 1f == 0 &&
				levelRef[(int)(goal.x - .5f) + 1, (int)(goal.y - .5f)] == 1) && nearGoal)
			{
				if (dir != 1)
					transform.position = goal;
				dir = 1;
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if (dir == 0)
			{
				dir = 2;
				Vector3 temp = goal;
				goal = invGoal;
				invGoal = temp;
				pacTop.transform.localPosition = new Vector3(0, 0, -5);
				pacSide.transform.localPosition = new Vector3(0, 0, 5);
				pacTop.transform.localScale = new Vector3(-1, 1, 1);
			}
			else if (!((int)(goal.y - .5f) - 1 < 0) && ((goal.y - .5f) % 1f == 0 && (goal.x - .5f) % 1f == 0 &&
				levelRef[(int)(goal.x - .5f), (int)(goal.y - .5f) - 1] == 1) && nearGoal)
			{
				if (dir != 2)
					transform.position = goal;
				dir = 2;
			}
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if (dir == 1)
			{
				dir = 3;
				Vector3 temp = goal;
				goal = invGoal;
				invGoal = temp;
				pacSide.transform.localPosition = new Vector3(0, 0, -5);
				pacTop.transform.localPosition = new Vector3(0, 0, 5);
				pacSide.transform.localScale = new Vector3(-1, 1, 1);
			}
			else if (!((int)(goal.x - .5f) - 1 < 0) && ((goal.y - .5f) % 1f == 0 && (goal.x - .5f) % 1f == 0 &&
				levelRef[(int)(goal.x - .5f) - 1, (int)(goal.y - .5f)] == 1) && nearGoal)
			{
				if (dir != 3)
					transform.position = goal;
				dir = 3;
			}
		}

		if (nearGoal)
		{
			if (dir == 0)
			{
				if ((int)(goal.y - .5f) + 1 > 31 && (goal.y - .5f) % 1f == 0)
					goal = new Vector3(goal.x, 0.5f, 0);
				if (levelRef[(int)(goal.x - .5f), (int)(goal.y - .5f) + 1] == 1)
				{
					pacTop.transform.localPosition = new Vector3(0, 0, -5);
					pacSide.transform.localPosition = new Vector3(0, 0, 5);
					pacTop.transform.localScale = new Vector3(1, 1, 1);

					invGoal = new Vector3((int)(goal.x) + .5f, (int)(goal.y) + .5f, 0);
					goal = new Vector3((int)(goal.x) + .5f, (int)(goal.y) + 1.5f, 0);
				}
				else
					invGoal = goal;
			}
			else if (dir == 1)
			{
				if ((int)(goal.x - .5f) + 1 > 27)
				{
					transform.position = new Vector3(0.5f, goal.y, 0);
					invGoal = transform.position;
					goal = new Vector3(1.5f, goal.y, 0);
				}

				if (levelRef[(int)(goal.x - .5f) + 1, (int)(goal.y - .5f)] == 1)
				{
					pacSide.transform.localPosition = new Vector3(0, 0, -5);
					pacTop.transform.localPosition = new Vector3(0, 0, 5);
					pacSide.transform.localScale = new Vector3(1, 1, 1);

					invGoal = new Vector3((int)(goal.x) + .5f, (int)(goal.y) + .5f, 0);
					goal = new Vector3((int)(goal.x) + 1.5f, (int)(goal.y) + .5f, 0);
				}
				else
					invGoal = goal;
			}
			else if (dir == 2)
			{
				if ((int)(goal.y - .5f) - 1 < 0 && (goal.y - .5f) % 1f == 0)
					goal = new Vector3(goal.x, 31.5f, 0);

				if (levelRef[(int)(goal.x - .5f), (int)(goal.y - .5f) - 1] == 1)
				{
					pacTop.transform.localPosition = new Vector3(0, 0, -5);
					pacSide.transform.localPosition = new Vector3(0, 0, 5);
					pacTop.transform.localScale = new Vector3(-1, 1, 1);

					invGoal = new Vector3((int)(goal.x) + .5f, (int)(goal.y) + .5f, 0);
					goal = new Vector3((int)(goal.x) + .5f, (int)(goal.y) - .5f, 0);
				}
				else
					invGoal = goal;
			}
			else
			{
				if ((int)(goal.x - .5f) - 1 < 0)
				{
					transform.position = new Vector3(27.5f, goal.y, 0);
					invGoal = transform.position;
					goal = new Vector3(26.5f, goal.y, 0);
				}

				if (levelRef[(int)(goal.x - .5f) - 1, (int)(goal.y - .5f)] == 1)
				{
					pacSide.transform.localPosition = new Vector3(0, 0, -5);
					pacTop.transform.localPosition = new Vector3(0, 0, 5);
					pacSide.transform.localScale = new Vector3(-1, 1, 1);

					invGoal = new Vector3((int)(goal.x) + .5f, (int)(goal.y) + .5f, 0);
					goal = new Vector3((int)(goal.x) - .5f, (int)(goal.y) + .5f, 0);
				}
				else
					invGoal = goal;
			}
		}
		
		transform.position += new Vector3((goal.x - invGoal.x) * Time.deltaTime * speed, (goal.y - invGoal.y) * Time.deltaTime * speed, 0);
	}

    private void UnSuper()
    {
        is_super = false;
        point_combo = 0;
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Pellet")
		{
            scoring.AddScore();
			Destroy(collision.gameObject);
            dots_eaten++;
        }
        if (collision.tag == "SuperPellet")
        {
            scoring.AddScore();
            Destroy(collision.gameObject);
            dots_eaten++;
            is_super = true;
            point_combo = 100;
            Invoke("UnSuper", 10f);
        }
        if (collision.tag == "Death")
		{
            if(!is_super)
            { 
                if(scoring.Lives > 0)
                {
                    transform.localPosition = initPos;
                    goal = initgoal ;
                    invGoal = initinvgoal ;
                    dir = initdir ;
                    scoring.kill();
                }
                else
                { 
    			Destroy(gameObject);
                scoring.EndGame();
                }
            }
            else
            {
                collision.GetComponentInParent<Ghost>().Death();
                scoring.AddScore(point_combo);
                point_combo *= 2;
            }
		}
	}
}