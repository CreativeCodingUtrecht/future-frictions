using System.Linq;
using UnityEngine;

public class ResultGroup : MonoBehaviour
{
    public Scenarios scenario;
    public ResultObject[] resultObjects;

    private void Awake()
    {
        //foreach (var rO in resultObjects)
        //{
        //    rO.gameObject.SetActive(false);
        //}

        //gameObject.SetActive(false);
    }

    public void Activate(Options selectedOptions)
    {
        gameObject.SetActive(true);

        var result = resultObjects.FirstOrDefault(x => x.option == selectedOptions);

        if (result != null)
        {
            result.gameObject.SetActive(true);
        }
    }
}
