using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Drzewo_ID3
{
    public partial class Form1 : Form
    {
        public List<List<int>> aSystem;
        
        public Form1()
        {
            this.aSystem = null;
            InitializeComponent();
        }

        private void btnWczytaj_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Wybierz System Decyzyjny";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtPath.Text = "";
                this.aSystem = new List<List<int>>();

                string sPath = openFileDialog.FileName;
                
                int nRows, nCols;
                string[] aLinie = null;

                aLinie = File.ReadAllLines(sPath);
                nRows = aLinie.Length;
                nCols = aLinie[0].Split(' ').Length;

                string[] aElementy;
                for (int i = 0; i < nRows; i++)
                {
                    List<int> aWiersz = new List<int>();
                    aElementy = aLinie[i].Split(' ');
                    nCols = aElementy.Length;
                    for (int j = 0; j < nCols; j++)
                    {
                        if (aElementy[j] != String.Empty)
                        {
                            aWiersz.Add(Convert.ToInt32(aElementy[j]));
                        }
                    }
                    if (aWiersz.Count > 0)
                    {
                        this.aSystem.Add(aWiersz);
                    }
                }

                if (this.aSystem.Count <= 0)
                {
                    MessageBox.Show("Plik jest pusty lub posiada nieodpowiedni format danych.");
                }
                else
                {
                    this.txtPath.Text = openFileDialog.FileName;
                    TreeNode root = UtworzDrzewo();
                    WypiszDrzewo(root);
                }
            }
        }

        private void WypiszDrzewo(TreeNode root)
        {
            System.Windows.Forms.TreeNode tmp;

            tvDrzewo.BeginUpdate();
            tmp = tvDrzewo.Nodes.Add(root.Label);
            WriteNode(root.aChild, tmp);
            tvDrzewo.EndUpdate();

            return;
        }

        private void WriteNode(List<TreeNode> aChild, System.Windows.Forms.TreeNode tmp)
        {
            System.Windows.Forms.TreeNode tmpC;

            foreach (TreeNode node in aChild)
            {
                tmpC = tmp;
                if (node.aChild.Count() == 0)
                {
                    tmp.Nodes.Add(node.Label);
                    continue;
                }

                tmp = tmp.Nodes.Add(node.Label);
                WriteNode(node.aChild, tmp);
                tmp = tmpC;
            }
            
            return;
        }



        private TreeNode UtworzDrzewo()
        {
            TreeNode root;

            List<Atrybut> aAtrybuty = new List<Atrybut>();

            for (int i = 0; i < (this.aSystem[0].Count - 1); i++)
            {
                List<int> aWartosci = new List<int>(0);

                for (int j = 0; j < this.aSystem.Count; j++)
                {
                    aWartosci.Add(this.aSystem[j][i]);
                }

                aAtrybuty.Add(new Atrybut(i, aWartosci));
            }

            DecisionTree ID3 = new DecisionTree();
            root = ID3.MountTree(this.aSystem, aAtrybuty);

            return root;
        }
    }
}
