using System.Collections.Generic;
using System.Windows.Forms;

namespace System.ComponentModel {
	/// <summary>
	/// Describes a sorting operation.
	/// </summary>
	public class SortComparerEventArgs : HandledEventArgs {
		/// <summary>
		/// The property on which to sort the object in ascending order.
		/// </summary>
		public readonly PropertyDescriptor PropertySort;
		/// <summary>
		/// The first item to compare.
		/// </summary>
		public readonly object Item1;
		/// <summary>
		/// The second item to compare.
		/// </summary>
		public readonly object Item2;
		/// <summary>
		/// The value of the sort property for Item1.
		/// </summary>
		public readonly object Cell1Value;
		/// <summary>
		/// The value of the sort property for Item2.
		/// </summary>
		public readonly object Cell2Value;

		/// <summary>
		/// Gets or sets a value indicating the order in which the compared cells will be sorted.
		/// </summary>
		public int SortResult {
			get;
			set;
		}

		/// <summary>
		/// Describes a sorting operation.
		/// </summary>
		/// <param name="propertySort">The property on which to sort the object in ascending order.</param>
		/// <param name="item1">The first item to compare.</param>
		/// <param name="item2">The second item to compare.</param>
		/// <param name="cell1Value">The value of the sort property for Item1.</param>
		/// <param name="cell2Value">The value of the sort property for Item2.</param>
		public SortComparerEventArgs(PropertyDescriptor propertySort, object item1, object item2, object cell1Value, object cell2Value) {
			PropertySort = propertySort;
			Item1 = item1;
			Item2 = item2;
			Cell1Value = cell1Value;
			Cell2Value = cell2Value;
		}
	}

	/// <summary>
	/// Provides a generic collection that supports data binding, sorting and filtering.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	[Serializable]
	public class BindingListView<T> : BindingList<T>, IBindingListView, IRaiseItemChangedEvents, ITypedList {
		/// <summary>
		/// Called when the items are about to be sorted. Allows overriding the default sort behavior.
		/// </summary>
		/// <param name="sender">The SortComparer that is currently handling the sorting.</param>
		/// <param name="comparer">The event handler. If Handled is left false, then the default sorting mechanism will be used.</param>
		public delegate void SortCompareHandler(object sender, SortComparerEventArgs comparer);
		/// <summary>
		/// Called when the items are about to be sorted. Allows overriding the default sort behavior.
		/// </summary>
		public event SortCompareHandler SortCompare;
		[NonSerialized]
		private PropertyDescriptorCollection properties;
		private bool sorted;
		[NonSerialized]
		private bool filtering;
		[NonSerialized]
		private string filterString = string.Empty;
		[NonSerialized]
		private string target = string.Empty;
		private ListSortDirection sortDirection = ListSortDirection.Ascending;
		[NonSerialized]
		private PropertyDescriptor sortProperty;
		[NonSerialized]
		private PropertyDescriptor filterProperty;
		[NonSerialized]
		private ListSortDescriptionCollection sortDescriptions;
		[NonSerialized]
		private List<T> unfilteredList;
		[NonSerialized]
		private Dictionary<int, int> mapping;
		[NonSerialized]
		private Predicate<T> filter, stringFilter;

		/// <summary>
		/// Gets the current item filter (or null if there is no filter currently applied).
		/// </summary>
		public Predicate<T> CurrentFilter {
			get {
				return filter;
			}
		}

		/// <summary>
		/// Gets whether the list is currently filtered.
		/// </summary>
		public virtual bool Filtered {
			get {
				return unfilteredList != null;
			}
		}

		/// <summary>
		/// Gets a list of the full unfiltered elements. The Items property returns filtered elements (if a filter is applied).
		/// </summary>
		public virtual IList<T> UnfilteredElements {
			get {
				List<T> unfiltered = unfilteredList;
				if (unfiltered == null)
					return Items;
				else
					return unfiltered;
			}
		}

		/// <summary>
		/// True
		/// </summary>
		protected override bool SupportsSortingCore {
			get {
				return true;
			}
		}

		/// <summary>
		/// Gets whether the list is currently sorted.
		/// </summary>
		protected override bool IsSortedCore {
			get {
				return sorted;
			}
		}

		/// <summary>
		/// Gets the current sort direction.
		/// </summary>
		protected override ListSortDirection SortDirectionCore {
			get {
				return sortDirection;
			}
		}

