using System;
using System.Collections.Generic;

namespace LiveDots
{

    public class BXBrailleMeasure : BXBrailleElement
    {
        public List<BXBrailleStaff> Staffs { get; set; }

        public BXBrailleAttribute Attribute { get; set; }



        public BXBrailleMeasure()
        {
            Staffs = new List<BXBrailleStaff>();
            //Attribute = new BrailleAttribute();
        }
        public BXBrailleMeasure(Object o)
        {
            Staffs = new List<BXBrailleStaff>();
            //Attribute = new BrailleAttribute();
            this.Attribute = (BXBrailleAttribute)o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Atribute of the Part</returns>
        public BXBrailleAttribute Parse(List<char> content, BrailleText brailleText)
        {
            BXBrailleAttribute res = null;
            
            if (Attribute == null)
            {   
                Attribute = new BXBrailleAttribute();
                res = Attribute.Parse(content, brailleText);
                content.RemoveAt(0); //\n
            }

            BXBrailleStaff staff = new BXBrailleStaff();
            staff.Parse(content, brailleText );
            this.Staffs.Add(staff);
            brailleText.AddSpace();



            return res;
        }

        void BXBrailleElement.Parse(List<char> content, BrailleText brailleText)
        {
            throw new NotImplementedException();
        }
    }
}
