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
	
	[Serializable]
	[XmlRoot("PersonDetails")]

	public class People{
		[XmlElement("Person")]
		public List<Person> items { get; set; }
	}

	public class Addresses{
		[XmlElement("Address")]
		public List<Address> items { get; set; }
	}

	public class Person{

		[XmlAttribute("id")]
		public long id { get; set; }
		[XmlElement("firstname")]
		public string firstname { get; set; }
		[XmlElement("lastname")]
		public string lastname { get; set; }
		[XmlElement("DOB")]
		public DateTime dob { get; set; }
		[XmlElement("nickname")]
		public string nickname { get; set; }
		[XmlElement("addressDetails")]
		public Addresses addresses { get; set; }

	}

	// [XmlRoot("AddressDetails")]
	public class Address{

		[XmlAttribute("id")]
		public long id { get; set; }
		[XmlElement("line1")]
		public string line1 { get; set; }
		[XmlElement("line2")]
		public string line2 { get; set; }
		[XmlElement("country")]
		public string country { get; set; }
		[XmlElement("postcode")]
		public string postcode { get; set; }

	}

	[XmlRoot("MetaDetails")]
	public class Meta{
		[XmlElement("personID")]
		public long personID { get; set; }
		[XmlElement("addressID")]
		public long addressID { get; set; }
	}

	// public class Mapping{
	// 	public Dictionary<long,long> addressPersonMapping { get; set; }
	// }

}