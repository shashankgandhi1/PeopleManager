using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace myapp{
	public class XmlUtil{
		public static void SerializeListToXmlFile(People people, string filepath){
			var xmlSerializer = new XmlSerializer(typeof(People));
			using (var writer = new StreamWriter(filepath)){
				xmlSerializer.Serialize(writer, people); 
			}
		}

		public static People DeserializeXmlFileToList(string filepath){
			var xmlSerializer = new XmlSerializer(typeof(People));
			using (var reader = new StreamReader(filepath)){
				return (People) xmlSerializer.Deserialize(reader);
			}
		}

		public static void SerializeMetaToXmlFile(Meta meta, string filepath){
			var xmlSerializer = new XmlSerializer(typeof(Meta));
			using (var writer = new StreamWriter(filepath)){
				xmlSerializer.Serialize(writer, meta);
			}
		}

		public static Meta DeserializeXmlFileToMeta(string filepath){
			var xmlSerializer = new XmlSerializer(typeof(Meta));
			using (var reader = new StreamReader(filepath)){
				return (Meta) xmlSerializer.Deserialize(reader);
			}
		}

		// public static void SerializeDictionaryToJson(Mapping mapping, string filepath){
		// 	var jsonString = JsonConvert.SerializeObject(mapping);
		// 	File.WriteAllText(filepath, jsonString);
		// }

		// public static Mapping DeserializeJsonToDictionary(string filepath){
		// 	String jsonString = File.ReadAllText(filepath);
		// 	return (Mapping) JsonConvert.DeserializeObject<Mapping>(jsonString);
		// }
	}
}