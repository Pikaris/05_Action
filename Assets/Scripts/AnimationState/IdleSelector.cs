using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSelector : StateMachineBehaviour
{
    const int Not_Select = -1;
    public int testSelect = Not_Select;

    readonly int idleSelect_Hash = Animator.StringToHash("IdleSelect");

    int prevSelect = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(idleSelect_Hash, RandomSelect());
    }

    int RandomSelect()
    {
        int select = 0;

        // 이전 선택이 0번일 경우에만 일정 확률로 1~4를 선택
        if (prevSelect == 0)
        {
            float num = Random.value;

            if (num < 0.01f)
            {
                select = 4;         // 1%
            }
            else if(num < 0.02f)
            {
                select = 3;         // 1%
            }
            else if (num < 0.03f)
            {
                select = 2;         // 1%
            }
            else if (num < 0.04f)
            {
                select = 1;         // 1%
            }
        }

        // testSelect가 Not_Select가 아닌 경우 무조건 설정된 값으로 변경(0~4만 가능)

        if (testSelect != Not_Select)
        {
            select = Mathf.Clamp(testSelect, 0, 4);
        }

        prevSelect = select;
        return select;
    }
}
