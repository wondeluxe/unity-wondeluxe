using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Wondeluxe
{
	/// <summary>
	/// Represents a strongly typed list of unique objects that can be accessed by index.
	/// Provides methods to search, sort, and manipulate lists.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <remarks>
	/// Implementation nearly identical to Microsoft's
	/// <see href="https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs">
	/// List&lt;T&gt;</see> implementation, with added functionality to ensure no duplicate elements are not added.
	/// </remarks>

	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class SetList<T> : IList<T>, IList, IReadOnlyList<T>
	{
		#region Constants

		private const int DefaultCapacity = 4;

		// Equal to Array.MaxArrayLength.
		// https://referencesource.microsoft.com/#mscorlib/system/array.cs,2d2b551eabe74985

		private const int MaxCapacity = 0X7FEFFFFF;

		#endregion

		#region Fields

		private T[] items;

		private int size;

		private int version;

		[NonSerialized]
		private object syncRoot;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new SetList instance that is empty and has the default initial capacity.
		/// </summary>

		public SetList()
		{
			items = emptyArray;
		}

		/// <summary>
		/// Initializes a new SetList instance that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new set list can initially store.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>capacity</c> is less than 0.</exception>

		public SetList(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity is less than 0.");
			}

			items = capacity == 0 ? emptyArray : new T[capacity];
		}

		/// <summary>
		/// Initializes a new SetList instance that contains elements copied from the specified collection.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		/// <exception cref="ArgumentNullException"><c>collection</c> is null.</exception>

		public SetList(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection), "Collection is null.");
			}

			if (collection is ICollection<T> c)
			{
				int count = c.Count;

				if (count == 0)
				{
					items = emptyArray;
				}
				else
				{
					items = new T[count];
					c.CopyTo(items, 0);
					size = count;
				}
			}
			else
			{
				size = 0;
				items = emptyArray;

				// This enumerable could be empty. Let Add allocate a new array, if needed.
				// Note it will also go to _defaultCapacity first, not 1, then 2, etc.

				using IEnumerator<T> en = collection.GetEnumerator();

				while (en.MoveNext())
				{
					Add(en.Current);
				}
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the total number of elements the internal data structure can hold without resizing.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Capacity is set to a value that is less than <c>Count</c>.</exception>

		public int Capacity
		{
			get { return items.Length; }
			set
			{
				if (value < size)
				{
					throw new ArgumentOutOfRangeException(nameof(value), "Capacity is set to a value that is less than Count.");
				}

				if (value != items.Length)
				{
					if (value > 0)
					{
						Array.Resize(ref items, value);
					}
					else
					{
						items = emptyArray;
					}
				}
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the SetList.
		/// </summary>

		public int Count
		{
			get { return size; }
		}

		/// <summary>
		/// Gets a value indicating whether the IList has a fixed size. Will always return <c>false</c>.
		/// </summary>

		bool IList.IsFixedSize
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether the ICollection is read-only. Will always return <c>false</c>.
		/// </summary>

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether the IList is read-only. Will always return <c>false</c>.
		/// </summary>

		bool IList.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether access to the ICollection is synchronized (thread safe).
		/// </summary>

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the ICollection.
		/// </summary>

		object ICollection.SyncRoot
		{
			get
			{
				if (syncRoot == null)
				{
					System.Threading.Interlocked.CompareExchange<object>(ref syncRoot, new object(), null);
				}

				return syncRoot;
			}
		}

		#endregion

		#region Indexer

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is equal to or greater than <c>Count</c>.</exception>

		public T this[int index]
		{
			get
			{
				// Following trick can reduce the range check by one
				if ((uint)index >= (uint)size)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				return items[index];
			}
			set
			{
				if ((uint)index >= (uint)size)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				items[index] = value;
				version++;
			}
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="ArgumentNullException"><c>null</c> isn't assignable to the IList.</exception>
		/// <exception cref="ArgumentException">The set value is of a type that isn't assignable to the IList.</exception>

		object IList.this[int index]
		{
			get { return this[index]; }
			set
			{
				if (value == null && default(T) != null)
				{
					throw new ArgumentNullException();
				}

				try
				{
					this[index] = (T)value;
				}
				catch (InvalidCastException)
				{
					throw new ArgumentException($"Value is not assignable to type {typeof(T)}.");
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds an object to the end of the SetList if it is not already present.
		/// </summary>
		/// <param name="item">The element to add to the set list. The value can be <c>null</c> for reference types.</param>
		/// <returns><c>true</c> if the element is added to the SetList object; <c>false</c> if the element is already present.</returns>

		public bool Add(T item)
		{
			if (Contains(item))
			{
				return false;
			}

			if (size == items.Length)
			{
				EnsureCapacity(size + 1);
			}

			items[size] = item;

			size++;
			version++;

			return true;
		}

		/// <summary>
		/// Adds an object to the end of the ICollection if it is not already present.
		/// </summary>
		/// <param name="item">The element to add to the set list. The value can be <c>null</c> for reference types.</param>

		void ICollection<T>.Add(T item)
		{
			Add(item);
		}

		/// <summary>
		/// Adds an object to the end of the IList if it is not already present.
		/// </summary>
		/// <param name="item">The <c>object</c> to add to the IList.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <exception cref="ArgumentNullException"><c>null</c> isn't assignable to the IList.</exception>
		/// <exception cref="ArgumentException"><c>item</c> is of a type that isn't assignable to the IList.</exception>

		int IList.Add(object item)
		{
			if (item == null && default(T) != null)
			{
				throw new ArgumentNullException();
			}

			try
			{
				Add((T)item);
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException($"Item is not assignable to type {typeof(T)}.");
			}

			return Count - 1;
		}

		/// <summary>
		/// Adds the elements of the specified collection to the end of the SetList if they are not already present.
		/// </summary>
		/// <param name="collection">The collection whose elements should be added to the end of the SetList. The collection itself cannot be <c>null</c>, but it can contain elements that are <c>null</c>, if type <c>T</c> is a reference type.</param>
		/// <remarks>
		/// This method may add none, some, or all of the elements in <c>collection</c>.
		/// </remarks>

		public int AddRange(IEnumerable<T> collection)
		{
			return InsertRange(size, collection);
		}

		/// <summary>
		/// Returns a read-only ReadOnlyCollection wrapper for the specified set list.
		/// </summary>
		/// <returns>An object that acts as a read-only wrapper around the current SetList.</returns>

		public ReadOnlyCollection<T> AsReadOnly()
		{
			return new ReadOnlyCollection<T>(this);
		}

		/// <summary>
		/// Searches a range of elements in the sorted SetList for an element using the specified comparer and returns the zero-based index of the element.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to search.</param>
		/// <param name="count">The length of the range to search.</param>
		/// <param name="item">The object to locate. The value can be <c>null</c> for reference types.</param>
		/// <param name="comparer">The IComparer implementation to use when comparing elements, or <c>null</c> to use the default comparer <c>Default</c>.</param>
		/// <returns>The zero-based index of <c>item</c> in the sorted SetList, if <c>item</c> is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than <c>item</c> or, if there is no larger element, the bitwise complement of <c>Count</c>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range in the SetList.</exception>

		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			if (size - index < count)
			{
				throw new ArgumentException("Search out of range.");
			}

			return Array.BinarySearch(items, index, count, item, comparer);
		}

		/// <summary>
		/// Searches the entire sorted SetList for an element using the default comparer and returns the zero-based index of the element.
		/// </summary>
		/// <param name="item">The object to locate. The value can be <c>null</c> for reference types.</param>
		/// <returns>The zero-based index of <c>item</c> in the sorted SetList, if <c>item</c> is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than <c>item</c> or, if there is no larger element, the bitwise complement of <c>Count</c>.</returns>

		public int BinarySearch(T item)
		{
			return BinarySearch(0, Count, item, null);
		}

		/// <summary>
		/// Searches the entire sorted SetList for an element using the specified comparer and returns the zero-based index of the element.
		/// </summary>
		/// <param name="item">The object to locate. The value can be <c>null</c> for reference types.</param>
		/// <param name="comparer">The IComparer implementation to use when comparing elements, or <c>null</c> to use the default comparer <c>Default</c>.</param>
		/// <returns>The zero-based index of <c>item</c> in the sorted SetList, if <c>item</c> is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than <c>item</c> or, if there is no larger element, the bitwise complement of <c>Count</c>.</returns>

		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return BinarySearch(0, Count, item, comparer);
		}

		/// <summary>
		/// Removes all elements from the SetList.
		/// </summary>

		public void Clear()
		{
			if (size > 0)
			{
				// Clear the elements so that the GC can reclaim the references.
				Array.Clear(items, 0, size);
				size = 0;

				version++;
			}
		}

		/// <summary>
		/// Determines whether an element is in the SetList.
		/// </summary>
		/// <param name="item">The object to locate in the SetList. The value can be <c>null</c> for reference types.</param>
		/// <returns><c>true</c> if item is found in the SetList; otherwise, <c>false</c>.</returns>

		public bool Contains(T item)
		{
			if (item == null)
			{
				for (int i = 0; i < size; i++)
				{
					if (items[i] == null)
					{
						return true;
					}
				}
			}
			else
			{
				EqualityComparer<T> c = EqualityComparer<T>.Default;

				for (int i = 0; i < size; i++)
				{
					if (c.Equals(items[i], item))
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the IList contains a specific value.
		/// </summary>
		/// <param name="item">The <c>object</c> to locate in the IList.</param>
		/// <returns><c>true</c> if item is found in the IList; otherwise, <c>false</c>.</returns>

		bool IList.Contains(object item)
		{
			return IsCompatibleObject(item) && Contains((T)item);
		}

		/// <summary>
		/// Copies the entire SetList to a compatible one-dimensional array, starting at the beginning of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from the SetList. The Array must have zero-based indexing.</param>

		public void CopyTo(T[] array)
		{
			CopyTo(array, 0);
		}

		/// <summary>
		/// Copies the entire SetList to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from the SetList. The Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <c>array</c> at which copying begins.</param>
		/// <exception cref="ArgumentException"><c>array</c> is multi-dimensional.</exception>
		/// <exception cref="ArgumentException">The elements of the set list are not assignable to the element type of <c>array</c>.</exception>

		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if ((array != null) && (array.Rank != 1))
			{
				throw new ArgumentException("Multi-dimensional arrays are not supported.");
			}

			try
			{
				// Array.Copy will check for null.
				Array.Copy(items, 0, array, arrayIndex, size);
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException("Array type mismatch.");
			}
		}

		/// <summary>
		/// Copies a range of elements from the SetList to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="index">The zero-based index in the source SetList at which copying begins.</param>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from the SetList. The Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <c>array</c> at which copying begins.</param>
		/// <param name="count">The number of elements to copy.</param>
		/// <exception cref="ArgumentException">The range of elements to copy is fewer than <c>count</c>.</exception>

		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (size - index < count)
			{
				throw new ArgumentException("The range of elements to copy is fewer than the requested number to copy.");
			}

			// Delegate rest of error checking to Array.Copy.
			Array.Copy(items, index, array, arrayIndex, count);
		}

		/// <summary>
		/// Copies the entire SetList to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from the SetList. The Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <c>array</c> at which copying begins.</param>

		public void CopyTo(T[] array, int arrayIndex)
		{
			// Delegate rest of error checking to Array.Copy.
			Array.Copy(items, 0, array, arrayIndex, size);
		}

		/// <summary>
		/// Determines whether the SetList contains elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to search for.</param>
		/// <returns><c>true</c> if the SetList contains one or more elements that match the conditions defined by the specified predicate; otherwise, <c>false</c>.</returns>

		public bool Exists(Predicate<T> match)
		{
			return FindIndex(match) != -1;
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire SetList.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions of the element to search for.</param>
		/// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <c>T</c>.</returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is null.</exception>

		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			for (int i = 0; i < size; i++)
			{
				if (match(items[i]))
				{
					return items[i];
				}
			}

			return default;
		}

		/// <summary>
		/// Retrieves all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to search for.</param>
		/// <returns>A SetList containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty SetList.</returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is null.</exception>

		public SetList<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			SetList<T> list = new();

			for (int i = 0; i < size; i++)
			{
				if (match(items[i]))
				{
					list.Add(items[i]);
				}
			}

			return list;
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire SetList.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to search for.</param>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>, if found; otherwise, -1.</returns>

		public int FindIndex(Predicate<T> match)
		{
			return FindIndex(0, size, match);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the SetList that extends from the specified index to the last element.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to search for.</param>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>, if found; otherwise, -1.</returns>

		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return FindIndex(startIndex, size - startIndex, match);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the SetList that starts at the specified index and contains the specified number of elements.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to search for.</param>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>, if found; otherwise, -1.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>startIndex</c> is outside the range of valid indexes for the SetList.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>startIndex</c> and <c>count</c> do not specify a valid section in the SetList.</exception>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>

		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if ((uint)startIndex > (uint)size)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			if (startIndex > size - count)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			int endIndex = startIndex + count;

			for (int i = startIndex; i < endIndex; i++)
			{
				if (match(items[i]))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire SetList.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions of the element to search for.</param>
		/// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <c>T</c>.</returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>

		public T FindLast(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			for (int i = size - 1; i >= 0; i--)
			{
				if (match(items[i]))
				{
					return items[i];
				}
			}

			return default;
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire SetList.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions of the element to search for.</param>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <c>match</c>, if found; otherwise, -1.</returns>

		public int FindLastIndex(Predicate<T> match)
		{
			return FindLastIndex(size - 1, size, match);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the SetList that extends from the first element to the specified index.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="match">The Predicate delegate that defines the conditions of the element to search for.</param>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <c>match</c>, if found; otherwise, -1.</returns>

		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return FindLastIndex(startIndex, startIndex + 1, match);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the SetList that contains the specified number of elements and ends at the specified index.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The Predicate delegate that defines the conditions of the element to search for.</param>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <c>match</c>, if found; otherwise, -1.</returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>startIndex</c> is outside the range of valid indexes for the SetList.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>startIndex</c> and <c>count</c> do not specify a valid section in the SetList.</exception>

		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			if (size == 0)
			{
				// Special case for 0 length List
				if (startIndex != -1)
				{
					throw new ArgumentOutOfRangeException(nameof(startIndex));
				}
			}
			else
			{
				// Make sure we're not out of range
				if ((uint)startIndex >= (uint)size)
				{
					throw new ArgumentOutOfRangeException(nameof(startIndex));
				}
			}

			// 2nd half of this also catches when startIndex == MAXINT, so MAXINT - 0 + 1 == -1, which is < 0.
			if (count < 0 || startIndex - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			int endIndex = startIndex - count;

			for (int i = startIndex; i > endIndex; i--)
			{
				if (match(items[i]))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Performs the specified action on each element of the SetList.
		/// </summary>
		/// <param name="action">The Action delegate to perform on each element of the List.</param>
		/// <exception cref="ArgumentNullException"><c>action</c> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">An element in the collection has been modified.</exception>

		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			int forVersion = version;

			for (int i = 0; i < size; i++)
			{
				if (forVersion != version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}

				action(items[i]);
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the SetList.
		/// </summary>
		/// <returns>An Enumerator for the SetList.</returns>

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the SetList.
		/// </summary>
		/// <returns>An Enumerator for the SetList.</returns>

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the SetList.
		/// </summary>
		/// <returns>An Enumerator for the SetList.</returns>

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Creates a shallow copy of a range of elements in the source SetList.
		/// </summary>
		/// <param name="index">The zero-based index at which the range starts.</param>
		/// <param name="count">The number of elements in the range.</param>
		/// <returns>A shallow copy of a range of elements in the source SetList.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range of elements in the SetList.</exception>

		public SetList<T> GetRange(int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} cannot be less than zero.");
			}

			if (size - index < count)
			{
				throw new ArgumentException("Index and range do not denote a valid range.");
			}

			SetList<T> list = new(count);
			Array.Copy(items, index, list.items, 0, count);
			list.size = count;

			return list;
		}

		/// <summary>
		/// Searches the entire SetList for a specified object and returns the zero-based index at which it was found.
		/// </summary>
		/// <param name="item">The object to locate in the SetList. The value can be <c>null</c> for reference types.</param>
		/// <returns>The zero-based index at which <c>item</c> was found, if present; otherwise, -1.</returns>

		public int IndexOf(T item)
		{
			return Array.IndexOf(items, item, 0, size);
		}

		/// <summary>
		/// Determines the index of a specific item in the IList.
		/// </summary>
		/// <param name="item">The object to locate in the IList. The value can be <c>null</c> for reference types.</param>
		/// <returns>The index of <c>item</c> if found in the list; otherwise, -1.</returns>

		int IList.IndexOf(object item)
		{
			if (IsCompatibleObject(item))
			{
				return IndexOf((T)item);
			}

			return -1;
		}

		/// <summary>
		/// Searches for the specified object and returns the zero-based index at which it was found within the range of elements in the SetList that extends from the specified index to the last element.
		/// </summary>
		/// <param name="item">The object to locate in the SetList. The value can be <c>null</c> for reference types.</param>
		/// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
		/// <returns>The zero-based index at which <c>item</c> was found within the range of elements in the SetList that extends from <c>index</c> to the last element, if present; otherwise, -1.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is outside the range of valid indexes for the SetList.</exception>

		public int IndexOf(T item, int index)
		{
			if (index > size)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			return Array.IndexOf(items, item, index, size - index);
		}

		/// <summary>
		/// Searches for the specified object and returns the zero-based index at which it was found within the range of elements in the SetList that starts at the specified index and contains the specified number of elements.
		/// </summary>
		/// <param name="item">The object to locate in the SetList. The value can be <c>null</c> for reference types.</param>
		/// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>The zero-based index at which <c>item</c> was found within the range of elements in the SetList that starts at <c>index</c> and contains <c>count</c> number of elements, if present; otherwise, -1.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is outside the range of valid indexes for the SetList.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> and <c>count</c> do not denote a valid range of elements in the SetList.</exception>

		public int IndexOf(T item, int index, int count)
		{
			if (index > size)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (count < 0 || index > size - count)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			return Array.IndexOf(items, item, index, count);
		}

		/// <summary>
		/// Inserts an element into the SetList at the specified index if it is not already present anywhere in the SetList.
		/// </summary>
		/// <param name="index">The zero-based index at which <c>item</c> should be inserted.</param>
		/// <param name="item">The object to insert. The value can be <c>null</c> for reference types.</param>
		/// <returns><c>true</c> if the element is inserted into the SetList; <c>false</c> if the element is already present anywhere in the SetList.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is outside the range of valid indexes for the SetList.</exception>

		public bool Insert(int index, T item)
		{
			// Note that insertions at the end are legal.
			if ((uint)index > (uint)size)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (Contains(item))
			{
				return false;
			}

			if (size == items.Length)
			{
				EnsureCapacity(size + 1);
			}

			if (index < size)
			{
				Array.Copy(items, index, items, index + 1, size - index);
			}

			items[index] = item;

			size++;
			version++;

			return true;
		}

		/// <summary>
		/// Inserts an element into the IList at the specified index if it is not already present anywhere in the IList.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert. The value can be <c>null</c> for reference types.</param>

		void IList<T>.Insert(int index, T item)
		{
			Insert(index, item);
		}

		/// <summary>
		/// Inserts an element into the IList at the specified index if it is not already present anywhere in the IList.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert. The value can be <c>null</c> for reference types.</param>

		void IList.Insert(int index, object item)
		{
			if (item == null && default(T) != null)
			{
				throw new ArgumentNullException();
			}

			try
			{
				Insert(index, (T)item);
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException($"Item is not of type {typeof(T)}.");
			}
		}

		/// <summary>
		/// Inserts the elements of the specified collection into the SetList at the specified index, if they are not already present anywhere in the SetList.
		/// </summary>
		/// <param name="index">The zero-based index at which the new elements should be inserted.</param>
		/// <param name="collection">The collection whose elements should be inserted into the SetList. The collection itself cannot be <c>null</c>, but it can contain elements that are <c>null</c>, if type <c>T</c> is a reference type.</param>
		/// <returns>The number of elements that were inserted into the SetList.</returns>
		/// <exception cref="ArgumentNullException"><c>collection</c> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is outside the range of valid indexes for the SetList.</exception>

		public int InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			if ((uint)index > (uint)size)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			int result = 0;

			using IEnumerator<T> en = collection.GetEnumerator();

			while (en.MoveNext())
			{
				if (Insert(index, en.Current))
				{
					result++;
					index++;
				}
			}

			if (result > 0)
			{
				version++;
			}

			return result;
		}

		/// <summary>
		/// Removes an object from the SetList if it is present.
		/// </summary>
		/// <param name="item">The object to remove from the SetList. The value can be <c>null</c> for reference types.</param>
		/// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if <c>item</c> was not found in the SetList.</returns>

		public bool Remove(T item)
		{
			int index = IndexOf(item);

			if (index >= 0)
			{
				RemoveAt(index);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Removes an object from the IList if it is present.
		/// </summary>
		/// <param name="item">The object to remove from the IList. The value can be <c>null</c> for reference types.</param>
		/// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if <c>item</c> was not found in the IList.</returns>

		void IList.Remove(object item)
		{
			if (IsCompatibleObject(item))
			{
				Remove((T)item);
			}
		}

		/// <summary>
		/// Removes all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to remove.</param>
		/// <returns>The number of elements removed from the SetList.</returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>

		public int RemoveAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			int freeIndex = 0; // the first free slot in items array

			// Find the first item which needs to be removed.
			while (freeIndex < size && !match(items[freeIndex]))
			{
				freeIndex++;
			}

			if (freeIndex >= size)
			{
				return 0;
			}

			int current = freeIndex + 1;

			while (current < size)
			{
				// Find the first item which needs to be kept.
				while (current < size && match(items[current]))
				{
					current++;
				}

				if (current < size)
				{
					// copy item to the free slot.
					items[freeIndex++] = items[current++];
				}
			}

			int result = size - freeIndex;

			if (result > 0)
			{
				Array.Clear(items, freeIndex, result);

				size = freeIndex;
				version++;
			}

			return result;
		}

		/// <summary>
		/// Removes the element at the specified index of the SetList.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is outside the range of valid indexes for the SetList.</exception>

		public void RemoveAt(int index)
		{
			if ((uint)index >= (uint)size)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			size--;

			if (index < size)
			{
				Array.Copy(items, index + 1, items, index, size - index);
			}

			items[size] = default;

			version++;
		}

		/// <summary>
		/// Removes a range of elements from the SetList.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range in the SetList.</exception>

		public void RemoveRange(int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero.");
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} cannot be less than zero.");
			}

			if (size - index < count)
			{
				throw new ArgumentException("Index and range do not denote a valid range.");
			}

			if (count > 0)
			{
				size -= count;

				if (index < size)
				{
					Array.Copy(items, index + count, items, index, size - index);
				}

				Array.Clear(items, size, count);

				version++;
			}
		}

		/// <summary>
		/// Reverses the order of the elements in the entire SetList.
		/// </summary>

		public void Reverse()
		{
			Reverse(0, Count);
		}

		/// <summary>
		/// Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to reverse.</param>
		/// <param name="count">The number of elements in the range to reverse.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range in the SetList.</exception>

		public void Reverse(int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero.");
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} cannot be less than zero.");
			}

			if (size - index < count)
			{
				throw new ArgumentException("Index and range do not denote a valid range.");
			}

			if (count == 0)
			{
				return;
			}

			Array.Reverse(items, index, count);

			version++;
		}

		/// <summary>
		/// Sorts the elements in the entire SetList using the default comparer.
		/// </summary>

		public void Sort()
		{
			Sort(0, Count, null);
		}

		/// <summary>
		/// Sorts the elements in the entire SetList using the specified comparer.
		/// </summary>
		/// <param name="comparer"></param>

		public void Sort(IComparer<T> comparer)
		{
			Sort(0, Count, comparer);
		}

		/// <summary>
		/// Sorts the elements in a range of elements in the SetList using the specified comparer.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to sort.</param>
		/// <param name="count">The length of the range to sort.</param>
		/// <param name="comparer">The IComparer implementation to use when comparing elements, or <c>null</c> to use the default comparer <c>Default</c>.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range in the SetList.</exception>

		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero.");
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} cannot be less than zero.");
			}

			if (size - index < count)
			{
				throw new ArgumentException("Index and range do not denote a valid range.");
			}

			if (count == 0)
			{
				return;
			}

			Array.Sort(items, index, count, comparer);

			version++;
		}

		/// <summary>
		/// Sorts the elements in the entire SetList using the specified Comparison.
		/// </summary>
		/// <param name="comparison">The Comparison to use when comparing elements.</param>
		/// <exception cref="ArgumentNullException"><c>comparison</c> is <c>null</c>.</exception>

		public void Sort(Comparison<T> comparison)
		{
			if (comparison == null)
			{
				throw new ArgumentNullException(nameof(comparison));
			}

			if (size > 0)
			{
				Array.Sort(items, comparison);
			}
		}

		/// <summary>
		/// Copies the elements of the SetList to a new array.
		/// </summary>
		/// <returns>An array containing copies of the elements of the SetList.</returns>

		public T[] ToArray()
		{
			T[] array = new T[size];

			Array.Copy(items, 0, array, 0, size);

			return array;
		}

		/// <summary>
		/// Sets the capacity to the actual number of elements in the SetList, if that number is less than a threshold value.
		/// </summary>

		public void TrimExcess()
		{
			int threshold = (int)(items.Length * 0.9);

			if (size < threshold)
			{
				Capacity = size;
			}
		}

		/// <summary>
		/// Determines whether every element in the SetList matches the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The Predicate delegate that defines the conditions to check against the elements.</param>
		/// <returns><c>true</c> if every element in the SetList matches the conditions defined by the specified predicate; otherwise, <c>false</c>. If the list has no elements, the return value is <c>true</c>.</returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>

		public bool TrueForAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}

			for (int i = 0; i < size; i++)
			{
				if (!match(items[i]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Ensures that the capacity of this set list is at least the given minimum value. If the current capacity of
		/// the list is less than min, the capacity is increased to twice the current capacity or to min, whichever is
		/// larger.
		/// </summary>
		/// <param name="min">The desired minimum capacity of the SetList.</param>

		private void EnsureCapacity(int min)
		{
			if (items.Length < min)
			{
				int newCapacity = items.Length == 0 ? DefaultCapacity : items.Length * 2;

				// Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
				// Note that this check works even when _items.Length overflowed thanks to the (uint) cast

				if ((uint)newCapacity > MaxCapacity)
				{
					newCapacity = MaxCapacity;
				}

				if (newCapacity < min)
				{
					newCapacity = min;
				}

				Capacity = newCapacity;
			}
		}

		#endregion

		#region Static

		private static readonly T[] emptyArray = Array.Empty<T>();

		private static bool IsCompatibleObject(object value)
		{
			// Non-null values are fine. Only accept nulls if T is a class or Nullable<U>.
			// Note that default(T) is not equal to null for value types except when T is Nullable<U>.
			return ((value is T) || (value == null && default(T) == null));
		}

		internal static IList<T> Synchronized(SetList<T> list)
		{
			return new SynchronizedSetList(list);
		}

		#endregion

		[Serializable]
		internal class SynchronizedSetList : IList<T>
		{
			private SetList<T> list;

			private object root;

			internal SynchronizedSetList(SetList<T> list)
			{
				this.list = list;
				root = ((ICollection)list).SyncRoot;
			}

			public int Count
			{
				get
				{
					lock (root)
					{
						return list.Count;
					}
				}
			}

			public bool IsReadOnly
			{
				get { return ((ICollection<T>)list).IsReadOnly; }
			}

			public T this[int index]
			{
				get
				{
					lock (root)
					{
						return list[index];
					}
				}
				set
				{
					lock (root)
					{
						list[index] = value;
					}
				}
			}

			public void Add(T item)
			{
				lock (root)
				{
					list.Add(item);
				}
			}

			public void Clear()
			{
				lock (root)
				{
					list.Clear();
				}
			}

			public bool Contains(T item)
			{
				lock (root)
				{
					return list.Contains(item);
				}
			}

			public void CopyTo(T[] array, int arrayIndex)
			{
				lock (root)
				{
					list.CopyTo(array, arrayIndex);
				}
			}

			public bool Remove(T item)
			{
				lock (root)
				{
					return list.Remove(item);
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				lock (root)
				{
					return list.GetEnumerator();
				}
			}

			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				lock (root)
				{
					return ((IEnumerable<T>)list).GetEnumerator();
				}
			}

			public int IndexOf(T item)
			{
				lock (root)
				{
					return list.IndexOf(item);
				}
			}

			public void Insert(int index, T item)
			{
				lock (root)
				{
					list.Insert(index, item);
				}
			}

			public void RemoveAt(int index)
			{
				lock (root)
				{
					list.RemoveAt(index);
				}
			}
		}

		[Serializable]
		public struct Enumerator : IEnumerator<T>
		{
			private SetList<T> list;
			private int index;
			private int version;
			private T current;

			internal Enumerator(SetList<T> list)
			{
				this.list = list;
				index = 0;
				version = list.version;
				current = default;
			}

			public T Current
			{
				get { return current; }
			}

			object IEnumerator.Current
			{
				get
				{
					if (index == 0 || index == list.size + 1)
					{
						throw new InvalidOperationException("Enumeration operation failed.");
					}

					return Current;
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				SetList<T> localList = list;

				if (version == localList.version && ((uint)index < (uint)localList.size))
				{
					current = localList.items[index];
					index++;

					return true;
				}

				return MoveNextRare();
			}

			private bool MoveNextRare()
			{
				if (version != list.version)
				{
					throw new InvalidOperationException("Enumeration failed version check.");
				}

				index = list.size + 1;
				current = default;

				return false;
			}

			void IEnumerator.Reset()
			{
				if (version != list.version)
				{
					throw new InvalidOperationException("Enumeration failed version check.");
				}

				index = 0;
				current = default;
			}
		}
	}
}