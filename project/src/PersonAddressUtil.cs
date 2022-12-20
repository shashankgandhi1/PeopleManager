using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace myapp{
	public class PersonAddressUtil{

		public static void InvokeAddPerson(string[] inputs){
			var true_personFields = promptsToDict(inputs);

			if (true_personFields.Item1){

				// Check for required fields
				string[] reqFields = {"firstname", "lastname", "dob"};
				string[] allowedFields = {"firstname", "lastname", "dob", "nickname"};

				bool isReqPresent = checkReqFields(reqFields, true_personFields.Item2);
				bool isAllowed = checkAllowedFields(allowedFields, true_personFields.Item2);

				if (isReqPresent && isAllowed){
					AddPerson(true_personFields.Item2);
				}	
			}
		}

		public static void AddPerson(Hashtable personFields){
			Tuple<bool, Hashtable> true_outPersonFields = filterValuesPerson(personFields);
			Hashtable outPersonFields = true_outPersonFields.Item2;
			
			if (true_outPersonFields.Item1){

				bool isValid = checkValidValue(outPersonFields);

				if (isValid){
					Person localPerson = new Person{
						id = Program.metaData.personID + 1,
						firstname = (string)(outPersonFields["firstname"]),
						lastname = (string)(outPersonFields["lastname"]),
						dob = (DateTime)(outPersonFields["dob"]),
						nickname = (string)(outPersonFields["nickname"]),
						addresses = new Addresses{ items = new List<Address>{}}
					};

					// Implement a check for duplicacy
					bool isDuplicate = checkDuplicatePerson(localPerson, Program.peopleData);

					if (!isDuplicate){
						Program.metaData.personID += 1;
						Program.peopleData.items.Add(localPerson);
						XmlUtil.SerializeListToXmlFile(Program.peopleData, Constants.datafilepath);
						XmlUtil.SerializeMetaToXmlFile(Program.metaData, Constants.configfilepath);

						Console.WriteLine("Added person successfully");
					}
				}
			}
		}

		public static void InvokeEditPerson(string[] inputs){
			var true_personFields = promptsToDict(inputs);

			if (true_personFields.Item1){

				// Check for required fields
				string[] reqFields = {"id"};
				string[] allowedFields = {"id", "firstname", "lastname", "dob", "nickname"};

				bool isReqPresent = checkReqFields(reqFields, true_personFields.Item2);
				bool isAllowed = checkAllowedFields(allowedFields, true_personFields.Item2);

				if (isReqPresent && isAllowed && inputs.Length > 4){
					EditPerson(true_personFields.Item2);
				}	
				else {
					Console.WriteLine("Please provide arguments to edit");
				}
			}
		}

		public static void EditPerson(Hashtable personFields){
			
			Tuple<bool, Hashtable> true_outPersonFields = filterValuesPerson(personFields);
			Hashtable outPersonFields = true_outPersonFields.Item2;

			if (true_outPersonFields.Item1){

				bool isValid = checkValidValue(outPersonFields);

				if (isValid){
					int idx = FindPersonOnID((long)Convert.ToDecimal(outPersonFields["id"]), Program.peopleData);
					if (idx == -1){
						Console.WriteLine(String.Format("Person not found for id '{0}'", outPersonFields["id"]));
					}
					else{
						Person foundPerson = Program.peopleData.items[idx];

						Person localPerson = new Person{
							id = (long)Convert.ToDecimal(outPersonFields["id"]),
							firstname = (outPersonFields.ContainsKey("firstname")) ? (string)(outPersonFields["firstname"]) : foundPerson.firstname,
							lastname = (outPersonFields.ContainsKey("lastname")) ? (string)(outPersonFields["lastname"]) : foundPerson.lastname,
							dob = (outPersonFields.ContainsKey("dob")) ? (DateTime)(outPersonFields["dob"]) : foundPerson.dob,
							nickname = (outPersonFields.ContainsKey("nickname")) ? (string)(outPersonFields["nickname"]) : foundPerson.nickname,
							addresses = foundPerson.addresses
						};

						// Implement a check for duplicacy
						bool isDuplicate = checkDuplicatePerson(localPerson, Program.peopleData);

						if (!isDuplicate){
							Program.peopleData.items[idx] = localPerson;
							XmlUtil.SerializeListToXmlFile(Program.peopleData, Constants.datafilepath);
							XmlUtil.SerializeMetaToXmlFile(Program.metaData, Constants.configfilepath);

							Console.WriteLine("Edited person successfully");
						}
					}
				}
			}
		}

		public static void InvokeDeletePerson(string[] inputs){
			var true_personFields = promptsToDict(inputs);

			if (true_personFields.Item1){

				// Check for required fields
				string[] reqFields = {"id"};
				string[] allowedFields = {"id"};

				bool isReqPresent = checkReqFields(reqFields, true_personFields.Item2);
				bool isAllowed = checkAllowedFields(allowedFields, true_personFields.Item2);

				if (isReqPresent && isAllowed){
					DeletePerson(true_personFields.Item2);
				}	
			}
		}


		public static void DeletePerson(Hashtable personFields){
			Tuple<bool, Hashtable> true_outPersonFields = filterValuesPerson(personFields);
			Hashtable outPersonFields = true_outPersonFields.Item2;

			if (true_outPersonFields.Item1){

				bool isValid = checkValidValue(outPersonFields);

				if (isValid){
					int idx = FindPersonOnID((long)Convert.ToDecimal(outPersonFields["id"]), Program.peopleData);
					if (idx == -1){
						Console.WriteLine(String.Format("Person not found for id '{0}'", outPersonFields["id"]));
					}
					else{
						Program.peopleData.items.RemoveAt(idx);
						XmlUtil.SerializeListToXmlFile(Program.peopleData, Constants.datafilepath);
						XmlUtil.SerializeMetaToXmlFile(Program.metaData, Constants.configfilepath);

						Console.WriteLine("Deleted person successfully");
					}
				}
			}
		}

		public static void InvokeSearchPerson(string[] inputs){
			if (inputs.Length != 3){
				Console.WriteLine("Please provide a single value in quoted strings to search");
			}
			else {
				Hashtable personFields = new Hashtable();
				personFields["searchParam"] = inputs[2];

				string[] reqFields = {"searchParam"};
				string[] allowedFields = {"searchParam"};

				bool isReqPresent = checkReqFields(reqFields, personFields);
				bool isAllowed = checkAllowedFields(allowedFields, personFields);

				if (isReqPresent && isAllowed){
					SearchPerson(personFields);
				}	
			}
		}

		public static void SearchPerson(Hashtable personFields){
			Tuple<bool, Hashtable> true_outPersonFields = filterValuesPerson(personFields);
			Hashtable outPersonFields = true_outPersonFields.Item2;

			if (true_outPersonFields.Item1){

				bool isValid = checkValidValue(outPersonFields);

				if (isValid){
					bool isFound = false;
					foreach (Person person in Program.peopleData.items){
						if (person.firstname == (string)outPersonFields["searchParam"] || person.lastname == (string)outPersonFields["searchParam"]){
							PrintPersonShort(person);
							isFound = true;
						}
					}
					if (!isFound){
						Console.WriteLine(String.Format("No person with first name or last name as '{0}' found", outPersonFields["searchParam"]));
					}
				}
			}
		}
		public static void InvokeViewAllPerson(){
			var stringBuilder = new StringBuilder();
			foreach(Person person in Program.peopleData.items){
				stringBuilder.AppendFormat("Person => ID : {0,-4}, firstname : {1,-15}, lastname : {2,-12}, dob : {3,-15}, nickname : {4, -15}\n", person.id, person.firstname, person.lastname, person.dob.ToString("dd/MM/yyyy"), person.nickname);			}
			Console.Write(stringBuilder.ToString());
		}

		public static void InvokeViewPerson(string[] inputs){
			var true_personFields = promptsToDict(inputs);

			if (true_personFields.Item1){

				// Check for required fields
				string[] reqFields = {"id"};
				string[] allowedFields = {"id"};

				bool isReqPresent = checkReqFields(reqFields, true_personFields.Item2);
				bool isAllowed = checkAllowedFields(allowedFields, true_personFields.Item2);

				if (isReqPresent && isAllowed){
					ViewPerson(true_personFields.Item2);
				}	
			}
		}

		public static void ViewPerson(Hashtable personFields){
			Tuple<bool, Hashtable> true_outPersonFields = filterValuesPerson(personFields);
			Hashtable outPersonFields = true_outPersonFields.Item2;

			if (true_outPersonFields.Item1){

				bool isValid = checkValidValue(outPersonFields);

				if (isValid){
					int idx = FindPersonOnID((long)Convert.ToDecimal(outPersonFields["id"]), Program.peopleData);
					if (idx == -1){
						Console.WriteLine(String.Format("Person not found for id '{0}'", outPersonFields["id"]));
					}
					else{
						PrintPersonDetail(Program.peopleData.items[idx]);
					}
				}
			}
		}

		public static void InvokeAddAddress(string[] inputs){
			var true_addressFields = promptsToDict(inputs);

			if (true_addressFields.Item1){

				// Check for required fields
				string[] reqFields = {"personId", "line1"};
				string[] allowedFields = {"personId", "line1", "line2", "country", "postcode"};

				bool isReqPresent = checkReqFields(reqFields, true_addressFields.Item2);
				bool isAllowed = checkAllowedFields(allowedFields, true_addressFields.Item2);

				if (isReqPresent && isAllowed){
					AddAddress(true_addressFields.Item2);
				}	
			}
		}

		public static void AddAddress(Hashtable addressFields){
			Tuple<bool, Hashtable> true_outAddressFields = filterValuesAddress(addressFields);
			Hashtable outAddressFields = true_outAddressFields.Item2;
			
			if (true_outAddressFields.Item1){

				bool isValid = checkValidValue(outAddressFields);

				if (isValid){
					Address localAddress = new Address{
						id = Program.metaData.addressID + 1,
						line1 = (string)(outAddressFields["line1"]),
						line2 = (string)(outAddressFields["line2"]),
						country = (string)(outAddressFields["country"]),
						postcode = (string)(outAddressFields["postcode"])
					};

					// // Implement a check for duplicacy
					int personIdx = FindPersonOnID((long)(Convert.ToDecimal(outAddressFields["personId"])), Program.peopleData);

					if (personIdx == -1){
						Console.WriteLine(String.Format("Person not found for id '{0}'", outAddressFields["personId"]));
					}
					else{
						Person person = Program.peopleData.items[personIdx];
						bool isDuplicate = checkDuplicateAddress(localAddress, person);

						if (!isDuplicate){
							Program.metaData.addressID += 1;
							person.addresses.items.Add(localAddress);
							XmlUtil.SerializeListToXmlFile(Program.peopleData, Constants.datafilepath);
							XmlUtil.SerializeMetaToXmlFile(Program.metaData, Constants.configfilepath);

							Console.WriteLine("Added address successfully");
						}						
					}
				}
			}	
		}

		public static void InvokeEditAddress(string[] inputs){
			var true_addressFields = promptsToDict(inputs);

			if (true_addressFields.Item1){

				// Check for required fields
				string[] reqFields = {"id"};
				string[] allowedFields = {"id", "line1", "line2", "country", "postcode"};

				bool isReqPresent = checkReqFields(reqFields, true_addressFields.Item2);
				bool isAllowed = checkAllowedFields(allowedFields, true_addressFields.Item2);

				if (isReqPresent && isAllowed && inputs.Length > 4){
					EditAddress(true_addressFields.Item2);
				}	
				else {
					Console.WriteLine("Please provide arguments to edit");
				}
			}
		}

		public static void EditAddress(Hashtable addressFields){
			Tuple<bool, Hashtable> true_outAddressFields = filterValuesAddress(addressFields);
			Hashtable outAddressFields = true_outAddressFields.Item2;
			
			if (true_outAddressFields.Item1){

				bool isValid = checkValidValue(outAddressFields);

				if (isValid){

					var idx_person_address = FindPersonOnAddressId((long)Convert.ToDecimal(outAddressFields["id"]), Program.peopleData);

					if (idx_person_address.Item2 == -1){
						Console.WriteLine(String.Format("Address ID : {0} not found", (string)outAddressFields["id"]));
					}
					else{
						Address foundAddress = Program.peopleData.items[idx_person_address.Item1].addresses.items[idx_person_address.Item2];

						Address localAddress = new Address{
							id = (long)Convert.ToDecimal(outAddressFields["id"]),
							line1 = (outAddressFields.ContainsKey("line1")) ? (string)(outAddressFields["line1"]) : foundAddress.line1,
							line2 = (outAddressFields.ContainsKey("line2")) ? (string)(outAddressFields["line2"]) : foundAddress.line2,
							country = (outAddressFields.ContainsKey("country")) ? (string)(outAddressFields["country"]) : foundAddress.country,
							postcode = (outAddressFields.ContainsKey("postcode")) ? (string)(outAddressFields["postcode"]) : foundAddress.postcode
						};

						bool isDuplicate = checkDuplicateAddress(localAddress, Program.peopleData.items[idx_person_address.Item1]);
						if (!isDuplicate){
							Program.peopleData.items[idx_person_address.Item1].addresses.items[idx_person_address.Item2] = localAddress;

							XmlUtil.SerializeListToXmlFile(Program.peopleData, Constants.datafilepath);
							XmlUtil.SerializeMetaToXmlFile(Program.metaData, Constants.configfilepath);

							Console.WriteLine("Edited address successfully");
						}	
					}
				}
			}
		}

		public static void InvokeDeleteAddress(string[] inputs){
			var true_addressFields = promptsToDict(inputs);

			if (true_addressFields.Item1){

				// Check for required fields
				string[] reqFields = {"id"};
				string[] allowedFields = {"id"};

				bool isReqPresent = checkReqFields(reqFields, true_addressFields.Item2);
				bool isAllowed = checkAllowedFields(allowedFields, true_addressFields.Item2);

				if (isReqPresent && isAllowed){
					DeleteAddress(true_addressFields.Item2);
				}	
			}
		}

		public static void DeleteAddress(Hashtable addressFields){
			Tuple<bool, Hashtable> true_outAddressFields = filterValuesAddress(addressFields);
			Hashtable outAddressFields = true_outAddressFields.Item2;
			
			if (true_outAddressFields.Item1){

				bool isValid = checkValidValue(outAddressFields);

				if (isValid){

					var idx_person_address = FindPersonOnAddressId((long)Convert.ToDecimal(outAddressFields["id"]), Program.peopleData);

					if (idx_person_address.Item2 == -1){
						Console.WriteLine(String.Format("Address ID : {0} not found", (string)outAddressFields["id"]));
					}
					else{
						Program.peopleData.items[idx_person_address.Item1].addresses.items.RemoveAt(idx_person_address.Item2);

						XmlUtil.SerializeListToXmlFile(Program.peopleData, Constants.datafilepath);
						XmlUtil.SerializeMetaToXmlFile(Program.metaData, Constants.configfilepath);

						Console.WriteLine("Deleted address successfully");
					}
				}
			}
		}

		public static int FindAddressOnId(long id, People people){
			var idx_person_address = FindPersonOnAddressId(id, people);
			return idx_person_address.Item2;
		}

		public static bool checkDuplicateAddress(Address address, Person person){
			bool isDuplicate = false;
			foreach(Address addr in person.addresses.items){
				if ((addr.line1 == address.line1 && addr.line2 == address.line2) && (addr.country == address.country && addr.postcode == address.postcode)){
					Console.WriteLine(String.Format("Address with the same arguments already exists for PersonID : {0}", person.id));
					isDuplicate = true;
					break;
				}
			}
			return isDuplicate;
		}

		public static void PrintPersonDetail(Person person){
			Console.WriteLine("Person found =>");
			Console.WriteLine(String.Format("\tpersonID : {0}", person.id));
			Console.WriteLine(String.Format("\tfirstname : {0}", person.firstname));
			Console.WriteLine(String.Format("\tlastname : {0}", person.lastname));
			Console.WriteLine(String.Format("\tdob : {0}", person.dob));
			Console.WriteLine(String.Format("\tnickname : {0}", person.nickname));
			Console.WriteLine("\taddresses =>");
			PrintAddressDetail(person);
		}

		public static void PrintAddressDetail(Person person){
			foreach(Address address in person.addresses.items){
				Console.WriteLine(String.Format("\t\taddressID : {0}", address.id));
				Console.WriteLine(String.Format("\t\tline1 : {0}", address.line1));
				Console.WriteLine(String.Format("\t\tline2 : {0}", address.line2));
				Console.WriteLine(String.Format("\t\tcountry : {0}", address.country));
				Console.WriteLine(String.Format("\t\tpostcode : {0}", address.postcode));
				Console.WriteLine();
			}
		}

		public static void PrintPersonShort(Person person){
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Person found => ID : {0,-4}, firstname : {1,-15}, lastname : {2,-12}, dob : {3,-15}, nickname : {4, -15}\n", person.id, person.firstname, person.lastname, person.dob.ToString("dd/MM/yyyy"), person.nickname);
			Console.Write(stringBuilder.ToString());
		}

		public static Tuple<int,int> FindPersonOnAddressId(long id, People people){
			for (int idx = 0 ; idx < people.items.Count ; idx++){
				Person person = people.items[idx];
				for (int addressIdx = 0 ; addressIdx < person.addresses.items.Count ; addressIdx++){
					Address address = person.addresses.items[addressIdx];
					if (address.id == id){
						return new Tuple<int, int> (idx, addressIdx);
					}
				}
			}
			return new Tuple<int, int> (-1, -1);
		}

		public static int FindPersonOnID(long id, People people){

			for (int idx = 0 ; idx < people.items.Count; idx++){
				Person person = people.items[idx];
				if (person.id == id){
					return idx;
				}	
			}
			return -1;
		}

		public static Tuple<bool, Hashtable> promptsToDict(string[] inputs){
			bool isSuccess = true;
			Hashtable personFields = new Hashtable();

			for (int idx = 2; idx < inputs.Length; idx += 2){
				string field = inputs[idx];

				if (field[0] != '-'){
					Console.WriteLine(String.Format("Missing argument for value '{0}'", field));
					isSuccess = false;
					break;
				}
				else{
					field = field.Substring(1, field.Length-1);
				}

				if ((idx + 1) >= inputs.Length){
					Console.WriteLine(String.Format("Missing value for argument '-{0}'", field));
					isSuccess = false;
					break;
				}
				else{
					string fieldValue = inputs[idx + 1];
					if (personFields.ContainsKey(field)){
						isSuccess = false;
						Console.WriteLine(String.Format("Duplicate argument '-{0}' provided. Please provide argument once", field));
						break;
					}
					else{
						personFields.Add(field, fieldValue);	
					}
					
				}
			}

			return new Tuple<bool, Hashtable> (isSuccess, personFields);
		}

		public static bool checkReqFields(string[] reqFields, Hashtable dict){
			bool isReqPresent = true;

			foreach (string field in reqFields){
				if (!dict.ContainsKey(field)){
					Console.WriteLine(String.Format("'-{0}' is a required argument", field));
					Constants.PrintHelpRequiredArguments(reqFields);
					isReqPresent = false;
					break;
				}
			}

			return isReqPresent;
		}

		public static bool checkAllowedFields(string[] allowedFields, Hashtable dict){
			bool isAllowed = true;

			foreach (DictionaryEntry de in dict){
				string localKey = (string) de.Key;
				if (!allowedFields.Any(str => localKey.Contains(str))){
					Console.WriteLine(String.Format("'-{0}' is not an allowed argument here", localKey));
					Constants.PrintHelpAllowedArguments(allowedFields);
					isAllowed = false;
					break;
				}
			}
			return isAllowed;
		}

		public static bool checkDuplicatePerson(Person person, People people){
			bool isDuplicate = false;

			foreach (Person prs in people.items){
				if (prs.firstname == person.firstname && prs.lastname == person.lastname && prs.id != person.id){
					isDuplicate = true;
					Console.WriteLine(String.Format("Person with firstname '{0}' and lastname '{1}' already exists.", person.firstname, person.lastname));
					break;
				}
			}

			return isDuplicate;
		}

		public static Tuple<bool, Hashtable> filterValuesPerson(Hashtable personFields){
			bool isSuccess = true;
			Hashtable outPersonFields = new Hashtable();

			string nameValueFiltered;
			DateTime dobValueFiltered;
			long idValue;

			foreach (DictionaryEntry de in personFields){
				string localKey = (string)de.Key;
				string localValue = (string)de.Value;

				if ((localKey == "firstname" || localKey == "lastname") || localKey == "nickname" || localKey == "searchParam"){
					
					if (localValue[0] == '\"' && localValue[localValue.Length-1] == '\"'){
						nameValueFiltered = localValue.Substring(1, localValue.Length-2);
					}
					else{
						Console.WriteLine(String.Format("Value for argument '-{0}' should be enclosed in double quotes", localKey));
						isSuccess = false;
						break;
					}
					outPersonFields[localKey] = nameValueFiltered;
				}
				else if (localKey == "dob"){
					try{
						dobValueFiltered = DateTime.Parse(localValue);	
						outPersonFields[localKey] = dobValueFiltered;
					}
					catch (Exception e){
						Console.WriteLine(String.Format("Cannot convert '{0}' to a valid date. Try date format dd/MM/yyyy", localValue));
						isSuccess = false;
						break;
					}

				}
				else if (localKey == "id"){
					try{
						idValue = (long)Convert.ToDouble(localValue);
						outPersonFields[localKey] = localValue;
					}
					catch (Exception e){
						Console.WriteLine(String.Format("ID '{0}'not found. Try a numeric ID", localValue));
					}
				}
				else {
					Console.WriteLine(String.Format("'-{0}' is not a recognized argument for a person", localKey));
					Constants.PrintHelpArgumentsPerson();
					isSuccess = false;
					break;
				}	
			}

			return new Tuple<bool, Hashtable> (isSuccess, outPersonFields);
		}


		public static Tuple<bool, Hashtable> filterValuesAddress(Hashtable addressFields){
			bool isSuccess = true;
			Hashtable outAddressFields = new Hashtable();

			string nameValueFiltered;
			long idValue;

			foreach (DictionaryEntry de in addressFields){
				string localKey = (string)de.Key;
				string localValue = (string)de.Value;

				if ((localKey == "line1" || localKey == "line2") || localKey == "country"){
					
					if (localValue[0] == '\"' && localValue[localValue.Length-1] == '\"'){
						nameValueFiltered = localValue.Substring(1, localValue.Length-2);
					}
					else{
						Console.WriteLine(String.Format("Value for argument '-{0}' should be enclosed in double quotes", localKey));
						isSuccess = false;
						break;
					}
					outAddressFields[localKey] = nameValueFiltered;
				}
				else if (localKey == "id" || localKey == "personId"){
					try{
						idValue = (long)Convert.ToDouble(localValue);
						outAddressFields[localKey] = localValue;
					}
					catch (Exception e){
						Console.WriteLine(String.Format("ID/PersonID '{0}'not found. Try a numeric ID", localValue));
					}
				}
				else if (localKey == "postcode"){
					nameValueFiltered = (string) localValue;
					outAddressFields[localKey] = nameValueFiltered;
				}
				else {
					Console.WriteLine(String.Format("'-{0}' is not a recognized argument for a address", localKey));
					Constants.PrintHelpArgumentsAddress();
					isSuccess = false;
					break;
				}	
			}

			return new Tuple<bool, Hashtable> (isSuccess, outAddressFields);
		}

		public static bool checkValidValue(Hashtable dict){
			bool isValid = true;

			foreach (DictionaryEntry de in dict){
				string localKey = (string) de.Key;
				string localValue = (localKey != "dob" && localKey != "id") ? (string) de.Value : "";

				if ((localKey == "firstname" || localKey == "lastname" || localKey == "nickname" || localKey == "line2") && localValue.Any(char.IsDigit)) {
					Console.WriteLine(String.Format("'{0}' is not a valid value for argument '-{1}'. Should not contain numbers.", localValue, localKey));
					isValid = false;
				}
				else if (localKey == "postcode" && localValue.Any(ch => !char.IsLetterOrDigit(ch))){
					Console.WriteLine(String.Format("'{0}' is not a valid value for argument '-{1}'. Should not contain any special characters.", localValue, localKey));
					isValid = false;
				}
				else if (localKey == "country" && Constants.EUROPE_COUNTRIES.All(str => !localValue.ToLower().Contains(str.ToLower()))){
					Console.WriteLine(String.Format("'{0}' is not a valid value for argument '-{1}'. Must be a country in Europe.", localValue, localKey));
					isValid = false;
				}
			}

			return isValid;
		}
	}
}
