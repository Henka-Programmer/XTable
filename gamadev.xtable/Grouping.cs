using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace gamadev.xtable
{
    public class GroupResult
    {
        public object Key { get; set; }
        public int Count { get; set; }
        public IEnumerable Items { get; set; }
        public IEnumerable<GroupResult> SubGroups { get; set; }

        public override string ToString()
        { return string.Format("{0} ({1})", Key, Count); }
    }

    public class GroupDescription
    {
        public virtual string PropertyName { get; set; }
    }

    public class GroupDescriptionCollection : Collection<GroupDescription>, INotifyCollectionChanged
    {
        public GroupDescriptionCollection()
        {
        }

        protected override void InsertItem(int index, GroupDescription item)
        {
            base.InsertItem(index, item);
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            }
        }

        protected override void SetItem(int index, GroupDescription item)
        {
            base.SetItem(index, item);
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}