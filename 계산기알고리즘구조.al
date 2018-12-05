struct 식[]
float[] 피연산자 1, 피연산자 2, 버퍼 	(알고리즘에선 '피연산자 스택'로 간주)
string[] 연산자		  	(알고리즘에선 '연산자 스택'으로 간주)

괄호가 한개 추가되는것이 식 배열의 인덱스가 1 늘어나는 것과 같음.

Screen : 계산결과가 출력될 라벨
ScreenBuffer : 피연산자가 입력될 공간

연산자를 입력하는 것이 피연산자의 '항 입력'이 끝났다는 것을 의미함.
연산자를 입력했을때 PushOperand -> TryCalc -> PushOperator -> deletebuffer의 순서로 일어난다.

[PushOperand(숫자; 없어도됨)]
- 숫자가 있으면 숫자를, 없으면 피연산자를 피연산자 스택에 push는다. 
(string(ScreenBuffer)에서 float으로 형변환해서 대입.)

[TryCalc]
- 연산이 유효한지 확인한다.(나머지연산을 하려는데, 소수가 있을경우) 유효하지 않으면 error.
- 큐가 꽉차지 않았을경우 return
elseif 스택에 연산자가 없을경우 return						(case1)
elseif 스택에 연산자가 1개있고 그 연산자가 *, / 인경우					
ㄴ Calculate를 1회 호출 한다.						(case2)
elseif 스택에 연산자가 1개있고 그 연산자가 +, - 인경우
ㄴ Calculate를 1회 호출 한다.						(case3)
ㄴ push 할 연산자가 *, / 인경우 return					(case4)
elseif 스택에 연산자가 2개있고, push하려는 연산자가 *, / 인경우				
ㄴ Calculate를 1회 호출 한다.						(case5)
elseif 스택에 연산자가 2개있고, push하려는 연산자가 +, - 인경우				
ㄴ 스택이 빌 때까지 Calculate(2회 수행)한다.					(case6)

[Calculate]
- (피연산자 pop) (연산자 pop) (피연산자 pop) 의 연산을 하여 결과값을 피연산자 스택에 push 한다.
- 연산결과를 AnswerBuffer에 출력하고 PushOperand(연산결과)를 호출한다.

[PushOperator]
- 연산자를 push한다.

[deletebuffer]
- ScreenBuffer 내용을 지운다.

[InitStruct]
- 피연산자, 연산자 스택을 비운다.

[OnCloseBracket]
- 연산자 스택이 빌때까지 Calculate 호출
- 식의 인덱스가 1이상 일경우 

[ReturnResult(IsReturnAll;기본값 true)]
- 연산자가 입력되지 않았을 경우 return
elseif 피연산자가 0개인 경우 return
elseif 피연산자가 1개일 경우 (Screen, 연산자, 피연산자)의 연산을 한다.
else(피연산자가 2개일 경우) 모든 식의 연산자 스택이 빌때까지 Calculate를 호출한다.
- 연산결과가 나온경우 연산결과의 IsDot여부(소수인지 자연수인지)를 판단한다.

[아직 제대로 생각하지 않은것]

단항연산자(피연산자를 바로 변환할 예정)
맨처음 or 괄호열고 처음 쓰는 '-' 연산자는 (isnegative로 할 예정)