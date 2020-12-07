using System;
using System.Collections.Generic;

namespace LiveDots
{

    public class BrailleMeasure : BrailleElement
    {
        public List<BrailleStaff> Staffs { get; set; }

        public BrailleAttribute Attribute { get; set; }



        public BrailleMeasure()
        {
            Staffs = new List<BrailleStaff>();
            //Attribute = new BrailleAttribute();
        }
        public BrailleMeasure(Object o)
        {
            Staffs = new List<BrailleStaff>();
            //Attribute = new BrailleAttribute();
            this.Attribute = (BrailleAttribute)o;
        }


        /*public void AppendStaff(BrailleStaff[] s)
        {
            Staff = Staff.Concat(s).ToArray();
        }*/
        public void Parse(BrailleText brailleText)
        {
            if (Attribute != null) Attribute.Parse(brailleText);
            foreach (BrailleStaff S in Staffs)
            {
                S.Parse(brailleText);
                brailleText.AddSpace();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Atribute of the Part</returns>
        internal BrailleAttribute ParseBraille(List<char> content, BrailleText brailleText)
        {
            BrailleAttribute res = null;
            
            if (Attribute == null)
            {   
                Attribute = new BrailleAttribute();
                res = Attribute.ParseBraille(content, brailleText);
                content.RemoveAt(0); //\n
            }

            BrailleStaff staff = new BrailleStaff();
            staff.ParseBraille(content, brailleText );
            this.Staffs.Add(staff);
            brailleText.AddSpace();



            return res;
        }
    }
}
