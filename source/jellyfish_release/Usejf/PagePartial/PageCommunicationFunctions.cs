using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Jellyfish.jfDeepZoom;

namespace Usejf
{
    /// <summary>
    /// [Communications]
    /// </summary>
    public partial class Page:UserControl
    {

        #region properties

        private bool isReset = false;

        // the states of each parts (FOLDED, OPENED).
        private string thumbnailState = CommonStates.FOLDED;
        private string searchState = CommonStates.FOLDED;
        private string sortState = CommonStates.FOLDED;
        private string filterState = CommonStates.FOLDED;
        private string selectableState = CommonStates.FOLDED;
        private string showListState = CommonStates.FOLDED;
        private string infoState = CommonStates.FOLDED;

        #endregion

        /// <summary>
        /// set ThumbnailListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jfd_JFDeepZoomSrcLoaded(object sender, JFDeepZoomEventArgs e)
        {
            ThumbnailListBox.ItemsSource = null;
            ThumbnailListBox.ItemsSource = jfd.ThumbnailList;

            // set SelectionChanged
            ThumbnailListBox.SelectionChanged += new SelectionChangedEventHandler(ThumbnailListBox_SelectionChanged);

            StopLoading();
        }
        
        #region select approachMode 

        private void StaticButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentApproachType = "static";
            Enterance.Visibility = Visibility.Collapsed;
            modal.Visibility = Visibility.Collapsed;

            ClearButtons();
            ShowStaticButtons();
        }

        private void SemiDynamicButton_Click(object sender, RoutedEventArgs e)
        {
            Enterance.Visibility = Visibility.Collapsed;
            modal.Visibility = Visibility.Collapsed;

            ClearButtons();
            ShowSemiDynamicButtons();
            CurrentApproachType = "semiDynamic";
        }

        private void DynamicButton_Click(object sender, RoutedEventArgs e)
        {
            Enterance.Visibility = Visibility.Collapsed;
            modal.Visibility = Visibility.Collapsed;
            CurrentApproachType = "dynamic";
            ClearButtons();
            ShowDynamicButtons();
        }

        private void InitTabs()
        {
            jfd.JFDeepZoomSrcLoaded += new JFDeepZoomEventHandler(jfd_JFDeepZoomSrcLoaded);

            // button event
            SortButton.Click += new RoutedEventHandler(SortButton_Click);
            FilterButton.Click += new RoutedEventHandler(FilterButton_Click);
            SearchButton.Click += new RoutedEventHandler(SearchButton_Click);
            InfoButton.Click += new RoutedEventHandler(InfoButton_Click);
            SelectableButton.Click += new RoutedEventHandler(SelectableButton_Click);
            ShowListButton.Click += new RoutedEventHandler(ShowListButton_Click);
            ThumbnailButton.Click += new RoutedEventHandler(ThumbnailButton_Click);

            // close button event
            SortCloseButton.Click += new RoutedEventHandler(SortCloseButton_Click);
            FilterCloseButton.Click += new RoutedEventHandler(FilterCloseButton_Click);
            SearchCloseButton.Click += new RoutedEventHandler(SearchCloseButton_Click);
            InformationCloseButton.Click += new RoutedEventHandler(InformationCloseButton_Click);
            SelectableCloseButton.Click += new RoutedEventHandler(SelectableCloseButton_Click);
            ListCloseButton.Click += new RoutedEventHandler(ListCloseButton_Click);

            // init tab function
            InitTabFunction();


        }

        #endregion

        #region Close functions

        private void InformationCloseButton_Click(object sender, RoutedEventArgs e)
        {
            infoState = CommonStates.FOLDED;
            InfoFadeOut.Begin();
        }
        private void ListCloseButton_Click(object sender, RoutedEventArgs e)
        {
            showListState = CommonStates.FOLDED;
            ListFadeOut.Begin();
        }

        private void SelectableCloseButton_Click(object sender, RoutedEventArgs e)
        {
            selectableState = CommonStates.FOLDED;
            SelectableFadeOut.Begin();
        }

        private void SearchCloseButton_Click(object sender, RoutedEventArgs e)
        {
            searchState = CommonStates.FOLDED;
            SearchFadeOut.Begin();
        }

