using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;

public class UIExample: EditorWindow
{
    private VisualTreeAsset tutorialPage;
    private VisualTreeAsset videoPage;
    private VisualTreeAsset listPage;
    private StyleSheet mutualStyle;
    private Action<Button> OnItemClicked;
    private Action<Button> OnBackButtonClicked;
    private Dictionary<string, Button> uiButtons;

    private string currentPage;

    private string[] pageNames = new string[] { "tutorialPage", "listPage", "videoPage" };

    [MenuItem("UI/UIExample")]

    public static void ShowWindow() 
    {
        UIExample uiExample = GetWindow<UIExample>();
        uiExample.titleContent = new GUIContent("UIExample");
    }

    
    public void CreateGUI()
    {
        //Initialize UI buttons dictionary
        uiButtons = new Dictionary<string, Button>();



        // Load UXML files to UnityEditor window. One as default. Other for next button onklick events.
        mutualStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Example/uxml&uss/style.uss");
        tutorialPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/tutorial_page.uxml");
        videoPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/video_page.uxml");
        listPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/list_page.uxml");
        rootVisualElement.styleSheets.Add(mutualStyle);

        //Load first page of UI
        LoadTutorialPage();

        
    }

    #region OnClickEvent Listeners and their methods.

    private void AddEventToButtonsByName(string name, Action<Button> action)
    {
        rootVisualElement.Query<Button>(name).ForEach(action);

    }
    
    /// <summary>
    /// Add listener function to a button called item. Which located on tutorial page.
    /// </summary>
    /// <param name="button"></param>
    private void AddListenerItemButton(Button button) 
    {
        button.clickable.clicked += ItemClicked;
    }
    /// <summary>
    /// Add listener to all back buttons in page
    /// </summary>
    /// <param name="button"></param>
    private void AddListenerBackButton(Button button) 
    {
        button.clickable.clicked += BackButtonClicked;
    }

    private void AddListenerListItemBtn(Button button) 
    {
        button.clickable.clicked += ListItemClicked;
    }

    /// <summary>
    /// Method called on Item clicked
    /// </summary>
    private void ItemClicked()
    {
        LoadListPage();
    }

    /// <summary>
    /// Method called on list_item clicked
    /// </summary>
    private void ListItemClicked()
    {
        LoadVideoPage();
    }

    /// <summary>
    /// Method called on back_btn and menu_btn_back clicked
    /// </summary>
    private void BackButtonClicked()
    {
        switch (currentPage)
        {
            case "listPage":
                LoadTutorialPage();
                break;
            case "videoPage":
                LoadListPage();
                break;

        }

    }
    #endregion

    /// <summary>
    /// Clear all pages from root visual element
    /// </summary>
    private void ClearRootVisualElement() 
    {
        rootVisualElement.Clear();
    }

    /// <summary>
    /// Load new visual tree asset to the rootVisualElement. But before clear all pages
    /// </summary>
    /// <param name="treeAsset"></param>
    private void LoadVisuals(VisualTreeAsset treeAsset) 
    {
        ClearRootVisualElement();
        rootVisualElement.Add(treeAsset.Instantiate());
        rootVisualElement.styleSheets.Add(mutualStyle);
    }

    

    /// <summary>
    /// Load first page
    /// </summary>
    private void LoadTutorialPage() 
    {
        //Add on click events to all clickable items
        LoadVisuals(tutorialPage);
        currentPage = "tutorialPage";
        OnItemClicked = null;
        OnItemClicked += AddListenerItemButton;
        AddEventToButtonsByName("item", OnItemClicked);
        OnBackButtonClicked = null;
        OnBackButtonClicked = AddListenerBackButton;
        AddEventToButtonsByName("back_btn", OnBackButtonClicked);
        AddEventToButtonsByName("menu_btn_back", OnBackButtonClicked);
    }
    
    /// <summary>
    /// Load third page
    /// </summary>
    private void LoadVideoPage() 
    {
        LoadVisuals(videoPage);
        currentPage = "videoPage";
        OnItemClicked = null;
        //OnItemClicked += AddListenerListItemBtn;
        //AddEventToButtonsByName("list_item", OnItemClicked);
        OnBackButtonClicked = null;
        OnBackButtonClicked = AddListenerBackButton;
        AddEventToButtonsByName("back_btn", OnBackButtonClicked);
        AddEventToButtonsByName("menu_btn_back", OnBackButtonClicked);
    }

    /// <summary>
    /// Load Second page.
    /// </summary>
    private void LoadListPage() 
    {
        //Add on click events to all clickable items
        LoadVisuals(listPage);
        currentPage = "listPage";
        OnItemClicked = null;
        OnItemClicked += AddListenerListItemBtn;
        AddEventToButtonsByName("list_item", OnItemClicked);
        OnBackButtonClicked = null;
        OnBackButtonClicked = AddListenerBackButton;
        AddEventToButtonsByName("back_btn", OnBackButtonClicked);
        AddEventToButtonsByName("menu_btn_back", OnBackButtonClicked);
    }

    private void Load(string name) 
    {
        switch (name)
        {
            case "tutorial_page":
                LoadTutorialPage();
                break;
            case "list_page":
                LoadListPage();
                break;
            case "video_page":
                LoadVideoPage();
                break;
        }
    }
    
}
