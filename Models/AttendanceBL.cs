using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using Newtonsoft.Json;
namespace AttendanceImport_Factory_Template.Models
{
    public interface IAttnProcess
    {
        List<AttendanceModel> ProcessData(System.IO.Stream FileData);
    }
    public class AttnJSONFileProcess: IAttnProcess
    {

        public List<AttendanceModel>  ProcessData(System.IO.Stream FileData)
        {
             
            using ( StreamReader reader = new StreamReader(FileData))
            {
                string attendancejsonData = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<AttendanceModel>>(attendancejsonData);
            }
             
        }
    }
    public class AttnXMLFileProcess : IAttnProcess
    {
        public List<AttendanceModel> ProcessData(System.IO.Stream FileData)
        {
            XmlDataDocument xmlDoc = new XmlDataDocument();
            xmlDoc.Load(FileData);
            XmlNodeList xmlNode;
            int i = 0;
            StringBuilder strdata = new StringBuilder();
            xmlNode = xmlDoc.GetElementsByTagName("employee");
            for(i=0; i<= xmlNode.Count-1; i++)
            {
                strdata.Append("{");
                strdata.Append($"{xmlNode[i].ChildNodes[0].Name }:'{xmlNode[i].ChildNodes.Item(0).InnerText}',");
                strdata.Append($"{xmlNode[i].ChildNodes[1].Name } : '{xmlNode[i].ChildNodes.Item(1).InnerText}',");
                strdata.Append($"{xmlNode[i].ChildNodes[2].Name }: '{xmlNode[i].ChildNodes.Item(2).InnerText}',");
                strdata.Append($"{xmlNode[i].ChildNodes[3].Name }:'{xmlNode[i].ChildNodes.Item(3).InnerText}',");
                strdata.Append($"{xmlNode[i].ChildNodes[4].Name }: '{xmlNode[i].ChildNodes.Item(4).InnerText}'");
                strdata.Append("},");
            }

            string stringifyJsonData = strdata.ToString();
            if (strdata.Length > 0)
            {
                stringifyJsonData = stringifyJsonData.TrimEnd(',');
                stringifyJsonData = "[" + stringifyJsonData + "]";
            }
            List<AttendanceModel> attData = JsonConvert.DeserializeObject<List<AttendanceModel>>(stringifyJsonData);
             return attData;
        }
    }
    public class AttnXLFileProcess : IAttnProcess
    {
        public List<AttendanceModel> ProcessData(System.IO.Stream FileData)
        {
          return  Utility.ConvertXLtoData(FileData);
            
        }
    }
    // Create Factory class for Centrally creation an object of several classes 
    public class FactoryOFAttnProcess
    { 
        private IAttnProcess objIAttnProcess;
        public IAttnProcess ProcessFile(string FileExt)
        {
            if (FileExt.ToUpper() == "JSON")
            {
                objIAttnProcess = new AttnJSONFileProcess();
            }
            else if (FileExt.ToUpper() == "XML")
            {
                objIAttnProcess = new AttnXMLFileProcess();
            }
            else if (FileExt.ToUpper() == "XLS" || FileExt.ToUpper() == "XLSX")
            {
                objIAttnProcess = new AttnXLFileProcess();
            }
            else
            {
                objIAttnProcess = null;
            }
            return objIAttnProcess;
        }
    }
   public class ProcessFileData : ProcessAttnDataAlg
    {
        public ProcessFileData(Stream inputFileStream, string fileExt): base(inputFileStream, fileExt )
        {
        }
        public override List<AttendanceModel> ReadFileData( Stream inputFileStream, string fileExt)
        {
            List<AttendanceModel> attData = new FactoryOFAttnProcess().ProcessFile(fileExt).ProcessData(inputFileStream);
            return attData;            
        }

    }
    // Template Method Pattern 
    public abstract  class ProcessAttnDataAlg
    {
        readonly Stream fileData; // Readonly allow to change or assign value in constructor only
        readonly string fileExtetion; // Readonly allow to change or assign value in constructor only
        public ProcessAttnDataAlg(Stream inputFileStream, string fileExt)
        {
            fileData = inputFileStream;
            fileExtetion = fileExt;
        }
        public virtual void connect()
        {
           
        }
        public abstract List<AttendanceModel> ReadFileData(Stream inputFileStream , string fileExt);
        //public abstract object ProcessData();
        //public abstract void  SaveToDB();        
        public virtual void disconnect()
        {

        }
        // Template  Method 
        public List<AttendanceModel> run()
        {
            connect();
            List<AttendanceModel> attData = ReadFileData(fileData, fileExtetion);
            return attData;
        }

    }
}