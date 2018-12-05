struct 식[]
float[] 피연산자 1, 피연산자 2, 버퍼 	(알고리즘에선 '피연산자 스택'으로 간주)
string[] 연산자		  		(알고리즘에선 '연산자 스택'으로 간주)

괄호가 한개 추가되는것이 식 배열의 인덱스가 1 늘어나는 것과 같음.

Screen : 계산결과가 출력될 라벨
ScreenBuffer : 피연산자가 입력될 공간

연산자를 입력하는 것이 피연산자의 '항 입력'이 끝났다는 것을 의미함.
연산자를 입력했을때 PushOperand -> TryCalc -> PushOperator -> deletebuffer의 순서로 일어난다.

[PushOperand(숫자)]
- 숫자를 피연산자 스택에 push한다. 
(string(ScreenBuffer)에서 float으로 형변환해서 대입.)

[TryCalc]
- 연산이 유효한지 확인한다.(나머지연산, 팩토리얼을 하려는데, 소수가 있을경우) 유효하지 않으면 error.
- 피연산자 스택에 피연산자가 2개 미만 일경우 return
elseif 연산자스택에 연산자가 없을경우 return					(case1)
elseif 연산자스택에 연산자가 1개있고 그 연산자가 *, / 인경우					
ㄴ PushOperand(Calculate)를 1회 호출 한다.					(case2)
elseif 연산자스택에 연산자가 1개있고 그 연산자가 +, - 인경우
ㄴ push 할 연산자가 +, - 인경우 PushOperand(Calculate)를 1회 호출 한다.		(case3)
ㄴ push 할 연산자가 *, / 인경우 return					(case4)
elseif 연산자스택에 연산자가 2개있고, push하려는 연산자가 *, / 인경우				
ㄴ PushOperand(Calculate)를 1회 호출 한다.					(case5)
elseif 연산자스택에 연산자가 2개있고, push하려는 연산자가 +, - 인경우				
ㄴ 연산자스택이 빌 때까지 PushOperand(Calculate)(2회 수행)한다.			(case6)

[Calculate]
- (피연산자 pop) (연산자 pop) (피연산자 pop) 의 연산을 하여 결과값을 반환한다.
- 연산결과를 AnswerBuffer에 대입한다.

[ScreenUpdate]
- AnswerBuffer에 있는것을 Screen에 형식에 맞게 출력한다.

[PushOperator]
- 연산자를 push한다.

[deletebuffer]
- ScreenBuffer 내용을 지운다.

[InitStruct(인덱스)]
- 인덱스가 있을경우 해당 식 인덱스의 피연산자, 연산자 스택를 초기화한다.

[OnCloseBracket]
- 연산자 스택이 빌때까지 PushOperand(Calculate) 호출
- 식의 인덱스가 1이상 일경우 이전 식 인덱스에 PushOperand(Caculate)를 호출하고 

[ReturnResult(IsReturnAll;기본값 true)]
- 연산자가 입력되지 않았을 경우 return
elseif 피연산자가 0개인 경우 return
elseif 피연산자가 1개일 경우 (Screen, 연산자, 피연산자)의 연산을 한다.
else(피연산자가 2개일 경우) 모든 식의 연산자 스택이 빌때까지 Calculate를 호출한다.
- 연산결과가 나온경우 연산결과의 IsDot여부(소수인지 자연수인지)를 판단한다.

[아직 제대로 생각하지 않은것]

단항연산자(피연산자를 바로 변환해서 push할 예정)
맨처음 or 괄호열고 처음 쓰는 '-' 연산자는 (isnegative로 할 예정)
입력한 숫자가 메모리 스크린에 대입되는 경우(연산자 클릭)