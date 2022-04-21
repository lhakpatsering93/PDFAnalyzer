using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
    public class pdfReadOutput
    {
        public PolicyDetails CallFunctions(string fileInfo)
        {
            var result = GetTextFromPDF(fileInfo);
            var result2 = GetPropertiesFromText(result);
            return CreatePolicyObject(result2);
        }
        private string GetTextFromPDF(string fileInfo)
        {
            StringBuilder text = new StringBuilder();
            using (PdfReader reader = new PdfReader(fileInfo))
            //using (PdfReader reader = new PdfReader(@"C:\Users\Lhakpa\Downloads\104N072V02.pdf"))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }
            return text.ToString();
        }
        private List<string> GetPropertiesFromText(string pdfText)
        {
            StringBuilder text = new StringBuilder(pdfText);

            List<string> found = new List<string>();
            string line;

            byte[] byteArray = Encoding.ASCII.GetBytes(pdfText);
            MemoryStream stream = new MemoryStream(byteArray);
            using (StreamReader file = new StreamReader(stream))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("Policy no.:") || line.Contains("Telephone:") || line.Contains("Email id:"))
                    {
                        found.Add(line);
                    }
                }
            }
            return found;
        }
        private PolicyDetails CreatePolicyObject(List<string> input)
        {
            var policyDetail = new PolicyDetails();
            foreach(var details in input)
            {
                var charIndex = details.IndexOf(":");
                var value = details.Substring(charIndex + 2).Trim();
                foreach (PropertyInfo p in typeof(PolicyDetails).GetProperties())
                {
                    string propertyName = p.Name.Replace("_", " ");
                    if (details.Contains(propertyName))
                    {
                        p.SetValue(policyDetail, value, null);
                    }
                }
            }           

            return policyDetail;
        }
    }    
}