		/// <summary>
		/// Gets the property on which the list is sorted.
		/// </summary>
		protected override PropertyDescriptor SortPropertyCore {
			get {
				return sortProperty;
			}
		}

		/// <summary>
		/// Gets or sets the associated filter string (use ApplyFilter() instead).
		/// </summary>
		public string Filter {
			get {
				return filterString;
			}
			set {
				if (value == null)
					value = string.Empty;
				else
					value = value.Trim();
				if (value.Length == 0) {
					filterString = string.Empty;
					RemoveFilter();
				} else {
					filterString = value;
					UpdateFilter();
				}
			}
		}

		ListSortDescriptionCollection IBindingListView.SortDescriptions {
			get {
				return sortDescriptions;
			}
		}

		bool IBindingListView.SupportsAdvancedSorting {
			get {
				return true;
			}
		}

		bool IBindingListView.SupportsFiltering {
			get {
				return true;
			}
		}

		bool IRaiseItemChangedEvents.RaisesItemChangedEvents {
			get {
				return true;
			}
		}

		/// <summary>
		/// True
		/// </summary>
		protected override bool SupportsSearchingCore {
			get {
				return true;
			}
		}

		/// <summary>
		/// Initializes a new empty bindable list.
		/// </summary>
		public BindingListView()
			: base() {
			stringFilter = FilterPredicate;
			AllowEdit = true;
			AllowNew = true;
			AllowRemove = true;
		}

		/// <summary>
		/// Initializes a new bindable list from the specified collection.
		/// </summary>
		/// <param name="list">The list to copy elements from.</param>
		public BindingListView(IList<T> list)
			: base(new List<T>(list)) {
			stringFilter = FilterPredicate;
			AllowEdit = true;
			AllowNew = true;
			AllowRemove = true;
		}

