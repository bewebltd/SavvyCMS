package {

	/*
	 * Provides Form creation of fields including validation, along with a few String related functions.
	 * Requires Validate Class
	 * Components Required: Button, CheckBox, ComboBox, RadioButton, TextArea, TextInput
	 * @author Jonathan Brake
	 */


	public class FlashForm {
		
		private var _elementList:Object = new Object();
		private var _action:String = "";
		private var _method:String = "";
		private var _fontSettings:Object;
		
		
		public function FlashForm():void {
			addEventListener(Event.ADDED_TO_STAGE,Init);
			// collect the input areas
			_elementList.inputFields = new Array();
			_elementList.comboBoxes = new Array();
			_elementList.radioButtons = new Array();
			_elementList.textAreas = new Array();
			_elementList.checkBoxes = new Array();
		}
		
		private function Init(e:Event):void {
			trace("Init: "+ this);
			removeEventListener(Event.ADDED_TO_STAGE,Init);
			addEventListener(Event.REMOVED_FROM_STAGE, Removed);
		}
		
		public function AddInput(){
			trace("AddInput")
			
		}
		
		public function AddComboBox(){
			trace("AddInput")
		}
		
		public function AddRadio(){
			trace("AddInput")
			
		}
		public function AddTextArea(){
			trace("AddInput")
			
		}
		
		public function AddCheckBox(){
			trace("AddInput")
		}
		
		public function AddSubmit(){
			trace("AddInput")
			
		}
		
		public function AddButton(){
			trace("AddInput")
		}
		
		public function AddLabel(){}
		/* */
		private function Validate():Boolean(){
			
			
		}
		
		/* End */
		private function Removed(e:Event):void {
			trace(this + ".Removed: "+e)
			removeEventListener(Event.REMOVED_FROM_STAGE, Removed);
		}
	}
}