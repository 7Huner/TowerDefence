using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.P))
		{
            if (Time.timeScale == 1f)
		    {
                PauseGame(0f);
		    }
		    else
		    {
                PauseGame(1f);
		    }
		}
    }

    void PauseGame(float timescale)
	{
        Time.timeScale = timescale;
    }
}
