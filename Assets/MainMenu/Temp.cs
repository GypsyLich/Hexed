using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour

{
    public Dropdown dropdown;
    Resolution[] res;
    void Start()
    {
        UnityEngine.Resolution[] resolution = Screen.resolutions;
        /*for (int i = 0; i < resolution.Length; i++) //Вывод доступных разрешений в консоль
        {
            Debug.Log(resolution[i]);
        }*/
        res = resolution.Distinct().ToArray(); //Убирает одинаковые разрешения экранов из списка
        string[] strRes = new string[res.Length];
        for (int i = 0; i < resolution.Length; i++) //Вывод доступных разрешений в консоль
        {
            //strRes[i] = res[i].ToString();
            strRes[i] = res[i].width.ToString() + "x" + res[i].height.ToString();
            Screen.SetResolution(res[res.Length - 1].width, res[res.Length - 1].height, true);

        }
        dropdown.ClearOptions();
        dropdown.AddOptions(strRes.ToList());
    }
    public void SetRes()
    {
        Screen.SetResolution(res[dropdown.value].width, res[dropdown.value].height, true);
    }
}