using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1107 {
    public partial class Form1 : Form {
        string ScreenBuffer = "";
        long[] Num = { 0,};
        decimal[] Fnum = { 0,}; //.이 입력됐을경우 모든 연산은 Fnum을 사용
        byte[] OpBuffer = { 0,}; // (무연산자), +, -, *, /
        byte bufferptr = 0; // 연산자의 포인터
        bool IsDot = false; // 실수형인지 정수형인지

        public Form1() {
            InitializeComponent();
        }

        private void MemoryScreenUpdate() {
            char op = '+';
            switch(OpBuffer[bufferptr]) {
                case 1: op = '+'; break;
                case 2: op = '-'; break;
                case 3: op = '*'; break;
                case 4: op = '/'; break;
            }
            MemoryScreen.Text = MemoryScreen.Text + " " + ScreenBuffer + " " + op;
        }

        private void AddNumToScreen(string op) {
            ScreenBuffer = ScreenBuffer + op;
            Screen.Text = ScreenBuffer;
        }

        private void AddOpToScreen(byte op) {
            OpBuffer[bufferptr] = op;
            MemoryScreenUpdate();
            bufferptr++;
            IsDot = false;
            ScreenBuffer = "";
        }

        private void NUM0_Click(object sender, EventArgs e) {
            //숫자 0은 1의 자릿수에서 2개이상 쓰이지 않음.
            //소수점 아래에서 위의 조건은 무시됨.
            if(ScreenBuffer.Length != 0) {
                AddNumToScreen("0");
            }
        }

        private void NUM1_Click(object sender, EventArgs e) {
            AddNumToScreen("1");
        }

        private void NUM2_Click(object sender, EventArgs e) {
            AddNumToScreen("2");
        }

        private void NUM3_Click(object sender, EventArgs e) {
            AddNumToScreen("3");
        }

        private void NUM4_Click(object sender, EventArgs e) {
            AddNumToScreen("4");
        }

        private void NUM5_Click(object sender, EventArgs e) {
            AddNumToScreen("5");
        }

        private void NUM6_Click(object sender, EventArgs e) {
            AddNumToScreen("6");
        }

        private void NUM7_Click(object sender, EventArgs e) {
            AddNumToScreen("7");
        }

        private void NUM8_Click(object sender, EventArgs e) {
            AddNumToScreen("8");
        }

        private void NUM9_Click(object sender, EventArgs e) {
            AddNumToScreen("9");
        }

        private void Dot_Click(object sender, EventArgs e) {
            if(!IsDot) {
                if (ScreenBuffer.Length == 0) AddNumToScreen("0.");
                else AddNumToScreen(".");
                IsDot = true;
            }
        }

        private void AC_Click(object sender, EventArgs e) { // 메모리 버퍼 + 스크린버퍼 모두 초기화 
            ScreenBuffer = "";
            foreach(int i in Num) Num[i] = 0;
            foreach(int i in Fnum) Fnum[i] = 0;
            foreach(int i in OpBuffer) OpBuffer[i] = 0;
            IsDot = false;
            Screen.Text = "";
            MemoryScreen.Text = "";
        }

        private void CE_Click(object sender, EventArgs e) { // 스크린버퍼만 초기화
            IsDot = false;
            ScreenBuffer = "";
            Screen.Text = "";
        }

        private void Backspace_Click(object sender, EventArgs e) {
            //기본적으로 c#에는 안정성문제로 포인터 연산자 사용을 막아두었다. (Unsafe 모드로 디버그 해야 한다고함)
            //따라서 문자열을 관리하기 위해서는 주워진 내장함수를 사용해야한다.
            //스트링이름.Remove(문자열인덱스, 지울 갯수)
            if(ScreenBuffer.Length == 0) return;
            if(ScreenBuffer[ScreenBuffer.Length - 1] == '.') IsDot = false;
            ScreenBuffer = ScreenBuffer.Remove(ScreenBuffer.Length - 1, 1);
            Screen.Text = ScreenBuffer;
        }

        private void Plus_Click(object sender, EventArgs e) {
            AddOpToScreen(1);
        }
    }
}
