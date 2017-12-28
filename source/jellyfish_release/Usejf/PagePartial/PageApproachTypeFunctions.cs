using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Jellyfish.jfDeepZoom;

namespace Usejf
{
    /// <summary>
    /// [ApproachType]
    /// </summary>
    public partial class Page:UserControl
    {
        #region properties
        private List<FrameworkElement> staticList = new List<FrameworkElement>();
        private List<FrameworkElement> semiDynamicList = new List<FrameworkElement>();
        private List<FrameworkElement> dynamicList = new List<FrameworkElement>();
        private List<FrameworkElement> allList = new List<FrameworkElement>();

        private string currentApproachType = "";
        /// <summary>
        /// set current approachType.
        /// </summary>
        public string CurrentApproachType
        {
            get
            {
                return currentApproachType;
            }
            set
            {
                currentApproachType = value;

                switch (currentApproachType)
                {
                    case "static":
                        //
                        break;
                    case "semiDynamic":
                        // retrieve collections for semi-dynamic approach.
                        SetSemiDynamic();
                        break;
                    case "dynamic":
                        //
                        break;
                    default:
                        // do nothing
                        break;
                }

            }
        }
        #endregion

        private void InitApproachType()
        {
            // initializing Buttons of each approach types.
            InitStaticMode();
            InitSemiDynamicMode();
            InitDynamicMode();
            InitAllButtons();

            // initializing button events.
            InitApproarchTypeEvents();
        }

        /// <summary>
        /// initializing button events.
        /// </summary>
        private void InitApproarchTypeEvents()
        {
            StaticButton.Click += new RoutedEventHandler(StaticButton_Click);
            SemiDynamicButton.Click += new RoutedEventHandler(SemiDynamicButton_Click);
            DynamicButton.Click += new RoutedEventHandler(DynamicButton_Click);
        }


        #region Static mode

        private void InitStaticMode() { }

        private void ShowStaticButtons(){ }

        #endregion


        #region Semi-Dynamic mode

        private void InitSemiDynamicMode()
        {
            semiDynamicList.Add(FilterButton);
            semiDynamicList.Add(SelectableButton);
            semiDynamicList.Add(ShowListButton);
            semiDynamicList.Add(InfoButton);
        }
        private void ShowSemiDynamicButtons()
        {
            for (int i = 0; i < semiDynamicList.Count; i++)
            {
                semiDynamicList[i].Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Set Semi-Dynamic LayoutList.
        /// </summary>
        private void SetSemiDynamic()
        {
            jfd.JFDownloadSemiDynamicListCompleted += new JFCommunicationEventHandler(jfd_JFDownloadSemiDynamicListCompleted);
            ShowListListBox.SelectionChanged += new SelectionChangedEventHandler(ShowListListBox_SelectionChanged);

            jfd.RetrieveCollectionList("../semi-dynamic.aspx");
        }

        /// <summary>
        /// Handles the JFDownloadSemiDynamicListCompleted event of the jfd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        private void jfd_JFDownloadSemiDynamicListCompleted(object sender, JFCommunicationEventArgs e)
        {
            SetSemiDynamicList();
        }

        private void SetSemiDynamicList()
        {
            ListSp.Visibility = Visibility.Visible;
            List<JFSemiDynamicColList> colList = jfd.SemiDynamicColList;
            int listCount = ShowListListBox.Items.Count;
            for (int i = 0; i < listCount; i++)
            {
                ShowListListBox.Items.RemoveAt(0);
            }
            for (int i = 0; i < colList.Count; i++)
            {
                string layoutTitle = colList[i].LTitle;
                string colTitle = colList[i].CTitle;
                string id = colList[i].CollectionId;

                string itemString = "[" + id + "]" + colTitle + ":" + layoutTitle;
                ShowListListBox.Items.Add(itemString);
            }
        }


        /// <summary>
        /// retrieve new collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowListListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // index selected in ShowListListBox.
            int selectListIndex = ShowListListBox.SelectedIndex;

            if (selectListIndex >= 0)
            {
                jfd.RetrieveCollectionInit("../semi-dynamic.aspx", selectListIndex);
            }
            ListSp.Visibility = Visibility.Collapsed;
        }

        #endregion


        #region Dynamic mode

        private void InitDynamicMode()
        {
            dynamicList.Add(FilterButton);
            dynamicList.Add(SearchButton);
            dynamicList.Add(SelectableButton);
            dynamicList.Add(InfoButton);
        }

        private void ShowDynamicButtons()
        {
            for (int i = 0; i < dynamicList.Count; i++)
            {
                dynamicList[i].Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void InitAllButtons()
        {
            allList.Add(FilterButton);
            allList.Add(SearchButton);
            allList.Add(SelectableButton);
            allList.Add(ShowListButton);
            allList.Add(InfoButton);
        }

        /// <summary>
        /// Clears the buttons.
        /// </summary>
        private void ClearButtons()
        {
            for (int i = 0; i < allList.Count; i++)
            {
                allList[i].Visibility = Visibility.Collapsed;
            }
        }
    }
}
