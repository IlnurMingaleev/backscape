using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;
using UnityEngine.Events;
using Unity.EditorCoroutines.Editor;

[System.Serializable]
[ExecuteInEditMode]
public class UIExample : EditorWindow
{
    #region Class data
    private VisualTreeAsset tutorialPage;
    private VisualTreeAsset videoPage;
    private VisualTreeAsset listPage;
    private StyleSheet mutualStyle;
    private Action<Button> OnItemClicked;
    private Action<Button> OnBackButtonClicked;
    private Dictionary<string, Button> uiButtons;

    private string currentPage;

    private float transitionDuration = 1;

    private EasingMode easing = EasingMode.EaseInOut;

    private StyleList<EasingFunction> easingValues;

    private List<TimeValue> durationValues;


    private ScrollView scrollView;

    private string animatedClass = "scrollview";

    private string marginClass = "margin_left_scrollview";

    #endregion

    #region Initialize GUI

    [MenuItem("UI/UIExample")]
    public static void ShowWindow()
    {
        UIExample uiExample = GetWindow<UIExample>();
        uiExample.titleContent = new GUIContent("UIExample");
    }

    /// <summary>
    /// Initialize GUI
    /// </summary>
    public void CreateGUI()
    {
        //Initialize UI buttons dictionary
        uiButtons = new Dictionary<string, Button>();

        // Initialising values for transition
        durationValues = new List<TimeValue>() { new TimeValue(transitionDuration, TimeUnit.Second) };
        easingValues = new StyleList<EasingFunction>(new List<EasingFunction> { new EasingFunction(easing) });


        // Load UXML files to UnityEditor window. One as default. Other for next button onklick events.
        mutualStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Example/uxml&uss/style.uss");
        tutorialPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/tutorial_page.uxml");
        videoPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/video_page.uxml");
        listPage = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Example/uxml&uss/list_page.uxml");
        rootVisualElement.styleSheets.Add(mutualStyle);

        //Load first page of UI
        LoadTutorialPage();


    }

    #endregion

    #region Courotine IEnumerator
    /// <summary>
    /// IEnumerator for performing transitions
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformTransition()
    {
        scrollView = rootVisualElement.Q<ScrollView>("scrollview");
        scrollView.style.transitionDuration = durationValues;
        scrollView.style.transitionTimingFunction = easingValues;

        scrollView.AddToClassList(marginClass);
        scrollView.AddToClassList(animatedClass);
        yield return null;
        scrollView.RemoveFromClassList(marginClass);
        //yield return new WaitForSeconds(transitionDuration);
        yield return new WaitForSeconds(transitionDuration);
        scrollView.RemoveFromClassList(animatedClass);
        LoadButtons(currentPage);
        //Destroy(coroutineRunnerWorker.gameObject);
        
    }
    #endregion

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

    #region Manage Visual elements
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
        VisualElement templateContainer = treeAsset.Instantiate();
        templateContainer.style.height = Length.Percent(100);
        rootVisualElement.Add(templateContainer);
        rootVisualElement.styleSheets.Add(mutualStyle);
        rootVisualElement.contentContainer.style.height = Length.Percent(100);
    }

    

 

    /// <summary>
    /// Start couroutine for transitions
    /// </summary>
    private void RunCouroutine()
    {
        EditorCoroutineUtility.StartCoroutine(PerformTransition(),this);
    }

    /// <summary>
    /// Add events to buttons every time they are loaded
    /// </summary>
    /// <param name="name"></param>
    private void LoadButtons(string name)
    {
        switch (name)
        {
            case "tutorialPage":
                OnItemClicked = null;
                OnItemClicked += AddListenerItemButton;
                AddEventToButtonsByName("item", OnItemClicked);
                break;
            case "listPage":
                OnItemClicked = null;
                OnItemClicked += AddListenerListItemBtn;
                AddEventToButtonsByName("list_item", OnItemClicked);
                break;
            case "videoPage":
                OnItemClicked = null;
                break;
        }
        
        OnBackButtonClicked = null;
        OnBackButtonClicked = AddListenerBackButton;
        AddEventToButtonsByName("back_btn", OnBackButtonClicked);
        AddEventToButtonsByName("menu_btn_back", OnBackButtonClicked);
    }
    #endregion

    #region Load Each page methoods.
    /// <summary>
    /// Load first page
    /// </summary>
    private void LoadTutorialPage()
    {
        //Add on click events to all clickable items

        LoadVisuals(tutorialPage);
        currentPage = "tutorialPage";
        RunCouroutine();
        //ResetContent();

    }

    /// <summary>
    /// Load Second page.
    /// </summary>
    private void LoadListPage() 
    {
        LoadVisuals(listPage);
        currentPage = "listPage";
        RunCouroutine();
    }

    /// <summary>
    /// Load third page
    /// </summary>
    private void LoadVideoPage()
    {
        LoadVisuals(videoPage);
        currentPage = "videoPage";
        RunCouroutine();
    }

    #endregion
}
