using System.Collections;
using UnityEngine;

public class PinnedQuestPanel : MonoBehaviour
{
    [SerializeField]
    private float hideAfterTime = 20;

    private void Start()
    {
        StartCoroutine(HideAfterTime());
    }

    private IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(hideAfterTime);
        gameObject.SetActive(false);
    }
}
