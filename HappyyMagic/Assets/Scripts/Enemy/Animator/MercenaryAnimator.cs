using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MercenaryAnimator
{
    public enum ParamName
    {
        DoStop,
        DoWalk,
        DoRun,
        DoDiscover,
        DoReactionMouse,
        DoReactionPumpkin
    }

    static readonly Dictionary<ParamName, int> s_paramHashDict = new Dictionary<ParamName, int>()
    {
        { ParamName.DoStop, Animator.StringToHash("DoStop") },
        { ParamName.DoWalk, Animator.StringToHash("DoWalk") },
        { ParamName.DoRun,  Animator.StringToHash("DoRun") },
        { ParamName.DoDiscover,        Animator.StringToHash("DoDiscover") },
        { ParamName.DoReactionMouse,   Animator.StringToHash("DoReactionMouse") },
        { ParamName.DoReactionPumpkin, Animator.StringToHash("DoReactionPumpkin") },
    };

    public enum StateName
    {
        Idle,
        Walk,
        Run,
        Discover,
        ReactionPumpkin,
        ReactionMouse,
    }

    static readonly Dictionary<StateName, int> s_stateHashDict = new Dictionary<StateName, int>()
    {
        { StateName.Idle, Animator.StringToHash("Base Layer.idle") },
        { StateName.Walk, Animator.StringToHash("Base Layer.walk") },
        { StateName.Run,  Animator.StringToHash("Base Layer.run")  },
        { StateName.Discover,        Animator.StringToHash("Base Layer.discover") },
        { StateName.ReactionMouse,   Animator.StringToHash("Base Layer.reaction_mouse") },
        { StateName.ReactionPumpkin, Animator.StringToHash("Base Layer.reaction_pumpkin") },
    };

    Animator m_animator;

    public MercenaryAnimator(Animator animator)
    {
        m_animator = animator;
    }

    public Animator Component
    {
        get
        {
            return m_animator;
        }
    }

    public int GetStateHash(StateName stateName)
    {
        return s_stateHashDict[stateName];
    }

    public int CurrentStateHash
    {
        get
        {
            return Component.GetCurrentAnimatorStateInfo(0).fullPathHash;
        }
    }

    public void SetDoStop()
    {
        m_animator.SetTrigger(s_paramHashDict[ParamName.DoStop]);
    }

    public void ResetDoStop()
    {
        m_animator.ResetTrigger(s_paramHashDict[ParamName.DoStop]);
    }

    public void SetDoWalk()
    {
        m_animator.SetTrigger(s_paramHashDict[ParamName.DoWalk]);
    }

    public void ResetDoWalk()
    {
        m_animator.ResetTrigger(s_paramHashDict[ParamName.DoWalk]);
    }

    public void SetDoRun()
    {
        m_animator.SetTrigger(s_paramHashDict[ParamName.DoRun]);
    }

    public void ResetDoRun()
    {
        m_animator.ResetTrigger(s_paramHashDict[ParamName.DoRun]);
    }

    public void SetDoDiscover()
    {
        m_animator.SetTrigger(s_paramHashDict[ParamName.DoDiscover]);
    }

    public void ResetDoDiscover()
    {
        m_animator.ResetTrigger(s_paramHashDict[ParamName.DoDiscover]);
    }

    public void SetDoReactionPumpkin()
    {
        m_animator.SetTrigger(s_paramHashDict[ParamName.DoReactionPumpkin]);
    }

    public void ResetDoReactionPumpkin()
    {
        m_animator.ResetTrigger(s_paramHashDict[ParamName.DoReactionPumpkin]);
    }

    public void SetDoReactionMouse()
    {
        m_animator.SetTrigger(s_paramHashDict[ParamName.DoReactionMouse]);
    }

    public void ResetDoReactionMouse()
    {
        m_animator.ResetTrigger(s_paramHashDict[ParamName.DoReactionPumpkin]);
    }
}
