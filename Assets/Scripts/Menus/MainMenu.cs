using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    public string SinglePlayerButton_Text;
    public string QuitButton_Text;

    public float ScreenPercentageYOffset;
    public float ButtonWidth;
    public float ButtonHeight;
    public float ButtonVerticalSpacing;

    void OnGUI()
    {
        int i = 0;

        // singleplayer button
        if (
            GUI.Button(
            new Rect(Screen.width / 2 - ButtonWidth / 2,
                     (ScreenPercentageYOffset / 100) * Screen.height + (ButtonHeight  + ButtonVerticalSpacing)* i++,
                     ButtonWidth,
                     ButtonHeight),
            new GUIContent(SinglePlayerButton_Text))
            )
        {
            Application.LoadLevel("Prototype1");
        }

        // exit button
        if (
            GUI.Button(
            new Rect(Screen.width / 2 - ButtonWidth / 2,
            (ScreenPercentageYOffset / 100) * Screen.height + (ButtonHeight + ButtonVerticalSpacing) * i++,
            ButtonWidth,
            ButtonHeight),
            new GUIContent(QuitButton_Text))
            )
        {
            Debug.Log("Quit pressed. Quit does not work in the editor, only in standalone builds.");
            Application.Quit();
        }
    }
}
