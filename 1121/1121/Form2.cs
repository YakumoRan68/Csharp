using System;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace _1121 {
    public partial class Form2 : Form {
        class Expression {
            private string[] element;
            private int top;
            public Expression() {
                element = new string[3];
                top = -1;
            }

            public bool isempty() {
                return top == -1;
            }

            public string peek() {
                return element[top];
            }

            public void push(string item) {
                if (top == 2) {
                    MessageBox.Show("Stack Overflow");
                    return;
                } else {
                    element[++top] = item;
                }
            }

            public int count() {
                int cnt = 0;
                foreach (var v in element) {
                    cnt = v == null ? cnt : ++cnt;
                }
                return cnt;
            }

            public string pop() {
                if (top == -1) {
                    MessageBox.Show("Stack Underflow");
                    return string.Empty;
                } else {
                    string str = element[top];
                    element[top--] = null;
                    return str;
                }
            }

            public void clear() {
                for (int i = 0; i < 3; i++) {
                    element[i] = null;
                }
                top = -1;
            }

            public string print() {
                string str = string.Empty;
                foreach (var v in element) {
                    str = v == null ? null : v;
                    Debug.WriteLine(v);
                }
                return str;
            }
        }

        class Memory {
            private string term = string.Empty;

            public Memory(string str) {
                term = str;
            }

            public string Term {
                get {
                    return term;
                }
                set {
                    term = value;
                }
            }

            public override string ToString() {
                return term;
            }
        }

        public Form2() {
            InitializeComponent();
            MemoryScreen.Text = string.Empty;
            overflowed.Visible = false;
            KeyPreview = true;
            for (int i = 0; i < size; i++) {
                Operands[i] = new Expression();
                Operators[i] = new Expression();
            }
        }
        static int size = 256; //field initialler(배열크기 설정하는 부분)은 고정적(static)으로 설정해야 한다.
        List<Memory> TheMemory = new List<Memory>();
        Expression[] Operands = new Expression[size];
        Expression[] Operators = new Expression[size];
        byte TermIndex = 0; //괄호가 열릴때마다 이 수치가 1씩올라간다.

        string ScreenBuffer = string.Empty; //수식을 입력받을 때의 버퍼.
        string LastResult = string.Empty; //수식에 적용될때의 버퍼.
        string LastOperator = string.Empty; //(방금 계산한) 마지막 연산자
        string LastOperand = string.Empty; // (방금 계산한) 마지막 피연산자
        string BufferedMemory = string.Empty; //메모리 스크린에 쓰기전에 쓰이는 버퍼.

        bool IsDot = false; // 실수형인지 정수형인지
        byte screen_max_width = 40; //스크린의 너비
        byte memory_max_width = 60; //메모리 스크린의 너비

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            //대부분의 경우 키보드에서 키를 누르면 KeyUp, KeyPress, KeyDown 등의 이벤트들이 발생한다.
            //그러나 Control, Alt, ReturnResult 같은 특수한 키들은 상기된 이벤트들을 발생시기 전에 미리 짜여진, 다른 행동을 취한다.
            //ProcessCmdKey는 해당 폼으로 들어오는 키들을 제일 먼저 인식하고 관련된 키에 대한 프로시저들을 실행하는 메소드인데, 이 함수를 오버라이드 한다.
            //Debug.WriteLine(Convert.ToString(keyData));
            if (keyData == Keys.Return) {
                Equal.PerformClick();
                return true; //여기서 return 함으로써 enter키를 누르는 행동이 '버튼을 누르는 등'의 프로시저들 행하지 않고 종료시킨다.
            }
            if (keyData == Keys.Tab) {
                foreach (Control ctr in Controls) {
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
            Type t = GetType();
            MethodInfo method = t.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            method.Invoke(this, new object[] { null, EventArgs.Empty });
        }
        private void Form2_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    CallMethodByName("NUM" + Convert.ToString((int)e.KeyCode - 48) + "_Click"); break;
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    CallMethodByName("NUM" + Convert.ToString((int)e.KeyCode - 96) + "_Click"); break;
                case Keys.Decimal:
                    Dot_Click(null, EventArgs.Empty); break;
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
                case Keys.Oemplus:
                    Equal_Click(null, EventArgs.Empty); break;
            }
        }
        private void SetIsDot(string x) {
            if (x.Equals("NaN")) {
                IsDot = false;
                return;
            }
            double temp = Convert.ToDouble(x);
            IsDot = temp != Convert.ToInt32(temp);
        }
        private bool IsValidModOp() {
            try {
                Convert.ToInt64(Screen.Text);
            } catch {
                return true;
            }
            return false;
        }
        private void ChangeOperator(string op) {
            if (TheMemory.Count == 0) {
                if (LastResult == string.Empty) {
                    ScreenBuffer = negate();
                    ShowScreenBuffer();
                    return;
                }
                TheMemory.Add(new Memory(LastResult));
                TheMemory.Add(new Memory(op));
                LastOperand = LastResult;
            } else {
                if (Operators[TermIndex].count() == 0) {
                    ScreenBuffer = negate();
                    ShowScreenBuffer();
                    return;
                } else {
                    Operators[TermIndex].pop();
                    Operators[TermIndex].push(op);
                }
            }
            IsDot = false;
            TheMemory[TheMemory.Count - 1].Term = op;
            LastOperator = op;
            WriteFormula();
            MemoryScreenUpdate();
        }
        private int ValidateBracket() {
            int NumOpenBracket = 0;
            //foreach (char c in TheFormula) if (c == '(') NumOpenBracket++;

            int NumCloseBracket = 0;
            //foreach (char c in TheFormula) if (c == ')') NumCloseBracket++;

            return NumOpenBracket - NumCloseBracket;
        }
        private void AddOperandToScreen(char operand) {
            ScreenBuffer = ScreenBuffer + operand;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private void AddBracketToScreen(char bracket) {
            ScreenBuffer = string.Empty;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private string GetOperand() {
            return ScreenBuffer != string.Empty
                ? ScreenBuffer
                : Operands[TermIndex].count() == 0 ? LastResult : LastOperand != string.Empty ? LastOperand : "0"; // ㅋㅋ
        }
        private string GetFormed() {
            if (TheMemory.Count == 0) return GetOperand();
            if (IsLastTerm()) {
                return TheMemory[TheMemory.Count - 1].Term;
            } else return TheMemory[TheMemory.Count - 2].Term;
        }
        private bool IsLastTerm() {
            if (TheMemory.Count == 0) return true;
            string[] operators = { "+", "-", "*", "/", "%", "^", "yroot" };
            bool result = false;
            foreach (var op in operators) {
                result = TheMemory[TheMemory.Count - 1].Term == op;
                if (result) return true;
            }
            Debug.WriteLine(result);
            return false;
        }
        private double Factorial(double i) {
            if (i < 1) return 0;
            return (i == 1) ? 1 : i * Factorial(i - 1);
        }
        private void UnaryOperater(string function) {
            if (ScreenBuffer != string.Empty) {
                PushOperand(ScreenBuffer);
            }
            for (int i = Operators[TermIndex].count(); i != 0; i--) {
                BinaryOperate();
            }
            string Operand = GetOperand();
            double ToCalc = Convert.ToDouble(Operand);
            double calculated = 0;
            string result = string.Empty;
            switch (function) {
                case "negate":
                    calculated = Convert.ToDouble(negate()); break;
                case "sqrt":
                    calculated = Math.Sqrt(ToCalc); break;
                case "sqrt3":
                    calculated = Math.Pow(ToCalc, (1.0 / 3.0)); break;
                case "sqrt4":
                    calculated = Math.Pow(ToCalc, (1.0 / 4.0)); break;
                case "reciproc":
                    calculated = 1 / ToCalc; break;
                case "sin":
                    calculated = Math.Sin(ToCalc / 180 * Math.PI); break;
                case "cos":
                    calculated = Math.Cos(ToCalc / 180 * Math.PI); break;
                case "tan":
                    calculated = Math.Tan(ToCalc / 180 * Math.PI); break;
                case "asin":
                    calculated = Math.Asin(ToCalc); break;
                case "acos":
                    calculated = Math.Acos(ToCalc); break;
                case "atan":
                    calculated = Math.Atan(ToCalc); break;
                case "pow2":
                    calculated = Math.Pow(ToCalc, 2); break;
                case "pow3":
                    calculated = Math.Pow(ToCalc, 3); break;
                case "pow4":
                    calculated = Math.Pow(ToCalc, 4); break;
                case "pow10":
                    calculated = Math.Pow(10, ToCalc); break;
                case "fact":
                    if (ToCalc == Convert.ToInt32(ToCalc)) {
                        calculated = Factorial(ToCalc);
                    } else MessageBox.Show("팩토리얼 연산은 정수끼리만 가능합니다.");
                    break;
                case "ln":
                    calculated = Math.Log(ToCalc, 10); break;
                case "l2":
                    calculated = Math.Log(ToCalc, 2); break;
                case "log":
                    calculated = Math.Log(ToCalc); break;
                case "ex":
                    calculated = Math.Exp(ToCalc); break;
            }
            result = Convert.ToString(calculated);
            LastResult = result;
            SetIsDot(result);
            ScreenBuffer = result;
            TheMemory.Clear();
            BufferedMemory = string.Empty;
            MemoryScreenUpdate();
            for (int i = 0; i < size; i++) {
                Operands[i].clear();
                Operators[i].clear();
            }
            ScreenUpdate();
        }
        private string negate() {
            return ScreenBuffer.Contains("-") ? ScreenBuffer.Remove(0, 1) : ScreenBuffer.Insert(0, "-");
        }
        private void DeleteBuffer() {
            ScreenBuffer = string.Empty;
        }
        private void WriteFormula() { // TheMemory에 적힌 수식을 메모리 버퍼에 포맷해서 적음
            string Formula = string.Empty;
            foreach (var list in TheMemory) {
                //괄호가 아니면
                Formula = string.Format("{0} {1}", Formula, list.ToString());
            }
            BufferedMemory = Formula;
        }
        private void WriteFormula(string PREFORMED, string function) {
            string _FORMED = function + "(" + PREFORMED + ")";
            int index = IsLastTerm() ? TheMemory.Count - 1 : TheMemory.Count - 2;
        }
        private void MemoryScreenUpdate() {
            MemoryScreen.Text = string.Format("{0," + Convert.ToString(memory_max_width) + "}", BufferedMemory);
        }
        private void ScreenUpdate() {
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", LastResult);
        }
        private void ShowScreenBuffer() {
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private void PushOperand(string op) {
            Operands[TermIndex].push(op);
        }
        private void PushOperator(string op) {
            Operators[TermIndex].push(op);
        }
        private string Compute(string Operand1, string Operand2, string Operator) {
            double result = 0;
            double Op1 = Convert.ToDouble(Operand1);
            double Op2 = Convert.ToDouble(Operand2);
            switch (Operator) {
                case "+": result = Op1 + Op2; break;
                case "-": result = Op1 - Op2; break;
                case "*": result = Op1 * Op2; break;
                case "/": result = Op1 / Op2; break;
                case "%": result = Op1 % Op2; break;
                case "^": result = Math.Pow(Op1, Op2); break;
                case "yroot": result = Math.Pow(Op1, (1 / Op2)); break;
            }
            return Convert.ToString(result);
        }
        private string BinaryOperate() {
            string Op2 = Operands[TermIndex].pop();
            string Op1 = Operands[TermIndex].pop();
            string Operator = Operators[TermIndex].pop();
            string result = Compute(Op1, Op2, Operator);
            LastResult = result;
            LastOperand = Op2;
            LastOperator = Operator;
            return LastResult;
        }
        private void TryCalculate(string op) {
            int opcnt = Operands[TermIndex].count();
            if (opcnt < 2) {
                if (ScreenBuffer != string.Empty) {                      //(TC case 1)
                    LastOperand = ScreenBuffer;
                    LastOperator = op;
                }
            } else {
                bool IsOpPlusMinus = op.Equals("+") || op.Equals("-");
                switch (Operators[TermIndex].count()) {
                    case 1:
                        if (!(Operators[TermIndex].peek().Equals("+") || Operators[TermIndex].peek().Equals("-"))) {
                            PushOperand(BinaryOperate());                //(TC case 2)
                            ScreenUpdate();
                        } else {
                            if (IsOpPlusMinus) {                         //(TC case 3)
                                PushOperand(BinaryOperate());
                                ScreenUpdate();
                            } else {                                     //(TC case 4)
                                //할 것 없음
                            }
                        }
                        break;
                    case 2:
                        if (!IsOpPlusMinus) {                            //(TC case 5)
                            PushOperand(BinaryOperate());
                            ScreenUpdate();
                        } else {                                         //(TC case 6)
                            PushOperand(BinaryOperate());
                            PushOperand(BinaryOperate());
                            ScreenUpdate();
                        }
                        break;
                }
            }
            IsDot = false;
        }
        private void BinaryOperater(string op) {
            //if (IsValidModOp()) return;
            if (ScreenBuffer == string.Empty && LastOperand == string.Empty && op != "-") return; //아무것도 입력 안했는데 눌렀을경우
            if (ScreenBuffer == string.Empty || ScreenBuffer == "-") { // 1 + 2 - +, 1 + 2 = -
                ChangeOperator(op);
                return;
            }
            string operand = ScreenBuffer;
            TheMemory.Add(new Memory(operand));
            TheMemory.Add(new Memory(op));
            WriteFormula();
            PushOperand(operand);
            TryCalculate(op);
            PushOperator(op);
            DeleteBuffer();
            MemoryScreenUpdate();
        }
        private void ReturnResult() {
            if (Operands[TermIndex].count() == 0 && LastOperator == string.Empty) { Debug.WriteLine("ReturnResult Invalid Formula"); return; } //연산자나 피연산자가 입력되지 않았을경우

            string result = string.Empty;

            if (Operands[TermIndex].count() > 0 && ScreenBuffer != string.Empty) { //RR case 1 (1 + 2 = )  
                PushOperand(ScreenBuffer);
                if (Operators[TermIndex].count() == 2 && !(Operators[TermIndex].peek().Equals("+") || Operators[TermIndex].peek().Equals("-"))) PushOperand(BinaryOperate());
                result = BinaryOperate();
            } else if (Operands[TermIndex].count() > 0 && ScreenBuffer == string.Empty) { //RR case 2 (1 + =), RR case 3
                string ToCalc = Operands[TermIndex].pop();
                LastOperand = LastOperand != string.Empty ? LastOperand : ToCalc;
                result = Compute(ToCalc, LastOperand, LastOperator);
                LastOperand = ToCalc;
                LastResult = result;
            } else if (Operands[TermIndex].count() == 0 && ScreenBuffer == string.Empty) { //RR case 2-1 (1 + = =)
                result = Compute(LastResult, LastOperand, LastOperator);
                LastResult = result;
            } else if (Operands[TermIndex].count() == 0 && ScreenBuffer != string.Empty) { //RR case 5 (1 + 2 = - 2 = )
                LastOperand = ScreenBuffer;
                result = Compute(LastResult, LastOperand, LastOperator);
                LastResult = result;
            }

            TheMemory.Clear();
            BufferedMemory = string.Empty;
            ScreenBuffer = string.Empty;
            MemoryScreenUpdate();
            for (int i = 0; i < size; i++) {
                Operands[i].clear();
                Operators[i].clear();
            }
            SetIsDot(result);
            ScreenUpdate();
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
        private void Negate_Click(object sender, EventArgs e) => UnaryOperater("negate");
        private void Root_Click(object sender, EventArgs e) => UnaryOperater("sqrt");
        private void Reciprocal_Click(object sender, EventArgs e) => UnaryOperater("reciproc");
        private void Percent_Click(object sender, EventArgs e) => UnaryOperater("percent");
        private void Sin_Click(object sender, EventArgs e) => UnaryOperater("sin");
        private void Cos_Click(object sender, EventArgs e) => UnaryOperater("cos");
        private void Tan_Click(object sender, EventArgs e) => UnaryOperater("tan");
        private void SinR_Click(object sender, EventArgs e) => UnaryOperater("asin");
        private void CosR_Click(object sender, EventArgs e) => UnaryOperater("acos");
        private void TanR_Click(object sender, EventArgs e) => UnaryOperater("atan");
        private void Power2_Click(object sender, EventArgs e) => UnaryOperater("pow2");
        private void Power3_Click(object sender, EventArgs e) => UnaryOperater("pow3");
        private void Power4_Click(object sender, EventArgs e) => UnaryOperater("pow4");
        private void sqrt3_Click(object sender, EventArgs e) => UnaryOperater("sqrt3");
        private void sqrt4_Click(object sender, EventArgs e) => UnaryOperater("sqrt4");
        private void fact_Click(object sender, EventArgs e) => UnaryOperater("fact");
        private void power10_Click(object sender, EventArgs e) => UnaryOperater("pow10");
        private void In_Click(object sender, EventArgs e) => UnaryOperater("ln");
        private void Log2_Click(object sender, EventArgs e) => UnaryOperater("l2");
        private void Log_Click(object sender, EventArgs e) => UnaryOperater("log");
        private void Ex_Click(object sender, EventArgs e) => UnaryOperater("ex");

        private void Plus_Click(object sender, EventArgs e) => BinaryOperater("+");
        private void Minus_Click(object sender, EventArgs e) => BinaryOperater("-");
        private void Multiply_Click(object sender, EventArgs e) => BinaryOperater("*");
        private void Divide_Click(object sender, EventArgs e) => BinaryOperater("/");
        private void Mod_Click(object sender, EventArgs e) => BinaryOperater("%");
        private void Power_Click(object sender, EventArgs e) => BinaryOperater("^");
        private void sqrtn_Click(object sender, EventArgs e) => BinaryOperater("yroot");
        private void Equal_Click(object sender, EventArgs e) => ReturnResult();
        private void AC_Click(object sender, EventArgs e) { // 메모리 + 스크린 모두 초기화 
            TheMemory.Clear();
            for (int i = 0; i < size; i++) {
                Operands[i].clear();
                Operators[i].clear();
            }
            TermIndex = 0;

            ScreenBuffer = string.Empty;
            LastResult = string.Empty;
            LastOperator = string.Empty;
            LastOperand = string.Empty;
            BufferedMemory = string.Empty;

            Screen.Text = string.Empty;
            MemoryScreen.Text = string.Empty;

            overflowed.Visible = false;

            IsDot = false;
        }
        private void CE_Click(object sender, EventArgs e) { // 스크린버퍼만 초기화
            IsDot = false;
            ScreenBuffer = string.Empty;
            Screen.Text = string.Empty;
        }
        private void Backspace_Click(object sender, EventArgs e) {
            //기본적으로 c#에는 안정성문제로 포인터 연산자 사용을 막아두었다. (Unsafe 모드로 디버그 해야 한다고함)
            //따라서 문자열을 관리하기 위해서는 주워진 메소드를 사용해야한다.
            //스트링이름.Remove(문자열인덱스, 지울 갯수)
            if (ScreenBuffer.Length == 0) return;
            if (ScreenBuffer[ScreenBuffer.Length - 1] == '.') IsDot = false;
            ScreenBuffer = ScreenBuffer.Remove(ScreenBuffer.Length - 1, 1);
            Screen.Text = Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private void 일반용계산기ToolStripMenuItem_Click(object sender, EventArgs e) {
            Owner.Show();
            Owner.Location = Location;

            Hide();
        }
        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e) {
            Owner.Dispose();
            Dispose();
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Owner.Dispose();
            Dispose();
        }
        private void 개발자소개ToolStripMenuItem_Click(object sender, EventArgs e) => new Form3().ShowDialog();
        private void 사용정보ToolStripMenuItem_Click(object sender, EventArgs e) => new Form4().ShowDialog();
        private void toolStripMenuItem4_Click(object sender, EventArgs e) => new Form5().ShowDialog();
        private void 개발환경ToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("운영 체제 : Windows 10\n개발 도구 : Microsoft Visual Studio 2017\n\t", "< 개발 환경 >");
        private void 버전정보ToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("C#으로 내가 만든 계산기 1.0", "< 버전 정보 >");
    }
}
