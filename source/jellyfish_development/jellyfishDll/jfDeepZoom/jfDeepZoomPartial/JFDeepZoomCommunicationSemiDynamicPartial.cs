using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Communication Semi-Dynamic]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        #region properties

        /// <summary>
        /// Current semiDynamic approach type.
        /// </summary>
        private string currSemiDynamicExecType = SemiDynamicExecType.NONE;

        /// <summary>
        /// List of collection and layout that get from server with SemiDynamic approach.
        /// </summary>
        private List<JFSemiDynamicColList> semiDynamicColList;

        /// <summary>
        /// List of collection and layout that get from server with SemiDynamic approach.
        /// </summary>
        public List<JFSemiDynamicColList> SemiDynamicColList
        {
            get
            {
                return semiDynamicColList;
            }
            set
            {
                semiDynamicColList = value;
            }
        }

        /// <summary>
        /// Custom layout information with SemiDynamic.
        /// </summary>
        private List<Dictionary<string, string>> newLayoutViewportList;

        /// <summary>
        /// Current CollectionID.
        /// </summary>
        private string currentCid = "";

        /// <summary>
        /// Current LayoutID.
        /// </summary>
        private string currentLid = "";

        /// <summary>
        /// Selected list collection information. (with SemiDynamic)
        /// </summary>
        private JFSemiDynamicColList currentSemiDynamicCollection = new JFSemiDynamicColList();

        #endregion

        #region Events

        /// <summary>
        /// It calls when load the "List" of SemiDynamic is completed.
        /// </summary>
        public event JFCommunicationEventHandler JFDownloadSemiDynamicListCompleted;

        /// <summary>
        /// It raises when load the "List" of SemiDynamic is completed.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDownloadSemiDynamicListCompleted(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFDownloadSemiDynamicListCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It calls when load the "Custom Layout" of SemiDynamic is completed.
        /// </summary>
        public event JFCommunicationEventHandler JFSemiDynamicLayoutChangeCompleted;

        /// <summary>
        /// It raises when load the "Custom Layout" of SemiDynamic is completed.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFSemiDynamicLayoutChangeCompleted(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFSemiDynamicLayoutChangeCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion


        #region Semi-Dynamic method

        /// <summary>
        /// [Semi-Dynamic] Communicate as LIST.
        /// </summary>
        /// <param name="uri">Uri of API</param>
        public void RetrieveCollectionList(string uri)
        {
            // communicate as LIST
            currSemiDynamicExecType = SemiDynamicExecType.LIST;
            currentApproachType = ApproachType.SEMI_DYNAMIC;

            sendFullUri = uri + "?exec=list";

            if (uri.Contains("http://") || uri.Contains("https://"))
            {
                wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Absolute));
            }
            else
            {
                wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Relative));
            }
        }

        /// <summary>
        /// [Semi-Dynamic] Communicate as INIT.
        /// </summary>
        /// <param name="uri">Uri of API</param>
        /// <param name="selectionIndex">selectedIndex of "List" of SemiDynamic</param>
        public void RetrieveCollectionInit(string uri, int selectionIndex)
        {
            if (!isSlide && !isTweening)
            {
                currSemiDynamicExecType = SemiDynamicExecType.INIT;
                currentApproachType = ApproachType.SEMI_DYNAMIC;

                // Current collection and layout with SemiDynamic approach.
                currentSemiDynamicCollection = semiDynamicColList[selectionIndex];

                // get Current CId
                string cid = semiDynamicColList[selectionIndex].CId;
                // get Current LId
                string lid = semiDynamicColList[selectionIndex].LId;

                // -----------------
                // communicate as INIT
                if (currentCid != cid)
                {

                    sendFullUri = uri + "?exec=init";
                    sendFullUri += "&cid=" + cid;

                    if (uri.Contains("http://") || uri.Contains("https://"))
                    {
                        try
                        {
                            wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Absolute));
                        }
                        catch (WebException exception)
                        {
                            Debug.WriteLine("an exception occured in wc_DownloadStringCompleted:" + exception.Message);
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Relative));
                        }
                        catch (WebException exception)
                        {
                            Debug.WriteLine("an exception occured in wc_DownloadStringCompleted:" + exception.Message);
                            return;
                        }
                    }
                }
                else
                {

                    if (currentLid != lid)
                    {
                        string lUrl = semiDynamicColList[selectionIndex].LUrl;

                        // Load dzcPath in XML as string 
                        RetrieveLayoutChange(lUrl);
                    }
                }
                currentCid = cid;
                currentLid = lid;
            }
        }

        /// <summary>
        /// [SemiDynamic] Change order of dzc in INIT.
        /// </summary>
        /// <param name="uri">Uri of API</param>
        protected void RetrieveInitChangeOrder(string url)
        {
            // Communicate as INIT_CHANGE_ORDER
            currSemiDynamicExecType = SemiDynamicExecType.INIT_CHANGE_ORDER;
            currentApproachType = ApproachType.SEMI_DYNAMIC;

            // ---------------------------------------
            // Load dzcPath in XML as string 
            // ---------------------------------------
            sendFullUri = url;

            if (url.Contains("http://") || url.Contains("https://"))
            {
                try
                {
                    wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Absolute));
                }
                catch (WebException exception)
                {
                    Debug.WriteLine(exception.Message);
                }
            }
            else
            {
                try
                {
                    wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Relative));
                }
                catch (WebException exception)
                {
                    Debug.WriteLine(exception.Message);
                }
            }

        }

        /// <summary>
        /// [SemiDynamic] Execute changing layout.
        /// </summary>
        /// <param name="uri">Uri of API</param>
        public void RetrieveLayoutChange(string url)
        {
            if (url != currentSemiDynamicCollection.CUrl)
            {
                // Communicate as CHANGE_LAYOUT
                currSemiDynamicExecType = SemiDynamicExecType.CHANGE_LAYOUT;
                currentApproachType = ApproachType.SEMI_DYNAMIC;

                // -----------------
                // Load dzcPath in XML as string, and layout by client side.
                sendFullUri = url;

                if (url.Contains("http://") || url.Contains("https://"))
                {
                    try
                    {
                        wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Absolute));
                    }
                    catch (WebException exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }
                }
                else
                {
                    try
                    {
                        wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Relative));
                    }
                    catch (WebException exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }
                }
                // -----------------

            }
            else if (!isSlide)
            {
                CurrentLayoutStyle = LayoutStyle.DEFAULT_POSITION;
            }
        }

        /// <summary>
        /// [SemiDynamic] Execute changing layout.
        /// </summary>
        /// <param name="uri">Uri of API</param>
        /// <param name="lid">LayoutID</param>
        public void RetrieveCollectionLayout(string uri, string lid)
        {

            // Communicate as LAYOUT
            currSemiDynamicExecType = SemiDynamicExecType.LAYOUT;
            currentApproachType = ApproachType.SEMI_DYNAMIC;

            if (currentLid != lid)
            {

                sendFullUri = uri + "?exec=layout";
                sendFullUri += "&lid=" + lid;

                if (uri.Contains("http://") || uri.Contains("https://"))
                {
                    wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Absolute));
                }
                else
                {
                    wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Relative));
                }
            }

            currentLid = lid;
        }

        #endregion

        #region Semi-Dynamic DownloadStringCompleted

        /// <summary>
        /// [SemiDynamic] It called, When the communication for semidynamic approach of LIST is completed.
        /// </summary>
        /// <param name="xmlstr">receive XML</param>
        private void SemiDynamicListStringCompleted(string xmlstr)
        {
            semiDynamicColList = new List<JFSemiDynamicColList>();

            try
            {
                XDocument metaXDocument = XDocument.Parse(xmlstr, LoadOptions.None);

                // check errorCode
                string statusCode = metaXDocument.Element("Metadata").Element("status").Attribute("code").Value;
                if (statusCode != normalCode)
                {
                    string errorDescription = metaXDocument.Element("Metadata").Element("status").Value;

                    JFCommunicationErrorEventArgs overEv = new JFCommunicationErrorEventArgs(statusCode, errorDescription);
                    OnJFDeepZoomSrcIsOverLimit(overEv);
                    return;
                }
                else
                {

                    XElement metaXElement = metaXDocument.Element("Metadata");

                    var q = from c in metaXElement.Descendants("Collection")
                            select (XElement)c;

                    if (q.Count() > 0)
                    {
                        foreach (XElement resultdata in q)
                        {
                            try
                            {
                                XElement resdata = resultdata;

                                JFSemiDynamicColList colList = new JFSemiDynamicColList();
                                colList.CollectionId = resdata.Attribute("id").Value;

                                colList.CId = resdata.Element("CId").Value;
                                colList.CTitle = resdata.Element("CTitle").Value;
                                colList.CUrl = resdata.Element("CUrl").Value;
                                colList.LId = resdata.Element("LId").Value;
                                colList.LTitle = resdata.Element("LTitle").Value;
                                colList.LUrl = resdata.Element("LUrl").Value;

                                colList.IsDefault = (colList.LUrl == colList.CUrl);

                                semiDynamicColList.Add(colList);
                            }
                            catch (Exception) { }
                        }
                    }
                }

            }
            catch (Exception)
            {
                JFDeepZoomEventArgs nullEv = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomSrcIsNull(nullEv);
                return;
            }

            JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
            OnJFDownloadSemiDynamicListCompleted(ev);

            currSemiDynamicExecType = SemiDynamicExecType.NONE;
        }

        /// <summary>
        /// [SemiDynamic] It called, When the communication for semidynamic approach of INIT is completed.
        /// (load Metainfo)
        /// </summary>
        /// <param name="xmlstr">receive XML</param>
        private void SemiDynamicInitStringCompleted(string xmlstr)
        {
            InitSubImages();

            string dzcpathString = "";
            try
            {
                detailData = XDocument.Parse(xmlstr, LoadOptions.None);
                ThumbnailList = new List<JFThumbnailList>();

                // check errorCode
                string statusCode = detailData.Element("Metadata").Element("status").Attribute("code").Value;
                if (statusCode != normalCode)
                {
                    string errorDescription = detailData.Element("Metadata").Element("status").Value;

                    JFCommunicationErrorEventArgs overEv = new JFCommunicationErrorEventArgs(statusCode, errorDescription);
                    OnJFDeepZoomSrcIsOverLimit(overEv);
                    return;
                }
                else
                {
                    string debugtxt = "";

                    var q = from c in detailData.Descendants("Image")
                            select (string)c.Element("AdditionalData").Element("Thumbnail");

                    int count = 0;
                    if (q.Count() > 0)
                    {
                        foreach (string resultdata in q)
                        {
                            try
                            {
                                string resdata = resultdata;

                                JFThumbnailList jft = new JFThumbnailList();
                                jft.ThumbnailPath = "" + resultdata;
                                jft.PathIndex = count;
                                ThumbnailList.Add(jft);
                                debugtxt += " " + jft.ThumbnailPath;

                                count++;
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
            catch (Exception)
            {
                dzcpathString = "";
                JFDeepZoomEventArgs nullEv = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomSrcIsNull(nullEv);
                return;
            }

            // DZC Configuration
            dzcpathString = currentSemiDynamicCollection.CUrl;

            if (dzcpathString != "")
            {
                Indices = new IndicesList();

                currSemiDynamicExecType = SemiDynamicExecType.INIT_CHANGE_ORDER;

                RetrieveInitChangeOrder(dzcpathString);
            }
        }

        /// <summary>
        /// [SemiDynamic] Continuation of semidynamic approach of INIT.
        /// (Change order ThumbnailList, ascending order in DZC)
        /// </summary>
        /// <param name="xmlstr">receive XML</param>
        private void SemiDynamicInitStringCompleted2(string xmlstr)
        {
            List<string> aSortUidList = new List<string>();

            try
            {

                // ---- delete xmlns
                string pattern = "xmlns=\"http://schemas.microsoft.com/deepzoom/2008\"";
                string safexml = xmlstr.Replace(pattern, "");

                XDocument metaXDocument = XDocument.Parse(safexml, LoadOptions.None);

                XElement colXElement = metaXDocument.Element("Collection").Element("Items");

                var q = from c in colXElement.Descendants("I")
                        select (XElement)c;

                if (q.Count() > 0)
                {
                    foreach (XElement resultdata in q)
                    {
                        try
                        {
                            XElement resdata = resultdata;

                            string aSource = resdata.Attribute("Source").Value;

                            string aUid = System.IO.Path.GetFileNameWithoutExtension(aSource);

                            aSortUidList.Add(aUid);

                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception)
            {
                JFDeepZoomEventArgs nullEv = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomSrcIsNull(nullEv);
                return;
            }

            IEnumerable<XElement> imageNodes = detailData.Element("Metadata").Elements("Image");

            List<int> aSortIndexList = new List<int>();

            foreach (XElement imageEl in imageNodes) {
                string aUId = imageEl.Element("AdditionalData").Element("UId").Value;

                int aNextIndex = aSortUidList.IndexOf(aUId);

                aSortIndexList.Add(aNextIndex);
            }

            List<XElement> tempImageNodeList = new List<XElement>();
            List<JFThumbnailList> tempThumbnailList = new List<JFThumbnailList>();

            for (int i = 0 ; i<aSortIndexList.Count ; i++)
            {
                XElement imageElement = imageNodes.ElementAt(i);

                int sortUidIndex = aSortIndexList[i];

                JFThumbnailList jfThumbnailOne = ThumbnailList[sortUidIndex];

                tempThumbnailList.Add(jfThumbnailOne);

                tempImageNodeList.Add(imageNodes.ElementAt(sortUidIndex));
            }

            // Change Order ThumbnailList List
            ThumbnailList.Clear();
            ThumbnailList.AddRange(tempThumbnailList);

            XElement metadataNodes = detailData.Element("Metadata");
            imageNodes.Remove();

            for (int i = 0; i < tempImageNodeList.Count; i++)
            {
                detailData.Element("Metadata").Add(tempImageNodeList[i]);
            }

            string dzcpathString = currentSemiDynamicCollection.CUrl;

            if (dzcpathString != "")
            {

                // get lUrl
                string lUrl = currentSemiDynamicCollection.LUrl;
                // get cUrl
                string cUrl = currentSemiDynamicCollection.CUrl;

                if (cUrl == lUrl)
                {
                    // configuration DZC
                    SetSrc(dzcpathString);

                    currSemiDynamicExecType = SemiDynamicExecType.NONE;
                }
                else
                {
                    this.msi.Opacity = 0;

                    // configuration DZC
                    SetSrc(dzcpathString);
                }
            }
        }

        /// <summary>
        /// [SemiDynamic] Continuation of semidynamic approach of LAYOUT.
        /// </summary>
        /// <param name="xmlstr">receive XML</param>
        private void SemiDynamicLayoutStringCompleted(string xmlstr)
        {
            try
            {
                XDocument metaXDocument = XDocument.Parse(xmlstr, LoadOptions.None);

                // check errorCode
                string statusCode = metaXDocument.Element("Metadata").Element("status").Attribute("code").Value;
                if (statusCode != normalCode)
                {
                    string errorDescription = metaXDocument.Element("Metadata").Element("status").Value;

                    JFCommunicationErrorEventArgs overEv = new JFCommunicationErrorEventArgs(statusCode, errorDescription);
                    OnJFDeepZoomSrcIsOverLimit(overEv);
                    return;
                }
                else
                {
                    XElement layoutXElement = metaXDocument.Element("Metadata").Element("Layout");

                    string lid = layoutXElement.Element("LId").Value;
                    string lTItle = layoutXElement.Element("LId").Value;
                    string lUrl = layoutXElement.Element("LUrl").Value;

                    currentLid = lid;
                    RetrieveLayoutChange(lUrl);
                }


            }
            catch (Exception)
            {
                JFDeepZoomEventArgs nullEv = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomSrcIsNull(nullEv);
            }
        }

        /// <summary>
        /// [SemiDynamic] Continuation of semidynamic approach of LAYOUT.
        /// （Load dzcPath in XML as string, and layout by client side.）
        /// </summary>
        /// <param name="xmlstr">receive XML</param>
        private void SemiDynamicChangeLayoutStringCompleted(string xmlstr)
        {
            newLayoutViewportList = new List<Dictionary<string, string>>();

            try
            {
                // ---- delete xmlns
                string pattern = "xmlns=\"http://schemas.microsoft.com/deepzoom/2008\"";
                string safexml = xmlstr.Replace(pattern, "");
                XDocument metaXDocument = XDocument.Parse(safexml, LoadOptions.None);

                XElement colXElement = metaXDocument.Element("Collection").Element("Items");

                var q = from c in colXElement.Descendants("I")
                        select (XElement)c;


                if (q.Count() > 0)
                {
                    foreach (XElement resultdata in q)
                    {
                        try
                        {
                            XElement resdata = resultdata;

                            string aId = resdata.Attribute("Id").Value;
                            string aX = resdata.Element("Viewport").Attribute("X").Value;
                            string aY = resdata.Element("Viewport").Attribute("Y").Value;
                            string aWidth = resdata.Element("Viewport").Attribute("Width").Value;

                            Dictionary<string, string> aDictionary = new Dictionary<string, string>();
                            aDictionary.Add("Id", aId);
                            aDictionary.Add("X", aX);
                            aDictionary.Add("Y", aY);
                            aDictionary.Add("Width", aWidth);

                            newLayoutViewportList.Add(aDictionary);
                        }
                        catch (Exception) { }
                    }
                }

            }
            catch (Exception)
            {
                JFDeepZoomEventArgs nullEv = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomSrcIsNull(nullEv);
                return;
            }

            CurrentLayoutStyle = LayoutStyle.CUSTOM;

            JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
            OnJFSemiDynamicLayoutChangeCompleted(ev);

            currSemiDynamicExecType = SemiDynamicExecType.NONE;

        }

        #endregion
    }
}