using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace gamadev.xtable
{
    public class SortDescription
    {
        [DefaultValue(false)]
        public virtual bool IsGrouped { get; set; }

        public virtual string PropertyName { get; set; }
        public virtual ListSortDirection Direction { get; set; }

        public static implicit operator SortDescription(System.ComponentModel.SortDescription sortDes)
        {
            return new SortDescription() { PropertyName = sortDes.PropertyName, Direction = sortDes.Direction };
        }

        public static implicit operator System.ComponentModel.SortDescription(SortDescription sortDes)
        {
            return new System.ComponentModel.SortDescription() { PropertyName = sortDes.PropertyName, Direction = sortDes.Direction };
        }
    }

    public class SortDescriptionCollection : Collection<SortDescription>, INotifyCollectionChanged
    {
        public SortDescriptionCollection()
        {
        }

        protected override void InsertItem(int index, SortDescription item)
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

        protected override void SetItem(int index, SortDescription item)
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