using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prjCsWebRemax
{
    public class Sendmail
    {
        private int vRefSendmail;
        private int vRefAgent;
        private int vRefHouse;
        private string vPhone;
        private string vMsg;

        public Sendmail()
        {
        }

        public Sendmail(int vRefAgent, int vRefHouse, string vPhone, string vMsg)
        {
            this.RefAgent = vRefAgent;
            this.RefHouse = vRefHouse;
            this.Phone = vPhone;
            this.Msg = vMsg;
        }

        public int RefSendmail { get => vRefSendmail; set => vRefSendmail = value; }
        public int RefAgent { get => vRefAgent; set => vRefAgent = value; }
        public int RefHouse { get => vRefHouse; set => vRefHouse = value; }
        public string Phone { get => vPhone; set => vPhone = value; }
        public string Msg { get => vMsg; set => vMsg = value; }
    }
}