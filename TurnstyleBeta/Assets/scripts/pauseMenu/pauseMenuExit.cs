using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuExit : MonoBehaviour
{

    public pauseMenu pauseMenu;

    private bool isShowing = false;

    public GameObject blackBox;
    public Animator blackBoxAnimator;
    private float transitionTime = .5f;

    // Start is called before the first frame update
    void Start()
    {
        blackBox.GetComponent<CanvasRenderer>().SetAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(transitionToMainMenu());
            }
            else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.goBack();
            }
        }

        isShowing = pauseMenu.pauseMenuItemsShowing[0];
    }

    IEnumerator transitionToMainMenu()
    {
        blackBox.GetComponent<CanvasRenderer>().SetAlpha(1);

        blackBoxAnimator.SetTrigger("toBlack");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(0);
    }
}
