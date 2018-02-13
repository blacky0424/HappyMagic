using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シンデレラのAnimator管理クラス
/// </summary>
[System.Serializable]
public class CinderellaAnimator
{
    public enum ParamName
    {
        MoveSpeed,
        Stalking,
        DoMouseSet,
        DoChange,
        DoPumpkin,
    }

    static readonly Dictionary<ParamName, int> s_paramHashDict = new Dictionary<ParamName, int>()
    {
        { ParamName.MoveSpeed,  Animator.StringToHash("MoveSpeed") },
        { ParamName.Stalking,   Animator.StringToHash("Stalking") },
        { ParamName.DoMouseSet, Animator.StringToHash("DoMouseSet") },
        { ParamName.DoChange,   Animator.StringToHash("DoChange") },
        { ParamName.DoPumpkin,  Animator.StringToHash("DoPumpkin") },
    };

    public enum StateName
    {
        Idle,
        Locomotion,
        Mouse,
        StalkingStart,
        Stalking,
        StalkingEnd,
        Change,
        Listen,
        SetPumpkin,
    }

    static readonly Dictionary<StateName, int> s_stateHashDict = new Dictionary<StateName, int>()
    {
        { StateName.Idle,       Animator.StringToHash("Base Layer.Idle") },
        { StateName.Locomotion, Animator.StringToHash("Base Layer.Locomotion") },
        { StateName.Mouse,      Animator.StringToHash("Base Layer.Mouse") },
        { StateName.StalkingStart, Animator.StringToHash("Base Layer.StalkingStart") },
        { StateName.Stalking,      Animator.StringToHash("Base Layer.Stalking") },
        { StateName.StalkingEnd,   Animator.StringToHash("Base Layer.StalkingEnd")},
        { StateName.Change, Animator.StringToHash("Base Layer.Change") },
        { StateName.Listen, Animator.StringToHash("Base Layer.Listen") },
        { StateName.SetPumpkin, Animator.StringToHash("Base Layer.SetPumpkin") },
    };

    List<Animator> m_animatorList;

    public CinderellaAnimator(Animator[] animator)
    {
        m_animatorList = new List<Animator>(animator);
    }
        
    public Animator ActiveComponent
    {
        get
        {
            var ret = m_animatorList.Find(a => a.gameObject.activeSelf);
            return ret ?? m_animatorList[0];
        }
    }

    public bool IsActive
    {
        get
        {
            return ActiveComponent.gameObject.activeSelf;
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
            return ActiveComponent.GetCurrentAnimatorStateInfo(0).fullPathHash;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return ActiveComponent.GetFloat(s_paramHashDict[ParamName.MoveSpeed]);
        }
        set
        {
            if (IsActive)
            {
                ActiveComponent.SetFloat(s_paramHashDict[ParamName.MoveSpeed], value);
            }
        }
    }

    public bool Stalking
    {
        get
        {
            return ActiveComponent.GetBool(s_paramHashDict[ParamName.Stalking]);
        }
        set
        {
            if (IsActive)
            {
                ActiveComponent.SetBool(s_paramHashDict[ParamName.Stalking], value);
            }
        }
    }

    public void SetDoMouseSet()
    {
        if (IsActive)
        {
            ActiveComponent.SetTrigger(s_paramHashDict[ParamName.DoMouseSet]);
        }
    }

    public void ResetDoMouseSet()
    {
        if (IsActive)
        {
            ActiveComponent.ResetTrigger(s_paramHashDict[ParamName.DoMouseSet]);
        }
    }

    public void SetDoChange()
    {
        if(IsActive)
        {
            ActiveComponent.SetTrigger(s_paramHashDict[ParamName.DoChange]);
        }
    }

    public void ResetDoChange()
    {
        if(IsActive)
        {
            ActiveComponent.ResetTrigger(s_paramHashDict[ParamName.DoChange]);
        }
    }

    public void SetDoPumpkin()
    {
        if (IsActive)
        {
            ActiveComponent.SetTrigger(s_paramHashDict[ParamName.DoPumpkin]);
        }
    }

    public void ResetDoPumpkin()
    {
        if (IsActive)
        {
            ActiveComponent.ResetTrigger(s_paramHashDict[ParamName.DoPumpkin]);
        }
    }
}
