namespace SavvyMVC.Helpers {
	/*   
	 * Class for storing model based error messages. 
	 * */
	public class ValidationError {
		//put whatever other fields that are useful in here
		public string ErrorMessage;
		public string Record;

		public ValidationError(string errorMessage) {
			this.ErrorMessage = errorMessage;
		}
		//overloaded second constructor is really just for backwards compatibility
		public ValidationError(string record,string errorMessage) {
			this.ErrorMessage = errorMessage;
			this.Record = record;
		}
	}
}
