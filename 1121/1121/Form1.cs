﻿using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    TODO LIST
    괄호 계산() 추가
    붙여넣기로 수식 복사
    자릿수 구분자
*/

/* TEST LIST
    아무것도 안넣어도 문제가 없는가.
    Screen.Text와 ScreenBuffer의 차이를 비교했는가
*/

/* 괄호 계산 알고리즘
    3 + 3 * 5 +(1)+ 2
    1   2   3   3   2
    () : 역수
    3 +(3 *(5 + 1)+ 2)

    1. 식은 순서대로 적는다. 그러므로 
    2. 괄호가 열릴때마다 인덱스의 우선순위가 1 높아진다.
    3. 괄호가 닫힐때마다 인덱스의 우선순위가 1 내려간다. 
    4. 같은 우선순위의 연산은 왼쪽부터 오른쪽으로 계산한다.
*/

namespace _1107 {
    public partial class Form1 : Form {
        private _1121.Form2 form_2;
        static int size = 300; //field initialler(배열크기 설정하는 부분)은 고정적(static)으로 설정해야 한다.
        string TheFormula = string.Empty;
        string ScreenBuffer = string.Empty; //raw string
        string BufferedNum = string.Empty;
        string BufferedOp = string.Empty;
        string[] MemorizedNum = new string[size];
        string[] MemorizedOp = new string[size];
        byte[] Priority = new byte[size];
        byte tick = 0;
        byte screen_max_width = 24; //스크린의 너비
        byte memory_max_width = 36; //메모리 스크린의 너비
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
            label1.Text = TheFormula;

            if (keyData == Keys.Return) {
                Equal.PerformClick();
                return true; //여기서 return 함으로써 enter키를 누르는 행동이 '버튼을 누르는 등'의 프로시저들 행하지 않고 종료시킨다.
            }
            if (keyData == Keys.Tab) {
                foreach(Control ctr in Controls) {
                    if (ctr.GetType() == typeof(Button)) ctr.TabStop = true;
                }
            }

