using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>Provides a base class for the hundreds of DTO classes that we will need.  A DTO class is a simple data storage type.  A DTO is the only format accepted by OpenDentBusiness.dll.</summary>
	public abstract class DataTransferObject {
			
		public byte[] Serialize(){
			XmlSerializer serializer = new XmlSerializer(this.GetType());
			MemoryStream memStream=new MemoryStream();
			serializer.Serialize(memStream,this);
			byte[] retVal=memStream.ToArray();
			memStream.Close();
			return retVal;
		}

		public static DataTransferObject Deserialize(byte[] data) {
			MemoryStream memStream=new MemoryStream(data);
			XmlDocument doc=new XmlDocument();
			doc.Load(memStream);
			XmlNode node=doc.SelectSingleNode("/*");
			//Console.WriteLine(node.Name+node.InnerText);
			Type type=Type.GetType("OpenDentBusiness."+node.Name);
			memStream=new MemoryStream(data);//resets to beginning of stream
			XmlSerializer serializer = new XmlSerializer(type);
			DataTransferObject retVal=(DataTransferObject)serializer.Deserialize(memStream);		
			memStream.Close();
			return retVal;
		}
	}

	///<summary>This is used for initial login.</summary>
	public class DtoLogin:DtoCommandBase {
		public string Database;
		public string OdUser;
		public string OdPassHash;
	}

	///<summary>All commands should inherit from this rather than directly from the DTO.  This kind of DTO will trigger a DtoServerAck (int) instead of a Dataset.</summary>
	public class DtoCommandBase:DataTransferObject {
	}

	///<summary>All queries should inherit from this rather than directly from the DTO.  This kind of DTO will trigger a Dataset result.</summary>
	public class DtoQueryBase:DataTransferObject {
	}

	public class DtoGeneralGetTable:DtoQueryBase {
		public string Command;
	}

	public class DtoGeneralGetTableLow:DtoQueryBase {
		public string Command;
	}

	public class DtoGeneralGetDataSet:DtoQueryBase {
		public string Commands;
	}

	public class DtoGeneralNonQ:DtoCommandBase {
		public string Command;
		public bool GetInsertID;
	}

	///<summary>IDorRows will be the InsertID for insert type commands.  For some other commands, it will be the rows changed, and for some commands, it will just be 0.</summary>
	public class DtoServerAck:DataTransferObject {
		public int IDorRows;
	}

	///<summary>OpenDentBusiness and all the DA classes are designed to throw an exception if something goes wrong.  If useing OpenDentBusiness through the remote server, then the server catches the exception and passes it back to the main program using this DTO.  The client then turns it back into an exception so that it behaves just like if OpenDentBusiness was getting called locally.</summary>
	public class DtoException{
		public string Message;
	}

	



}
