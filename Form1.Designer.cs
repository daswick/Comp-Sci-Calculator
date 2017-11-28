using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace Calculator
{
	partial class Form1
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			label = new Label();

			for(int i = 0; i < 25; i++)
			{
				buttons[i] = new Button();
			}

			SuspendLayout();

			label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			label.Location = new System.Drawing.Point(12, 18);
			label.Name = "label";
			label.Size = new System.Drawing.Size(324, 40);
			label.TabIndex = 20;
			label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

			buttons[0].Location = new System.Drawing.Point(12, 330);
			buttons[0].Text = "0";

			buttons[1].Location = new System.Drawing.Point(78, 330);
			buttons[1].Text = "1";

			buttons[2].Location = new System.Drawing.Point(144, 330);
			buttons[2].Text = "2";

			buttons[3].Location = new System.Drawing.Point(210, 330);
			buttons[3].Text = "3";

			buttons[4].Location = new System.Drawing.Point(12, 264);
			buttons[4].Text = "4";

			buttons[5].Location = new System.Drawing.Point(78, 264);
			buttons[5].Text = "5";

			buttons[6].Location = new System.Drawing.Point(144, 264);
			buttons[6].Text = "6";

			buttons[7].Location = new System.Drawing.Point(210, 264);
			buttons[7].Text = "7";

			buttons[8].Location = new System.Drawing.Point(12, 198);
			buttons[8].Text = "8";

			buttons[9].Location = new System.Drawing.Point(78, 198);
			buttons[9].Text = "9";

			buttons[10].Location = new System.Drawing.Point(144, 198);
			buttons[10].Text = "A";

			buttons[11].Location = new System.Drawing.Point(210, 198);
			buttons[11].Text = "B";

			buttons[12].Location = new System.Drawing.Point(12, 132);
			buttons[12].Text = "C";

			buttons[13].Location = new System.Drawing.Point(78, 132);
			buttons[13].Text = "D";

			buttons[14].Location = new System.Drawing.Point(144, 132);
			buttons[14].Text = "E";

			buttons[15].Location = new System.Drawing.Point(210, 132);
			buttons[15].Text = "F";

			buttons[16].Location = new System.Drawing.Point(276, 132);
			buttons[16].Text = "+";

			buttons[17].Location = new System.Drawing.Point(276, 198);
			buttons[17].Text = "-";

			buttons[18].Location = new System.Drawing.Point(276, 264);
			buttons[18].Text = "%";

			buttons[19].Location = new System.Drawing.Point(276, 330);
			buttons[19].Text = "=";

			buttons[20].Location = new System.Drawing.Point(276, 66);
			buttons[20].Text = "⌫";

			buttons[21].Location = new System.Drawing.Point(210, 66);
			buttons[21].Text = "CE";

			buttons[22].Location = new System.Drawing.Point(144, 66);
			buttons[22].Text = "Dec";

			buttons[23].Location = new System.Drawing.Point(78, 66);
			buttons[23].Text = "Hex";

			buttons[24].Location = new System.Drawing.Point(12, 66);
			buttons[24].Text = "Bin";


			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(348, 404);
			Controls.Add(label);
			for (int i = 0; i < 25; i++)
				Controls.Add(buttons[i]);

			for(int i = 0; i < 25; i++)
			{
				buttons[i].Name = "button" + i.ToString();
				buttons[i].TabIndex = i;
				buttons[i].Size = new System.Drawing.Size(60, 60);
				buttons[i].UseVisualStyleBackColor = true;
				buttons[i].Click += ButtonEvent;
			}

			SetButtons();

			KeyPreview = true;
			KeyPress += Form1_KeyPress;

			Name = "Form1";
			Text = "Calculator";
			label.Select();
			Load += new System.EventHandler(Form1_Load);
			ResumeLayout(false);
		}

		private void UpdateLabel(string input)
		{
			switch (input)
			{
				case "0":
				case "1":
					label.Text += input;
					break;
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
					if (buttons[2].Enabled)
						label.Text += input;
					break;
				case "a":
				case "A":
				case "b":
				case "B":
				case "c":
				case "C":
				case "d":
				case "D":
				case "e":
				case "E":
				case "f":
				case "F":
					if (buttons[10].Enabled)
						label.Text += input.ToUpper();
					break;
				case "Bin":
					if (label.Text.ToString() == string.Empty || label.Text.ToString()[label.Text.ToString().Length - 1] == ' ')
						label.Text += "0b";
					else
						ConvertAndCompute(input);
					break;
				case "x":
				case "X":
				case "Hex":
					if (label.Text.ToString() == string.Empty || label.Text.ToString()[label.Text.ToString().Length - 1] == ' ')
						label.Text += "0x";
					else
						ConvertAndCompute(input);
					break;
				case "#":
				case "Dec":
					if (label.Text.ToString() == string.Empty || label.Text.ToString()[label.Text.ToString().Length - 1] == ' ')
						label.Text += "#";
					else
						ConvertAndCompute(input);
					break;
				case "\b":
				case "⌫":
					if (label.Text.ToString() == string.Empty)
						break;

					if (label.Text.ToString()[label.Text.ToString().Length - 1] == ' ')
						label.Text = label.Text.ToString().Substring(0, label.Text.ToString().Length - 3);
					else
					{
						char c = label.Text.ToString()[label.Text.ToString().Length - 1];
						if (c == 'x' || c == 'b')
							label.Text = label.Text.ToString().Substring(0, label.Text.ToString().Length - 2);
						else
							label.Text = label.Text.ToString().Substring(0, label.Text.ToString().Length - 1);
					}
					break;
				case "CE":
					label.Text = string.Empty;
					break;
				case "=":
					ConvertAndCompute();
					break;
				case "+":
				case "-":
				case "%":
					if (label.Text.ToString() != string.Empty && label.Text.ToString()[label.Text.ToString().Length - 1] != ' ')
						label.Text += " " + input + " ";
					break;
				default:
					break;
			}
		}

		private void SetButtons()
		{
			if (label.Text.ToString() == string.Empty || label.Text.ToString()[label.Text.ToString().Length - 1] == ' ')
			{
				for (int i = 0; i < 20; i++)
					buttons[i].Enabled = false;

				if (label.Text.ToString() == string.Empty)
				{
					buttons[20].Enabled = false;
					buttons[21].Enabled = false;
				}
				else
				{
					buttons[20].Enabled = true;
					buttons[21].Enabled = true;
				}

				buttons[22].Enabled = true;
				buttons[23].Enabled = true;
				buttons[24].Enabled = true;
			}
			else
			{
				char c = label.Text[label.Text.Length - 1];

				if (c == 'b')
				{
					buttons[0].Enabled = true;
					buttons[1].Enabled = true;

					for (int i = 2; i < 20; i++)
						buttons[i].Enabled = false;

					buttons[22].Enabled = false;
					buttons[23].Enabled = false;
					buttons[24].Enabled = false;
				}
				else if (c == '#')
				{
					for (int i = 0; i < 10; i++)
						buttons[i].Enabled = true;

					for (int i = 10; i < 20; i++)
						buttons[i].Enabled = false;

					buttons[22].Enabled = false;
					buttons[23].Enabled = false;
					buttons[24].Enabled = false;
				}
				else if (c == 'x')
				{
					for (int i = 0; i < 16; i++)
						buttons[i].Enabled = true;

					for (int i = 16; i < 20; i++)
						buttons[i].Enabled = false;

					buttons[22].Enabled = false;
					buttons[23].Enabled = false;
					buttons[24].Enabled = false;
				}
				else
				{
					buttons[16].Enabled = true;
					buttons[17].Enabled = true;
					buttons[18].Enabled = true;
					buttons[19].Enabled = true;
					buttons[22].Enabled = true;
					buttons[23].Enabled = true;
					buttons[24].Enabled = true;
				}

				buttons[20].Enabled = true;
				buttons[21].Enabled = true;
			}
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e)
		{
			string key = e.KeyChar.ToString();

			UpdateLabel(key);

			SetButtons();
		}

		private void ButtonEvent(object sender, System.EventArgs e)
		{
			string input = (sender as Button).Text;

			UpdateLabel(input);

			SetButtons();

			label.Select();
		}

		private object Parse(string s)
		{
			if (s[1] == 'b')
				return Binary.Parse(s);
			else if (s[1] == 'x')
				return Hex.Parse(s);
			else
				return int.Parse(s.Substring(1, s.Length - 1));
		}

		private void ConvertAndCompute(string input = "")
		{
			string[] entries = label.Text.Split(' ');
			string result = string.Empty;

			if(entries.Length == 1)
			{
				var o = Parse(entries[0]);

				if (o.GetType() == typeof(Binary))
				{
					Binary b = (Binary)o;
					if (input == "Bin" || input == "")
						result = b.ToString();
					else if (input == "Hex")
						result = Hex.ConvertBinToHex(b).ToString();
					else
						result = "#" + Binary.ConvertBinToInt(b).ToString();
				}
				else if (o.GetType() == typeof(Hex))
				{
					Hex h = (Hex)o;
					if (input == "Hex" || input == "")
						result = h.ToString();
					else if (input == "Bin")
						result = Binary.ConvertHexToBin(h).ToString();
					else
						result = "#" + Hex.ConvertHexToInt(h).ToString();
				}
				else
				{
					int n = (int)o;
					if (input == "Dec" || input == "")
						result = "#" + n.ToString();
					else if (input == "Hex")
						result = Hex.ConvertIntToHex(n).ToString();
					else
						result = Binary.ConvertIntToBin(n).ToString();
				}

				label.Text = result;
				previous = result;

				return;
			}

			Stack<string> ops = new Stack<string>();

			string post = string.Empty;

			foreach (string str in entries)
			{
				bool isNum = false;
				if (str.Length > 1 && (str[0] == '#' || str[1] == 'x' || str[1] == 'b'))
					isNum = true;

				if (isNum)
					post += str + " ";
				else if (str == "(")
					ops.Push("(");
				else if (str == ")")
				{
					string top = ops.Pop();
					while (top != "(")
					{
						post += top + " ";
						top = ops.Pop();
					}
				}
				else
				{
					while (ops.Count > 0 && ops.Peek() != "(")
						post += ops.Pop() + " ";
					ops.Push(str);
				}
			}

			while (ops.Count > 1)
				post += ops.Pop() + " ";
			post += ops.Pop();

			Stack<string> stack = new Stack<string>();

			entries = post.Split(' ');

			for (int i = 0; i < entries.Length; i++)
			{
				bool isNum = false;
				if (entries[i].Length > 1 && (entries[i][0] == '#' || entries[i][1] == 'x' || entries[i][1] == 'b'))
					isNum = true;

				if (isNum)
					stack.Push(entries[i]);
				else
				{
					var o1 = Parse(stack.Pop());
					var o2 = Parse(stack.Pop());

					string oper = string.Empty;
					if (o2.GetType() == typeof(Binary))
					{
						Binary b1 = (Binary)o2;
						Binary b2;

						if (o1.GetType() == typeof(Binary))
							b2 = (Binary)o1;
						else if (o1.GetType() == typeof(Hex))
							b2 = Binary.ConvertHexToBin((Hex)o1);
						else
							b2 = Binary.ConvertIntToBin((int)o1);

						if (entries[i] == "+")
							oper = (b1 + b2).ToString();
						else if (entries[i] == "-")
							oper = (b1 - b2).ToString();
						else if (entries[i] == "%")
							oper = (b1 % b2).ToString();
					}
					else if (o2.GetType() == typeof(Hex))
					{
						Hex h1 = (Hex)o2;
						Hex h2;

						if (o1.GetType() == typeof(Hex))
							h2 = (Hex)o1;
						else if (o1.GetType() == typeof(Binary))
							h2 = Hex.ConvertBinToHex((Binary)o1);
						else
							h2 = Hex.ConvertIntToHex((int)o1);

						if (entries[i] == "+")
							oper = (h1 + h2).ToString();
						else if (entries[i] == "-")
							oper = (h1 - h2).ToString();
						else if (entries[i] == "%")
							oper = (h1 % h2).ToString();
					}
					else
					{
						int n1 = (int)o2;
						int n2;

						if (o1.GetType() == typeof(int))
							n2 = (int)o1;
						else if (o1.GetType() == typeof(Hex))
							n2 = Hex.ConvertHexToInt((Hex)o1);
						else
							n2 = Binary.ConvertBinToInt((Binary)o1);

						if (entries[i] == "+")
							oper = "#" + (n1 + n2).ToString();
						else if (entries[i] == "-")
							oper = "#" + (n1 - n2).ToString();
						else if (entries[i] == "%")
							oper = "#" + (n1 % n2).ToString();
					}

					stack.Push(oper);
				}
			}

			label.Text = stack.Pop();
		}

		private string previous;
		private Label label;
		private Button[] buttons = new Button[25];
	}
}