using Manufaktura.Controls.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDots
{
    class StaffForNotes
    {
        private Staff ownStaff;
        public StaffForNotes()
        {
            ownStaff = new Staff();
            //las reglas de una partitura generica
        }
        public void addStafftoSymbol(ref Note s)
        {
            //s.Staff = ownStaff;
        }
        public StaffForNotes returnMusicalSymbol()
        {
            return this;
        }
    }
}