        private void FilterCloseButton_Click(object sender, RoutedEventArgs e)
        {
            filterState = CommonStates.FOLDED;
            FilterFadeOut.Begin();
        }

        private void SortCloseButton_Click(object sender, RoutedEventArgs e)
        {
            sortState = CommonStates.FOLDED;
            SortFadeOut.Begin();
        }
        #endregion

        private void InitTabFunction()
        {
            InitSortFunction();
            InitFilterFunction();
            InitSearchFunction();
            InitSelectableFunction();
            InitShowListFunction();
            InitThumbnailListFunction();
            InitInfoFunction();
        }

        #region Sort function

        private void InitSortFunction()
        {
            ExecSortButton.Click += new RoutedEventHandler(ExecSortButton_Click);
            // Reverse
            ReverseButton.Click += new RoutedEventHandler(ReverseButton_Click);

            SortFadeOut.Completed += new EventHandler(SortFadeOut_Completed);
        }

        private string sortOrder = SortOrder.ASC;

        /// <summary>
        /// execute Sort function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecSortButton_Click(object sender, RoutedEventArgs e)
        {
            // 
            if (CurrentApproachType == "static")
            {
                if (sortOrder == SortOrder.ASC) sortOrder = SortOrder.DESC;
                else
                {
                    sortOrder = SortOrder.ASC;
                }
                jfd.Sort(sortOrder);
            }
            else
            {
                string sortType = "";
                if ((bool)OrderDESCRb.IsChecked)
                {
                    //desc
                    sortType = SortOrder.DESC;
                }
                else
                {
                    //asc
                    sortType = SortOrder.ASC;
                }

                // 
                string sortBy = "";
                if ((bool)OrderByOwnerRb.IsChecked)
                {
                    sortBy = "Owner";
                }
                else
                {
                    sortBy = "Title";
                }
                jfd.Sort(sortType, sortBy);
            }
        }

