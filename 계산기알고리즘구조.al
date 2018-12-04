struct 식[]
float 피연산자 1, 피연산자 2	(알고리즘에선 버퍼크기가 2인 큐로 간주)
string 연산자		(알고리즘에선 버퍼크기가 1인 스택으로 간주)

괄호가 한개 추가되는것이 식 배열의 인덱스가 1 늘어나는 것과 같음.

Screen : 계산결과가 출력될 라벨
ScreenBuffer : 피연산자가 입력될 공간

연산자를 입력하는 것이 피연산자의 '항 입력'이 끝났다는 것을 의미함.
기본적인 수식입력은 연산자를 입력했을때 enqueue -> push -> deletebuffer의 틀에서 일어난다.(EPD)

[enqueue]
- 피연산자를 넣는다. (피연산자를 enqueue할땐 string(ScreenBuffer)에서 float으로 형변환.)
else 연산결과를 넣는다.

[push]
- 스택이 꽉찬경우 TryCalc 호출.
else 스택중에 *, / 연산자가 있으면 TryCalc 호출.
- 연산자를 넣는다.

[deletebuffer]
- ScreenBuffer 내용을 지운다.

[TryCalc]
- 연산이 유효한지 확인한다.(나머지연산을 하려는데, 소수가 있을경우) 유효하지 않으면 Return.
- 피연산자2개와 연산자의 계산을 한다. 계산에 성공하면 InitStruct를 호출.

[InitStruct]
- 스택과 큐를 비운다.

[ReturnResult('=' 눌렀을때)]
- 연산자가 입력되지 않았을 경우 Return
elseif 피연산자가 0개인 경우 Return
elseif 피연산자가 1개일 경우 Screen, 연산자, 피연산자의 연산을 한다.
else(피연산자가 2개일 경우) TryCalc를 호출한다.
- IsDot여부(소수인지 자연수인지)를 판단한다.

[아직 제대로 생각하지 않은것]

단항연산자(피연산자를 바로 변환할 예정)
맨처음 or 괄호열고 처음 쓰는 '-' 연산자는 (isnegative로 할 예정)