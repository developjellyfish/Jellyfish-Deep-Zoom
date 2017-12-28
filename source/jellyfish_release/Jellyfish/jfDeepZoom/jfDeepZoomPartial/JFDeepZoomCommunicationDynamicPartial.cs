using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// [Communication Dynamic]
    /// </summary>
    public partial class JFDeepZoom : Canvas
    {

        /// <summary>
        /// Normal code of communication
        /// </summary>
        protected string normalCode = "200";

        /// <summary>
        /// It called, when the communication for dynamic approach is completed. 
        /// </summary>
        /// <param name="xmlstr">Return xml from server</param>
        protected virtual void DynamicDownloadStringCompleted(string xmlstr)
        {
            #region Dynamic

            ThumbnailList = new List<JFThumbnailList>();

            string dzcpathString = "";
            try
            {
                detailData = XDocument.Parse(xmlstr, LoadOptions.None);

                // check errorCode
                string statusCode = detailData.Element("Metadata").Element("status").Attribute("code").Value;
                if (statusCode != normalCode)
                {
                    string errorDescription = detailData.Element("Metadata").Element("status").Value;

                    JFCommunicationErrorEventArgs overEv = new JFCommunicationErrorEventArgs(statusCode, errorDescription);
                    OnJFDeepZoomSrcIsOverLimit(overEv);
                }
                else
                {
                    XElement dzcpathNode = detailData.Element("Metadata").Element("Dzcpath");

                    dzcpathString = dzcpathNode.Value;

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
            }

            if (dzcpathString != "")
            {

                InitSubImages();

                // initialize(if not, The index value is not going to be suitable)
                Indices = new IndicesList();

                SetSrc(dzcpathString);

                JFCommunicationEventArgs ev = new JFCommunicationEventArgs();
                OnJFDownloadStringCompleted(ev);

            }

            #endregion
        }

        /// <summary>
        /// Send data("GET" from Silverlight. Return Metadata.xml and file path of dzc.).
        /// </summary>
        /// <param name="uri">Uri of API.</param>
        /// <param name="dataList">It contains one column for each index of list.
        /// dataList[index]["column name"]["value"]</param>
        /// <param name="order">order</param>
        /// <param name="orderby">orderby (asc / desc)</param>
        /// <param name="searchType">Type of search (and / or)</param>
        public void Send(string uri, List<Dictionary<string, string>> dataList, string order, string orderby, string searchType)
        {
            string queryString = "";

            // Build query string.
            for (int i = 0; i < dataList.Count; i++)
            {
                foreach (string key in dataList[i].Keys)
                {
                    queryString += "&" + key + "=" + dataList[i][key];
                }
            }

            currentApproachType = ApproachType.DYNAMIC;
            currSemiDynamicExecType = SemiDynamicExecType.NONE;

            sendFullUri = uri + "?" + queryString + "&searchType=" + searchType + "&order=" + order + "&orderby=" + orderby;

            if (uri.Contains("http://") || uri.Contains("https://"))
            {
                wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Absolute));
            }
            else
            {
                wc.DownloadStringAsync(new Uri(sendFullUri, UriKind.Relative));
            }
        }
    }
}