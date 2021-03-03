using Manufaktura.Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LiveDots
{
    public class StringToNote
    {
        public static RhythmicDuration SetRitmo(string rKey)
        {
            RhythmicDuration rhythmicDuration = RhythmicDuration.Quarter;

            string rDuration = rKey.Substring(1);
            switch (rDuration)
            {
                case "1/2":
                    rhythmicDuration = RhythmicDuration.Eighth;
                    break;
                case "1":
                    rhythmicDuration = RhythmicDuration.Quarter;
                    break;
                case "2":
                    rhythmicDuration = RhythmicDuration.Half;
                    break;
                case "4":
                    rhythmicDuration = RhythmicDuration.Whole;
                    break;
            }
            return rhythmicDuration;
        }
        //Mira que octava es y lo transforma a numero
        public static string GetNote(string octava)
        {
            string numTono = octava switch
            {
                "Primera" => "1",
                "Segunda" => "2",
                "Tercera" => "3",
                "Quinta" => "5",
                "Sexta" => "6",
                _ => "4",
            };
            return numTono;
        }
        public static string GetResNote(string ViewerValue)
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(ViewerRES));

            //Metodo 2, lo busca con el string directamente, pero he duplicado el resources por comodidad, se limpiara mas adelante
            string key = MyResourceClass.GetString(ViewerValue);
            //string clave = Viewer.GetElement(5).Trim(); // quiza esto influya en los tonos
            if (key != null) return key;
            else return null;
        }
        public static List<string> BrailleToStringNote(int ini_index, int fin_index, BrailleMusicViewer viewer)
        {
            List<string> lista_notas = new List<string>();
            for (int i = ini_index; i < fin_index; i++)
            {
                string aux = viewer.GetElement(i).Trim();

                var aux_first_word = Regex.Match(aux, @"^([\w\-]+)");          
                if (aux_first_word.ToString() != "Espacio"  && aux_first_word.ToString() != "Clave" && aux_first_word.ToString() != "Armadura"
                    && aux_first_word.ToString() != "Compás" && aux_first_word.ToString() != "Salto")

                    lista_notas.Add(aux);
            }
            return lista_notas;
        }
        public static void SetNoteForPlay(ref string nota, out string num_octava)
        {
            num_octava = null;
            string aux_nota = nota.Split(' ').Skip(1).FirstOrDefault();
            if (aux_nota == "octava")
            {
                num_octava = nota.Split(' ')[0];
                var WordsArray = nota.Split();
                //coge los ultimos dos, que contienen las notas
                nota = WordsArray[WordsArray.Length - 2] + ' ' + WordsArray[WordsArray.Length - 1];
            }
        }
    }
}



/*
//rkey viene con la nota [A-G] y la duracion del tono, octava viene con la tonalidad [primera-sexta]
        private void setNote(string rKey, string octava)
        {
            Pitch pitch = null;
            RhythmicDuration rhythmicDuration = RhythmicDuration.Quarter;

            //mira el tono de la octava y busca en el diccionario esa nota, de default coge el tono 4            
            string numTono = "";
            switch (octava)
            {
                case "Primera":
                    numTono = "1";
                    break;
                case "Segunda":
                    numTono = "2";
                    break;
                case "Tercera":
                    numTono = "3";
                    break;
                default:
                case "Cuarta":
                    numTono = "4";
                    break;
                case "Quinta":
                    numTono = "5";
                    break;
                case "Sexta":
                    numTono = "6";
                    break;
            }
            pitch = noteDic._NoteDic[rKey[0] + numTono].pitch_;
           

            string rDuration = rKey.Substring(1);
            switch(rDuration)
            {
                case "1/2":
                    rhythmicDuration = RhythmicDuration.Eighth;
                    break;
                case "1":
                    rhythmicDuration = RhythmicDuration.Quarter;
                    break;
                case "2":
                    rhythmicDuration = RhythmicDuration.Half;
                    break;
                case "4":
                    rhythmicDuration = RhythmicDuration.Whole;
                    break;
            }
            cursorSound.Play(pitch, rhythmicDuration);
        }
        private void getNote(string ViewerValue, string octava)
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(ViewerRES));
            //Metodo 1, con un for, poco optimo
        
            ResourceSet resourceSet =
                 ViewerRES.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                string resourceValue = (string)entry.Value;
                if (resourceValue == ViewerValue)
                {
                    setNote(resourceKey);
                    //cursorSound.play(Pitch.A1, RhythmicDuration.Half);
                    break;
                }
            }
            //Metodo 2, lo busca con el string directamente, pero he duplicado el resources por comodidad, se limpiara mas adelante
            string key = MyResourceClass.GetString(ViewerValue);
            string clave = Viewer.GetElement(5).Trim(); // quiza esto influya en los tonos
            if (key != null) setNote(key, octava);
        }
*/