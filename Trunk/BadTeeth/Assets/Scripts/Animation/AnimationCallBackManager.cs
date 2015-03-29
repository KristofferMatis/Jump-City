using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum CallBackEvents
{
    
};

public interface CallBack
{
    void CallBack(CallBackEvents callBack);
}

public class AnimationCallBackManager : MonoBehaviour 
{
    string[] m_Events = new string[]
    {
        
    };

    protected List<CallBack> m_Listeners = new List<CallBack>();

    public void callBack(string callbackEVent)
    {
        for(int i = 0; i < m_Events.Length; i++)
        {
            if (m_Events[i].Equals(callbackEVent, System.StringComparison.OrdinalIgnoreCase))
            {
                sendcallBackEvent((CallBackEvents)i);
                return;
            }
        }
    }

    void sendcallBackEvent(CallBackEvents callBackEvent)
    {
        for (int i = 0; i < m_Listeners.Count; i++)
        {
            m_Listeners[i].CallBack(callBackEvent);
        }
    }

    public void registerCallBack(CallBack callbackToRegister)
    {
        if(!m_Listeners.Contains(callbackToRegister))
        {
            m_Listeners.Add(callbackToRegister);
        }
    }

    public void removeCallBack(CallBack callbackToRemove)
    {
        if (m_Listeners.Contains(callbackToRemove))
        {
            m_Listeners.Remove(callbackToRemove);
        }
    }
}

