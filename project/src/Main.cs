/*
=========== Developer notes ==============
Description : To create and manage People information
Version : 1.0
Date : 20-Dec-2022
Developer : Shashank Gandhi
==========================================
*/

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

	public class Program{

		// Main data object
		public static People peopleData = new People{ items = new List<Person>{}};
		public static Meta metaData = new Meta { personID = 0, addressID = 0};
		// public static Mapping mapping = new Mapping { addressPersonMapping = new Dictionary<long, long>{}};

		static void Main(string[] args){			

			if (File.Exists(Constants.datafilepath)){
				Console.WriteLine("Datafile detected. Reading data ...");
				peopleData = XmlUtil.DeserializeXmlFileToList(Constants.datafilepath);
				if (File.Exists(Constants.configfilepath)){
					metaData = XmlUtil.DeserializeXmlFileToMeta(Constants.configfilepath);
				}
				else{
					Console.WriteLine("Configuration file not found. Recreating ...");
					recreateConfig();
					XmlUtil.SerializeMetaToXmlFile(metaData, Constants.configfilepath);
				}
			}
			else {
				Console.WriteLine("No data file found. Creating new ...");
				XmlUtil.SerializeListToXmlFile(peopleData, Constants.datafilepath);
				XmlUtil.SerializeMetaToXmlFile(metaData, Constants.configfilepath);
				// XmlUtil.SerializeDictionaryToJson(mapping, Constants.mappingfilepath);
			}

			Console.WriteLine("Ready");

			Console.WriteLine("** Manage People/Addresses: (type help to find out available commands)");

			readCommand();
		}

		static void recreateConfig(){
			foreach (Person person in peopleData.items){
				metaData.personID = (person.id > metaData.personID) ? person.id : metaData.personID;
				foreach (Address address in person.addresses.items){
					metaData.addressID = (address.id > metaData.addressID) ? address.id : metaData.addressID;
				}
			}
		}
		static void readCommand(){
			while (true){
				Console.Write("> ");
				String input = Console.ReadLine().Trim();
				if (input.ToLower() == "quit" || input.ToLower() == "exit"){
					break;
				}
				else if (input.ToLower() == "help"){
					Constants.PrintCommandHelp();
					continue;
				}

				String[] inputs = input.Split('"').Select((element, index) => index % 2 == 0 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { "\"" + element + "\"" }).SelectMany(element => element).ToList().ToArray();

				readCommand_1(inputs[0].ToLower(), inputs);
			}
		}

		static void readCommand_1(String cmd1, String[] inputs){
			if (cmd1 == "person"){
				if (inputs.Length < 2) {
					expActionMissing(cmd1);
					Constants.PrintActionHelp(cmd1);
					return;
				}
				readPersonAction(cmd1, inputs[1].ToLower(), inputs);
			}
			else if (cmd1 == "address"){
				if (inputs.Length < 2) {
					expActionMissing(cmd1);
					Constants.PrintActionHelp(cmd1);
					return;
				}
				readAddressAction(cmd1, inputs[1].ToLower(), inputs);
			}
			else if (cmd1 == ""){}
			else {
				Console.WriteLine(String.Format("'{0}' is not a recognized command", cmd1));
				Constants.PrintCommandHelp();
			}
		}

		static void readPersonAction(String cmd1, String cmd2, String[] inputs){
			if (cmd2 == "add"){
				PersonAddressUtil.InvokeAddPerson(inputs);
			}
			else if (cmd2 == "view" && inputs.Length > 2){
				PersonAddressUtil.InvokeViewPerson(inputs);
			}
			else if (cmd2 == "view" && inputs.Length == 2){
				PersonAddressUtil.InvokeViewAllPerson();
			}
			else if (cmd2 == "edit"){
				PersonAddressUtil.InvokeEditPerson(inputs);
			}
			else if (cmd2 == "delete"){
				PersonAddressUtil.InvokeDeletePerson(inputs);
			}
			else if (cmd2 == "search"){
				PersonAddressUtil.InvokeSearchPerson(inputs);
			}
			else {
				actionNotRecognized(cmd1, cmd2);
				Constants.PrintActionHelp(cmd1);
			}	
		}

		static void readAddressAction(String cmd1, String cmd2, String[] inputs){
			if (cmd2 == "add"){
				PersonAddressUtil.InvokeAddAddress(inputs);
			}
			// else if (cmd2 == "view"){
			// 	Console.WriteLine("TBD : view address");
			// }
			else if (cmd2 == "edit"){
				PersonAddressUtil.InvokeEditAddress(inputs);
			}
			else if (cmd2 == "delete"){
				PersonAddressUtil.InvokeDeleteAddress(inputs);
			}
			else {
				actionNotRecognized(cmd1, cmd2);
				Constants.PrintActionHelp(cmd1);
			}	
		}

		static void expActionMissing(String cmd1){
			Console.WriteLine(String.Format("Please choose an action for {0}", cmd1));	
		}

		static void actionNotRecognized(String cmd1, String cmd2){
			Console.WriteLine(String.Format("'{0}' is not a recognizable action for command '{1}'", cmd2, cmd1));
		}
	}
}