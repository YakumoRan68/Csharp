using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

/*
    TODO LIST
    괄호 계산() 추가
    붙여넣기로 수식 복사
    자릿수 구분자
*/

namespace _1107
{
    public partial class Form1 : Form
    {
        static int buffersize = 300; //field initialler(배열크기 설정하는 부분)은 고정적(static)으로 설정해야 한다.
        string ScreenBuffer = String.Empty;
        long[] Num = new long[buffersize];
        decimal[] Fnum = new decimal[buffersize];
        byte[] OpBuffer = new byte[buffersize]; // (무연산자), +, -, *, /
        byte bufferptr = 0; // 연산자의 포인터
        byte memory_max_width = 35; //메모리 스크린의 너비
        bool IsDot = false; // 실수형인지 정수형인지

        public Form1()
        {
            InitializeComponent();
            overflowed.Visible = false;
            KeyPreview = true;
        }

        private void GetResult()
        {

        }

        private void MemoryScreenUpdate()
        {
            char op = '+';
            switch (OpBuffer[bufferptr])
            {
                case 1: op = '+'; break;
                case 2: op = '-'; break;
                case 3: op = '*'; break;
                case 4: op = '/'; break;
            }
            int numToTrim = (op + " " + ScreenBuffer + " ").Length;
            Memory.Text = op + " " + ScreenBuffer + " " + Memory.Text;
            if (Memory.Text.Length > memory_max_width)
            {
                MemoryScreen.Text = Memory.Text.Substring(0, Memory.Text.Length);
                overflowed.Visible = true;
            }
            else MemoryScreen.Text = Memory.Text;

        }

        private void AddNumToScreen(string op)
        {
            if (op == "." || op == "0.")
            {
                Screen.Text = op + ScreenBuffer;
                ScreenBuffer = ScreenBuffer + op;
            }
            else
            {
                ScreenBuffer = ScreenBuffer + op;
                Screen.Text = ScreenBuffer;
            }
        }

        private void AddOpToScreen(byte op)
        {
            if (ScreenBuffer == String.Empty) return;
            OpBuffer[bufferptr] = op;
            if (IsDot) Fnum[bufferptr] = Convert.ToDecimal(ScreenBuffer);
            else Num[bufferptr] = Convert.ToInt64(ScreenBuffer);
            MemoryScreenUpdate();
            bufferptr++;
            IsDot = false;
            ScreenBuffer = String.Empty;
        }

        private void NUM0_Click(object sender, EventArgs e)
        {
            //숫자 0은 1의 자릿수에서 2개이상 쓰이지 않음.
            //소수점 아래에서 위의 조건은 무시됨.
            if (ScreenBuffer.Length != 0) AddNumToScreen("0");
        }

        private void NUM1_Click(object sender, EventArgs e)
        {
            AddNumToScreen("1");
        }

        private void NUM2_Click(object sender, EventArgs e)
        {
            AddNumToScreen("2");
        }

        private void NUM3_Click(object sender, EventArgs e)
        {
            AddNumToScreen("3");
        }

        private void NUM4_Click(object sender, EventArgs e)
        {
            AddNumToScreen("4");
        }

        private void NUM5_Click(object sender, EventArgs e)
        {
            AddNumToScreen("5");
        }

        private void NUM6_Click(object sender, EventArgs e)
        {
            AddNumToScreen("6");
        }

        private void NUM7_Click(object sender, EventArgs e)
        {
            AddNumToScreen("7");
        }

        private void NUM8_Click(object sender, EventArgs e)
        {
            AddNumToScreen("8");
        }

        private void NUM9_Click(object sender, EventArgs e)
        {
            AddNumToScreen("9");
        }

        private void Dot_Click(object sender, EventArgs e)
        {
            if (!IsDot)
            {
                if (ScreenBuffer.Length == 0) AddNumToScreen("0");
                AddNumToScreen(".");
                IsDot = true;
            }
        }

        private void AC_Click(object sender, EventArgs e)
        { // 메모리 버퍼 + 스크린버퍼 모두 초기화 
            ScreenBuffer = String.Empty;
            foreach (int i in Num) Num[i] = 0;
            foreach (int i in Fnum) Fnum[i] = 0;
            foreach (int i in OpBuffer) OpBuffer[i] = 0;
            IsDot = false;
            Screen.Text = String.Empty;
            Memory.Text = String.Empty;
            MemoryScreen.Text = String.Empty;
            overflowed.Visible = false;
        }

        private void CE_Click(object sender, EventArgs e)
        { // 스크린버퍼만 초기화
            IsDot = false;
            ScreenBuffer = String.Empty;
            Screen.Text = String.Empty;
        }

        private void Backspace_Click(object sender, EventArgs e)
        {
            //기본적으로 c#에는 안정성문제로 포인터 연산자 사용을 막아두었다. (Unsafe 모드로 디버그 해야 한다고함)
            //따라서 문자열을 관리하기 위해서는 주워진 내장함수를 사용해야한다.
            //스트링이름.Remove(문자열인덱스, 지울 갯수)
            if (ScreenBuffer.Length == 0) return;
            if (ScreenBuffer[ScreenBuffer.Length - 1] == '.') IsDot = false;
            ScreenBuffer = ScreenBuffer.Remove(ScreenBuffer.Length - 1, 1);
            Screen.Text = ScreenBuffer;
        }

        private void Plus_Click(object sender, EventArgs e)
        {
            AddOpToScreen(1);
        }

        private void Minus_Click(object sender, EventArgs e)
        {
            AddOpToScreen(2);
        }

        private void Multiply_Click(object sender, EventArgs e)
        {
            AddOpToScreen(3);
        }

        private void Divide_Click(object sender, EventArgs e)
        {
            AddOpToScreen(4);
        }

        private void TestB_Click(object sender, EventArgs e)
        {

        }

        private void Equal_Click(object sender, EventArgs e)
        {

        }

        void CallFunctionByName(string name, object sender, EventArgs e) {
            Type t = GetType(); //현재 인스턴스(_1107.Form1 클래스)에 대한 형식(Type)을 가져옴
            MethodInfo method = t.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            //메서드(함수)를 이름으로 검색하되, 지정된 바인드 플래그 형식으로 검색함.
            //바인드 플래그가 없는 버전은 Public 메소드 밖에 검색하지 않음.
            // | 연산자 : 기본적으로 || 와 비슷함. A | B; A의 성질이 있거나, B의 성질이 있는 플래그들을 포함시킴.
            method.Invoke(this, new object[] { sender, e });
            //메서드 호출함수. 해당 인스턴스에서 어규먼트(sender, e)를 가진 채로 호출함.
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(Convert.ToString((int)e.KeyCode));
            switch (e.KeyCode)
            {
                case Keys.D0: //e.KeyCode = 48
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    CallFunctionByName("NUM" + Convert.ToString((int)e.KeyCode - 48) + "_Click", sender, e); break;
                case Keys.NumPad0: //e.KeyCode = 96
                case Keys.NumPad1: 
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    CallFunctionByName("NUM" + Convert.ToString((int)e.KeyCode - 96) + "_Click", sender, e); break;
                //case Keys.Return:
                //case Keys.Separator:
                //MessageBox.Show("hey"); break;

                case Keys.Decimal: Dot_Click(sender, e); break;
                case Keys.Add: Plus_Click(sender, e); break;
                case Keys.Subtract: Minus_Click(sender, e); break;
                case Keys.Multiply: Multiply_Click(sender, e); break;
                case Keys.Divide: Divide_Click(sender, e); break;


            }
        }

    }
}