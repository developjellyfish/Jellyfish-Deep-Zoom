using System;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// EventArgs of Communication Error for JFDeepZoom
    /// </summary>
    public class JFCommunicationErrorEventArgs : EventArgs
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        public JFCommunicationErrorEventArgs(string inCode, string inDes)
        {
            code = inCode;
            description = inDes;
        }

        /// <summary>
        /// ErrorCode
        /// </summary>
        private string code = "000";

        /// <summary>
        /// ErrorCode
        /// </summary>
        public string Code
        {
            get 
            {
                return code; 
            }
            set
            {
                code = value;
            }
        }

        /// <summary>
        /// ErrorDescription
        /// </summary>
        private string description = "";

        /// <summary>
        /// ErrorDescription
        /// </summary>
        public string Description
        {
            get 
            {
                return description; 
            }
            set
            {
                description = value;
            }
        }
    }
}
