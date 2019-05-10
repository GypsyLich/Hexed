using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GraphSetting : MonoBehaviour
{
    public Dropdown dropDown;
    void Start()
    {
        dropDown.AddOptions(QualitySettings.names.ToList()); //добавили массив и преобразовали в коллекцию
        dropDown.value = QualitySettings.GetQualityLevel();
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(dropDown.value);
    }
}