            if (keyData == (Keys.Shift | Keys.D9)) {
                BracketOpen.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D0)) {
                BracketClose.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D5)) {
                Mod.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D6)) {
                Power.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D8)) {
                Multiply.PerformClick();
                return true;
            }

            if (keyData == (Keys.OemMinus)) {
                Minus.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.Oemplus)) {
                Plus.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        void CallMethodByName(string name) {
            Type t = GetType(); //현재 인스턴스(_1107.Form1 클래스)에 대한 형식(Type)을 가져옴
            MethodInfo method = t.GetMethod(name, BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance);
            //메서드(함수)를 이름으로 검색하되, 지정된 바인드 플래그 형식으로 검색함.
            //바인드 플래그가 없는 버전은 Public 메소드 밖에 검색하지 않음.
            // | 연산자 : 기본적으로 || 와 비슷함. A | B; A의 성질이 있으면서, B의 성질이 있는 플래그들을 포함시킴.
            method.Invoke(this, new object[] { null, EventArgs.Empty });
            //메서드 호출함수. 해당 인스턴스에서 어규먼트(sender, e)를 가진 채로 호출함.
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.D0: case Keys.D1: case Keys.D2: case Keys.D3: case Keys.D4: case Keys.D5: case Keys.D6: case Keys.D7: case Keys.D8: case Keys.D9:
                    CallMethodByName("NUM"+Convert.ToString((int)e.KeyCode-48)+"_Click"); break;
                case Keys.NumPad0: case Keys.NumPad1: case Keys.NumPad2: case Keys.NumPad3: case Keys.NumPad4: case Keys.NumPad5: case Keys.NumPad6: case Keys.NumPad7: case Keys.NumPad8: case Keys.NumPad9:
                    CallMethodByName("NUM"+Convert.ToString((int)e.KeyCode-96)+"_Click"); break;
                case Keys.Decimal:
                    Dot_Click(null, EventArgs.Empty); break;
                case Keys.Oemplus:
                case Keys.Add:
                    Plus_Click(null, EventArgs.Empty); break;
                case Keys.Subtract:
                    Minus_Click(null, EventArgs.Empty); break;
                case Keys.Multiply:
                    Multiply_Click(null, EventArgs.Empty); break;
                case Keys.OemQuestion:
                case Keys.Divide:
                    Divide_Click(null, EventArgs.Empty); break;
                case Keys.Back:
                    Backspace_Click(null, EventArgs.Empty); break;
            }
        }
        private bool IsInvalidModOp() {
            try {
                Convert.ToInt64(Screen.Text);
            } catch {
                MessageBox.Show("나머지 연산은 정수끼리만 가능합니다.");
                return true;
            }
            return false;
        }
        private int ValidateBracket() {
            int NumOpenBracket = 0;
            foreach (char c in TheFormula) if (c == '(') NumOpenBracket++;

            int NumCloseBracket = 0;
            foreach (char c in TheFormula) if (c == ')') NumCloseBracket++;

            return NumOpenBracket - NumCloseBracket;
        }
        private void WriteFormula(string input) {
            //인수분해 모드일땐 숫자 뒤에 (입력이 오면 *(로 바꿔 넣는다
            bool IsFactorize = false; //temp

            switch(input) {
                case "(": TheFormula = IsFactorize ? TheFormula + "*" + "(" : input + TheFormula; break;
                case ")": if (ValidateBracket() <= 0) return; goto default; //여기선 goto써도 합법
                default: TheFormula = TheFormula + input; break;
            }

            label1.Text = TheFormula;
        }
        private string GetFormula() { //수식에 적혀있는 닫히지 않은 괄호들을 전부 닫고 호출한다.
            string _FORMED = TheFormula;
            int numbra = ValidateBracket();

            if (!char.IsNumber(TheFormula[TheFormula.Length - 1])) TheFormula = TheFormula.Remove(TheFormula.Length - 1);
            if (numbra < 0) for (int i = 1; i <= numbra; i++) TheFormula = TheFormula + ")";;
            return _FORMED;
        }
        private void TryCalculate(string op) {
            if (BufferedNum == string.Empty && Memory.Text == string.Empty) { //처음 입력하는 연산자일 경우
                BufferedNum = Screen.Text;
                BufferedOp = op;
            } else {
                string formula = string.Format("{0}", TheFormula);
                if (ValidateBracket() == 0) {
                    double result = double.Parse(new DataTable().Compute(formula, null).ToString());
                    BufferedNum = Convert.ToString(result);
                }
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
        private void AddOperandToScreen(char operand) {
            WriteFormula(Convert.ToString(operand));
            ScreenBuffer = ScreenBuffer + operand;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
            IsOp = false;
        }
        private void AddBracketToScreen(char bracket) {
            WriteFormula(Convert.ToString(bracket));
            ScreenBuffer = string.Empty;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", TheFormula);
        }
        private void UnaryOperate(string op) {
            if (Screen.Text == string.Empty && ScreenBuffer == string.Empty) return;
            ScreenBuffer = Screen.Text != string.Empty && ScreenBuffer == string.Empty ? Screen.Text.Trim() : ScreenBuffer; //return 버튼을 누르고 바로 버튼을 눌렀을때
            BufferedNum = BufferedOp = string.Empty;
            double calculated;
            IsOp = false;
            string FORMEDTEXT = string.Empty;
            switch(op) {
                case "negate":
                    FORMEDTEXT = ScreenBuffer.Contains("-") ? ScreenBuffer.Remove(0, 1) : ScreenBuffer.Insert(0, "-"); break;
                case "sqrt":
                    calculated = Math.Sqrt(Convert.ToDouble(ScreenBuffer));
                    IsDot = calculated != Convert.ToInt32(calculated);
                    FORMEDTEXT = Convert.ToString(calculated); break;
                case "reciproc":
                    calculated = 1 / Convert.ToDouble(ScreenBuffer);
                    IsDot = calculated != Convert.ToInt32(calculated);
                    FORMEDTEXT = Convert.ToString(calculated); break;
            }
            ScreenBuffer = FORMEDTEXT;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", FORMEDTEXT);
        }
        private void BinaryOperate(string op) {
            if ((op == "%" || BufferedOp == "%") && IsInvalidModOp()) return;
            if (ScreenBuffer == string.Empty) {
                if (Memory.Text == String.Empty && Screen.Text == string.Empty) return; //아무것도 안누르고 눌렀을경우
                else if (op == "(") {
                    //결과 계산
                }
                else if (Screen.Text != String.Empty && IsOp) { //피연산자와 연산자를 입력한 상태에서 연산자만 바꾸는 경우 (ex 3 + -)    
                    Memory.Text = Memory.Text.Remove(Memory.Text.Length - 1);
                    Memory.Text += op;
                    MemoryScreen.Text = MemoryScreen.Text.Remove(MemoryScreen.Text.Length - 1);
                    TheFormula = TheFormula.Remove(TheFormula.Length - 1);
                    MemoryScreen.Text += op;
                    WriteFormula(op);
                    BufferedOp = op;
                    ScreenBuffer = string.Empty;
                }
                else { //결과값이 나온 상태에서 연산자를 눌렀을 경우
                    BufferedOp = op;
                    BufferedNum = Screen.Text;
                    AppendMemoryText(op, Screen.Text.Trim());
                }
            } else {
                AppendMemoryText(op, Screen.Text.Trim());
                TryCalculate(op);
            }
            IsDot = false;
            IsOp = true;
            WriteFormula(op);
            ScreenBuffer = string.Empty;
        }
        private void ReturnResult() {
            string formula = string.Empty;
            double result = 0;
            if (BufferedNum == string.Empty && BufferedOp == string.Empty) return; //아무것도 입력 안했는데 누른경우
            else if (ScreenBuffer == string.Empty) {
                formula = string.Format("{0}{1}{2}", Screen.Text, BufferedOp, BufferedNum);
            } else {
                formula = string.Format("{0}{1}{2}", BufferedNum, BufferedOp, Screen.Text);
                BufferedNum = IsOp ? BufferedNum : ScreenBuffer;
            }

            result = double.Parse(new DataTable().Compute(formula, null).ToString());

            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", result); // DataTable.Compute를 통해 수식 계산
            IsDot = result != Convert.ToInt32(result);
            ScreenBuffer = string.Empty;
            Memory.Text = string.Empty;
            MemoryScreen.Text = string.Empty;
        }
        private void NUM0_Click(object sender, EventArgs e) {
            //숫자 0은 1의 자릿수에서 2개이상 쓰이지 않음.
            //소수점 아래에서 위의 조건은 무시됨.
            if (ScreenBuffer.Length != 0) AddOperandToScreen('0');
        }
        private void NUM00_Click(object sender, EventArgs e) {
            if (ScreenBuffer.Length != 0) {
                AddOperandToScreen('0');
                AddOperandToScreen('0');
            }
        }
        private void NUM1_Click(object sender, EventArgs e) => AddOperandToScreen('1');
        private void NUM2_Click(object sender, EventArgs e) => AddOperandToScreen('2');
        private void NUM3_Click(object sender, EventArgs e) => AddOperandToScreen('3');
        private void NUM4_Click(object sender, EventArgs e) => AddOperandToScreen('4');
        private void NUM5_Click(object sender, EventArgs e) => AddOperandToScreen('5');
        private void NUM6_Click(object sender, EventArgs e) => AddOperandToScreen('6');
        private void NUM7_Click(object sender, EventArgs e) => AddOperandToScreen('7');
        private void NUM8_Click(object sender, EventArgs e) => AddOperandToScreen('8');
        private void NUM9_Click(object sender, EventArgs e) => AddOperandToScreen('9');
        private void BracketOpen_Click(object sender, EventArgs e) => AddBracketToScreen('(');
        private void BracketClose_Click(object sender, EventArgs e) => AddBracketToScreen(')');
        private void Dot_Click(object sender, EventArgs e) {
            if (!IsDot) {
                if (ScreenBuffer.Length == 0) AddOperandToScreen('0');
                AddOperandToScreen('.');
                IsDot = true;
            }
        }
        private void Negate_Click(object sender, EventArgs e) => UnaryOperate("negate");
        private void Root_Click(object sender, EventArgs e) => UnaryOperate("sqrt");
        private void Reciprocal_Click(object sender, EventArgs e) => UnaryOperate("reciproc");
        private void Percent_Click(object sender, EventArgs e) => UnaryOperate("percent");
        private void Plus_Click(object sender, EventArgs e) => BinaryOperate("+");
        private void Minus_Click(object sender, EventArgs e) => BinaryOperate("-");
        private void Multiply_Click(object sender, EventArgs e) => BinaryOperate("*");
        private void Divide_Click(object sender, EventArgs e) => BinaryOperate("/");
        private void Mod_Click(object sender, EventArgs e) => BinaryOperate("%");
        private void Power_Click(object sender, EventArgs e) => BinaryOperate("^");
        private void Equal_Click(object sender, EventArgs e) => ReturnResult();
        private void AC_Click(object sender, EventArgs e) { // 메모리 버퍼 + 스크린버퍼 모두 초기화 
            for (int x = 0; x < size; x++) {
                MemorizedNum[x] = string.Empty;
                MemorizedOp[x] = string.Empty;
            }
            IsDot = false;
            Screen.Text = string.Empty;
            ScreenBuffer = string.Empty;
            Memory.Text = string.Empty;
            MemoryScreen.Text = string.Empty;
            BufferedNum = string.Empty;
            BufferedOp = string.Empty;
            TheFormula = string.Empty;
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
            TheFormula = TheFormula.Remove(TheFormula.Length - 1, 1);
            ScreenBuffer = ScreenBuffer.Remove(ScreenBuffer.Length - 1, 1);
            Screen.Text = Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private void 공학용계산기ToolStripMenuItem_Click(object sender, EventArgs e) {
            form_2 = new _1121.Form2();
            form_2.Owner = this;
            form_2.Show();
            form_2.Location = this.Location;
            
            Hide();
        }
        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }
        private void timer1_Tick(object sender, EventArgs e) {

        }
    }
}