using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drzewo_ID3
{
    public class DecisionTree
    {
        private int nPositives;
        private int nNegatives;
        private double dEntropy;

        public DecisionTree()
        {
            this.nPositives = 0;
            this.nNegatives = 0;
            this.dEntropy = 0.0;
        }

        // Budujemy drzewo
        public TreeNode MountTree(List<List<int>> aSystem, List<Atrybut> aAtrybuty)
        {
            int decision;
            if (AllDecisionOne(aSystem, out decision))
            {
                return new TreeNode("decision: " + Convert.ToString(decision));
            }
            if (aAtrybuty.Count == 0)
            {
                return new TreeNode("decision: " + Convert.ToString(getMostCommonValue(aSystem)));
            }

            this.nNegatives = CountTotalNegatives(aSystem);
            this.nPositives = aSystem.Count() - this.nNegatives;

            this.dEntropy = GetEntropy(this.nPositives, this.nNegatives);

            Atrybut oBestAtri = GetBestAtribute(aSystem, aAtrybuty);

            TreeNode root = new TreeNode(oBestAtri.ToString());

            foreach (int value in oBestAtri.aValues)
            {
                TreeNode subroot = root.AddTreeNode(new TreeNode("value: " + Convert.ToString(value)));
                List<List<int>> aSystemC = new List<List<int>>();
                List<Atrybut> aAtrybutyC = new List<Atrybut>();

                //zbior obiektow zawierajacy atrybut - bez klonowania !
                foreach (List<int> Obj in aSystem)
                {
                    if (Obj[oBestAtri.nNumerKol] == value)
                    {
                        aSystemC.Add(Obj);
                    }
                }

                //atrybuty bez wybranego
                foreach (Atrybut item in aAtrybuty)
                {
                    if (item != oBestAtri)
                    {
                        aAtrybutyC.Add(item);
                    }
                }

                if (aSystemC.Count == 0)
                {
                    return new TreeNode("decision: " + Convert.ToString(getMostCommonValue(aSystemC)));
                }
                else
                {
                    DecisionTree id3 = new DecisionTree();
                    TreeNode subtree = id3.MountTree(aSystemC, aAtrybutyC);
                    subroot.AddTreeNode(subtree);
                }
            }

            return root;
        }

        // Zwraca najczesciej udzielana odpowiedz w systemie
        private int getMostCommonValue(List<List<int>> aSystem)
        {
            int MostValue = -999;
            int MostCount = -999;
            Dictionary<int, int> Tmp = new Dictionary<int, int>();
            
            foreach (List<int> Obj in aSystem)
            {
                int decyzja = Obj.Last();

                if (Tmp.ContainsKey(decyzja))
                {
                    Tmp[decyzja]++;
                }
                else
                {
                    Tmp.Add(decyzja, 1);
                }

                if (Tmp[decyzja] > MostCount)
                {
                    MostCount = Tmp[decyzja];
                    MostValue = decyzja;
                }
            }

            return MostValue;
        }

        // Zwracamy ile jest decyzji pozytywnych ile negatywnych dla obiektu o podanym atrybucie i wartosci
        private void CountDecAtAtrByVal(List<List<int>> aSystem, Atrybut oAtrybut, int value, out int positives, out int negatives)
        {
            positives = negatives = 0;

            foreach (List<int> Obj in aSystem)
            {
                if (Obj[oAtrybut.nNumerKol] == value)
                {
                    if (Obj.Last() == 0)
                    {
                        negatives++;
                    }
                    else
                    {
                        positives++;
                    }
                }
            }

            return;
        }

        // Najlepszy atrybut z najlepsza metryka
        private Atrybut GetBestAtribute(List<List<int>> aSystem, List<Atrybut> aAtrybuty)
        {
            double MaxGain = -999.9;                // brak
            Atrybut oBestAtr = null;

            foreach (Atrybut atrybut in aAtrybuty)
            {
                double tmp = GetGain(aSystem, atrybut);
                if (tmp > MaxGain)
                {
                    MaxGain = tmp;
                    oBestAtr = atrybut;
                }
            }

            return oBestAtr;
        }

        private double GetGain(List<List<int>> aSystem, Atrybut atrybut)
        {
            double sum = 0.0;
            int positives, negatives;

            foreach (int value in atrybut.aValues)
            {
                positives = negatives = 0;

                CountDecAtAtrByVal(aSystem, atrybut, value, out positives, out negatives);
                
                double entropy = GetEntropy(positives, negatives);

                double total = this.nPositives + this.nNegatives;

                sum += -(double)(positives + negatives) / total * entropy;
            }

            return this.dEntropy + sum;
        }

        private double GetEntropy(int positives, int negatives)
        {
            int total = positives + negatives;
            double ratioPositive = (double)positives / total;
            double ratioNegative = (double)negatives / total;

            if (ratioPositive != 0)
                ratioPositive = -(ratioPositive) * System.Math.Log(ratioPositive, 2);
            if (ratioNegative != 0)
                ratioNegative = -(ratioNegative) * System.Math.Log(ratioNegative, 2);

            double result = ratioPositive + ratioNegative;

            return result;
        }

        // Zwracamy ile jest negatywnych decyzji w podanym systemie
        private int CountTotalNegatives(List<List<int>> aSystem)
        {
            int count = 0;

            foreach (List<int> Obj in aSystem)
            {
                if (Obj.Last() == 0)
                {
                    count++;
                }
            }

            return count;
        } 


        // Sprawdzamy czy wszystkie decyzje w podanym systemie sa jednakowe, jezeli tak zwracamy ta decyzje
        private bool AllDecisionOne(List<List<int>> aSystem, out int decision)
        {
            decision = -999;                    // brak decyzji

            foreach (List<int> Obj in aSystem)
            {
                if (decision == -999)
                {
                    decision = Obj.Last();
                }
                else if (Obj.Last() != decision)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
