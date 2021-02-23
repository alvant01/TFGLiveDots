using System;
using System.Data;
using System.Xml;
namespace LiveDots { 
    public class CreateXML
    {
        public static void createDocument()
        {
            XmlTextWriter writer = new XmlTextWriter("Notas.xml", System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("Octavas");

            //not sure how many we need but Pitch only has 6
            createNode("Primera", 1, writer);
            createNode("Segunda", 2, writer);
            createNode("Tercera", 3, writer);
            createNode("Cuarta", 4, writer);
            createNode("Quinta", 5, writer);
            createNode("Sexta", 6, writer);
            //createNode("Septima", 7, writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        static void createNode(string numOctava,int num, XmlTextWriter writer)
        {
            char[] notas = new char[7];
            notas[0] = 'A';
            notas[1] = 'B';
            notas[2] = 'C';
            notas[3] = 'D';
            notas[4] = 'E';
            notas[5] = 'F';
            notas[6] = 'G';
            //writer.WriteStartElement("Octavas");
            writer.WriteStartElement(numOctava);
            for (int i = 0; i < notas.Length; i++)
            {
                writer.WriteStartElement("Nota");
                string nota = notas[i].ToString() + num.ToString();
                writer.WriteString(nota);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            //writer.WriteEndElement();
        }
    }
}
