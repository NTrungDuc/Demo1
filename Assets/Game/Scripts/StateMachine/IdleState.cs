using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float timer = 0;
    private float ranTime;
    public void OnEnter(BotController bot)
    {
        ranTime = Random.Range(0.5f, 1f);
        bot.ChangeAnim(Constant.ANIM_WALK, false);
        bot.stopMoving();
    }
    public void OnExecute(BotController bot)
    {
        timer += Time.deltaTime;
        if (timer > ranTime)
        {
            bot.ChangeState(new PatrolState());
        }
    }
    public void OnExit(BotController bot)
    {

    }
}
