using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Communication common]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {
        #region properties

        /// <summary>
        /// XML of Matainfo Data
        /// </summary>
        private XDocument detailData = new XDocument();

        /// <summary>
        /// MultiScaleImage is loaded or not.
        /// </summary>
        /// 
        private bool isMsiLoaded = false;

        /// <summary>
        /// Uri for dzc of MultiScaleImage.
        /// </summary>
        private string src;

        /// <summary>
        /// Uri for dzc of MultiScaleImage.
        /// </summary>
        public string Src
        {
            get 
            {
                return src; 
            }
            set
            {
                // set msi.Source if CurrentApproachType == ApproachType.STATIC.
                if (CurrentApproachType == ApproachType.STATIC && isMsiLoaded)
                {
                    if (src.Contains("http://") || src.Contains("https://"))
                    {
                        try
                        {
                            indices = new IndicesList();
                            msi.Source = new DeepZoomImageTileSource(new Uri(src, UriKind.Absolute));
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        try
                        {
                            indices = new IndicesList();
                            msi.Source = new DeepZoomImageTileSource(new Uri(src, UriKind.Relative));
                        }
                        catch (Exception) { }
                    }
                }
                else if (!isMsiLoaded)
                {
                    src = value;
                }
            }
        }

        /// <summary>
        /// If CurrentApproachType is SEMI_DYNAMIC or DYNAMIC ApproachType,
        /// you have to use method to set Source
        /// </summary>
        /// <param name="collectionPath">path of dzc</param>
        private void SetSrc(string collectionPath)
        {
            if (msi.SubImages.Count != 0)
            {   
                // De-Select all Images
                RemoveAllSelectedIndex();
            }

            src = collectionPath;

            if (CurrentApproachType != ApproachType.STATIC || isMsiLoaded)
            {
                if (src.Contains("http://") || src.Contains("https://"))
                {
                    try
                    {
                        indices = new IndicesList();
                        msi.Source = new DeepZoomImageTileSource(new Uri(src, UriKind.Absolute));
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        indices = new IndicesList();
                        msi.Source = new DeepZoomImageTileSource(new Uri(src, UriKind.Relative));
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        /// <summary>
        /// Getter / Setter for the type of the current approach(e.g. "Static", "Semi-Dynamic", "Dynamic").
        /// </summary>
        private string currentApproachType = ApproachType.DYNAMIC;

        /// <summary>
        /// Getter / Setter for the type of the current approach(e.g. "Static", "Semi-Dynamic", "Dynamic").
        /// </summary>
        /// <value>The type of the current approach.</value>
        public string CurrentApproachType
        {
            get
            {
                return currentApproachType;
            }
            set
            {
                currentApproachType = value;
            }
        }

        /// <summary>
        /// List of thumbnail images.
        /// </summary>
        public List<JFThumbnailList> ThumbnailList = new List<JFThumbnailList>();

        /// <summary>
        /// WebClient
        /// </summary>
        private WebClient wc;

        #endregion

        #region Communication

        #region Communication Events

        /// <summary>
        /// Initialize communication event. 
        /// </summary>
        protected virtual void InitCommunicationEvent()
        {
            wc = new WebClient();
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
            wc.OpenWriteCompleted += new OpenWriteCompletedEventHandler(wc_OpenWriteCompleted);
            wc.UploadProgressChanged += new UploadProgressChangedEventHandler(wc_UploadProgressChanged);
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
        }


        #region JFDeepZoom event handler

        /// <summary>
        /// It call when Source of MultiScaleImage is set and file loading is completed.
        /// </summary>
        public event JFDeepZoomEventHandler JFDeepZoomSrcLoaded;

        /// <summary>
        /// It raises when Source of MultiScaleImage is set and file loading is completed.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDeepZoomSrcLoaded(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = JFDeepZoomSrcLoaded;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// This event is called if Src property is null.
        /// </summary>
        public event JFDeepZoomEventHandler JFDeepZoomSrcIsNull;

        /// <summary>
        /// This event is raised if Src property is null.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFDeepZoomEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDeepZoomSrcIsNull(JFDeepZoomEventArgs e)
        {
            JFDeepZoomEventHandler handler = JFDeepZoomSrcIsNull;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// This event is called if Src property exceeds the limit.
        /// </summary>
        public event JFCommunicationErrorEventHandler JFDeepZoomSrcIsOverLimit;

        /// <summary>
        /// This event is raised if Src property exceeds the limit.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDeepZoomSrcIsOverLimit(JFCommunicationErrorEventArgs e)
        {
            JFCommunicationErrorEventHandler handler = JFDeepZoomSrcIsOverLimit;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion


        #region webclient Event Handler

        /// <summary>
        /// Event handler for UploadStringCompleted event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Net.UploadStringCompletedEventArgs"/> instance containing the event data.</param>
        private void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
            OnJFUploadStringCompleted(ev);
        }

        /// <summary>
        /// Event handler for UploadProgressChanged event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Net.UploadProgressChangedEventArgs"/> instance containing the event data.</param>
        private void wc_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
            OnJFUploadProgressChanged(ev);
        }

        /// <summary>
        /// Event handler for OpenReadCompleted event.
        /// 
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Net.OpenReadCompletedEventArgs"/> instance containing the event data.</param>
        private void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
            OnJFOpenReadCompleted(ev);
        }

        /// <summary>
        /// Event handler for OpenWriteCompleted event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Net.OpenWriteCompletedEventArgs"/> instance containing the event data.</param>
        private void wc_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
            OnJFOpenWriteCompleted(ev);
        }

        /// <summary>
        /// Event handler for DownloadProgressChanged event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Net.DownloadProgressChangedEventArgs"/> instance containing the event data.</param>
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
            OnJFDownloadProgressChanged(ev);
        }

        /// <summary>
        /// Event handler for DownloadStringCompleted event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The <see cref="System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // In case of "Dynamic"
            if (currentApproachType == ApproachType.DYNAMIC)
            {
                DynamicDownloadStringCompleted(e.Result);

            // In case of "semi-Dynamic"
            } 
            else 
            {

                // In case of "INIT"
                if (currSemiDynamicExecType == SemiDynamicExecType.INIT)
                {
                    SemiDynamicInitStringCompleted(e.Result);
                }
                // --------------------------------------
                // In case of "INIT_CHANGE_ORDER"
                else if (currSemiDynamicExecType == SemiDynamicExecType.INIT_CHANGE_ORDER)
                {
                    SemiDynamicInitStringCompleted2(e.Result);
                }
                // --------------------------------------
                // In case of "LIST"
                else if (currSemiDynamicExecType == SemiDynamicExecType.LIST)
                {
                    SemiDynamicListStringCompleted(e.Result);
                }
                // --------------------------------------
                // In case of "LAYOUT"
                else if (currSemiDynamicExecType == SemiDynamicExecType.LAYOUT)
                {
                    SemiDynamicLayoutStringCompleted(e.Result);
                }

                // --------------------------------------
                // In case of "CHANGE_LAYOUT"
                else if (currSemiDynamicExecType == SemiDynamicExecType.CHANGE_LAYOUT)
                {
                    SemiDynamicChangeLayoutStringCompleted(e.Result);
                }
            }
        }

        /// <summary>
        /// Get the filteringUid list.
        /// </summary>
        /// <returns>Filtered UID list</returns>
        private List<string> getFilteringUIdList()
        {
            List<string> outList = new List<string>();

            // It takes current MaximumUid List.
            List<string> allUidList = getUIdListFromMetaInfo();

            // It takes subimage index currently displayed.
            IndicesList indices = this.Indices;

            // filteringUid = allUidList - indices
            for (int i = 0; i < allUidList.Count; i++)
            {
                if (indices.IndexOf(i) == -1)
                {
                    outList.Add(allUidList[i]);
                }
            }
            return outList;
        }

        /// <summary>
        /// Get the Uid List in the MetaInfo.
        /// </summary>
        /// <returns>Uid List in the MetaInfo</returns>
        private List<string> getUIdListFromMetaInfo()
        {
            List<string> outList = new List<string>();

            try
            {
                var q = from c in detailData.Descendants("Image")
                        select (string)c.Element("AdditionalData").Element("UId");

                if (q.Count() > 0)
                {
                    foreach (string resultdata in q)
                    {
                        try
                        {
                            string resdata = resultdata;

                            outList.Add(resdata);
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception)
            {
                JFDeepZoomEventArgs nullEv = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomSrcIsNull(nullEv);
            }

            return outList;
        }

        /// <summary>
        /// Get the Sid List in the MetaInfo.
        /// </summary>
        /// <returns>Sid List in the MetaInfo</returns>
        private string getSidLFromMetaInfo()
        {
            string outStr = "";

            try
            {
                outStr = detailData.Element("Metadata").Element("SId").Value;
            }
            catch (Exception)
            {
                JFDeepZoomEventArgs nullEv = new JFDeepZoomEventArgs(msi);
                OnJFDeepZoomSrcIsNull(nullEv);
            }

            return outStr;
        }

        #endregion

        #endregion

        #region Communication properties
  
        /// <summary>
        /// parameterized Uri
        /// </summary>
        private string sendFullUri = "";

        #endregion

        #region Custom Events

        /// <summary>
        /// It call when DownloadStringCompleted event of the WebClient
        /// </summary>
        public event JFCommunicationEventHandler JFDownloadStringCompleted;

        /// <summary>
        /// It raises when DownloadStringCompleted event of the WebClient.
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDownloadStringCompleted(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFDownloadStringCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It call when DownloadProgressChanged event of the WebClient
        /// </summary>
        public event JFCommunicationEventHandler JFDownloadProgressChanged;

        /// <summary>
        /// It raises the DownloadProgressChanged event of the WebClient
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFDownloadProgressChanged(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFDownloadProgressChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It call when OpenReadCompleted event of the WebClient
        /// </summary>
        public event JFCommunicationEventHandler JFOpenReadCompleted;

        /// <summary>
        /// It raises the OpenReadCompleted event of the WebClient
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFOpenReadCompleted(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFOpenReadCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It call when OpenWriteCompleted event of the WebClient
        /// </summary>
        public event JFCommunicationEventHandler JFOpenWriteCompleted;

        /// <summary>
        /// It raises the OpenWriteCompleted event of the WebClient
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFOpenWriteCompleted(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFOpenWriteCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It call when UploadProgressChanged event of the WebClient
        /// </summary>
        public event JFCommunicationEventHandler JFUploadProgressChanged;

        /// <summary>
        /// It raises the UploadProgressChanged event of the WebClient
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFUploadProgressChanged(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFUploadProgressChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// It call when UploadStringCompleted event of the WebClient
        /// </summary>
        public event JFCommunicationEventHandler JFUploadStringCompleted;

        /// <summary>
        /// It raises the UploadStringCompleted event of the WebClient
        /// </summary>
        /// <param name="e">The <see cref="Jellyfish.jfDeepZoom.JFCommunicationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnJFUploadStringCompleted(JFCommunicationEventArgs e)
        {
            JFCommunicationEventHandler handler = JFUploadStringCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #endregion

        #region SubImageData

        /// <summary>
        /// It takes MetaInfo data related to SubImage.
        /// </summary>
        /// <param name="id">Index of SubImage</param>
        /// <param name="tagName">Element name</param>
        /// <returns>MetaInfo data related to SubImage</returns>
        public string GetSubImageData(int id, string tagName)
        {
            string res = "";
            res = doGetSubImageData(id, tagName);
            return res;
        }

        /// <summary>
        /// It takes MetaInfo data related to SubImage.
        /// </summary>
        /// <param name="id">Index of SubImage</param>
        /// <param name="tagName">Element name</param>
        /// <returns>MetaInfo data related to SubImage</returns>
        private string doGetSubImageData(int id, string tagName)
        {
            string res = "";

            // Specified index of SubImage is existing or not
            judgeInRangeSubImage(id);

            // It takes current All of UId list.
            List<string> allUidList = getUIdListFromMetaInfo();

            // It takes UID.
            string aUId = allUidList[id];

            if (detailData != null && detailData.ToString() != "")
            {
                if (tagName == "Tag")
                {
                    // In case of "Tag"
                    var q = from c in detailData.Descendants("Image")
                            where (string)c.Element("AdditionalData").Element("UId") == aUId
                            select (string)c.Element("Tag");

                    if (q.Count() > 0)
                    {
                        foreach (string resultdata in q)
                        {
                            try
                            {
                                res = resultdata.ToString();
                            }
                            catch (Exception) { }
                        }
                    }
                }
                else
                {
                    var q = from c in detailData.Descendants("Image")
                            where (string)c.Element("AdditionalData").Element("UId") == aUId
                            select (string)c.Element("AdditionalData").Element(tagName);

                    if (q.Count() > 0)
                    {
                        foreach (string resultdata in q)
                        {
                            try
                            {
                                res = resultdata.ToString();
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Get the DZCPath
        /// </summary>
        /// <returns>*.dzcPath</returns>
        private string doGetDZCPath()
        {
            string res = "";

            var q = from c in detailData.Descendants("dzc")
                    select (string)c.Element("dzc");
            if (q.Count() > 0)
            {
                foreach (string resultdata in q)
                {
                    try
                    {
                        res = resultdata.ToString();
                    }
                    catch (Exception) { }
                }
            }

            return res;
        }

        #endregion

        #region utils

        /// <summary>
        /// Convert thumbnail element to index of SubImage and return, return -1, if it is not.
        /// </summary>
        /// <param name="imageName">Thumbnail element</param>
        /// <returns>Index of SubImage</returns>
        public int ImageNameToIndex(string imageName)
        {
            int outInt = -1;

            string res = "";

            if (detailData != null)
            {
                var q = from c in detailData.Descendants("Image")
                        where (string)c.Element("AdditionalData").Element("Thumbnail") == imageName
                        select (string)c.Element("AdditionalData").Element("UId");

                if (q.Count() > 0)
                {
                    foreach (string resultdata in q)
                    {
                        try
                        {
                            res = resultdata;
                        }
                        catch (Exception) { }
                    }
                }
            }

            // It takes current All of Uid list.
            List<string> allUidList = getUIdListFromMetaInfo();

            outInt = allUidList.IndexOf(res);

            return outInt;
        }

        /// <summary>
        /// Regenerate ThumbnailList from Indices.
        /// </summary>
        private void IndicesToThumbnailList()
        {
            ThumbnailList = new List<JFThumbnailList>();
            for (int i = 0; i < Indices.Count; i++)
            {
                ThumbnailList.Add(ChangeIndicesToThumbnail(Indices[i]));
            }
        }

        /// <summary>
        /// Return index of SubImages which converted JFTthumbnailList type.  
        /// </summary>
        /// <param name="index">Index of SubImage</param>
        /// <returns>Type of JFThumbnailList</returns>
        private JFThumbnailList ChangeIndicesToThumbnail(int index)
        {
            JFThumbnailList res = new JFThumbnailList();

            res.ThumbnailPath = detailData.Element("Metadata").Elements("Image").ElementAt(index)
                .Element("AdditionalData").Element("Thumbnail").Value;
            Debug.WriteLine(res.ThumbnailPath);

            return res;
        }

        /// <summary>
        /// Return index of SubImages which converted Thumbnail file name
        /// return "", if it is not. 
        /// </summary>
        /// <param name="imageName">Index of SubImage</param>
        /// <returns>ThumbnailPath of SubImage</returns>
        public string IndexToImageName(int index)
        {
            string res = "";

            // It takes current All of Uid list.
            List<string> allUidList = getUIdListFromMetaInfo();

            // It takes UID.
            string aUId = allUidList[index];

            if (detailData != null)
            {
                var q = from c in detailData.Descendants("Image")
                        where (string)c.Element("AdditionalData").Element("UId") == aUId
                        select (string)c.Element("AdditionalData").Element("Thumbnail");
                if (q.Count() > 0)
                {
                    foreach (string resultdata in q)
                    {
                        try
                        {
                            res = resultdata;
                        }
                        catch (Exception) { }
                    }
                }
            }
            return res;
        }

        #endregion
    }

}
