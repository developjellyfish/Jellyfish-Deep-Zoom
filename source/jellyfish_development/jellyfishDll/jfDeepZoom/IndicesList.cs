using System.Collections.Generic;
using System.Collections.Specialized;

namespace Jellyfish.jfDeepZoom
{
    /// <summary>
    /// Display List that stores Index Of SubImage.
    /// </summary>
    public class IndicesList: List<int>,INotifyCollectionChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public IndicesList()
        {
            
        }

        #region reverse

        /// <summary>
        /// Reverse. [override]
        /// </summary>
        public new void Reverse()
        {
            base.Reverse();
        }

        /// <summary>
        /// Override. Reverse.
        /// </summary>
        /// <param name="index">index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        public new void Reverse(int index, int count)
        {
            base.Reverse(index, count);
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }
        #endregion

        /// <summary>
        /// Override. RemoveRange
        /// </summary>
        /// <param name="index">index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, index);
        }

        /// <summary>
        /// Override. Remove all data.
        /// </summary>
        private new void Clear()
        {
            this.RemoveRange(0, this.Count);
        }

        /// <summary>
        /// Override. Add data.
        /// </summary>
        /// <param name="index">Index of SubImage</param>
        public new void Add(int index)
        {
            //--- Repetition is not admitted. 
            if (! this.Contains(index))
            {
                base.Add(index);
                OnCollectionChanged(NotifyCollectionChangedAction.Add, index);
            }
        }

        /// <summary>
        /// Adds the specified index.
        /// </summary>
        /// <param name="index">Index of SubImage</param>
        /// <param name="callEvent">if set to <c>true</c> execute Add Method</param>
        private void Add(int index, bool callEvent)
        {
            if(! callEvent) base.Add(index);
        }

        /// <summary>
        /// Override. Remove specified index.
        /// </summary>
        /// <param name="index">index of the element to remove.</param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, index);
        }

        /// <summary>
        /// Swap Indices
        /// </summary>
        /// <param name="obj">target object</param>
        public void SwapObject(IndicesList obj)
        {
            int len = obj.Count;
            for (int i = 0; i < len; i++)
            {
                Add(obj[i]);
            }
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        /// <summary>
        /// It calles when Collection Changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// It raises when Collection Changed.
        /// </summary>
        /// <param name="action">NotifyCollectionChangedAction</param>
        /// <param name="index">The index.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action,
           int index)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(action, this, index));
            }
        }

        /// <summary>
        /// It raises when Collection Changed.
        /// </summary>
        /// <param name="action">NotifyCollectionChangedAction</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(action));
            }
        }
    }
}
