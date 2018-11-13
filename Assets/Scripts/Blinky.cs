using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : MonoBehaviour
{
	public ParseLevel pl;
	private bool levelSet = false;
	private int[,] levelRef;
	private Vector3 goal;
	private Vector3 invGoal;
	private int dir = 1;
	private float t = 0;
	public float speed = .2f;
	private float rTime = 0;
	private float rChange = .3f;
	private int rDir = 0;

	void Start()
	{

	}

	void Update()
	{
		if (!levelSet)
		{
			levelSet = true;
			levelRef = pl.levelArray;
			invGoal = transform.position;
			goal = new Vector3(transform.position.x + 1, transform.position.y, -5);
		}

		t += Time.deltaTime;
		rTime += Time.deltaTime;
		transform.position = Vector3.Lerp(invGoal, goal, t / speed);

		if (rTime > rChange)
		{
			rDir = Mathf.FloorToInt(Random.value * 4);
		}

		if (rDir == 0)
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) + 1] >= 1))
			{
				dir = 0;
				t = 0;
			}
		}
		else if (rDir == 1)
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f) + 1, (int)(transform.position.y - .5f)] >= 1))
			{
				dir = 1;
				t = 0;
			}
		}
		else if (rDir == 2)
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) - 1] >= 1))
			{
				dir = 2;
				t = 0;
			}
		}
		else if (rDir == 3)
		{
			if (((transform.position.y - .5f) % 1f == 0 && (transform.position.x - .5f) % 1f == 0 &&
				levelRef[(int)(transform.position.x - .5f) - 1, (int)(transform.position.y - .5f)] >= 1))
			{
				dir = 3;
				t = 0;
			}
		}

		if (transform.position == goal || t == 0)
		{
			t = 0;

			if (dir == 0)
			{
				if ((int)(transform.position.y - .5f) + 1 > 31 && (transform.position.y - .5f) % 1f == 0)
					transform.position = new Vector3(transform.position.x, 0, -5);
				if (levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) + 1] >= 1)
				{
					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + 1.5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) - .5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
					dir = 2;
				}
			}
			else if (dir == 1)
			{
				if ((int)(transform.position.x - .5f) + 1 > 27 && (transform.position.x - .5f) % 1f == 0)
					transform.position = new Vector3(0, transform.position.y, -5);

				if (levelRef[(int)(transform.position.x - .5f) + 1, (int)(transform.position.y - .5f)] >= 1)
				{
					goal = new Vector3((int)(transform.position.x) + 1.5f, (int)(transform.position.y) + .5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) - .5f, (int)(transform.position.y) + .5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
					dir = 3;
				}
			}
			else if (dir == 2)
			{
				if ((int)(transform.position.y - .5f) - 1 < 0 && (transform.position.y - .5f) % 1f == 0)
					transform.position = new Vector3(transform.position.x, 31, -5);

				if (levelRef[(int)(transform.position.x - .5f), (int)(transform.position.y - .5f) - 1] >= 1)
				{
					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) - .5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + 1.5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
					dir = 0;
				}
			}
			else
			{
				if ((int)(transform.position.x - .5f) - 1 < 0 && (transform.position.x - .5f) % 1f == 0)
					transform.position = new Vector3(27, transform.position.y, -5);

				if (levelRef[(int)(transform.position.x - .5f) - 1, (int)(transform.position.y - .5f)] >= 1)
				{
					goal = new Vector3((int)(transform.position.x) - .5f, (int)(transform.position.y) + .5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
				}
				else
				{
					goal = new Vector3((int)(transform.position.x) + 1.5f, (int)(transform.position.y) + .5f, -5);
					invGoal = new Vector3((int)(transform.position.x) + .5f, (int)(transform.position.y) + .5f, -5);
					dir = 1;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Ghost")
		{
			if (dir > 1)
				dir -= 2;
			else
				dir += 2;
		}
	}
}
