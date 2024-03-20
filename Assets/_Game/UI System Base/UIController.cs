using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace UISystem
{
    //------------------------------------//
    public enum UILayer
    {
        Bottom, Menu, Popup, Top
    }

    //------------------------------------//
    public enum GameUI
    {
        //LoadingUI, LoadingPlayUI, LoadingHomeUI, HomeUI, GameplayUI, /*CoinBarUI,*/ RateUI, SettingUI, UpgradeUI, ShopUI, WinUI, LoseUI, MissionUI, AchievementUI
    }

    //------------------------------------//
    public class UIController : SingletonBehaviour<UIController>
    {
        #region Variables

        private static Dictionary<string, UIBase> LoadedUI = new();
        private static List<Transform> UILayerParents = new();
        private static Stack<UIBase> StackUI = new();

        [field: BoxGroup("Config")]
        [field: HorizontalGroup("Config/Split", 0.5f), LabelWidth(80f), SerializeField]
        public GameUI StartScreen { get; set; }
        [field: HorizontalGroup("Config/Split", 0.5f), LabelWidth(80f), SerializeField]
        public string UIPath { get; set; } = "UI/";
        [field: BoxGroup("Config"), LabelWidth(80f), SerializeField]
        public bool QuitInMenu { get; set; } = true;
        [field: HorizontalGroup("Config/Split1", 0.5f), LabelWidth(80f), SerializeField, Sirenix.OdinInspector.ShowIf("QuitInMenu"), LabelText("Count Back")]
        public int QuitCountBack { get; set; } = 2;
        [field: HorizontalGroup("Config/Split1", 0.5f), LabelWidth(80f), SerializeField, Sirenix.OdinInspector.ShowIf("QuitInMenu"), LabelText("Delay Back")]
        public float DelayQuitBack { get; set; } = 1.5f;

        private int cacheCountBack;
        private float cacheDelayTime;

        public bool Initialized { get; set; }

        #endregion

        #region Unity callback functions

        public override void LoadInAwake()
        {
            //base.LoadInAwake();
            var layerNames = Enum.GetNames(typeof(UILayer));
            foreach (var layerName in layerNames)
            {
                var rectLayer = new GameObject(layerName, typeof(RectTransform)).GetComponent<RectTransform>();
                rectLayer.SetParent(transform);
                rectLayer.SetStretch();
                UILayerParents.Add(rectLayer);
            }

            //LoadUI(GameUI.LoadingUI);
            //LoadUI(GameUI.LoadingPlayUI);
            //LoadUI(GameUI.LoadingHomeUI);
            //LoadUI(GameUI.HomeUI);
            //LoadUI(GameUI.GameplayUI);
            //LoadUI(GameUI.UpgradeUI);
            //LoadUI(GameUI.ShopUI);
            //LoadUI(GameUI.WinUI);
            //LoadUI(GameUI.LoseUI);
            //LoadUI(GameUI.MissionUI);
        }

        public void Start()
        {
            ShowUI(StartScreen);
            Initialized = true;
        }

        private void Update()
        {
            if (QuitInMenu && cacheDelayTime > 0)
            {
                cacheDelayTime -= Time.deltaTime;
                if (cacheDelayTime <= 0f) cacheCountBack = 0;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && StackUI != null && StackUI.Count > 0)
            {
                if (StackUI.Count == 1 && QuitInMenu)
                {
                    cacheCountBack++;
                    if (cacheCountBack == QuitCountBack)
                    {
                        Debug.Log("Quit");
                        Application.Quit();
                    }
                    else cacheDelayTime = DelayQuitBack;
                }
                else
                {
                    StackUI.Peek().OnBackPressed();
                }
            }
        }

        #endregion

        #region Control functions

        // Check ui truyền vào có phải là ui đang được hiện không
        // trả về bool cho vào if
        public static bool IsLastUI(GameUI ui)
        {
            if (StackUI.Count != 1)
            {
                var currentUI = StackUI.Pop();
                var lastUI = StackUI.Peek();
                StackUI.Push(currentUI);
                if (lastUI.UIID == Enum.GetName(typeof(GameUI), ui)) return true;
            }
            return false;
        }

        // Lấy cái script ui truyền vào cái enum của ui đấy
        // cái này để thay đổi giá trị có trong cái ui cần đổi
        // vd như home ui có cái float a, mà đang trong gameplay ui muốn đổi float a đấy thì dùng cái này
        public static UIBase GetUI(GameUI ui)
        {
            foreach (var uiBase in StackUI)
            {
                if (uiBase.UIID == Enum.GetName(typeof(GameUI), ui)) return uiBase;
            }
            return null;
        }

        // loadui nếu như không có trong stack, cái này để load lúc ui đấy nặng nhiều thứ cần load và để trong
        // awake của ui controller
        public static void LoadUI(GameUI ui, bool delayDeactive = false)
        {
            var nameUI = Enum.GetName(typeof(GameUI), ui);
            LoadUI(nameUI, delayDeactive);
        }

        // loadui nếu như không có trong stack, cái này để load lúc ui đấy nặng nhiều thứ cần load và để trong
        // awake của ui controller
        public static void LoadUI(string nameUI, bool delayDeactive = false)
        {
            if (!LoadedUI.TryGetValue(nameUI, out var newUI))
            {
                //print(Ins.UIPath + nameUI);
                var uiPrefab = Resources.Load<UIBase>(Ins.UIPath + nameUI);
                //print(uiPrefab);
                newUI = Instantiate(uiPrefab, UILayerParents[(int)uiPrefab.UILayer]);
                newUI.UIID = nameUI;
                if (!newUI.IsInitialize) newUI.Initialize();
                LoadedUI.Add(nameUI, newUI);
                newUI.transform.SetAsLastSibling();
                if (delayDeactive) Ins.StartCoroutine(DelayDeactive(newUI.gameObject));
                else newUI.gameObject.SetActive(false);
            }
            return;

            IEnumerator DelayDeactive(GameObject target)
            {
                yield return new WaitForSeconds(0.5f);
                target.SetActive(false);
            }
        }

        // Show ui, muốn ẩn ui cũ thì không thêm gì còn muốn ui cũ vẫn hiện thì thêm bool false
        public static void ShowUI(string nameUI, bool hideCurrentUI = true, int overrideChildIndex = 99)
        {
            if (StackUI.Count > 0)
            {
                var oldScreen = StackUI.Peek();
                if (hideCurrentUI) oldScreen.Hide();
            }

            if (LoadedUI.TryGetValue(nameUI, out var newUI))
            {
                if (overrideChildIndex != 99) newUI.transform.SetSiblingIndex(overrideChildIndex);
                else newUI.transform.SetAsLastSibling();
                newUI.Show();
            }
            else
            {
                var uiPrefab = Resources.Load<UIBase>(Ins.UIPath + nameUI);
                newUI = Instantiate(uiPrefab, UILayerParents[(int)uiPrefab.UILayer]);
                newUI.UIID = nameUI;
                if (!newUI.IsInitialize) newUI.Initialize();
                LoadedUI.Add(nameUI, newUI);

                if (overrideChildIndex != 99) newUI.transform.SetSiblingIndex(overrideChildIndex);
                else newUI.transform.SetAsLastSibling();
                newUI.Show();
            }

            StackUI.Push(newUI);
        }

        // Show ui, muốn ẩn ui cũ thì không thêm gì còn muốn ui cũ vẫn hiện thì thêm bool false
        public static void ShowUI(GameUI ui, bool hideCurrentUI = true, int overrideChildIndex = 99)
        {
            var nameUI = Enum.GetName(typeof(GameUI), ui);
            ShowUI(nameUI, hideCurrentUI, overrideChildIndex);
        }

        // Hide ui hiện tại, muốn show ui trước đó thì không thêm gì còn nếu ui trước đó đã hiện rồi thì thêm bool false
        public static void HideUI(bool showPeekScreen = true)
        {
            if (StackUI.Count > 1)
            {
                var popScreen = StackUI.Pop();
                popScreen.Hide();

                var peekScreen = StackUI.Peek();
                if (showPeekScreen) peekScreen.Show();
            }
        }

        // ẩn hết tất cả ui và show cái start screen
        public static void HideAllUI()
        {
            var totalInStack = StackUI.Count;
            while (totalInStack > 2)
            {
                HideUI(false);
                totalInStack--;
            }

            HideUI();
        }

        #endregion
    }

    public static class Helper
    {
        public static void SetStretch(this RectTransform rect)
        {
            rect.transform.localScale = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}