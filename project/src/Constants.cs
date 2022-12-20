using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp{
	public class Constants{
		public static string datadirpath = @".\";
		public static string datafilename = "data.xml";
		public static string datafilepath = datadirpath + datafilename;

		public static string configdirpath = @".\";
		public static string configfilename = "config.xml";
		public static string configfilepath = configdirpath + configfilename;

		public static string mappingdirpath = @".\";
		public static string mappingfilename = "mapping.json";
		public static string mappingfilepath = mappingdirpath + mappingfilename;

		public static string[] EUROPE_COUNTRIES = new String[]{"Russia","Germany","United Kingdom","France","Italy","Spain","Ukraine","Poland","Romania","Netherlands","Belgium","Czechia","Greece","Portugal","Sweden","Hungary","Belarus","Austria","Serbia","Switzerland","Bulgaria","Denmark","Finland","Slovakia","Norway","Ireland","Croatia","Moldova","Bosnia and Herzegovina","Albania","Lithuania","North Macedonia","Slovenia","Latvia","Kosovo","Estonia","Montenegro","Luxembourg","Malta","Iceland","Andorra","Monaco","Liechtenstein","San Marino","Holy See"};

		public static void PrintActionHelp(String cmd1){
			if (cmd1 == "person"){
				Console.WriteLine();
				Console.WriteLine("Available actions for person: ");
				Console.WriteLine("1. add");
				Console.WriteLine("2. edit");
				Console.WriteLine("3. delete");
				Console.WriteLine("4. view");
				Console.WriteLine("5. search");
				Console.WriteLine();
			}

			else if (cmd1 == "address"){
				Console.WriteLine();
				Console.WriteLine("Available actions for address: ");
				Console.WriteLine("1. add");
				Console.WriteLine("2. edit");
				Console.WriteLine("3. delete");
				Console.WriteLine();
			}
		}
		public static void PrintCommandHelp(){
			Console.WriteLine();
			Console.WriteLine("Available commands: ");
			Console.WriteLine("1. person");
			Console.WriteLine("2. address");
			Console.WriteLine();
		}

		public static void PrintHelpArgumentsPerson(){
			Console.WriteLine();
			Console.WriteLine("Availble arguments for person: ");
			Console.WriteLine("1. -id");
			Console.WriteLine("2. -firstname");
			Console.WriteLine("3. -lastname");
			Console.WriteLine("4. -dob");
			Console.WriteLine("5. -nickname");
			Console.WriteLine();
		}

		public static void PrintHelpArgumentsAddress(){
			Console.WriteLine();
			Console.WriteLine("Available arguments for address: ");
			Console.WriteLine("1. -personId");
			Console.WriteLine("2. -id");
			Console.WriteLine("3. -line1");
			Console.WriteLine("4. -line2");
			Console.WriteLine("5. -country");
			Console.WriteLine("6. -postcode");
			Console.WriteLine();
		}

		public static void PrintHelpAllowedArguments(string[] allowedFields){
			Console.WriteLine();
			Console.WriteLine("Allowed arguments: ");
			int idx = 1;
			foreach (string str in allowedFields){
				Console.WriteLine(String.Format("{1}. -{0}", str, idx));
				idx++;
			}
			Console.WriteLine();
		}

		public static void PrintHelpRequiredArguments(string[] requiredFields){
			Console.WriteLine();
			Console.WriteLine("Required arguments: ");
			int idx = 1;
			foreach (string str in requiredFields){
				Console.WriteLine(String.Format("{1}. -{0}", str, idx));
				idx++;
			}
			Console.WriteLine();
		}
	}
}