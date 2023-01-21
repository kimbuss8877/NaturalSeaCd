using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T m_Instance = null;

	public static T instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

				if (m_Instance == null)
					Debug.LogError("No instance of " + typeof(T).ToString());
				else
					m_Instance.Init();
			}
			return m_Instance;
		}
	}

	private void Awake()
	{
		if (m_Instance == null)
		{
			m_Instance = this as T;
			m_Instance.Init();
		}
	}

	public virtual void Init() { } // 초기화를 상속을 통해 구현    

	private void OnApplicationQuit()
	{
		m_Instance = null;
	}
}