		/// <summary>
		/// Searches for the specified element in the current (filtered) list.
		/// </summary>
		/// <param name="property">The property to search for.</param>
		/// <param name="key">The value of the property to search for.</param>
		protected override int FindCore(PropertyDescriptor property, object key) {
			for (int i = 0; i < Count; i++) {
				if (property.GetValue(Items[i]).Equals(key))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Sorts the list using the specified property and direction.
		/// </summary>
		/// <param name="property">The property to sort from.</param>
		/// <param name="direction">The sorting direction.</param>
		protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction) {
			sortDirection = direction;
			sortProperty = property;
			ApplySortInternal(new SortComparer<T>(property, direction, SortCompare));
		}

		private void ApplySortInternal(SortComparer<T> comparer) {
			RemoveFilter(true);
			((List<T>) Items).Sort(comparer);
			sorted = true;
			ApplyFilter(filter);
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		/// <summary>
		/// Marks the list as unsorted.
		/// </summary>
		protected override void RemoveSortCore() {
			sortProperty = null;
			sortDescriptions = null;
			sorted = false;
		}

		void IBindingListView.ApplySort(ListSortDescriptionCollection sorts) {
			sortProperty = null;
			sortDescriptions = sorts;
			ApplySortInternal(new SortComparer<T>(sorts, SortCompare));
		}

		/// <summary>
		/// Gets the index in the unfiltered list that corresponds to the specified index of the same item in the Items list.
		/// </summary>
		/// <param name="index">The index of the item in the Items list.</param>
		public virtual int GetUnfilteredIndex(int index) {
			Dictionary<int, int> mapping = this.mapping;
			return mapping == null ? index : mapping[index];
		}

		/// <summary>
		/// Applies a filter to the Items list. The specified filter will replace previous filters.
		/// You can use Extensions.And or Extensions.Or to concatenate predicates.
		/// </summary>
		/// <param name="predicate">The filter to apply. Returnin true for an item means it is allowed into the filtered list.</param>
		public virtual void ApplyFilter(Predicate<T> predicate) {
			if (predicate == null)
				return;
			List<T> unfiltered = unfilteredList;
			if (unfiltered == null) {
				unfiltered = new List<T>(this);
				unfilteredList = unfiltered;
			}
			mapping = new Dictionary<int, int>();
			filtering = true;
			filter = predicate;
			Clear();
			T item;
			for (int i = 0; i < unfiltered.Count; i++) {
				item = unfiltered[i];
				if (predicate(item)) {
					mapping[Count] = i;
					Add(item);
				}
			}
			if (predicate != stringFilter) {
				filterString = string.Empty;
				target = string.Empty;
				filterProperty = null;
			}
			filtering = false;
		}

		/// <summary>
		/// Called when the filter has changed. Use ApplyFilter() instead.
		/// </summary>
		protected virtual void UpdateFilter() {
			string filter = filterString;
			if (string.IsNullOrEmpty(filter))
				return;
			int equalsPos = filter.IndexOf('=');
			target = filter.Substring(equalsPos + 1);
			filterProperty = TypeDescriptor.GetProperties(typeof(T))[filter.Substring(0, equalsPos).Trim()];
			this.filter = stringFilter;
			ApplyFilter(this.filter);
		}

		private bool FilterPredicate(T item) {
			return filterProperty.GetValue(item).ToString() == target;
		}

		/// <summary>
		/// Removes any filter that is applied to the list.
		/// </summary>
		public void RemoveFilter() {
			RemoveFilter(false);
		}

		/// <summary>
		/// Removes any filter that is applied to the list.
		/// </summary>
		private void RemoveFilter(bool willReapply) {
			mapping = null;
			if (!willReapply) {
				filterString = string.Empty;
				target = string.Empty;
				filterProperty = null;
				filter = null;
			}
			List<T> unfiltered = unfilteredList;
			unfilteredList = null;
			if (unfiltered != null) {
				filtering = true;
				Clear();
				for (int i = 0; i < unfiltered.Count; i++)
					Add(unfiltered[i]);
				filtering = false;
			}
		}

		/// <summary>
		/// Inserts the specified item at the specified index.
		/// </summary>
		/// <param name="index">The index tha</param>
		/// <param name="item">The item to insert.</param>
		protected override void InsertItem(int index, T item) {
			foreach (PropertyDescriptor propDesc in TypeDescriptor.GetProperties(item)) {
				if (propDesc.SupportsChangeEvents)
					propDesc.AddValueChanged(item, OnItemChanged);
			}
			base.InsertItem(index, item);
			if (!filtering) {
				List<T> unfiltered = unfilteredList;
				Dictionary<int, int> mapping = this.mapping;
				if (!(mapping == null || unfiltered == null)) {
					int targetIndex;
					if (mapping.TryGetValue(index, out targetIndex)) {
						unfiltered.Insert(targetIndex, item);
						int value;
						foreach (int key in new List<int>(mapping.Keys)) {
							value = mapping[key];
							if (value > index)
								mapping[key] = value + 1;
						}
					} else {
						mapping[index] = unfiltered.Count;
						unfiltered.Add(item);
					}
				}
			}
		}

		/// <summary>
		/// Removes all elements from the collection.
		/// </summary>
		protected override void ClearItems() {
			base.ClearItems();
			if (!filtering) {
				List<T> unfiltered = unfilteredList;
				if (unfiltered != null)
					unfiltered.Clear();
			}
		}

		/// <summary>
		/// Sets the item at the specified index. 
		/// </summary>
		/// <param name="index">The index to set.</param>
		/// <param name="item">The index to set the item.</param>
		protected override void SetItem(int index, T item) {
			base.SetItem(index, item);
			List<T> unfiltered = unfilteredList;
			if (unfiltered != null) {
				Dictionary<int, int> mapping = this.mapping;
				if (mapping != null)
					unfiltered[mapping[index]] = item;
			}
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The index of the item to remove.</param>
		protected override void RemoveItem(int index) {
			T item = Items[index];
			foreach (PropertyDescriptor propDesc in TypeDescriptor.GetProperties(item)) {
				if (propDesc.SupportsChangeEvents)
					propDesc.RemoveValueChanged(item, OnItemChanged);
			}
			base.RemoveItem(index);
			if (!filtering) {
				List<T> unfiltered = unfilteredList;
				Dictionary<int, int> mapping = this.mapping;
				if (mapping == null || unfiltered == null)
					return;
				unfiltered.RemoveAt(mapping[index]);
				int value;
				foreach (int key in new List<int>(mapping.Keys)) {
					value = mapping[key];
					if (value > index)
						mapping[key] = value - 1;
				}
			}
		}

		/// <summary>
		/// Gets a list of the properties that the items in the list have.
		/// </summary>
		/// <param name="listAccessors">A list of the properties in the item.</param>
		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors) {
			PropertyDescriptorCollection pdc;
			if (listAccessors != null && listAccessors.Length > 0)
				pdc = ListBindingHelper.GetListItemProperties(listAccessors[0].PropertyType);
			else {
				if (properties == null) {
					pdc = TypeDescriptor.GetProperties(typeof(T), new Attribute[] { new BrowsableAttribute(true) });
					properties = pdc/*.Sort()*/;
				}
				pdc = properties;
			}
			return pdc;
		}

		/// <summary>
		/// Gets the name of the view.
		/// </summary>
		/// <param name="listAccessors">Unused. Can be null.</param>
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors) {
			return typeof(T).Name;
		}

		private void OnItemChanged(object sender, EventArgs args) {
			int index = Items.IndexOf((T) sender);
			if (index != -1)
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
		}
	}