        /// <summary>
        /// execute Reverse function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.Reverse();
        }

        private void SortFadeOut_Completed(object sender, EventArgs e)
        {
            SortGrid.Opacity = 0;
            SortGrid.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Filter function

        private void InitFilterFunction()
        {
            ExecFilterButton.Click += new RoutedEventHandler(ExecFilterButton_Click);

            // Reset
            ResetFilterButton.Click += new RoutedEventHandler(ResetFilterButton_Click);
            jfd.FilterCompleted += new JFDeepZoomEventHandler(jfd_Opacity0);

            FilterFadeIn.Completed += new EventHandler(FilterFadeIn_Completed);
            FilterFadeOut.Completed += new EventHandler(FilterFadeOut_Completed);

        }

        /// <summary>
        /// [Semi-Dynamic] execute filter function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecFilterButton_Click(object sender, RoutedEventArgs e)
        {
            List<FilterObject> targetList = new List<FilterObject>();

            for (int i = 0; i <= 2; i++)
            {
                FilterObject fo = new FilterObject();

                if (i == 0)
                {
                    //----------------------- set up filtering information
                    fo.FilterKey = "Title";
                    fo.FilterValue = TitleFilterTb.Text;
                    fo.FilterOperation = FilterOperationType.OPERATION_CONTAIN;
                    targetList.Add(fo);
                }
                else if (i == 1)
                {
                    //----------------------- set up filtering information
                    fo.FilterKey = "Tag";
                    fo.FilterValue = TagFilterTb.Text;
                    // FilterOperationType has 4 types:
                    // EQUAL, CONTAIN, LESS_THAN, GREATER_THAN
                    fo.FilterOperation = FilterOperationType.OPERATION_EQUAL;
                    targetList.Add(fo);
                }
            }
            string stype = "";
            if ((bool)FilterConditionAndRb.IsChecked)
            {
                stype = SearchCondition.AND;
            }
            else
            {
                stype = SearchCondition.OR;
            }
            if (TagFilterTb.Text == "" && TitleFilterTb.Text == "")
            {
                jfd.ResetFilter();
                isReset = true;
            }
            else if (targetList.Count > 0)
            {
                jfd.Filter(targetList, stype);
            }
        }

        /// <summary>
        /// [Semi-Dynamic] execute reset filter function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            jfd.ResetFilter();
            isReset = true;
        }

        /// <summary>
        /// [Semi-Dynamic]  event handler 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jfd_Opacity0(object sender, JFDeepZoomEventArgs e)
        {
            if (isReset)
            {
                // if you want to layout subimages as specific layoutstyle, you can set new layoutstyle.
                jfd.CurrentLayoutStyle = LayoutStyle.LINE;
                jfd.JFDeepZoomMotionFinished += new JFDeepZoomEventHandler(jfd_JFDeepZoomMotionFinished);
                isReset = false;
            }
            ThumbnailListBox.ItemsSource = jfd.ThumbnailList;
        }

        private void jfd_JFDeepZoomMotionFinished(object sender, JFDeepZoomEventArgs e)
        {
            jfd.DoFit();
            jfd.JFDeepZoomMotionFinished -= new JFDeepZoomEventHandler(jfd_JFDeepZoomMotionFinished);
        }

        private void FilterFadeOut_Completed(object sender, EventArgs e)
        {
            FilterGird.Opacity = 0;
            FilterGird.Visibility = Visibility.Collapsed;
        }

        private void FilterFadeIn_Completed(object sender, EventArgs e){ }

        #endregion

        #region Search function

        private void InitSearchFunction()
        {
            ExecSearchButton.Click += new RoutedEventHandler(ExecSearchButton_Click);
            ResetSearchButton.Click += new RoutedEventHandler(ResetSearchButton_Click);
            SearchFadeOut.Completed += new EventHandler(SearchFadeOut_Completed);
        }

        void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
            Dictionary<string, string> table1 = new Dictionary<string, string>();
            table1["condition1"] = TitleSearchTb.Text;
            dataList.Add(table1);

            string search = "";
            search = SearchCondition.OR;
            
            string orderBy = "";
            orderBy = "Title";

            string orderType = "";
            orderType = SortOrder.ASC;

            // set loading
            StartLoading();

            jfd.Send("../dynamic.aspx", dataList, orderType, orderBy, search);
        }

        /// <summary>
        /// [Dynamic] execute search function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecSearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
            Dictionary<string, string> table1 = new Dictionary<string, string>();
            table1["condition1"] = TitleSearchTb.Text;
            dataList.Add(table1);

            // " " of "tag" is replaced ","
            if (TagSearchTb.Text != "")
            {
                Dictionary<string, string> table2 = new Dictionary<string, string>();
                //table2["condition2"] = tag_tb.Text.Replace(" ",",");
                table2["condition2"] = Regex.Replace(TagSearchTb.Text, @"(\s)+", ",");

                dataList.Add(table2);
            }

            string search = "";
            if ((bool)SearchConditionOrRb.IsChecked)
            {
                search = SearchCondition.OR;
            }
            else
            {
                search = SearchCondition.AND;
            }

            string orderBy = "";
            orderBy = "Title";

            string orderType = "";
            orderType = SortOrder.ASC;

            // set loading
            StartLoading();

            jfd.Send("../dynamic.aspx", dataList, orderType, orderBy, search);
        }

        private void SearchFadeOut_Completed(object sender, EventArgs e)
        {
            SearchGird.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region info function 


        private bool showInfo = false;

        private void InitInfoFunction()
        {
            ExecInformationButton.Click += new RoutedEventHandler(ExecInformationButton_Click);
        }

        /// <summary>
        /// switch Information rect, "on, off".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecInformationButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool) InformationOnRb.IsChecked)
            {
                showInfo = true;
                additionalDataFadeIn.Begin();
            }
            else
            {
                showInfo = false;
                additionalDataFadeOut.Begin();
            }
        }

        #endregion

        #region selectable function

        /// <summary>
        /// [Semi-Dynamic] [Dynamic]
        /// </summary>
        private void InitSelectableFunction()
        {
            ExecSelectableButton.Click += new RoutedEventHandler(ExecSelectableButton_Click);
        }

        private void ExecSelectableButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)SelectableOnRb.IsChecked)
            {
                jfd.IsSelectable = true;
                jfd.SetVisiblitySubImageCheckMark(true);

                jfd.CurrentMouseEventType = MouseEventType.EVENT_ONLY;
            }
            else
            {
                jfd.IsSelectable = false;
                jfd.SetVisiblitySubImageCheckMark(false);

                jfd.CurrentMouseEventType = MouseEventType.ZOOM;
            }
        }

        #endregion

        #region show list

        private void InitShowListFunction()
        {
            ListFadeOut.Completed += new EventHandler(ListFadeOut_Completed);
        }

        private void ListFadeOut_Completed(object sender, EventArgs e)
        {
            ListSp.Opacity = 0;
            ListSp.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region ThumbnailList

        private void InitThumbnailListFunction()
        {
            ThumbnailFadeIn.Completed += new EventHandler(ThumbnailFadeIn_Completed);
            ThumbnailFadeOut.Completed += new EventHandler(ThumbnailFadeOut_Completed);
        }

        private void ThumbnailFadeOut_Completed(object sender, EventArgs e)
        {
            Storyboard sb = ThumbListClosed;
            sb.Begin();
        }

        private void ThumbnailFadeIn_Completed(object sender, EventArgs e)
        {
            Storyboard sb = ThumbListOpened;
            sb.Begin();
        }

        private void ThumbnailListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SwitchNextPreviousButtonEnable(false);

            int index = ThumbnailListBox.SelectedIndex;
            if (index >= 0)
            {
                int jfd_index = jfd.ImageNameToIndex(jfd.ThumbnailList[index].ThumbnailPath);
                jfd.ZoomSingleSubImage(jfd_index);
                jfd.CurrentSlideIndex = jfd_index;
            }
        }

        #endregion

        #region set toggle function

        /// <summary>
        /// Appear Sort Input area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            if (sortState == CommonStates.FOLDED)
            {
                SortGrid.Visibility = Visibility.Visible;
                SortFadeIn.Begin();
                sortState = CommonStates.OPENED;
            }
            else
            {
                SortFadeOut.Begin();
                sortState = CommonStates.FOLDED;
            }
        }

        /// <summary>
        /// [Semi-Dynamic]: Filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (filterState == CommonStates.FOLDED)
            {
                FilterGird.Visibility = Visibility.Visible;
                FilterFadeIn.Begin();
                filterState = CommonStates.OPENED;
            }
            else
            {
                FilterFadeOut.Begin();
                filterState = CommonStates.FOLDED;
            }
        }

        /// <summary>
        /// [Semi-Dynamic]: Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchState == CommonStates.FOLDED)
            {
                SearchGird.Visibility = Visibility.Visible;
                SearchFadeIn.Begin();
                searchState = CommonStates.OPENED;
            }
            else
            {
                SearchFadeOut.Begin();
                searchState = CommonStates.FOLDED;
            }
        }

        #region Information 

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (infoState == CommonStates.FOLDED)
            {
                InfoSp.Visibility = Visibility.Visible;
                InfoFadeIn.Begin();
                infoState = CommonStates.OPENED;
            }
            else
            {
                InfoFadeOut.Begin();
                infoState = CommonStates.FOLDED;
            }
        }

        #endregion

        /// <summary>
        /// [Semi-Dynamic]: Selectable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectableButton_Click(object sender, RoutedEventArgs e)
        {
            //UtilUI.toggleControlPanel(SelectableSp);
            if (selectableState == CommonStates.FOLDED)
            {
                SelectableSp.Visibility = Visibility.Visible;
                SelectableFadeIn.Begin();
                selectableState = CommonStates.OPENED;
            }
            else
            {
                SelectableFadeOut.Begin();
                selectableState = CommonStates.FOLDED;
            }
        }

        /// <summary>
        /// [Semi-Dynamic]:  Layout List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowListButton_Click(object sender, RoutedEventArgs e)
        {
            if (showListState == CommonStates.FOLDED)
            {
                ListSp.Visibility = Visibility.Visible;
                ListFadeIn.Begin();
                showListState = CommonStates.OPENED;
            }
            else
            {
                ListFadeOut.Begin();
                showListState = CommonStates.FOLDED;
            }
        }

        /// <summary>
        /// [Semi-Dynamic]: Thumbnail List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThumbnailButton_Click(object sender, RoutedEventArgs e)
        {
            if (thumbnailState == CommonStates.FOLDED)
            {
                ThumbnailListSp.Visibility = Visibility.Visible;
                ThumbnailFadeIn.Begin();
                thumbnailState = CommonStates.OPENED;
            }
            else
            {
                ThumbnailFadeOut.Begin();
                thumbnailState = CommonStates.FOLDED;
            }
        }

        #endregion

    }
}
