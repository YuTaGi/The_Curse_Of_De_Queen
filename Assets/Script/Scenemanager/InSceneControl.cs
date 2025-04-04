using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InSceneControl : MonoBehaviour
{
    public Image Fader;
    private static InSceneControl instance;

    private GameObject player;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public static void TransitionPlayer(Vector3 pos)
    {
        instance.StartCoroutine(instance.Transition(pos));
    }
   private IEnumerator Transition(Vector3 pos)
    {
        Fader.gameObject.SetActive(true);

        for(float f = 0; f < 1; f += Time.deltaTime / 0.25f)
        {
            Fader.color = new Color(0, 0, 0,Mathf.Lerp(0,1,f));
            yield return null;
        }
        player.transform.position = pos;

        yield return new WaitForSeconds(1);

        for (float f = 0; f < 1; f += Time.deltaTime / 0.25f)
        {
            Fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, f));
            yield return null;
        }
        Fader.gameObject.SetActive(false);
    }
}
