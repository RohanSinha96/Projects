using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace FacialRecognition
{
public partial class StartScreen : Form
{
public StartScreen()
{
InitializeComponent();
}
private void StartScreen_Load(object sender, EventArgs e)

{
}
private void button1_Click(object sender, EventArgs e)
{
Form1 f = new Form1();
f.Show();
this.Hide();
}
private void button2_Click(object sender, EventArgs e)
{
Recognizer r = new Recognizer();
r.Show();
this.Hide();
}
}
}