
namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// Object using Filter.
    /// contains
    ///   FilterKey
    ///   FilterValue
    ///   filterOperation
    /// </summary>
    public class FilterObject
    {

        /// <summary>
        /// value of FilterKey
        /// </summary>
        private string filterKey = "";

        /// <summary>
        /// value of FilterKey
        /// </summary>
        public string FilterKey
        {
            get
            {
                return filterKey;
            }
            set
            {
                filterKey = value;
            }
        }

        /// <summary>
        /// value of FilterValue
        /// </summary>
        private string filterValue = "";

        /// <summary>
        /// value of FilterValue
        /// </summary>
        public string FilterValue
        {
            get
            {
                return filterValue;
            }
            set
            {
                filterValue = value;
            }
        }

        /// <summary>
        /// Operation kind using Filter
        /// </summary>
        private string filterOperation = "";

        /// <summary>
        /// Operation kind using Filter
        /// </summary>
        public string FilterOperation
        {
            get
            {
                return filterOperation;
            }
            set
            {
                filterOperation = value;

                switch (value)
                {
                    case FilterOperationType.OPERATION_EQUAL:
                        filterOperation = "equal";
                        break;
                    case FilterOperationType.OPERATION_LESSTHAN:
                        filterOperation = "lt";  // < 
                        break;
                    case FilterOperationType.OPERATION_GREATERTHAN:
                        filterOperation = "gt";  // >
                        break;
                    case FilterOperationType.OPERATION_CONTAIN:
                        filterOperation = "contain";
                        break;
                    default:
                        filterOperation = "equal";
                        break;
                }
            }
        }
    }



    /// <summary>
    /// Filter operation type
    /// </summary>
    public static class FilterOperationType
    {
        /// <summary>
        /// in case of "equal"
        /// </summary>
        public const string OPERATION_EQUAL = "equal";

        /// <summary>
        /// in case of "&lt;"
        /// </summary>
        public const string OPERATION_LESSTHAN = "lt";

        /// <summary>
        /// in case of "&gt;"
        /// </summary>
        public const string OPERATION_GREATERTHAN = "gt";

        /// <summary>
        /// in case of "contain"
        /// </summary>
        public const string OPERATION_CONTAIN = "contain";
    }
}