	/// <summary>
	/// Compares properties for sorting.
	/// </summary>
	/// <typeparam name="T">The type of items supported by the collection.</typeparam>
	public sealed class SortComparer<T> : IComparer<T> {
		private ListSortDescriptionCollection SortCollection;
		private PropertyDescriptor PropertyToSort;
		private ListSortDirection Direction = ListSortDirection.Ascending;
		private BindingListView<T>.SortCompareHandler comparer;

		/// <summary>
		/// Initializes a new sort comparer using the specified property and direction.
		/// </summary>
		/// <param name="propDesc">The property to sort.</param>
		/// <param name="direction">The direction to sort at.</param>
		public SortComparer(PropertyDescriptor propDesc, ListSortDirection direction = ListSortDirection.Ascending) {
			PropertyToSort = propDesc;
			Direction = direction;
		}

		/// <summary>
		/// Initializes a new sort comparer from the specified description collection.
		/// </summary>
		/// <param name="sortCollection"></param>
		public SortComparer(ListSortDescriptionCollection sortCollection) {
			SortCollection = sortCollection;
		}

		internal SortComparer(PropertyDescriptor propDesc, ListSortDirection direction, BindingListView<T>.SortCompareHandler comparer) {
			PropertyToSort = propDesc;
			Direction = direction;
			this.comparer = comparer;
		}

		internal SortComparer(ListSortDescriptionCollection sortCollection, BindingListView<T>.SortCompareHandler comparer) {
			SortCollection = sortCollection;
			this.comparer = comparer;
		}

		/// <summary>
		/// Compares the specified items and returns which is considered larger using the sorting configuration specified in the constructor.
		/// </summary>
		/// <param name="x">The first value to compare.</param>
		/// <param name="y">The second value to compare.</param>
		public int Compare(T x, T y) {
			int result;
			if (PropertyToSort != null) {
				result = CompareValues(PropertyToSort, x, y, PropertyToSort.GetValue(x), PropertyToSort.GetValue(y));
				if (Direction == ListSortDirection.Descending)
					result = -result;
				return result;
			} else if (SortCollection != null) {
				ListSortDescription listSort;
				for (int i = 0; i < SortCollection.Count; i++) {
					listSort = SortCollection[i];
					result = CompareValues(listSort.PropertyDescriptor, x, y, listSort.PropertyDescriptor.GetValue(x), listSort.PropertyDescriptor.GetValue(y));
					if (result != 0) {
						if (listSort.SortDirection == ListSortDirection.Descending)
							result = -result;
						return result;
					}
				}
			}
			return 0;
		}

		private int CompareValues(PropertyDescriptor propertyToSort, T item1, T item2, object cell1Value, object cell2Value) {
			if (comparer != null) {
				SortComparerEventArgs eventArgs = new SortComparerEventArgs(propertyToSort, item1, item2, cell1Value, cell2Value);
				comparer(this, eventArgs);
				if (eventArgs.Handled)
					return eventArgs.SortResult;
			}
			IComparable value1 = cell1Value as IComparable;
			if (value1 != null)
				return value1.CompareTo(cell2Value);
			else {
				IComparable value2 = cell2Value as IComparable;
				if (value2 == null)
					return cell1Value.ToString().CompareTo(cell2Value.ToString());
				else
					return value2.CompareTo(cell1Value);
			}
		}
	}
}