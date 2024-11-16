using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureOpen : MonoBehaviour
{
    public GameObject picture;
    private bool isPictureActive = false;
    private bool isPictureRotated = false;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isPictureActive)
            {
                picture.SetActive(true);
                isPictureActive = true;
            }
            else
            {
                picture.transform.Rotate(0, 180, 0);
                isPictureRotated = true;
                isPictureActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && isPictureRotated)
        {
            isPictureRotated = false;
            picture.SetActive(false);

        }

    }
}
