using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Xand0
{
    public partial class Form1 : Form
    {
        int i = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Click any square";

        }
        private void button1_Click(object sender, EventArgs e)
        {
            restart();
            i = 0;
        }
        private void clc(object sender, EventArgs e)
        {
            label1.Text = "";
            TextBox clickedTextBox = sender as TextBox;
            clickedTextBox.Text = "X";
            clickedTextBox.Enabled = false;
            computerTurn();
        }
        public void restart()
        {
            foreach (TextBox tb in Controls.Cast<Control>().OfType<TextBox>())
            {
                tb.Text = "";
                tb.Enabled = true;
            }
            label1.Text = "Click any square";
            button1.Enabled = false;
        }
        public bool computerTurn()
        {
            List<Control> field = createField(); int cmpTurn;
            if (checkWinner(field)) return true;

            cmpCheckColumnsToBlock(field, "0", out cmpTurn);
            if (cmpTurn == -1) cmpCheckRowsToBlock(field, "0", out cmpTurn);
            if (cmpTurn == -1) cmpCheckDiagonalsToBlock(field, "0", out cmpTurn);

            if (cmpTurn == -1) cmpCheckColumnsToBlock(field, "X", out cmpTurn);
            if (cmpTurn == -1) cmpCheckRowsToBlock(field, "X", out cmpTurn);
            if (cmpTurn == -1) cmpCheckDiagonalsToBlock(field, "X", out cmpTurn);
            if (cmpTurn == -1) checkWhenNothingToBlock(field, "0", out cmpTurn);

            if (field[4].Text == "" && i == 0)
            {
                cmpTurn = 4; i = 1;
            }
            //when it doesn't matter where to put "0" choose it randomly
            if (cmpTurn == -1) cmpRandomChoice(field, out cmpTurn);           
            field[cmpTurn].Text = "0"; field[cmpTurn].Enabled = false;
            if (checkWinner(field)) return true;
            return true;
        }
        private void cmpRandomChoice(List<Control> field, out int cmpTurn)
        {         
            while (true)
            {
                Random rnd = new Random();
                cmpTurn = rnd.Next(0, 9);
                if (field[cmpTurn].Text == "") break;
            }          
        }
        private void cmpCheckColumnsToBlock(List<Control> field, string str, out int cmpTurn)
        {
            int qnt = 0, empty = -1;
            cmpTurn = -1;
            for (var l = 0; l < 3; l++)
            {
                for (var i = 0; i < 9; i += 3)
                {
                    if (field[i + l].Text == "") empty = i + l;
                    if (field[i + l].Text == str) qnt++;
                    
                }
                if (qnt == 2 && empty != -1)
                {
                    cmpTurn = empty;
                    break;
                }
                else
                {
                    cmpTurn = -1;
                    qnt = 0; empty = -1;
                }
            }
        }
        private void cmpCheckRowsToBlock(List<Control> field, string str, out int cmpTurn)
        {
            int qnt = 0, empty = -1; 
            cmpTurn = -1;
            for (var i = 0; i < 9; i += 3)
            {
                for (var l = 0; l < 3; l++)
                {
                    if (field[i + l].Text == "") empty = i + l;
                    if (field[i + l].Text == str) qnt++;
                }
                if (qnt == 2 && empty != -1)
                {
                    cmpTurn = empty;
                    break;
                }
                else
                {
                    cmpTurn = -1;
                    qnt = 0; empty = -1; 
                }
            }
        }
        private void cmpCheckDiagonalsToBlock(List<Control> field, string str, out int cmpTurn)
        {
            int qnt = 0, empty = -1;
            cmpTurn = -1;
            for (var i = 0; i < 9; i += 4)
            {
                if (field[i].Text == "") empty = i;
                if (field[i].Text == str) qnt++;
              
            }
            if (qnt == 2 && empty != -1)
            {
                cmpTurn = empty;
            }
            else
            {
                qnt = 0; empty = -1;
                for (var i = 2; i < 7; i += 2)
                {
                    if (field[i].Text == "") empty = i;
                    if (field[i].Text == str) qnt++;                
                }
                if (qnt == 2 && empty != -1)
                {
                    cmpTurn = empty;
                }
                else
                {
                    cmpTurn = -1;
                }
            }
        }
        private void checkWhenNothingToBlock(List<Control> field, string str, out int cmpTurn)
        {
            int qnt = 0, empty = -1, e = 0;
            cmpTurn = -1;
            for (var i = 0; i < 9; i += 3)
            {
                for (var l = 0; l < 3; l++)
                {
                    if (field[i + l].Text == "") { empty = i + l; e++; }
                    if (field[i + l].Text == str) qnt++;
                }
                if (e == 2 && qnt == 1)
                {
                    cmpTurn = empty; break;
                }
                else
                {
                    e = 0; qnt = 0; cmpTurn = -1;
                }
            }
        }
              
        public List<Control> createField()
        {
            List<Control> field = new List<Control>();
            foreach (TextBox tb in Controls.Cast<Control>().OfType<TextBox>())
            {
                field.Add(tb);
            }
            return field;
        }
        public void disableField()
        {
            foreach (TextBox tb in Controls.Cast<Control>().OfType<TextBox>())
            {
                tb.Enabled = false;              
            }
            button1.Enabled = true;
        }
        
        public bool checkRows(List<Control> field)
        {
            for (var i = 0; i < 9; i += 3)
            {
                var l = i + 1; var k = i + 2;
                if (field[i].Text == "X" && field[l].Text == "X" && field[k].Text == "X")
                {
                    label1.Text = "You won";
                    return true;
                }
                else if (field[i].Text == "0" && field[l].Text == "0" && field[k].Text == "0")
                {
                    label1.Text = "Computer won"; 
                    return true;
                }
            }
            return false;
        }

        public bool checkColumns(List<Control> field)
        {
            for (var i = 0; i < 3; i++)
            {
                var l = i + 3; var k = i + 6;
                if (field[i].Text == "X" && field[l].Text == "X" && field[k].Text == "X")
                {
                    label1.Text = "You won";
                    return true;
                }
                else if (field[i].Text == "0" && field[l].Text == "0" && field[k].Text == "0")
                {
                    label1.Text = "Computer won";
                    return true;
                }
            }
            return false;
        }

        public bool checkDiagonals(List<Control> field)
        {
            if ((field[0].Text == "X" && field[4].Text == "X" && field[8].Text == "X")
                || (field[2].Text == "X" && field[4].Text == "X" && field[6].Text == "X"))
            {
                label1.Text = "You won";
                return true;
            }
            else if ((field[0].Text == "0" && field[4].Text == "0" && field[8].Text == "0")
                || (field[2].Text == "0" && field[4].Text == "0" && field[6].Text == "0"))
            {
                label1.Text = "Computer won";
                return true;
            }
            return false;
        }

        public bool checkWinner(List<Control> field)
        {
            if (checkRows(field) || checkColumns(field) || checkDiagonals(field))
            {
                disableField();
                return true;
            }
            else
            {
                var num = 0;
                for (var i = 0; i < 9; i++)
                {
                    if (field[i].Text == "") num++;
                }
                if (num == 0)
                {
                    label1.Text = "A draw";
                    disableField();
                    return true;
                }
            }
            return false;
        }                                                       
    }
}
