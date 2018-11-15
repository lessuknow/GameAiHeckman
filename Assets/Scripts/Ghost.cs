using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	public ParseLevel pl;
	protected bool levelSet = false;
	protected int[,] levelRef;
	protected Vector3 moveGoal;
	protected Vector3 invMoveGoal;
	protected int dir = 1;
	public float speed = .2f;
	private float rTime = 0;
	private float rChange = 2f;
	private int rDir = 0;
    public bool passed_door = false;
	protected GameObject[] ghosts;
	public GameObject pacman;

	void Start()
	{
		ghosts = GameObject.FindGameObjectsWithTag("Ghost");
		pacman = GameObject.FindGameObjectWithTag("Player");
	}

	public virtual void Update()
	{

	}

	public void prep()
	{
		if (!levelSet)
		{
			levelSet = true;
			levelRef = new int[pl.levelArray.GetLength(0),pl.levelArray.GetLength(1)];
            for(int i=0;i < pl.levelArray.GetLength(0);i++)
            {
                for(int j=0;j<pl.levelArray.GetLength(1);j++)
                {
                    levelRef[i, j] = pl.levelArray[i, j];
                }
            }

			invMoveGoal = transform.position;
			moveGoal = new Vector3(transform.position.x, transform.position.y, -5);
		}
	}

	public int getDirection()
	{
		return dir;
	}

	public bool atGoal()	//returns true if the ghost is centered on goal, and thus can change dimension of direction
	{
		return Mathf.Abs(moveGoal.x - transform.position.x) < .1f && Mathf.Abs(moveGoal.y - transform.position.y) < .1f;
	}

	public bool atCrossroad()   //returns true if a turn can be made
	{
		bool up = canMoveUp();
		bool right = canMoveRight();
		bool down = canMoveDown();
		bool left = canMoveLeft();

		int paths = 0;
		if (up) paths++;
		if (right) paths++;
		if (down) paths++;
		if (left) paths++;

		return paths > 2;
	}

	public void pathContinue()
	{
		if (dir == 0 && !canMoveUp())
		{
			if (!setGoalRight())
				if (!setGoalLeft())
					setGoalDown();
		}
		else if (dir == 1 && !canMoveRight())
		{
			if (!setGoalUp())
				if (!setGoalDown())
					setGoalLeft();
		}
		else if (dir == 2 && !canMoveDown())
		{
			if (!setGoalRight())
				if (!setGoalLeft())
					setGoalUp();
		}
		else if (dir == 3 && !canMoveLeft())
		{
			if (!setGoalDown())
				if (!setGoalUp())
					setGoalRight();
		}
	}

	//the set goal methods attempt to set the ghost's movement goal to a specified direction, return false if that direction is blocked by a wall (or once implemented, a ghost)

	public bool setGoalUp()
	{
		for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.x - transform.position.x) < 1f && ghosts[i].transform.position.y - transform.position.y < 2f && ghosts[i].transform.position.y - transform.position.y > 0)
				return false;
		}

		if (dir == 2)
		{
			dir = 0;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.y - .5f) + 1 > 31) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) + 1] >= 1) && atGoal())
		{
			if (dir != 0)
				transform.position = moveGoal;
			dir = 0;
			return true;
		}

		return false;
	}

	public bool canMoveUp()
	{
		for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.x - transform.position.x) < 1f && ghosts[i].transform.position.y - transform.position.y < 2f && ghosts[i].transform.position.y - transform.position.y > 0)
				return false;
		}
		if (dir == 2)
			return true;
		if (!((int)(moveGoal.y - .5f) + 1 > 31) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) + 1] >= 1) && atGoal())
			return true;
		return false;
	}

	public bool setGoalRight()
	{
		for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.y - transform.position.y) < 1f && ghosts[i].transform.position.x - transform.position.x < 2f && ghosts[i].transform.position.x - transform.position.x > 0)
				return false;
		}

		if (dir == 3)
		{
			dir = 1;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.x - .5f) + 1 > 27) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) + 1, (int)(moveGoal.y - .5f)] >= 1) && atGoal())
		{
			if (dir != 1)
				transform.position = moveGoal;
			dir = 1;
			return true;
		}
		return false;
	}

	public bool canMoveRight()
	{
		for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.y - transform.position.y) < 1f && ghosts[i].transform.position.x - transform.position.x < 2f && ghosts[i].transform.position.x - transform.position.x > 0)
				return false;
		}
		if (dir == 3)
			return true;
		if (!((int)(moveGoal.x - .5f) + 1 > 27) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) + 1, (int)(moveGoal.y - .5f)] >= 1) && atGoal())
			return true;
		return false;
	}

	public bool setGoalDown()
	{
		for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.x - transform.position.x) < 1f && ghosts[i].transform.position.y - transform.position.y > -2f && ghosts[i].transform.position.y - transform.position.y < 0)
				return false;
		}

		if (dir == 0)
		{
			dir = 2;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.y - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) - 1] >= 1) && atGoal())
		{
			if (dir != 2)
				transform.position = moveGoal;
			dir = 2;
			return true;
		}
		return false;
	}

	public bool canMoveDown()
	{
        for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.x - transform.position.x) < 1f && ghosts[i].transform.position.y - transform.position.y > -2f && ghosts[i].transform.position.y - transform.position.y < 0)
				return false;
		}
		if (dir == 0)
			return true;
		if (!((int)(moveGoal.y - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) - 1] >= 1) && atGoal())
			return true;
		return false;
	}

	public bool setGoalLeft()
	{
		for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.y - transform.position.y) < 1f && ghosts[i].transform.position.x - transform.position.x > -2f && ghosts[i].transform.position.x - transform.position.x < 0)
				return false;
		}

		if (dir == 1)
		{
			dir = 3;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.x - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) - 1, (int)(moveGoal.y - .5f)] >= 1) && atGoal())
		{
			if (dir != 3)
				transform.position = moveGoal;
			dir = 3;
			return true;
		}
		return false;
	}

	public bool canMoveLeft()
	{
		for (int i = 0; i < ghosts.Length; i++)
		{
			if (Mathf.Abs(ghosts[i].transform.position.y - transform.position.y) < 1f && ghosts[i].transform.position.x - transform.position.x > -2f && ghosts[i].transform.position.x - transform.position.x < 0)
				return false;
		}
		if (dir == 1)
			return true;
		if (!((int)(moveGoal.x - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) - 1, (int)(moveGoal.y - .5f)] >= 1) && atGoal())
			return true;
		return false;
	}

	public void ghostCollisions()
	{
		/*if (ghosts.Length == 0)
			return;

		for (int i = 0; i < ghosts.Length; i++)
		{
			int nearDir = ghosts[i].GetComponent<Ghost>().getDirection();
			if (dir != nearDir && Vector3.Distance(ghosts[i].transform.position, transform.position) <= 3f)
			{
				if (!atGoal())
				{
					Vector3 temp = moveGoal;
					moveGoal = invMoveGoal;
					invMoveGoal = temp;
					if (dir > 1)
						dir -= 2;
					else
						dir += 2;
				}
				else if (dir == 0)
				{
					if (!setGoalDown())
					{
						if (!setGoalLeft())
							setGoalRight();
					}
				}
				else if (dir == 1)
				{
					if (!setGoalLeft())
					{
						if (!setGoalUp())
							setGoalDown();
					}
				}
				else if (dir == 2)
				{
					if (!setGoalUp())
					{
						if (!setGoalLeft())
							setGoalRight();
					}
				}
				else
				{
					if (!setGoalRight())
					{
						if (!setGoalUp())
							setGoalDown();
					}
				}
			}
		}*/
	}

	public void move()	//the movement method of the ghost
	{
		if (atGoal())
		{
			if (dir == 0)
			{
				if ((int)(moveGoal.y - .5f) + 1 > 31 && (moveGoal.y - .5f) % 1f == 0)
					moveGoal = new Vector3(moveGoal.x, 0.5f, -5);
				if (levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) + 1] >= 1)
				{
					invMoveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) + .5f, -5);
					moveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) + 1.5f, -5);
				}
				else
					invMoveGoal = moveGoal;
			}
			else if (dir == 1)
			{
				if ((int)(moveGoal.x - .5f) + 1 > 27 && (moveGoal.x - .5f) % 1f == 0)
					moveGoal = new Vector3(0.5f, moveGoal.y, -5);

				if (levelRef[(int)(moveGoal.x - .5f) + 1, (int)(moveGoal.y - .5f)] >= 1)
				{
					invMoveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) + .5f, -5);
					moveGoal = new Vector3((int)(moveGoal.x) + 1.5f, (int)(moveGoal.y) + .5f, -5);
				}
				else
					invMoveGoal = moveGoal;
			}
			else if (dir == 2)
			{
				if ((int)(moveGoal.y - .5f) - 1 < 0 && (moveGoal.y - .5f) % 1f == 0)
					moveGoal = new Vector3(moveGoal.x, 31.5f, -5);

				if (levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) - 1] >= 1)
				{
					invMoveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) + .5f, -5);
					moveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) - .5f, -5);
				}
				else
					invMoveGoal = moveGoal;
			}
			else if (dir == 3)
			{
				if ((int)(moveGoal.x - .5f) - 1 < 0 && (moveGoal.x - .5f) % 1f == 0)
					moveGoal = new Vector3(27.5f, moveGoal.y, -5);

				if (levelRef[(int)(moveGoal.x - .5f) - 1, (int)(moveGoal.y - .5f)] >= 1)
				{
					invMoveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) + .5f, -5);
					moveGoal = new Vector3((int)(moveGoal.x) - .5f, (int)(moveGoal.y) + .5f, -5);
				}
				else
					invMoveGoal = moveGoal;
			}
		}
        if(!passed_door)
            if(pl.tm.GetTile(pl.tm.WorldToCell(transform.position))&&
                pl.tm.GetTile(pl.tm.WorldToCell(transform.position)).name == "Door")
            {
                print(levelRef[13, 18]);
                passed_door = true;
                levelRef[13, 18] = 0;
                levelRef[14, 18] = 0;
            }
		transform.position += new Vector3((moveGoal.x - invMoveGoal.x) * Time.deltaTime * speed, (moveGoal.y - invMoveGoal.y) * Time.deltaTime * speed, 0);
	}
}
