using System;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

class Class1
{
	static void Main(string[] args)
	{
		try
		{
			if (args.Length < 2)
			{
				Console.WriteLine("usage: <gedcomFileName> <xmlOutputFileName> [-s <xsltFileName>]");
				return;
			}

			string gedcomFileName = args[0];
			string outputFileName = args[1];
			string xsltFileName = "";

			GedcomReader gr = new GedcomReader(gedcomFileName);
			XmlDocument doc = new XmlDocument();
			doc.Load(gr);
			gr.Close();

			if (args.Length > 3 && args[2].Equals("-s"))
			{
				xsltFileName = args[3];
				XslTransform tx = new XslTransform();
				tx.Load(xsltFileName);
				FileStream fs = new FileStream(outputFileName, FileMode.Create);
				tx.Transform(doc, null, fs, null);
			}
			else 
				doc.Save(args[1]);
		}	
		catch(Exception e)
		{
			Console.WriteLine("###error: {0}", e.Message);
		}		
	}
}