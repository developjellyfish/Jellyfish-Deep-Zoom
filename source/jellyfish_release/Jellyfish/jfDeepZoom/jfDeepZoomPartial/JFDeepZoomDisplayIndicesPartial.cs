using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Display]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region Display Property

        /// <summary>
        /// Display Indeices
        /// </summary>
        private IndicesList indices = new IndicesList();

        /// <summary>
        /// Display Indeices
        /// </summary>
        public IndicesList Indices
        {
            get 
            {
                return indices;
            }
            set
            {
                if (CurrentLayoutStyle == LayoutStyle.NONE)
                {
                    indices = value;
                }
            }
        }

        /// <summary>
        /// Display Items (reference of MultiScaleSubImage) <br />
        /// </summary>
        public List<MultiScaleSubImage> Items
        {
            get
            {
                List<MultiScaleSubImage> items = new List<MultiScaleSubImage>();

                for (int i = 0; i < Indices.Count; i++)
                {
                    items.Add(msi.SubImages[Indices[i]]);
                }
                
                return items;
            }
        }

        #endregion

        #region Display Method

        /// <summary>
        /// Initialize Indices
        /// </summary>
        private void InitIndices()
        {
            for (int i = 0; i < msi.SubImages.Count; i++)
            {
                Indices.Add(i);
            }
        }

        /// <summary>
        /// Add index into Indices
        /// </summary>
        /// <param name="subImageIndex"></param>
        public void AddIndex(int subImageIndex)
        {
            if (!Indices.Contains(subImageIndex) && CurrentLayoutStyle == LayoutStyle.NONE)
            {
                Indices.Add(subImageIndex);
            }
        }

        /// <summary>
        /// remove index at specified position.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveIndexAt(int index)
        {
            if ((Indices.Count - 1) >= index && index >= 0 && CurrentLayoutStyle == LayoutStyle.NONE)
            {
                Indices.RemoveAt(index);
            }
        }

        /// <summary>
        /// If specified index of SubImage is existed, removed it.
        /// </summary>
        /// <param name="subImageIndex">Index of the sub image.</param>
        public void RemoveSubImageIndex(int subImageIndex)
        {
            int listIndex = ConvertSubImageIndexToIndex(subImageIndex);
            if (Indices.Contains(subImageIndex) && ((Indices.Count - 1) >= listIndex && listIndex >= 0) && CurrentLayoutStyle == LayoutStyle.NONE)
            {
                Indices.RemoveAt(listIndex);
            }
        }

        /// <summary>
        /// Converts the index of the subImage to Indices.
        /// </summary>
        /// <param name="subImageIndex">Index of the subImage.</param>
        /// <returns></returns>
        public int ConvertSubImageIndexToIndex(int subImageIndex)
        {
            int res = -1;

            res = Indices.IndexOf(subImageIndex);

            return res;
        }

        #region sort

        /// <summary>
        /// It calls when Filter animation is completed
        /// </summary>
        public event JFDeepZoomEventHandler FilterCompleted;

        /// <summary>
        /// It raises when Filter animation is completed
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFilterCompleted(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = FilterCompleted;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// sort by index of SubImageのindex
        /// </summary>
        /// <param name="orderBy">ASC or DESC</param>
        public void Sort(string orderBy)
        {
            IndicesList tempIndices = Indices;

            IndicesList temp = new IndicesList();
            if (orderBy == SortOrder.ASC)
            {
                for (int i = 0; i < Indices.Count; i++)
                {
                    temp.Add(i);
                }

            }
            else if (orderBy == SortOrder.DESC)
            {
                for (int i = Indices.Count - 1; i >= 0; i--)
                {
                    temp.Add(i);
                }

            }
            Indices = new IndicesList();
            Indices.SwapObject(temp);

            // when swapping done, do Layout again.
            CurrentLayoutStyle = PreviousLayoutStyle;
            if (PreviousLayoutStyle == "")
            {
                CurrentLayoutStyle = LayoutStyle.DEFAULT_POSITION;
            }
        }

        /// <summary>
        /// Sorting
        /// </summary>
        /// <param name="orderBy">"asc" or "desc"</param>
        /// <param name="key">target key </param>
        public void Sort(string orderBy, string key)
        {
            IndicesList temp = new IndicesList();

            // -- other tag
            if (key == "Tag")
            {
                #region Tag

                if (orderBy == SortOrder.ASC)
                {
                    var q = from c in detailData.Element("Metadata").Elements("Image")
                            orderby (string)c.Element("Tag") ascending
                            select (string)c.Element("AdditionalData").Element("Thumbnail");

                    detailData.Element("Metadata").Elements("Image");
                    if (q.Count() > 0)
                    {
                        foreach (string resultdata in q)
                        {
                            try
                            {
                                if (indices.Contains(ImageNameToIndex(resultdata)))
                                {
                                    int index = ImageNameToIndex(resultdata);
                                    temp.Add(index);
                                }
                            }
                            catch (Exception) { }
                        }
                    }

                }
                else if (orderBy == SortOrder.DESC)
                {
                    var q = from c in detailData.Element("Metadata").Elements("Image")
                            orderby (string)c.Element("Tag") descending
                            select (string)c.Element("AdditionalData").Element("Thumbnail");

                    if (q.Count() > 0)
                    {
                        foreach (string resultdata in q)
                        {
                            try
                            {
                                if (indices.Contains(ImageNameToIndex(resultdata)))
                                {
                                    int index = ImageNameToIndex(resultdata);
                                    temp.Add(index);
                                }
                            }
                            catch (Exception) { }
                        }
                    }

                }
                else
                {
                    return;
                }

                #endregion
            }
            else
            {
                #region Not Tag
                if (orderBy == SortOrder.ASC)
                {
                    var q = from c in detailData.Element("Metadata").Elements("Image")
                            orderby (string)c.Element("AdditionalData").Element(key) ascending
                            select (string)c.Element("AdditionalData").Element("Thumbnail");

                    detailData.Element("Metadata").Elements("Image");
                    if (q.Count() > 0)
                    {
                        foreach (string resultdata in q)
                        {
                            try
                            {
                                // If data is contain Indices, Add it.
                                if (indices.Contains(ImageNameToIndex(resultdata)))
                                {
                                    int index = ImageNameToIndex(resultdata);
                                    temp.Add(index);
                                }
                            }
                            catch (Exception) { }
                        }
                    }

                }
                else if (orderBy == SortOrder.DESC)
                {
                    var q = from c in detailData.Element("Metadata").Elements("Image")
                            orderby (string)c.Element("AdditionalData").Element(key) descending
                            select (string)c.Element("AdditionalData").Element("Thumbnail");

                    if (q.Count() > 0)
                    {
                        foreach (string resultdata in q)
                        {
                            try
                            {
                                if (indices.Contains(ImageNameToIndex(resultdata)))
                                {
                                    int index = ImageNameToIndex(resultdata);
                                    temp.Add(index);
                                }
                            }
                            catch (Exception) { }
                        }
                    }

                }
                else
                {
                    return;
                }
                #endregion
            }

            Indices = new IndicesList();
            Indices.SwapObject(temp);

            CurrentLayoutStyle = PreviousLayoutStyle;
            if (PreviousLayoutStyle == "")
            {
                CurrentLayoutStyle = LayoutStyle.DEFAULT_POSITION;
            }
        }

        #endregion

        /// <summary>
        /// Reverses Indices.
        /// </summary>
        public void Reverse()
        {
            indices.Reverse();
            CurrentLayoutStyle = PreviousLayoutStyle;
        }

        #region filter

        /// <summary>
        /// only 1 filter value
        /// </summary>
        /// <param name="condition">
        /// FilterKey: key for filtering
        /// FilterValue: value for Filtering.
        /// </param>
        public void Filter(FilterObject condition)
        {
            List<FilterObject> data = new List<FilterObject>();
            data.Add(condition);
            Filter(data, SearchCondition.AND);
        }

        /// <summary>
        /// Filtering (2 or more filtering)
        /// </summary>
        /// <param name="conditions">
        /// FilterKey: key for filtering
        /// FilterValue: value for Filtering.
        /// </param>
        /// <param name="searchCondition">
        /// Search with condition
        /// "and"
        /// "or"
        /// e.g. A and B and C / A or B or C
        /// </param>
        public void Filter(List<FilterObject> conditions, string searchCondition)
        {
            List<string> tagValues = new List<string>();

            // It cannot until layout done.
            if (isTweening == false && isSlide == false)
            {

                // parse XML into Image Unit
                IEnumerable<XElement> imagetagdata = detailData.Element("Metadata").Elements("Image");

                //
                List<List<int>> indicesEachConditions = new List<List<int>>();

                int emptyCount = 0;
                for (int i = 0; i < conditions.Count; i++)
                {
                    indicesEachConditions.Add(new List<int>());
                    if (conditions[i].FilterValue == "")
                    {
                        emptyCount++;
                    }
                }
                if (emptyCount != conditions.Count)
                {

                    int imageIndex = 0;
                    // (Loop by Image Tag)
                    foreach (XElement data in imagetagdata)
                    {
                        // recognize search value in loop.
                        // make search result List(Array) i-Loop ,then filter according / setting.
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            string target = "";

                            //imageIndex = (Int32)data.Attribute("id");
                            #region tag or not
                            // target is search value DB
                            if (conditions[i].FilterKey == "Tag")
                            {
                                tagValues = CommaSeparatedStrToList(conditions[i].FilterValue);

                                target = data.Element("Tag").Value;
                            }
                            else
                            {
                                target = data.Element("AdditionalData").Element(conditions[i].FilterKey).Value;
                            }
                            #endregion

                            // Process varies depends on operation.
                            if (conditions[i].FilterOperation == FilterOperationType.OPERATION_GREATERTHAN) // >=
                            {
                                try
                                {
                                    if (Double.Parse(conditions[i].FilterValue) <= Double.Parse(target) && conditions[i].FilterValue != "")
                                    {
                                        indicesEachConditions[i].Add(imageIndex);
                                        break;
                                    }
                                }
                                catch (Exception) { }
                            }
                            else if (conditions[i].FilterOperation == FilterOperationType.OPERATION_LESSTHAN)   // <=
                            {
                                try
                                {
                                    if (Double.Parse(conditions[i].FilterValue) >= Double.Parse(target) && conditions[i].FilterValue != "")
                                    {
                                        indicesEachConditions[i].Add(imageIndex);
                                        break;
                                    }
                                }
                                catch (Exception) { }
                            }
                            else if (conditions[i].FilterOperation == FilterOperationType.OPERATION_CONTAIN)    // has
                            {
                                //if (tagValues.Count == 0 && conditions[i].FilterKey != "Tag")
                                //if (tagValues.Count == 0 )
                                if (conditions[i].FilterKey != "Tag")
                                {
                                    // other tag
                                    if (target.IndexOf(conditions[i].FilterValue) >= 0 && conditions[i].FilterValue != "")
                                    {
                                        indicesEachConditions[i].Add(imageIndex);
                                        //break;
                                    }
                                }
                                else
                                {
                                    // TAG
                                    #region Generation of tags that exists in XML

                                    // separate string in XML into tags
                                    List<string> xmlTags = new List<string>();
                                    string[] str;
                                    str = target.Split(new Char[] { ',' });

                                    // the delimiter is comma
                                    for (int j = 0; j < str.Length; j++)
                                    {
                                        // trim
                                        string word = str[j].Trim(new Char[] { ' ', '	' });
                                        if (word != "")
                                        {
                                            xmlTags.Add(word);
                                        }
                                    }
                                    #endregion

                                    // If value of filterValue exists in XML, hit.
                                    for (int k = 0; k < tagValues.Count; k++)
                                    {
                                        if (xmlTags.Contains(tagValues[k]))
                                        {
                                            indicesEachConditions[k].Add(imageIndex);
                                            //break;
                                        }
                                    }
                                }
                            }
                            else if (conditions[i].FilterValue != "" && tagValues.Count > 0)    // == equal TAG
                            {
                                #region Generation of tags that exists in XML

                                // separate string in XML into tags

                                List<string> xmlTags = new List<string>();
                                xmlTags = CommaSeparatedStrToList(target);

                                #endregion

                                // If value of filterValue exists in XML, hit.
                                // 
                                for (int k = 0; k < tagValues.Count; k++)
                                {
                                    for (int l = 0; l < xmlTags.Count; l++)
                                    {
                                        if (xmlTags[l] == tagValues[k])
                                        {
                                            indicesEachConditions[i].Add(imageIndex);
                                        }
                                    }

                                }
                            }
                            else if (target == conditions[i].FilterValue && conditions[i].FilterValue != "")    // == equal NOT TAG
                            {
                                indicesEachConditions[i].Add(imageIndex);
                                break;
                            }

                        }
                        imageIndex++;
                    }

                    IndicesList swapIndices = new IndicesList();
                    IndicesList preSwapIndices = new IndicesList();

                    // Raw list before processing by "or" / "and".
                    for (int i = 0; i < indicesEachConditions.Count; i++)
                    {
                        for (int j = 0; j < indicesEachConditions[i].Count; j++)
                        {
                            preSwapIndices.Add(indicesEachConditions[i][j]);
                        }
                    }
                    //
                    preSwapIndices.Distinct();
                    preSwapIndices.Sort();


                    if (searchCondition == SearchCondition.AND)
                    {
                        // and

                        for (int i = 0; i < preSwapIndices.Count; i++)
                        {
                            int containsLen = 0;
                            for (int j = 0; j < indicesEachConditions.Count; j++)
                            {
                                // if it is included in every indicesEachConditions, hit with "and" Condition.
                                if (indicesEachConditions[j].Contains(preSwapIndices[i]))
                                {
                                    int pre = preSwapIndices[i];
                                    List<int> indcond = indicesEachConditions[j];
                                    containsLen++;
                                }
                            }
                            if (containsLen == indicesEachConditions.Count)
                            {
                                swapIndices.Add(preSwapIndices[i]);
                            }
                        }
                    }
                    else if (searchCondition == SearchCondition.OR)
                    {
                        swapIndices = preSwapIndices;
                    }

                    Indices = new IndicesList();
                    Indices.SwapObject(swapIndices);
                }
                else
                {
                    Indices = new IndicesList();
                }
                DoFiltering();
            }
        }

        /// <summary>
        /// Return List&lt;string&gt; from comma delimitered charactor.
        /// </summary>
        /// <param name="strArg">string that is separated by ",".</param>
        /// <returns>List&lt;string&gt;</returns>
        private List<string> CommaSeparatedStrToList(string strArg)
        {
            List<string> res = new List<string>();

            string[] str;
            str = strArg.Split(new Char[] { ',' });

            // by each word that delimitered comma.
            for (int j = 0; j < str.Length; j++)
            {
                // trim
                string word = str[j].Trim(new Char[] { ' ', '	' });

                //string sophisticatedWord = word.Replace('　', char. );
                // if it was not empty (or was spaces only), it is defined as search target.
                if (word != "")
                {
                    res.Add(word);
                }
            }

            return res;
        }

        #endregion

        /// <summary>
        /// reset filtering configuration.
        /// </summary>
        public void ResetFilter()
        {
            indices = new IndicesList();

            InitIndices();
            IndicesToThumbnailList();

            DoFiltering();
        }

        #endregion
    }
}
