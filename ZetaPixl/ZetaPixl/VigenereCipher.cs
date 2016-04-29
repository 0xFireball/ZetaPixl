using System;
 
namespace ZetaPhase.ZetaPixl
{
	//use d=1 for encrypting, use d=-1 for decrypting
    class VCipher
    {
        public string Compile(string txt, string pw, int d)
        {
            int pwi = 0, tmp;
            string ns = "";
            txt = txt.ToUpper();
            pw = pw.ToUpper();
            foreach (char t in txt)
            {
                if (t < 65) continue;
                tmp = t - 65 + d * (pw[pwi] - 65);
                if (tmp < 0) tmp += 26;
                ns += Convert.ToChar(65 + ( tmp % 26) );
                if (++pwi == pw.Length) pwi = 0;
            }
 
            return ns;
        }
    }
}