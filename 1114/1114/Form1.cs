﻿using System;
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


namespace _1107 {
    public partial class Form1 : Form {
        static int buffersize = 300; //field initialler(배열크기 설정하는 부분)은 고정적(static)으로 설정해야 한다.
        string ScreenBuffer = string.Empty;
        string BufferedNum = string.Empty;
        string BufferedOp = string.Empty;
        double[] Fnum = new double[buffersize];
        long[] Num = new long[buffersize];
        byte screen_max_width = 26; //스크린의 너비
        byte memory_max_width = 39; //메모리 스크린의 너비
        bool IsDot = false; // 실수형인지 정수형인지
        bool IsOp = false; //false : 피연산자, true:연산자

        public Form1() {
            InitializeComponent();
            MemoryScreen.Text = string.Empty;
            overflowed.Visible = false;
            KeyPreview = true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            //대부분의 경우 키보드에서 키를 누르면 KeyUp, KeyPress, KeyDown 등의 이벤트들이 발생한다.
            //그러나 Control, Alt, ReturnResult 같은 특수한 키들은 상기된 이벤트들을 발생시기 전에 미리 짜여진, 다른 행동을 취한다.
            //ProcessCmdKey는 해당 폼으로 들어오는 키들을 제일 먼저 인식하고 관련된 키에 대한 프로시저들을 실행하는 메소드인데, 이 함수를 오버라이드 한다.
            if (keyData == Keys.Return) {
                
                Equal.PerformClick();

                return true; //여기서 return 함으로써 enter키를 누르는 행동이 '버튼을 누르는 등'의 프로시저들 행하지 않고 종료시킨다.
            }

            if (keyData == Keys.Tab) {
                foreach(Control ctr in Controls) {
                    if (ctr.GetType() == typeof(Button)) ctr.TabStop = true;
                }
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void CallMethodByName(string name) {
            Type t = GetType(); //현재 인스턴스(_1107.Form1 클래스)에 대한 형식(Type)을 가져옴
            MethodInfo method = t.GetMethod(name, BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance);
            //메서드(함수)를 이름으로 검색하되, 지정된 바인드 플래그 형식으로 검색함.
            //바인드 플래그가 없는 버전은 Public 메소드 밖에 검색하지 않음.
            // | 연산자 : 기본적으로 || 와 비슷함. A | B; A의 성질이 있거나, B의 성질이 있는 플래그들을 포함시킴.
            method.Invoke(this, new object[] { null, EventArgs.Empty });
            //메서드 호출함수. 해당 인스턴스에서 어규먼트(sender, e)를 가진 채로 호출함.
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            //MessageBox.Show(Convert.ToString(e.KeyCode));
            switch (e.KeyCode) {
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
                    CallMethodByName("NUM"+Convert.ToString((int)e.KeyCode-48)+"_Click"); break;
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
                    CallMethodByName("NUM"+Convert.ToString((int)e.KeyCode-96)+"_Click"); break;
                case Keys.Decimal: Dot_Click(null, EventArgs.Empty); break;
                case Keys.Add: Plus_Click(null, EventArgs.Empty); break;
                case Keys.Subtract: Minus_Click(null, EventArgs.Empty); break;
                case Keys.Multiply: Multiply_Click(null, EventArgs.Empty); break;
                case Keys.Divide: Divide_Click(null, EventArgs.Empty); break;
            }
        }
        private void ReturnResult() {
            //label1.Text = string.Format("{0} {1}", BufferedNum.Trim(), BufferedOp.Trim());
            string formula = string.Empty;
            double result = 0;
            if (BufferedNum == string.Empty && BufferedOp == string.Empty) return; //아무것도 입력 안했는데 누른경우
            else if (ScreenBuffer == String.Empty) {
                formula = string.Format("{0}{1}{2}", Screen.Text, BufferedOp, BufferedNum);
            } else {
                formula = string.Format("{0}{1}{2}", BufferedNum, BufferedOp, Screen.Text);
                BufferedNum = IsOp ? BufferedNum : ScreenBuffer;
            }

            try {//Debug
                result = double.Parse(new DataTable().Compute(formula, null).ToString());
            } catch {
                MessageBox.Show("Error");
                label1.Text = String.Format("BufferedOp = {0} BufferedNum = {1} IsOp = {2} formula = {3}", BufferedOp, BufferedNum, IsOp, formula);
            }

            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", result); // DataTable.Compute를 통해 수식 계산
            IsDot = false;
            ScreenBuffer = string.Empty;
            Memory.Text = string.Empty;
            MemoryScreen.Text = string.Empty;
        }
        private void TryCalculate(string op) {
            if (BufferedNum == string.Empty && Memory.Text == string.Empty) { //처음 입력하는 연산자일 경우
                BufferedNum = Screen.Text;
                BufferedOp = op;
            } else {
                string formula = string.Format("{0}{1}{2}", BufferedNum, BufferedOp, Screen.Text); 
                double result = double.Parse(new DataTable().Compute(formula, null).ToString());  
                BufferedNum = Convert.ToString(result);
                BufferedOp = op;
                Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", BufferedNum);
            }
        }
        private void AppendMemoryText(string op, string operand) {
            Memory.Text = string.Format("{0} {1} {2}", Memory.Text, operand, op);
            MemoryScreenUpdate();
        }
        private void MemoryScreenUpdate() {
            if (Memory.Text.Length > memory_max_width) {
                MemoryScreen.Text = string.Format("{0,"+ Convert.ToString(memory_max_width) + "}", Memory.Text.Substring(Memory.Text.Length - memory_max_width - 1, memory_max_width));
                overflowed.Visible = true;
            }
            else MemoryScreen.Text = string.Format("{0," + Convert.ToString(memory_max_width) + "}", Memory.Text);
        }
        private void AddNumToScreen(char operand) {
            ScreenBuffer = ScreenBuffer + operand;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
            IsOp = false;
        }
        private void AddOpToScreen(string op) {
            if (ScreenBuffer == string.Empty) {
                if (Memory.Text == String.Empty && Screen.Text == String.Empty) return; //아무것도 안누르고 눌렀을경우
                else if (Screen.Text != String.Empty) { //결과값이 나온 상태에서 연산자를 눌렀을 경우
                    BufferedOp = op;
                    BufferedNum = Screen.Text;
                    AppendMemoryText(op, Screen.Text);
                }
                else { //Case 피연산자와 연산자를 입력한 상태에서 연산자만 바꾸는 경우
                    Memory.Text = Memory.Text.Remove(Memory.Text.Length - 1);
                    Memory.Text += op;
                    MemoryScreen.Text = MemoryScreen.Text.Remove(MemoryScreen.Text.Length - 1);
                    MemoryScreen.Text += op;
                    BufferedOp = op;
                    ScreenBuffer = string.Empty;
                }
            } else {
                TryCalculate(op);
                AppendMemoryText(op, ScreenBuffer);
            }
            //OpBuffer[bufferptr] = op; 괄호계산에 필요함
            //switch(OpBuffer[bufferptr]) {
            //    case 1: op = '+'; break;
            //    case 2: op = '-'; break;
            //    case 3: op = '*'; break;
            //    case 4: op = '/'; break;
            //}
            //bufferptr++;
            IsDot = false;
            IsOp = true;
            ScreenBuffer = string.Empty;
        }
        private void NUM0_Click(object sender, EventArgs e) {
            //숫자 0은 1의 자릿수에서 2개이상 쓰이지 않음.
            //소수점 아래에서 위의 조건은 무시됨.
            if (ScreenBuffer.Length != 0) AddNumToScreen('0');
        }
        private void NUM1_Click(object sender, EventArgs e)
        {
            AddNumToScreen('1');
            //MouseEventArgs eventArgs = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 1);
            //OnMouseDown(eventArgs);
            //OnMouseClick(eventArgs);
            //System.Threading.Thread.Sleep(1);
            //OnMouseUp(eventArgs);
            //System.Threading.Thread.Sleep(1);
        }
        private void NUM2_Click(object sender, EventArgs e)
        {
            AddNumToScreen('2');
        }
        private void NUM3_Click(object sender, EventArgs e)
        {
            AddNumToScreen('3');
        }
        private void NUM4_Click(object sender, EventArgs e)
        {
            AddNumToScreen('4');
        }
        private void NUM5_Click(object sender, EventArgs e)
        {
            AddNumToScreen('5');
        }
        private void NUM6_Click(object sender, EventArgs e)
        {
            AddNumToScreen('6');
        }
        private void NUM7_Click(object sender, EventArgs e)
        {
            AddNumToScreen('7');
        }
        private void NUM8_Click(object sender, EventArgs e)
        {
            AddNumToScreen('8');
        }
        private void NUM9_Click(object sender, EventArgs e)
        {
            AddNumToScreen('9');
        }
        private void Dot_Click(object sender, EventArgs e) {
            if (!IsDot) {
                if (ScreenBuffer.Length == 0) AddNumToScreen('0');
                AddNumToScreen('.');
                IsDot = true;
            }
        }
        private void AC_Click(object sender, EventArgs e) { // 메모리 버퍼 + 스크린버퍼 모두 초기화 
            foreach (int i in Num) Num[i] = 0;
            foreach (int i in Fnum) Fnum[i] = 0;
            IsDot = false;
            Screen.Text = string.Empty;
            ScreenBuffer = string.Empty;
            Memory.Text = string.Empty;
            MemoryScreen.Text = string.Empty;
            BufferedNum = string.Empty;
            BufferedOp = string.Empty;
            IsOp = false;
            overflowed.Visible = false;
        }
        private void CE_Click(object sender, EventArgs e) { // 스크린버퍼만 초기화
            IsDot = false;
            ScreenBuffer = string.Empty;
            Screen.Text = string.Empty;
            IsOp = false;
        }
        private void Backspace_Click(object sender, EventArgs e) {
            //기본적으로 c#에는 안정성문제로 포인터 연산자 사용을 막아두었다. (Unsafe 모드로 디버그 해야 한다고함)
            //따라서 문자열을 관리하기 위해서는 주워진 내장함수를 사용해야한다.
            //스트링이름.Remove(문자열인덱스, 지울 갯수)
            if (ScreenBuffer.Length == 0) return;
            if (ScreenBuffer[ScreenBuffer.Length - 1] == '.') IsDot = false;
            ScreenBuffer = ScreenBuffer.Remove(ScreenBuffer.Length - 1, 1);
            Screen.Text = ScreenBuffer;
        }
        private void Plus_Click(object sender, EventArgs e) {
            AddOpToScreen("+");
        }
        private void Minus_Click(object sender, EventArgs e) {
            AddOpToScreen("-");
        }
        private void Multiply_Click(object sender, EventArgs e) {
            AddOpToScreen("*");
        }
        private void Divide_Click(object sender, EventArgs e) {
            AddOpToScreen("/");
        }
        private void TestB_Click(object sender, EventArgs e) {

        }
        private void Equal_Click(object sender, EventArgs e) {
            ReturnResult();
        }
        private void NUM00_Click(object sender, EventArgs e) {

        }
    }
}