using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBrains : MonoBehaviour
{

    public enum phase { scatter, chase, run, leave, idle };
    private phase ps;
    private int phase_num = 0;
    public Ghost red, orange, pink, purple;
    private int scatter_time = 7, chase_time = 20, count = 0;

    private void Start()
    {
        ps = phase.leave;
    }


    private void SetChase()
    {

    }

    private void SetScatter()
    {
        Invoke("SetChase", scatter_time);
        ps = phase.scatter;
    }

    private void SetRun()
    {

    }

    private void DoScatter()
    {
        if(!red.setGoalUp())
        {
            red.setGoalLeft();
        }

        if (!orange.setGoalUp())
        {
            orange.setGoalRight();
        }

        if (!pink.setGoalDown())
        {
            pink.setGoalLeft();
        }

        if (!purple.setGoalDown())
        {
            purple.setGoalRight();
        }
    }

    private void DoChase()
    {
    }



    private void DoLeaveTop()
    {
        if (!purple.setGoalUp())
            purple.setGoalRight();

        if (!red.setGoalUp())
            red.setGoalLeft();
    }

  

    private void DoLeaveBottom()
    {
        if (!orange.setGoalUp())
            orange.setGoalRight();

        if (!pink.setGoalUp())
            pink.setGoalLeft();
    }

    private void DoRun()
    {

    }

    private void Update()
    {
        switch(ps)
        {
            case phase.scatter:
                DoScatter();
                break;
            case phase.chase:
                DoChase();
                break;
            case phase.run:
                DoRun();
                break;
            case phase.idle:
                return;
            case phase.leave:
                DoLeaveTop();
                DoLeaveBottom();
                break;
        }
 
            red.prep();
            if (red.atGoal())
            {
                red.pathContinue();
            }
            red.ghostCollisions();
            red.move();

            purple.prep();
            if (purple.atGoal())
            {
                purple.pathContinue();
            }
            purple.ghostCollisions();
            purple.move();
     
            orange.prep();
            if (orange.atGoal())
            {
                orange.pathContinue();
            }
            orange.ghostCollisions();
            orange.move();

            pink.prep();
            if (pink.atGoal())
            {
                pink.pathContinue();
            }
            pink.ghostCollisions();
            pink.move();
        

    }
    
}
