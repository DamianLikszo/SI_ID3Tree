using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drzewo_ID3
{
    public class Atrybut
    {
        public int nNumerKol;                           // numer kolumny 
        public List<int> aValues;                       // unikalne wartosci atrybutu
        public List<int> aWartosci;                     // wszystkie wartosci na atrybucie

        public Atrybut(int numer, List<int> aWartosci)
        {
            this.nNumerKol = numer;
            this.aValues = new List<int>();
            this.aWartosci = new List<int>();

            foreach (int item in aWartosci)
            {
                if (!this.aValues.Contains(item))
                {
                    this.aValues.Add(item);
                }
                this.aWartosci.Add(item);
            }
        }

        public override string ToString()
        {
            return "a"+ (this.nNumerKol + 1);
        }
    }
}
