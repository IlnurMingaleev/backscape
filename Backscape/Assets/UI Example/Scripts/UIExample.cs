using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class UIExample: EditorWindow
{
    private VisualTreeAsset tutorialPage;
    private VisualTreeAsset videoPage;
    private VisualTreeAsset listPage;
    private StyleSheet mutualStyle;

    private Dictionary<string, Button> uiButtons;

    private string currentPage;

    private string[] pageNames = new string[] { "tutorialPage", "listPage", "videoPage" };

    [MenuItem("UI/UIExample")]

    public static void ShowWindow() 
    {
        UIExample uiExample = GetWindow<UIExample>();
        uiExample.titleContent = new GUIContent("UIExample");

        

    }

    // Load UXML files to UnityEditor window. One as default. Other for next button onklick events.
    public void CreateGUI()
    {
        //Initialize UI buttons dictionary
        uiButtons = new Dictionary<string, Button>();




        mutualStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Example/uxml&uss/style.uss");
        tutorialPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/tutorial_page.uxml");
        videoPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/video_page.uxml");
        listPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/list_page.uxml");
        rootVisualElement.styleSheets.Add(mutualStyle);


        rootVisualElement.Add(tutorialPage.Instantiate());
        uiButtons.Add("item", rootVisualElement.Q<Button>("item"));


        /*videoPage = new VisualElement();
        
        videoPage.Add(videoPageVTA.Instantiate());
        uiButtons.Add("menu_btn_back", videoPage.Q<Button>("menu_btn_back"));

        listPage = new VisualElement();
        
        listPage.Add(listPageVTA.Instantiate());
        uiButtons.Add("list_item", videoPage.Q<Button>("list_item"));

        
        tutorialPage.styleSheets.Add(mutualStyle);
        videoPage.styleSheets.Add(mutualStyle);
        listPage.styleSheets.Add(mutualStyle);*/
        Button button = uiButtons["item"];
        if (button != null)
        {
            button.clickable.clicked += ItemClicked;
        }
        else 
        {
            Debug.Log("Button is null");
        }
        
        //["menu_btn_back"].RegisterCallback<MouseDownEvent>(BackButtonClicked);

    }

    private void ClearRootVisualElement() 
    {
        rootVisualElement.Clear();
    }
    private void LoadVisuals(VisualTreeAsset treeAsset) 
    {
        ClearRootVisualElement();
        rootVisualElement.Add(treeAsset.Instantiate());
        rootVisualElement.styleSheets.Add(mutualStyle);
    }
    private void ItemClicked() 
    {
        LoadVisuals(listPage);
    }

    private void BackButtonClicked() 
    {
        switch (rootVisualElement.name) 
        {
            case "list_page":
                LoadVisuals(tutorialPage);
                break;
            case "video_page":
                LoadVisuals(listPage);
                break ;
            
        }
    
    }
}
