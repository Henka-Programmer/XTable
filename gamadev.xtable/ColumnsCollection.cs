using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace gamadev.xtable
{
    [Serializable]
    public class ColumnCollection : IList<XTableColumn>, ICollection<XTableColumn>, IEnumerable<XTableColumn>, IList, ICollection, IEnumerable, INotifyCollectionChanged
    {
        private ObservableCollection<XTableColumn> Source = new ObservableCollection<XTableColumn>();

        public ColumnCollection()
        {
            Source.CollectionChanged += (s, e) =>
            {
                if (CollectionChanged != null)
                {
                    CollectionChanged(s, e);
                }
            };
        }

        #region Notify

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion Notify

        #region IList<XTableColumn>

        public XTableColumn this[int index]
        {
            get
            {
                return Source[index];
            }

            set
            {
                Source[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return Source.Count;
            }
        }

        public bool IsReadOnly
        {
            get; private set;
        }

        public object SyncRoot
        {
            get; private set;
        }

        public bool IsSynchronized
        {
            get; private set;
        }

        object IList.this[int index]
        {
            get
            {
                return Source[index];
            }

            set
            {
                Source[index] = (XTableColumn)value;
            }
        }

        public void Add(XTableColumn item)
        {
            Source.Add(item);
        }

        public void Clear()
        {
            Source.Clear();
        }

        public bool Contains(XTableColumn item)
        {
            return Source.Contains(item);
        }

        public void CopyTo(XTableColumn[] array, int arrayIndex)
        {
            Source.CopyTo(array, arrayIndex);
        }

        public IEnumerator<XTableColumn> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        public int IndexOf(XTableColumn item)
        {
            return Source.IndexOf(item);
        }

        public void Insert(int index, XTableColumn item)
        {
            Source.Insert(index, item);
        }

        public bool Remove(XTableColumn item)
        {
            return Source.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Source.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        #endregion IList<XTableColumn>

        #region IList

        public bool IsFixedSize
        {
            get; private set;
        }

        public int Add(object value)
        {
            Source.Add((XTableColumn)value);
            return Source.IndexOf((XTableColumn)value);
        }

        public bool Contains(object value)
        {
            return Source.Contains((XTableColumn)value);
        }

        public int IndexOf(object value)
        {
            return Source.IndexOf((XTableColumn)value);
        }

        public void Insert(int index, object value)
        {
            Source.Insert(index, (XTableColumn)value);
        }

        public void Remove(object value)
        {
            Source.Remove((XTableColumn)value);
        }

        public void CopyTo(Array array, int index)
        {
            Source.CopyTo((XTableColumn[])array, index);
        }

        #endregion IList
    }
}