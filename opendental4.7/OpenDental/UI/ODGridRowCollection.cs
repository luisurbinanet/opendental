using System;
using System.Collections;

namespace OpenDental.UI{ 
	
	///<summary>A strongly typed collection of ODGridColumns</summary>
	public class ODGridRowCollection:CollectionBase{

		///<summary>Returns the GridRow with the given index.</summary>
		public ODGridRow this[int index]{
      get{
				return (ODGridRow)List[index];
      }
      set{
				List[index]=value;
      }
		}

		///<summary></summary>
		public int Add(ODGridRow value){
			return(List.Add(value));
		}

		///<summary></summary>
		public int IndexOf(ODGridRow value){
      return(List.IndexOf(value));
		}

		///<summary></summary>
		public void Insert(int index,ODGridRow value){
      List.Insert(index,value);
		}

		///<summary></summary>
		public void Remove(ODGridRow value){
      List.Remove(value);
		}

		///<summary></summary>
		public bool Contains(ODGridRow value){
      //If value is not of type ODGridRow, this will return false.
      return(List.Contains(value));
		}

		///<summary></summary>
		protected override void OnInsert(int index,Object value){
      if(value.GetType()!=typeof(ODGridRow))
				throw new ArgumentException("value must be of type ODGridRow.","value");
		}

		///<summary></summary>
		protected override void OnRemove( int index,Object value ){
      if(value.GetType()!=typeof(ODGridRow))
        throw new ArgumentException("value must be of type ODGridRow.","value");
		}

		///<summary></summary>
		protected override void OnSet(int index,Object oldValue,Object newValue){
      if(newValue.GetType()!=typeof(ODGridRow))
        throw new ArgumentException("newValue must be of type ODGridRow.","newValue");
		}

		///<summary></summary>
		protected override void OnValidate(Object value){
      if(value.GetType()!=typeof(ODGridRow))
        throw new ArgumentException("value must be of type ODGridRow.");
		}







/*
		///<summary></summary>
		public void Remove(int index){
			if((index>Count-1) || (index<0)){
				throw new System.IndexOutOfRangeException();
			}
			else{
				List.RemoveAt(index);
			}
		}*/

		/*
		///<summary>The button is retrieved from List and explicitly cast to the button type.</summary>
		public ODToolBarButton Item(int index){
			return (ODToolBarButton)List[index];
		}*/

		/*
		///<summary></summary>
		public int IndexOf(ODGridColumn value){
			return(List.IndexOf(value));
		}*/

	}
}





















