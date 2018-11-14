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
	protected List<Ghost> nearGhosts;
	public Ghost ghost1;
	public Ghost ghost2;
	public Ghost ghost3;
	public GameObject pacman;

	void Start()
	{
		nearGhosts = new List<Ghost>();
		nearGhosts.Add(ghost1);
		nearGhosts.Add(ghost2);
		nearGhosts.Add(ghost3);
		pacman = GameObject.FindGameObjectWithTag("Player");
	}

	public virtual void Update()
	{
		prep();

		if (atGoal())
		{
			pathContinue();
		}

		move();
	}

	protected void prep()
	{
		if (!levelSet)
		{
			levelSet = true;
			levelRef = pl.levelArray;
			invMoveGoal = transform.position;
			moveGoal = new Vector3(transform.position.x + 1, transform.position.y, -5);
		}
	}

	public int getDirection()
	{
		return dir;
	}

	protected bool atGoal()	//returns true if the ghost is centered on goal, and thus can change dimension of direction
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

	protected void pathContinue()
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
		if (dir == 2)
		{
			dir = 0;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.y - .5f) + 1 > 31) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) + 1] == 1) && atGoal())
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
		if (dir == 2)
			return true;
		if (!((int)(moveGoal.y - .5f) + 1 > 31) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) + 1] == 1) && atGoal())
			return true;
		return false;
	}

	public bool setGoalRight()
	{
		if (dir == 3)
		{
			dir = 1;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.x - .5f) + 1 > 27) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) + 1, (int)(moveGoal.y - .5f)] == 1) && atGoal())
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
		if (dir == 3)
			return true;
		if (!((int)(moveGoal.x - .5f) + 1 > 27) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) + 1, (int)(moveGoal.y - .5f)] == 1) && atGoal())
			return true;
		return false;
	}

	public bool setGoalDown()
	{
		if (dir == 0)
		{
			dir = 2;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.y - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) - 1] == 1) && atGoal())
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
		if (dir == 0)
			return true;
		if (!((int)(moveGoal.y - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) - 1] == 1) && atGoal())
			return true;
		return false;
	}

	public bool setGoalLeft()
	{
		if (dir == 1)
		{
			dir = 3;
			Vector3 temp = moveGoal;
			moveGoal = invMoveGoal;
			invMoveGoal = temp;
			return true;
		}
		else if (!((int)(moveGoal.x - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) - 1, (int)(moveGoal.y - .5f)] == 1) && atGoal())
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
		if (dir == 1)
			return true;
		if (!((int)(moveGoal.x - .5f) - 1 < 0) && ((moveGoal.y - .5f) % 1f == 0 && (moveGoal.x - .5f) % 1f == 0 &&
			levelRef[(int)(moveGoal.x - .5f) - 1, (int)(moveGoal.y - .5f)] == 1) && atGoal())
			return true;
		return false;
	}

	public void ghostCollisions(bool smart)
	{
		if (nearGhosts.Count == 0)
			return;

		for (int i = 0; i < nearGhosts.Count; i++)
		{
			int nearDir = nearGhosts[i].getDirection();
			if (dir != nearDir && Vector3.Distance(nearGhosts[i].transform.position, transform.position) <= 2f)
			{
				if (!smart)
					autoAvoid();
				else
					smartAvoid();
			}
		}
	}

	private void autoAvoid()	//tries to find any new direction
	{
		bool success = setGoalUp();
		if (success && dir != 0)
			return;
		success = setGoalRight();
		if (success && dir != 1)
			return;
		success = setGoalDown();
		if (success && dir != 2)
			return;
		setGoalLeft();
		return;
	}

	private void smartAvoid()	//makes use of direction to goal (note implemented yet)
	{
		bool success = setGoalUp();
		if (success)
			return;
		success = setGoalRight();
		if (success)
			return;
		success = setGoalDown();
		if (success)
			return;
		setGoalLeft();
		return;
	}

	public void move()	//the movement method of the ghost
	{
		if (atGoal())
		{
			if (dir == 0)
			{
				if ((int)(moveGoal.y - .5f) + 1 > 31 && (moveGoal.y - .5f) % 1f == 0)
					moveGoal = new Vector3(moveGoal.x, 0.5f, 0);
				if (levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) + 1] == 1)
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
					moveGoal = new Vector3(0.5f, moveGoal.y, 0);

				if (levelRef[(int)(moveGoal.x - .5f) + 1, (int)(moveGoal.y - .5f)] == 1)
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
					moveGoal = new Vector3(moveGoal.x, 31.5f, 0);

				if (levelRef[(int)(moveGoal.x - .5f), (int)(moveGoal.y - .5f) - 1] == 1)
				{
					invMoveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) + .5f, -5);
					moveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) - .5f, -5);
				}
				else
					invMoveGoal = moveGoal;
			}
			else
			{
				if ((int)(moveGoal.x - .5f) - 1 < 0 && (moveGoal.x - .5f) % 1f == 0)
					moveGoal = new Vector3(27.5f, moveGoal.y, 0);

				if (levelRef[(int)(moveGoal.x - .5f) - 1, (int)(moveGoal.y - .5f)] == 1)
				{
					invMoveGoal = new Vector3((int)(moveGoal.x) + .5f, (int)(moveGoal.y) + .5f, -5);
					moveGoal = new Vector3((int)(moveGoal.x) - .5f, (int)(moveGoal.y) + .5f, -5);
				}
				else
					invMoveGoal = moveGoal;
			}
		}

		transform.position += new Vector3((moveGoal.x - invMoveGoal.x) * Time.deltaTime * speed, (moveGoal.y - invMoveGoal.y) * Time.deltaTime * speed, 0);
	}
}
